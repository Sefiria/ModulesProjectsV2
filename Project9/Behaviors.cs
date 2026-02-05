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
        // --- Vision/Raycast helpers ---
        public struct VisionHit
        {
            public Point Cell;
            public CellType Type;
            public int Distance;     // en cases
            public float AngleDeg;   // 0..360
        }

        // Bresenham integer line entre 2 cellules
        private static IEnumerable<Point> BresenhamLine(Point a, Point b)
        {
            int x0 = a.X, y0 = a.Y, x1 = b.X, y1 = b.Y;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                yield return new Point(x0, y0);
                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        // Raycast 360° (stepDeg=2), stop sur Wall, portée >= 16 (ou ParamB si supérieur)
        private static List<VisionHit> Scan360(Point origin, int paramB, ChunkManager chunks, int stepDeg = 2)
        {
            int range = Math.Max(16, Math.Max(1, paramB)); // jamais < 16
            var hits = new List<VisionHit>(180);

            for (int deg = 0; deg < 360; deg += stepDeg)
            {
                float rad = MathHelper.ToRadians(deg);
                int endX = origin.X + (int)Math.Round(Math.Cos(rad) * range);
                int endY = origin.Y + (int)Math.Round(Math.Sin(rad) * range);
                var end = new Point(endX, endY);

                int dist = 0;
                foreach (var cell in BresenhamLine(origin, end))
                {
                    if (cell == origin) { continue; }
                    dist++;

                    var ct = chunks.GetCell(cell);

                    // stop si mur (occlusion)
                    if (ct == CellType.Wall)
                        break;

                    // on enregistre le PREMIER élément intéressant sur ce rayon
                    if (ct == CellType.FoodVegetable || ct == CellType.FoodMeat || ct == CellType.Danger)
                    {
                        hits.Add(new VisionHit
                        {
                            Cell = cell,
                            Type = ct,
                            Distance = dist,
                            AngleDeg = deg
                        });
                        break;
                    }

                    // fin de portée atteinte
                    if (dist >= range)
                        break;
                }
            }

            return hits;
        }

        // Convertit un angle en delta 8‑directions (−1/0/1) pour Move
        private static Point DirectionFromAngle(float deg)
        {
            float rad = MathHelper.ToRadians(deg);
            int dx = (int)Math.Round(Math.Cos(rad));
            int dy = (int)Math.Round(Math.Sin(rad));
            // Clamp sur [-1, 1]
            dx = Math.Max(-1, Math.Min(1, dx));
            dy = Math.Max(-1, Math.Min(1, dy));
            return new Point(dx, dy);
        }

        public enum ActionType { Move = 0, Eat = 1, Talk = 2, See = 3, Life = 4 }
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
            public int GeneIndex;
        }
        public static ActionProposal GetProposal(PCA pca, Gene g, int gene_id, ChunkManager chunks)
        {
            switch (g.Type)
            {
                case 0: return MoveProposal(pca, g, gene_id);
                case 1: return EatProposal(pca, g, gene_id, chunks);
                case 2: return SeeProposal(pca, g, gene_id, chunks);
                case 3: return TalkProposal(pca, g, gene_id);
                case 4: return LifeProposal(pca, g, gene_id);
            }

            return default;
        }
        public static void ExecuteProposal(PCA pca, ActionProposal p, ChunkManager chunks)
        {
            float reward = 0f;

            pca.CurrentAction = "Thinking...";
            switch ((ActionType)p.Type)
            {
                case ActionType.Move:
                    Logger.Write($"Action=Move Pos={pca.Position.X},{pca.Position.Y} GeneIndex={p.GeneIndex} Score={p.Score:F2}");
                    pca.CurrentAction = $"Move dir=({p.Target.X},{p.Target.Y})";

                    var oldPos = pca.Position;
                    var targetPos = oldPos + p.Target;

                    // --- COLLISION WITH WALL ---
                    var cell = chunks.GetCell(targetPos);
                    if (cell == CellType.Wall || !Game1.Instance._map.InBounds(targetPos))
                    {
                        // Collision detected → PCA does NOT move
                        Logger.Write("Collision: WALL");

                        // Encourage SEE genes a bit
                        if (p.GeneIndex >= 0 && p.GeneIndex < pca.Genome.Genes.Count)
                        {
                            for (int i = 0; i < pca.Genome.Genes.Count; i++)
                            {
                                if (pca.Genome.Genes[i].Type == (byte)ActionType.See) // See gene
                                {
                                    var g = pca.Genome.Genes[i];
                                    g.Weight = Math.Clamp(g.Weight + 0.05f, 0.1f, 5f);  // small boost
                                    pca.Genome.Genes[i] = g;
                                }
                            }
                        }

                        // Penalize useless bumping
                        reward = -0.3f;
                        pca.stagnationCounter++;
                        break;
                    }

                    // --- NO WALL → NORMAL MOVE ---
                    pca.Position = targetPos;
                    chunks.ClampPosition(ref pca.Position);

                    reward = ComputeMoveReward(pca, oldPos, chunks);

                    if (!pca.Visited.Contains(pca.Position))
                    {
                        pca.Visited.Add(pca.Position);
                        reward += 0.2f; // exploration reward
                    }

                    break;

                case ActionType.Eat: // Eat
                    Logger.Write("EAT at " + pca.Position);
                    pca.CurrentAction = "Eat";
                    pca.hunger = Math.Max(0, pca.hunger - 20);
                    chunks.SetCell(pca.Position, CellType.Floor);
                    reward = +5f; // gros bonus

                    AdjustGeneWeight(ref p.SourceGene, +1.0f);
                    break;

                case ActionType.Talk:
                    // Talk interdit si social trop bas
                    if (pca.social < 20)  // valeur ajustable
                    {
                        reward = -0.2f;   // punition pour Talk inutile
                        break;
                    }

                    Logger.Write("TALK symbol=" + pca.lastSymbol);
                    pca.CurrentAction = $"Talk sym={pca.lastSymbol}";
                    pca.lastSymbol = 1;
                    pca.talkTimer = 3;

                    pca.social = Math.Max(0, pca.social - 20);

                    reward = +0.1f;
                    break;

                case ActionType.See:
                    pca.See(p.SourceGene, chunks);
                    reward = +0.01f;
                    break;

                case ActionType.Life:
                    Logger.Write("LIFE");
                    pca.CurrentAction = "Life";
                    Behaviors.UpdateLifeSpan(pca, p.SourceGene);
                    reward = 0;
                    break;
            }

            // éventuellement : ajuster aussi le poids en fonction du reward global
            if (p.GeneIndex >= 0 && p.GeneIndex < pca.Genome.Genes.Count)
            {
                var g = pca.Genome.Genes[p.GeneIndex];   // copie locale
                AdjustGeneWeight(ref g, reward);         // modif sur la copie
                pca.Genome.Genes[p.GeneIndex] = g;       // réécriture du struct modifié
            }


            if (reward > 0)
                pca.stagnationCounter = 0;
            else
                pca.stagnationCounter += Helper.RandomRange(1, 5);

        }

        private static ActionProposal MoveProposal(PCA pca, Gene g, int gene_id)
        {
            Point dir = Behaviors.DirectionFromParam(g.ParamA);

            return new ActionProposal
            {
                Type = (byte)ActionType.Move,
                Target = dir,
                Score = 0.2f * g.Weight,
                SourceGene = g,
                GeneIndex = gene_id
            };
        }
        private static ActionProposal SeeProposal(PCA pca, Gene g, int gene_id, ChunkManager chunks)
        {
            int range = Math.Max(16, (int)g.ParamB);
            var hits = Scan360(pca.Position, range, chunks, stepDeg: 2);

            if (hits.Count == 0)
            {
                // Pas d'infos → See = observation pure → score faible
                return new ActionProposal
                {
                    Type = (byte)ActionType.See,
                    Target = Point.Zero,
                    Score = 0.2f * g.Weight,
                    SourceGene = g,
                    GeneIndex = gene_id
                };
            }

            // Priorités
            VisionHit? bestFood = hits
                .Where(h => (h.Type == CellType.FoodVegetable || h.Type == CellType.FoodMeat) && CanEat(pca, h.Type))
                .OrderBy(h => h.Distance)
                .FirstOrDefault();

            VisionHit? bestDanger = hits
                .Where(h => h.Type == CellType.Danger)
                .OrderBy(h => h.Distance)
                .FirstOrDefault();

            // Si nourriture visible → See puis Move vers elle
            if (bestFood != null)
            {
                var dir = DirectionFromAngle(bestFood.Value.AngleDeg);

                return new ActionProposal
                {
                    Type = (byte)ActionType.Move,
                    Target = dir,
                    Score = 0.8f * g.Weight + (10f / bestFood.Value.Distance) + (pca.hunger / 100f),
                    SourceGene = g,
                    GeneIndex = gene_id
                };
            }

            // Si danger visible → See puis Move (fuite)
            if (bestDanger != null)
            {
                var dir = DirectionFromAngle(bestDanger.Value.AngleDeg);
                dir = new Point(-dir.X, -dir.Y);

                return new ActionProposal
                {
                    Type = (byte)ActionType.Move,
                    Target = dir,
                    Score = 0.7f * g.Weight + (8f / bestDanger.Value.Distance),
                    SourceGene = g,
                    GeneIndex = gene_id
                };
            }

            // Sinon simple observation
            return new ActionProposal
            {
                Type = (byte)ActionType.See,
                Target = Point.Zero,
                Score = 0.3f * g.Weight,
                SourceGene = g,
                GeneIndex = gene_id
            };
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
        private static ActionProposal EatProposal(PCA pca, Gene g, int gene_id, ChunkManager chunks)
        {
            var cell = chunks.GetCell(pca.Position);

            if (!Behaviors.CanEat(pca, cell))
                return default;

            float baseScore = 0.5f;

            // faim augmente la priorité
            baseScore *= 1f + (pca.hunger / 50f);

            return new ActionProposal
            {
                Type = (byte)ActionType.Eat,
                Target = pca.Position,
                Score = baseScore * g.Weight,
                SourceGene = g,
                GeneIndex = gene_id
            };
        }
        public static ActionProposal InstinctEatProposal(PCA pca, ChunkManager chunks)
        {
            var cell = chunks.GetCell(pca.Position);

            if (cell == CellType.FoodVegetable || cell == CellType.FoodMeat)
            {
                return new ActionProposal
                {
                    Type = (byte)ActionType.Eat,
                    Target = pca.Position,
                    Score = 0.3f + (pca.hunger / 40f), // faim = priorité
                    SourceGene = default
                };
            }

            return default;
        }
        private static ActionProposal TalkProposal(PCA pca, Gene g, int gene_id)
        {
            // Ici on peut décider que Talk sert surtout à "signaler" un état
            // On lui met une priorité faible par défaut
            return new ActionProposal
            {
                Type = (byte)ActionType.Talk,              // 2 = Talk
                Target = pca.Position, // pas de déplacement
                Score = 0.2f * g.Weight,
                SourceGene = g,
                GeneIndex = gene_id
            };
        }
        private static ActionProposal LifeProposal(PCA pca, Gene g, int gene_id)
        {
            if (pca.lifeSpan < 1f)
            {
                return new ActionProposal
                {
                    Type = (byte)ActionType.Talk,              // Talk
                    Target = pca.Position,
                    Score = 0.5f * g.Weight,
                    SourceGene = g,
                    GeneIndex = gene_id
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
                pca.hunger = Math.Max(0, pca.hunger - (20 + g.ParamB));
                chunks.SetCell(pca.Position, CellType.Floor);
                return;
            }

            // --- Sinon → on bouge (gène Move cohérent) ---

            // 1) récupérer un gène Move existant
            var moveGene = pca.Genome.Genes
                .FirstOrDefault(x => x.Type == (byte)ActionType.Move);

            // 2) si absent → créer un gène Move de secours
            if (moveGene.Type != (byte)ActionType.Move)
            {
                moveGene = new Gene
                {
                    Type = (byte)ActionType.Move,
                    ParamA = Helper.RandomDir(), // direction aléatoire parmi 8
                    ParamB = 1,                  // vitesse de base
                    Weight = 1f
                };
            }

            // 3) exécuter le mouvement avec le gène trouvé / créé
            pca.Move(moveGene);
        }
        public static void See(this PCA pca, Gene g, ChunkManager chunks)
        {
            int range = Math.Max(16, (int)g.ParamB);
            var hits = Scan360(pca.Position, range, chunks, stepDeg: 2);

            foreach (var h in hits)
            {
                if (h.Type == CellType.Danger)
                    pca.stress += 0.01f;
                if (h.Type == CellType.FoodVegetable || h.Type == CellType.FoodMeat)
                    pca.stress = Math.Max(0, pca.stress - 0.005f);
            }
            pca.CurrentAction = $"See 360° hits={hits.Count}";
        }
        public static void Talk(this PCA pca, Gene g)
        {
            pca.lastSymbol = g.ParamA; // ex: 0=faim, 1=peur, 2=explore…
            pca.talkTimer = 3;        // affiché pendant 3 frames
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
