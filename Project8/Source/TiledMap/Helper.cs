using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Project8.Source.TiledMap
{
    public static class MapFile
    {
        private const uint MAGIC = 0x334F504D; // "MPO3" (évite "MAP" générique)
        private const byte VERSION = 1;

        // ---------- PUBLIC API ----------
        public static void SaveMap(string path)
        {
            var Tiles = GameMain.Instance.Map.Tiles;
            if (Tiles == null) throw new ArgumentNullException(nameof(Tiles));
            int d0 = Tiles.GetLength(0), d1 = Tiles.GetLength(1), d2 = Tiles.GetLength(2);

            // CRC sur les valeurs brutes (little-endian) pour intégrité
            var crc = new Crc32();

            // RLE + VarInt dans un buffer mémoire, puis Deflate
            using var rleBuffer = new MemoryStream(capacity: Math.Max(1024, d0 * d1 * d2 / 4));
            EncodeRleVarInt(Tiles, rleBuffer, crc);
            rleBuffer.Position = 0;

            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            using var bw = new BinaryWriter(fs, Encoding.UTF8, leaveOpen: true);

            // Header
            bw.Write(MAGIC);
            bw.Write(VERSION);
            WriteVarUInt32(bw.BaseStream, (uint)d0);
            WriteVarUInt32(bw.BaseStream, (uint)d1);
            WriteVarUInt32(bw.BaseStream, (uint)d2);

            // Compressed payload (Deflate)
            using var compMs = new MemoryStream();
            using (var def = new DeflateStream(compMs, CompressionLevel.SmallestSize, leaveOpen: true))
            {
                rleBuffer.CopyTo(def);
            }
            var compressed = compMs.ToArray();

            // Taille + données
            WriteVarUInt32(bw.BaseStream, (uint)compressed.Length);
            bw.Write(compressed);

            // CRC32 (brut des tuiles)
            bw.Write(crc.Value);
        }

        public static int[,,] LoadMap(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var br = new BinaryReader(fs, Encoding.UTF8, leaveOpen: true);

            // Header
            uint magic = br.ReadUInt32();
            byte ver = br.ReadByte();
            if (magic != MAGIC || ver != VERSION) throw new InvalidDataException("Format .map inconnu ou version non supportée.");

            int d0 = (int)ReadVarUInt32(br.BaseStream);
            int d1 = (int)ReadVarUInt32(br.BaseStream);
            int d2 = (int)ReadVarUInt32(br.BaseStream);
            long cellCount = (long)d0 * d1 * d2;
            if (d0 < 0 || d1 < 0 || d2 < 0 || cellCount < 0) throw new InvalidDataException("Dimensions invalides.");

            // Compressed payload
            int compLen = (int)ReadVarUInt32(br.BaseStream);
            if (compLen < 0 || compLen > fs.Length - fs.Position - 4) throw new InvalidDataException("Taille compressée invalide.");
            byte[] compressed = br.ReadBytes(compLen);

            // CRC stocké
            uint storedCrc = br.ReadUInt32();

            // Décompression + décodage RLE/VarInt
            using var compMs = new MemoryStream(compressed, writable: false);
            using var def = new DeflateStream(compMs, CompressionMode.Decompress, leaveOpen: false);
            var Tiles = new int[d0, d1, d2];
            var crc = new Crc32();
            DecodeRleVarIntInto(def, Tiles, crc);

            // Vérification CRC
            if (crc.Value != storedCrc) throw new InvalidDataException("CRC invalide: fichier corrompu.");

            return Tiles;
        }

        // ---------- ENCODAGE RLE + VARINT ----------
        private static void EncodeRleVarInt(int[,,] tiles, Stream outStream, Crc32 crc)
        {
            int d0 = tiles.GetLength(0), d1 = tiles.GetLength(1), d2 = tiles.GetLength(2);
            if (d0 == 0 || d1 == 0 || d2 == 0) return;

            using var bw = new BinaryWriter(outStream, Encoding.UTF8, leaveOpen: true);

            bool first = true;
            int cur = 0;
            uint run = 0;

            void FlushRun()
            {
                if (run == 0) return;
                WriteVarInt32(bw.BaseStream, cur);
                WriteVarUInt32(bw.BaseStream, run);
                run = 0;
            }

            for (int i = 0; i < d0; i++)
                for (int j = 0; j < d1; j++)
                    for (int k = 0; k < d2; k++)
                    {
                        int v = tiles[i, j, k];
                        // CRC des 4 octets little-endian
                        crc.Update(BitConverter.GetBytes(v));

                        if (first)
                        {
                            cur = v; run = 1; first = false;
                        }
                        else if (v == cur && run < uint.MaxValue)
                        {
                            run++;
                        }
                        else
                        {
                            FlushRun();
                            cur = v; run = 1;
                        }
                    }
            FlushRun();
        }

        private static void DecodeRleVarIntInto(Stream inStream, int[,,] tiles, Crc32 crc)
        {
            using var br = new BinaryReader(inStream, Encoding.UTF8, leaveOpen: true);
            int d0 = tiles.GetLength(0), d1 = tiles.GetLength(1), d2 = tiles.GetLength(2);

            long total = (long)d0 * d1 * d2;
            long filled = 0;

            int i = 0, j = 0, k = 0;

            while (filled < total)
            {
                int value = ReadVarInt32(br.BaseStream);
                uint run = ReadVarUInt32(br.BaseStream);
                for (uint r = 0; r < run; r++)
                {
                    tiles[i, j, k] = value;
                    crc.Update(BitConverter.GetBytes(value));
                    filled++;

                    // avancée (i,j,k) en ordre i,j,k
                    if (++k == d2) { k = 0; if (++j == d1) { j = 0; i++; } }
                }
            }
            if (filled != total) throw new InvalidDataException("RLE tronqué ou excédentaire.");
        }

        // ---------- VARINT + ZIGZAG ----------
        private static void WriteVarUInt32(Stream s, uint value)
        {
            while (value >= 0x80)
            {
                s.WriteByte((byte)(value | 0x80));
                value >>= 7;
            }
            s.WriteByte((byte)value);
        }

        private static uint ReadVarUInt32(Stream s)
        {
            int shift = 0;
            uint result = 0;
            while (true)
            {
                int b = s.ReadByte();
                if (b < 0) throw new EndOfStreamException();
                result |= (uint)(b & 0x7F) << shift;
                if ((b & 0x80) == 0) break;
                shift += 7;
                if (shift > 35) throw new InvalidDataException("VarUInt32 invalide.");
            }
            return result;
        }

        private static void WriteVarInt32(Stream s, int value)
        {
            uint zz = (uint)((value << 1) ^ (value >> 31));
            WriteVarUInt32(s, zz);
        }

        private static int ReadVarInt32(Stream s)
        {
            uint zz = ReadVarUInt32(s);
            return (int)((zz >> 1) ^ (uint)-(int)(zz & 1));
        }

        // ---------- CRC32 ----------
        private sealed class Crc32
        {
            private const uint Poly = 0xEDB88320u;
            private static readonly uint[] Table = BuildTable();
            private uint _crc = 0xFFFFFFFFu;
            public uint Value => ~_crc;

            public void Update(byte[] bytes)
            {
                for (int i = 0; i < bytes.Length; i++)
                    _crc = (_crc >> 8) ^ Table[(bytes[i] ^ _crc) & 0xFF];
            }
            private static uint[] BuildTable()
            {
                var t = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    uint c = i;
                    for (int k = 0; k < 8; k++)
                        c = (c & 1) != 0 ? (Poly ^ (c >> 1)) : (c >> 1);
                    t[i] = c;
                }
                return t;
            }
        }
    }
}