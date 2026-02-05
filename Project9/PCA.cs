using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project9.Behaviors;

namespace Project9
{
    public class PCA
    {
        public Point Position;

        public Genome Genome = new Genome();

        public float hunger = 0f;
        public float social = 0f;
        public float stress = 0f;
        public int seeLock = 0;

        public float lifeSpan = 5f; // minutes
        public byte lastSymbol = 255; // 255 = rien
        public int talkTimer = 0;
        public HashSet<Point> Visited = new();
        public int stagnationCounter = 0;
        public string CurrentAction = "None";

        public PCA()
        {
            Respawn();
        }

        public void Update(GameTime time, ChunkManager chunks)
        {
            List<ActionProposal> proposals = new();

            // 1) Chaque gène propose une action
            for(int i=0; i<Genome.Genes.Count; i++)
            {
                var p = Behaviors.GetProposal(this, Genome.Genes[i], i, chunks);
                if (p.Score > 0)
                    proposals.Add(p);
            }

            // 1.5) Modulation des scores par la faim
            for (int i = 0; i < proposals.Count; i++)
            {
                var p = proposals[i];
                // faim faible → petit boost
                if (hunger > 20)
                    p.Score *= 1f + (hunger / 100f);
                // faim forte → Eat devient prioritaire
                if (p.Type == (byte)ActionType.Eat && hunger > 60)
                    p.Score *= 2f;
                // faim critique → Eat écrase tout
                if (p.Type == (byte)ActionType.Eat && hunger > 80)
                    p.Score *= 1.5f;
                proposals[i] = p; // réassignation obligatoire
            }

            if (hunger > 30)
            {
                var instinct = InstinctEatProposal(this, chunks);
                if (instinct.Score > 0)
                    proposals.Add(instinct);
            }
            for (int i = 0; i < proposals.Count; i++)
            {
                var p = proposals[i];

                // SOCIAL → boost Talk
                if (p.Type == (byte)ActionType.Talk)
                    p.Score += social / 200f;

                proposals[i] = p;
            }

            bool hasFoodUnder = (chunks.GetCell(Position) == CellType.FoodVegetable
                     || chunks.GetCell(Position) == CellType.FoodMeat);
            if (!hasFoodUnder)
            {
                // augmenter Move et See un peu
                for (int i = 0; i < proposals.Count; i++)
                {
                    if (proposals[i].Type == 0 || proposals[i].Type == 2)
                    {
                        var p = proposals[i];
                        p.Score *= 1.5f;
                        proposals[i] = p;
                    }
                }
            }

            var best = proposals.OrderByDescending(p => p.Score).First();

            // 2) Choisir la meilleure action
            if (proposals.Count > 0)
            {
                Logger.Write($"Chosen action: {best.Type} Score={best.Score:F2} FromGene={best.GeneIndex}");
                Behaviors.ExecuteProposal(this, best, chunks);
            }

            if (best.Type == (byte)ActionType.See)
                seeLock++;
            else
                seeLock = 0;
            if (seeLock > 3)  // après 3 frames bloqué
                best.Score = 0.0f; // interdit See temporairement

            // 3) Feedback + mutation
            var fb = ComputeFeedback(chunks);

            if (talkTimer > 0)
                talkTimer--;

            foreach (var g in Genome.Genes)
                if (g.Type == (byte)ActionType.Life)
                    Behaviors.UpdateLifeSpan(this, g);

            if (fb.MutationRate > 0.1f)
                Genome.Mutate(this, fb);

            if (stagnationCounter > 20)
            {
                MutateHard(); // mutation plus forte
                stagnationCounter = 0;
            }

            // -----------------------------
            // GENE CLEANUP (APPLY AT END)
            // -----------------------------

            // ---- Limit ANY gene type to max 5 ----
            const int MAX_PER_TYPE = 5;

            Genome.Genes = Genome.Genes
                .GroupBy(g => g.Type)                     // groupe par Type
                .SelectMany(grp =>
                    grp.OrderByDescending(g => g.Weight)  // garde les meilleurs d’abord
                       .Take(MAX_PER_TYPE))               // max 5 par type
                .ToList();

            // ---- Limit TOTAL to 50 (best 50 retained) ----
            if (Genome.Genes.Count > 50)
            {
                Genome.Genes = Genome.Genes
                    .OrderByDescending(g => g.Weight)
                    .Take(50)
                    .ToList();
            }
        }
        public void MutateHard()
        {
            if (Genome.Genes.Count > 40)
            {
                // au lieu d’ajouter → on renforce les existants
                for (int i = 0; i < Genome.Genes.Count; i++)
                {
                    var g = Genome.Genes[i];
                    g.Weight = Math.Clamp(g.Weight + 0.02f, 0.1f, 5f);
                    Genome.Genes[i] = g;
                }
                return;
            }

            // 1) Légère chance de purge partielle si le génome est trop gros
            if (Genome.Genes.Count > 12 && Helper.RandomChance(0.3))
            {
                Genome.Genes = Genome.Genes
                    .OrderByDescending(g => g.Weight)   // on garde les meilleurs
                    .Take(8)
                    .ToList();
            }

            // 2) Booster / secouer les poids existants
            for (int i = 0; i < Genome.Genes.Count; i++)
            {
                var g = Genome.Genes[i];   // copie locale

                // petit bruit aléatoire
                float delta = (float)(Helper.Rng.NextDouble() * 0.6 - 0.3); // [-0.3 ; +0.3]
                g.Weight = Math.Clamp(g.Weight + delta, 0.1f, 5f);

                // si gène très mauvais → petite chance de le re-rouler
                if (g.Weight < 0.3f && Helper.RandomChance(0.4))
                {
                    g.ParamA = Helper.RandomDir();
                    g.ParamB = (byte)Helper.Rng.Next(1, 5);
                    g.Weight = 1f;
                }

                Genome.Genes[i] = g;       // réassignation
            }

            // 3) Ajouter des gènes utiles selon l’état du PCA
            // Faim → priorité à Eat / See
            if (hunger > 50)
            {
                if (Helper.RandomChance(0.3))
                    AddGeneEat(); // priorité
            }
            else
            {
                // pas trop faim → on diversifie surtout le mouvement / exploration
                if (Helper.RandomChance(0.5))
                {
                    Genome.Genes.Add(new Gene
                    {
                        Type = (byte)ActionType.Move, // Move
                        ParamA = Helper.RandomDir(),
                        ParamB = 1,
                        Weight = 1f
                    });
                }
            }

            // 4) Petite chance d’ajouter un Talk ou un Life pour la variété
            if (Helper.RandomChance(0.2))
            {
                Genome.Genes.Add(new Gene
                {
                    Type = (byte)ActionType.Talk, // Talk
                    ParamA = (byte)Helper.Rng.Next(0, 4),
                    ParamB = 0,
                    Weight = 0.8f
                });
            }

            if (Genome.Genes.Count > 40)
                Genome.Genes = Genome.Genes
                    .OrderByDescending(g => g.Weight)
                    .Take(30)
                    .ToList();

            // Garder au moins 1 Move
            if (!Genome.Genes.Any(g => g.Type == 0))
                Genome.Genes.Add(new Gene { Type = 0, ParamA = Helper.RandomDir(), ParamB = 1, Weight = 1f });

            // Garder au moins 1 See
            if (!Genome.Genes.Any(g => g.Type == 2))
                Genome.Genes.Add(new Gene { Type = 2, ParamA = Helper.RandomDir(), ParamB = 3, Weight = 1f });
        }
        private void AddGeneEat()
        {
            // omnivore par défaut, simple et efficace
            Genome.Genes.Add(new Gene
            {
                Type = (byte)ActionType.Eat,      // Eat
                ParamA = 2,    // 0 = végétal, 1 = viande, 2 = omnivore
                ParamB = 1,
                Weight = 1.3f
            });
        }
        private void AddGeneSee()
        {
            Genome.Genes.Add(new Gene
            {
                Type = (byte)ActionType.See,                          // See
                ParamA = Helper.RandomDir(),       // direction
                ParamB = (byte)Helper.Rng.Next(2, 6), // distance de vue
                Weight = 1.2f
            });
        }

