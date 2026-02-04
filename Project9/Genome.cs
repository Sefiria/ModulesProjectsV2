using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    public struct Gene
    {
        public byte Type;     // 0=Move, 1=Eat, 2=See, 3=Talk, 4=LifeSpan
        public byte ParamA;   // dépend du type
        public byte ParamB;   // dépend du type
        public float Weight; // efficacité du gène
    }
    public struct EnvironmentFeedback
    {
        public float MutationRate;          // ex: augmente si stress
        public float AddGeneProbability;    // ex: mort par faim → +Eat
        public float RemoveGeneProbability; // ex: gènes inutiles
        public byte SuggestedGeneType;      // ex: danger → See
    }


    public class Genome
    {
        public List<Gene> Genes = new List<Gene>();

        private Random rng = new Random();

        public void Mutate(PCA pca, EnvironmentFeedback fb)
        {
            // micro-mutations
            foreach (ref var g in CollectionsMarshal.AsSpan(Genes))
            {
                if (rng.NextDouble() < fb.MutationRate)
                {
                    g.ParamA = (byte)(g.ParamA + rng.Next(-1, 2));
                    g.ParamB = (byte)(g.ParamB + rng.Next(-1, 2));
                }
                if (fb.MutationRate > 0.2f)
                {
                    // si faim → chance d’ajouter un gène Eat ou See
                    if (pca.hunger > 50 && Helper.RandomChance(0.3f))
                        AddGeneSeeOrEat();
                }

            }

            // ajout de gènes si pression environnementale
            if (rng.NextDouble() < fb.AddGeneProbability)
                Genes.Add(RandomGene(fb));

            // suppression si gène inutile
            if (Genes.Count > 3 && rng.NextDouble() < fb.RemoveGeneProbability)
                Genes.RemoveAt(rng.Next(Genes.Count));
        }
        private void AddGeneSeeOrEat()
        {
            if (Helper.RandomChance(0.5f))
                Genes.Add(new Gene { Type = 2, ParamA = Helper.RandomDir(), ParamB = 3, Weight = 1f }); // See
            else
                Genes.Add(new Gene { Type = 1, ParamA = 2, ParamB = 1, Weight = 1f }); // Eat
        }


        private Gene RandomGene(EnvironmentFeedback fb)
        {
            // biaisé par le feedback
            byte type = fb.SuggestedGeneType;
            return new Gene
            {
                Type = type,
                ParamA = (byte)rng.Next(0, 10),
                ParamB = (byte)rng.Next(0, 10)
            };
        }
    }
}