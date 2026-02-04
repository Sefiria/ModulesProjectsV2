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
        Danger // par ex: vide mortel, trou, etc.
    }

    public class MAP
    {
        public const int Width = 20;
        public const int Height = 20;

        private readonly CellType[,] _cells;
        private readonly Random _rng = new Random();

        public MAP()
        {
            _cells = new CellType[Width, Height];
            Generate();
        }

        private void Generate()
        {
            // Exemple simple : tout en Floor, quelques nourritures et dangers
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _cells[x, y] = CellType.Floor;

                    double r = _rng.NextDouble();
                    if (r < 0.05) _cells[x, y] = CellType.FoodVegetable;
                    else if (r < 0.08) _cells[x, y] = CellType.FoodMeat;
                    else if (r < 0.10) _cells[x, y] = CellType.Danger;
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
