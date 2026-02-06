using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    public enum CellType
    {
        Empty,
        Floor,
        FoodVegetable,
        FoodMeat,
        Danger,
        Wall
    }

    public class MAP
    {
        public const int Width = 80;
        public const int Height = 80;

        private readonly CellType[,] _cells;
        private readonly Random _rng = new Random();

        public MAP()
        {
            _cells = new CellType[Width, Height];
            Generate();
        }

        private void Generate()
        {
            // 1) Bordures solides
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _cells[x, y] =
                        (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                        ? CellType.Wall
                        : CellType.Floor;

            // 2) Génération
            int roomCount = _rng.Next(10, 30);
            List<Rectangle> rooms = new List<Rectangle>();

            for (int i = 0; i < roomCount; i++)
            {
                int w = _rng.Next(8, 32);
                int h = _rng.Next(8, 32);
                int rx = _rng.Next(1, Width - w - 1);
                int ry = _rng.Next(1, Height - h - 1);

                var room = new Rectangle(rx, ry, w, h);
                rooms.Add(room);

                // Carve floor inside
                for (int y = ry; y < ry + h; y++)
                    for (int x = rx; x < rx + w; x++)
                        _cells[x, y] = CellType.Floor;

                // Murs autour
                for (int y = ry - 1; y <= ry + h; y++)
                    if (InBounds(new Point(rx - 1, y)))
                        _cells[rx - 1, y] = CellType.Wall;
                for (int y = ry - 1; y <= ry + h; y++)
                    if (InBounds(new Point(rx + w, y)))
                        _cells[rx + w, y] = CellType.Wall;
                for (int x = rx - 1; x <= rx + w; x++)
                    if (InBounds(new Point(x, ry - 1)))
                        _cells[x, ry - 1] = CellType.Wall;
                for (int x = rx - 1; x <= rx + w; x++)
                    if (InBounds(new Point(x, ry + h)))
                        _cells[x, ry + h] = CellType.Wall;
            }

            // 3) Couloirs entre pièces
            for (int r = 0; r < rooms.Count - 1; r++)
            {
                var a = rooms[r];
                var b = rooms[r + 1];

                int ax = a.Center.X;
                int ay = a.Center.Y;
                int bx = b.Center.X;
                int by = b.Center.Y;

                // Couloir en L
                for (int x = Math.Min(ax, bx); x <= Math.Max(ax, bx); x++)
                    _cells[x, ay] = CellType.Floor;

                for (int y = Math.Min(ay, by); y <= Math.Max(ay, by); y++)
                    _cells[bx, y] = CellType.Floor;
            }

            // 4) Remplissage aléatoire nourriture/danger
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    if (_cells[x, y] != CellType.Floor) continue;

                    double r = _rng.NextDouble();
                    if (r < 0.001) _cells[x, y] = CellType.FoodMeat;
                    else if (r < 0.002) _cells[x, y] = CellType.FoodVegetable;
                    else if (r < 0.005) _cells[x, y] = CellType.Danger;
                }
            }
        }

        public bool InBounds(Point p)
            => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

        public CellType GetCell(Point p)
            => InBounds(p) ? _cells[p.X, p.Y] : CellType.Danger; // hors map = danger

        public void SetCell(Point p, CellType type)
        {
            if (InBounds(p))
                _cells[p.X, p.Y] = type;
        }

        public bool IsDangerNear(Point p)
        {
            // voisinage de Moore (8 cases autour)
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var n = new Point(p.X + dx, p.Y + dy);
                    if (InBounds(n) && _cells[n.X, n.Y] == CellType.Danger)
                        return true;
                }
            }
            return false;
        }

        public bool ClampPosition(ref Point p)
        {
            int x = Math.Clamp(p.X, 0, Width - 1);
            int y = Math.Clamp(p.Y, 0, Height - 1);

            bool changed = (x != p.X || y != p.Y);
            p = new Point(x, y);
            return changed;
        }

        internal void ClearMap()
        {
            for (int i = 0; i < _cells.GetLength(0); i++)
                for (int j = 0; j < _cells.GetLength(1); j++)
                    _cells[i, j] = CellType.Floor;
        }
    }

    // ChunkManager devient juste un wrapper autour de MAP pour l’instant
    public class ChunkManager
    {
        private readonly MAP _map;

        public ChunkManager(MAP map)
        {
            _map = map;
        }

        public bool IsDangerNear(Point pos) => _map.IsDangerNear(pos);
        public CellType GetCell(Point pos) => _map.GetCell(pos);
        public void SetCell(Point pos, CellType type) => _map.SetCell(pos, type);
        public void ClampPosition(ref Point p) => _map.ClampPosition(ref p);
    }
}
