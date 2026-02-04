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
        public float stress = 0f;

        public float lifeSpan = 5f; // minutes
        public byte lastSymbol = 255; // 255 = rien
        public int talkTimer = 0;
        public HashSet<Point> Visited = new();
        public int stagnationCounter = 0;

        public PCA()
        {
            Respawn();
        }

        public void Update(GameTime time, ChunkManager chunks)
        {
            List<ActionProposal> proposals = new();

            // 1) Chaque gène propose une action
            foreach (var g in Genome.Genes)
            {
                var p = Behaviors.GetProposal(this, g, chunks);
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
                if (p.Type == 1 && hunger > 60)
                    p.Score *= 2f;
                // faim critique → Eat écrase tout
                if (p.Type == 1 && hunger > 80)
                    p.Score *= 4f;
                proposals[i] = p; // réassignation obligatoire
            }

            if (hunger > 30)
            {
                var instinct = InstinctEatProposal(this, chunks);
                if (instinct.Score > 0)
                    proposals.Add(instinct);
            }

            // 2) Choisir la meilleure action
            if (proposals.Count > 0)
            {
                var best = proposals.OrderByDescending(p => p.Score).First();
                Behaviors.ExecuteProposal(this, best, chunks);
            }

            // 3) Feedback + mutation
            var fb = ComputeFeedback(chunks);
            if (fb.MutationRate > 0.1f)
                Genome.Mutate(this, fb);

            if (talkTimer > 0)
                talkTimer--;

            if (stagnationCounter > 20)
            {
                MutateHard(); // mutation plus forte
                stagnationCounter = 0;
            }
        }
        public void MutateHard()
        {
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
                if (Helper.RandomChance(0.8))
                    AddGeneEat(); // priorité
                if (Helper.RandomChance(0.6))
                    AddGeneSee();
            }
            else
            {
                // pas trop faim → on diversifie surtout le mouvement / exploration
                if (Helper.RandomChance(0.5))
                {
                    Genome.Genes.Add(new Gene
                    {
                        Type = 0, // Move
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
                    Type = 3, // Talk
                    ParamA = (byte)Helper.Rng.Next(0, 4),
                    ParamB = 0,
                    Weight = 0.8f
                });
            }
        }
        private void AddGeneEat()
        {
            // omnivore par défaut, simple et efficace
            Genome.Genes.Add(new Gene
            {
                Type = 1,      // Eat
                ParamA = 2,    // 0 = végétal, 1 = viande, 2 = omnivore
                ParamB = 1,
                Weight = 1.3f
            });
        }
        private void AddGeneSee()
        {
            Genome.Genes.Add(new Gene
            {
                Type = 2,                          // See
                ParamA = Helper.RandomDir(),       // direction
                ParamB = (byte)Helper.Rng.Next(2, 6), // distance de vue
                Weight = 1.2f
            });
        }

        public EnvironmentFeedback ComputeFeedback(ChunkManager chunks)
        {
            // faim augmente naturellement
            hunger += 0.5f;

            // faim élevée = stress
            if (hunger > 50)
                stress += 0.05f;

            // faim critique = mutation forcée
            if (hunger > 80)
                stress += 0.1f;

            // Exemple simple : danger proche → stress
            bool danger = chunks.IsDangerNear(Position);
            if (danger) stress += 0.05f;
            else stress *= 0.95f;

            return new EnvironmentFeedback
            {
                MutationRate = Math.Clamp(stress, 0f, 0.3f),
                AddGeneProbability = danger ? 0.1f : 0.02f,
                RemoveGeneProbability = 0.01f,
                SuggestedGeneType = danger ? (byte)2 : (byte)0 // danger → See
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
                Type = 0,
                ParamA = (byte)Rng.Next(0, 8),
                ParamB = 1,
                Weight = 1f
            });
        }

    }
}