        public EnvironmentFeedback ComputeFeedback(ChunkManager chunks)
        {
            // faim augmente naturellement
            hunger += 0.5f;
            social += 0.05f;
            social = Math.Clamp(social, 0, 100);

            // faim élevée = stress
            if (hunger > 50)
                stress += 0.1f;

            // faim critique = mutation forcée
            if (hunger > 80)
                stress += 0.5f;

            // Exemple simple : danger proche → stress
            bool danger = chunks.IsDangerNear(Position);
            if (danger) stress += 0.5f;
            else stress *= 0.95f;

            return new EnvironmentFeedback
            {
                MutationRate = Math.Clamp(stress, 0f, 0.3f),
                AddGeneProbability = danger ? 0.1f : 0.02f,
                RemoveGeneProbability = 0.01f,
                SuggestedGeneType = danger ? (byte)ActionType.See : (byte)ActionType.Move // danger → See
            };
        }

        private static readonly Random Rng = new Random();
        public void Respawn()
        {
            Position = new Point(MAP.Width / 2, MAP.Height / 2);
            hunger = 0;
            stress = 0;
            lifeSpan = 5f;

            Genome.Genes.Clear();

            Genome.Genes.Add(new Gene
            {
                Type = (byte)ActionType.Move,
                ParamA = (byte)Rng.Next(0, 8),
                ParamB = 1,
                Weight = 1f
            });
        }

    }
}
