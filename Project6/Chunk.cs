using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Project6
{
    public class Chunk
    {
        public static readonly int Size = 16;

        [BsonId]
        public ObjectId Id { get; set; }

        public int ChunkX { get; set; }
        public int ChunkY { get; set; }

        public List<Tile> Tiles { get; set; } = new List<Tile>();

        public Tile this[int x, int y] => Tiles[y * Size + x];
        public void Set(int x, int y, Tile tile) => Tiles[y * Size + x] = tile;

        public static Chunk CreateDefault(int chunkX, int chunkY)
        {
            var tiles = new List<Tile>();
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    tiles.Add(new Tile { X = x, Y = y, Type = "Grass" });
                }
            }
            return new Chunk
            {
                ChunkX = chunkX,
                ChunkY = chunkY,
                Tiles = tiles
            };
        }
    }

    public static class ChunkHelper
    {
        public static List<Tile> ToList(this Tile[,] grid)
        {
            var list = new List<Tile>();
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    list.Add(grid[x, y]);
                }
            }
            return list;
        }
        public static Tile[,] ToArray(this List<Tile> tiles)
        {
            var grid = new Tile[Chunk.Size, Chunk.Size];
            foreach (var tile in tiles)
            {
                if (tile.X >= 0 && tile.X < Chunk.Size && tile.Y >= 0 && tile.Y < Chunk.Size)
                {
                    grid[tile.X, tile.Y] = tile;
                }
            }
            return grid;
        }

        public static void Save(this Chunk chunk, MongoService mongodb)
        {
            mongodb.SaveChunk(chunk);
        }
    }
}
