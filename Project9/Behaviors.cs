using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    public static class Behaviors
    {
        public static Point DirectionFromParam(byte p)
        {
            return p switch
            {
                0 => new Point(0, -1),
                1 => new Point(0, 1),
                2 => new Point(-1, 0),
                3 => new Point(1, 0),
                4 => new Point(-1, -1),
                5 => new Point(1, -1),
                6 => new Point(-1, 1),
                7 => new Point(1, 1),
                _ => Point.Zero
            };
        }
        public struct ActionProposal
        {
            public byte Type;   // 0=Move, 1=Eat, 2=Talk...
            public Point Target; // direction ou position
            public float Score;  // importance estimée
            public Gene SourceGene; // <- important
        }
        public static ActionProposal GetProposal(PCA pca, Gene g, ChunkManager chunks)
        {
            switch (g.Type)
            {
                case 0: return MoveProposal(pca, g);
                case 1: return EatProposal(pca, g, chunks);
                case 2: return SeeProposal(pca, g, chunks);
                case 3: return TalkProposal(pca, g);
                case 4: return LifeProposal(pca, g);
            }

            return default;
        }
        public static void ExecuteProposal(PCA pca, ActionProposal p, ChunkManager chunks)
        {
            float reward = 0f;

            switch (p.Type)
            {
                case 0: // Move
                    var oldPos = pca.Position;
                    pca.Position += p.Target;
                    chunks.ClampPosition(ref pca.Position);

                    reward = ComputeMoveReward(pca, oldPos, chunks);

                    if (!pca.Visited.Contains(pca.Position))
                    {
                        pca.Visited.Add(pca.Position);
                        reward += 0.2f; // exploration récompensée
                    }

                    break;

                case 1: // Eat
                    pca.hunger = Math.Max(0, pca.hunger - 20);
                    chunks.SetCell(pca.Position, CellType.Floor);
                    reward = +5f; // gros bonus

                    AdjustGeneWeight(ref p.SourceGene, +1.0f);
                    break;

                case 2: // Talk
                    pca.lastSymbol = 1;
                    pca.talkTimer = 30;
                    reward = +0.1f;
                    break;
            }

            // éventuellement : ajuster aussi le poids en fonction du reward global
            if (reward != 0)
                AdjustGeneWeight(ref p.SourceGene, reward);

            if (reward > 0)
                pca.stagnationCounter = 0;
            else
                pca.stagnationCounter++;

        }

        private static ActionProposal MoveProposal(PCA pca, Gene g)
        {
            Point dir = Behaviors.DirectionFromParam(g.ParamA);

            return new ActionProposal
            {
                Type = 0,
                Target = dir,
                Score = 0.1f * g.Weight,
                SourceGene = g
            };
        }
        private static ActionProposal SeeProposal(PCA pca, Gene g, ChunkManager chunks)
        {
            int maxDist = Math.Max(1, (int)g.ParamB);
            Point dir = Behaviors.DirectionFromParam(g.ParamA);

            for (int i = 1; i <= maxDist; i++)
            {
                Point p = new Point(pca.Position.X + dir.X * i, pca.Position.Y + dir.Y * i);
                var cell = chunks.GetCell(p);

                // Si c'est de la nourriture que le PCA peut manger
                if ((cell == CellType.FoodVegetable || cell == CellType.FoodMeat)
                    && Behaviors.CanEat(pca, cell))
                {
                    return new ActionProposal
                    {
                        Type = 0, // Move vers la nourriture
                        Target = dir,
                        Score = 1.0f * g.Weight + 0.5f, // bonus perception
                        SourceGene = g
                    };
                }

                // Si c'est du danger → proposer de fuir
                if (cell == CellType.Danger)
                {
                    return new ActionProposal
                    {
                        Type = 0,
                        Target = new Point(-dir.X, -dir.Y), // fuite
                        Score = 0.8f * g.Weight, // important mais moins que la nourriture
                        SourceGene = g
                    };
                }
            }

            return default;
        }
        public static bool CanEat(PCA pca, CellType cell)
        {
            foreach (var g in pca.Genome.Genes)
            {
                if (g.Type != 1) continue; // Eat gene

                // Herbivore
                if (cell == CellType.FoodVegetable && (g.ParamA == 0 || g.ParamA == 2))
                    return true;

                // Carnivore
                if (cell == CellType.FoodMeat && (g.ParamA == 1 || g.ParamA == 2))
                    return true;
            }

            return false;
        }
        private static ActionProposal EatProposal(PCA pca, Gene g, ChunkManager chunks)
        {
            var cell = chunks.GetCell(pca.Position);

            if (!Behaviors.CanEat(pca, cell))
                return default;

            float baseScore = 0.5f;

            // faim augmente la priorité
            baseScore *= 1f + (pca.hunger / 50f);

            return new ActionProposal
            {
                Type = 1,
                Target = pca.Position,
                Score = baseScore * g.Weight,
                SourceGene = g
            };
        }
        public static ActionProposal InstinctEatProposal(PCA pca, ChunkManager chunks)
        {
            var cell = chunks.GetCell(pca.Position);

            if (cell == CellType.FoodVegetable || cell == CellType.FoodMeat)
            {
                return new ActionProposal
                {
                    Type = 1,
                    Target = pca.Position,
                    Score = 0.3f + (pca.hunger / 40f), // faim = priorité
                    SourceGene = default
                };
            }

            return default;
        }
        private static ActionProposal TalkProposal(PCA pca, Gene g)
        {
            // Ici on peut décider que Talk sert surtout à "signaler" un état
            // On lui met une priorité faible par défaut
            return new ActionProposal
            {
                Type = 2,              // 2 = Talk
                Target = pca.Position, // pas de déplacement
                Score = 0.2f * g.Weight,
                SourceGene = g
            };
        }
        private static ActionProposal LifeProposal(PCA pca, Gene g)
        {
            if (pca.lifeSpan < 1f)
            {
                return new ActionProposal
                {
                    Type = 2,              // Talk
                    Target = pca.Position,
                    Score = 0.5f * g.Weight,
                    SourceGene = g
                };
            }

            return default;
        }



        public static float ComputeMoveReward(PCA pca, Point oldPos, ChunkManager chunks)
        {
            // bonus si on se rapproche d’une nourriture
            int oldDist = DistanceToNearestFood(oldPos, chunks);
            int newDist = DistanceToNearestFood(pca.Position, chunks);

            if (newDist < oldDist) return +0.5f;  // on se rapproche
            if (newDist > oldDist) return -0.2f;  // on s’éloigne

            if (pca.Position == oldPos)
                return -0.3f; // bloqué = gros malus

            return -0.1f; // mouvement inutile
        }
        public static void AdjustGeneWeight(ref Gene g, float reward)
        {
            g.Weight += reward * 0.1f; // apprentissage lent
            g.Weight = Math.Clamp(g.Weight, 0.1f, 5f);
        }
        public static int DistanceToNearestFood(Point pos, ChunkManager chunks)
        {
            int best = int.MaxValue;

            for (int y = 0; y < MAP.Height; y++)
            {
                for (int x = 0; x < MAP.Width; x++)
                {
                    var cell = chunks.GetCell(new Point(x, y));

                    if (cell == CellType.FoodVegetable || cell == CellType.FoodMeat)
                    {
                        int dist = Math.Abs(pos.X - x) + Math.Abs(pos.Y - y);
                        if (dist < best)
                            best = dist;
                    }
                }
            }

            return best == int.MaxValue ? 9999 : best;
        }
        public static int DistanceToNearestDanger(Point pos, ChunkManager chunks)
        {
            int best = int.MaxValue;

            for (int y = 0; y < MAP.Height; y++)
            {
                for (int x = 0; x < MAP.Width; x++)
                {
                    var cell = chunks.GetCell(new Point(x, y));

                    if (cell == CellType.Danger)
                    {
                        int dist = Math.Abs(pos.X - x) + Math.Abs(pos.Y - y);
                        if (dist < best)
                            best = dist;
                    }
                }
            }

            return best == int.MaxValue ? 9999 : best;
        }


        public static void Move(this PCA pca, Gene g)
        {
            int speed = Math.Max(1, (int)g.ParamB);
            Point delta = Point.Zero;

            switch (g.ParamA)
            {
                case 0: delta = new Point(0, -speed); break;
                case 1: delta = new Point(0, speed); break;
                case 2: delta = new Point(-speed, 0); break;
                case 3: delta = new Point(speed, 0); break;
                case 4: delta = new Point(-speed, -speed); break;
                case 5: delta = new Point(speed, -speed); break;
                case 6: delta = new Point(-speed, speed); break;
                case 7: delta = new Point(speed, speed); break;
            }

            pca.Position += delta;
            Game1.Instance._chunks.ClampPosition(ref pca.Position);
        }
        public static void Eat(this PCA pca, Gene g, ChunkManager chunks)
        {
            var cell = chunks.GetCell(pca.Position);

            bool canEat =
                (cell == CellType.FoodVegetable && (g.ParamA == 0 || g.ParamA == 2)) ||
                (cell == CellType.FoodMeat && (g.ParamA == 1 || g.ParamA == 2));

            if (canEat)
            {
                pca.hunger = Math.Max(0, pca.hunger - (20 + g.ParamB)); // faim réduite
                chunks.SetCell(pca.Position, CellType.Floor);        // nourriture consommée
            }
        }
        public static void See(this PCA pca, Gene g, ChunkManager chunks)
        {
            int maxDist = Math.Max(1, (int)g.ParamB);
            Point dir = DirectionFromParam(g.ParamA);

            for (int i = 1; i <= maxDist; i++)
            {
                Point p = new Point(pca.Position.X + dir.X * i, pca.Position.Y + dir.Y * i);
                var cell = chunks.GetCell(p);

                if (cell == CellType.Danger)
                    pca.stress += 0.02f;

                if (cell == CellType.FoodVegetable || cell == CellType.FoodMeat)
                    pca.stress -= 0.01f;
            }
        }
        public static void Talk(this PCA pca, Gene g)
        {
            pca.lastSymbol = g.ParamA; // ex: 0=faim, 1=peur, 2=explore…
            pca.talkTimer = 30;        // affiché pendant 30 frames
        }

        public static void UpdateLifeSpan(this PCA pca, Gene g)
        {
            if (pca.hunger >= 50)
            {
                pca.Respawn();
                return;
            }

            if (pca.lifeSpan <= 0)
            {
                pca.Respawn();
                return;
            }

            pca.lifeSpan -= 1f / 60f; // décrément par seconde (approx)
        }
    }
}
