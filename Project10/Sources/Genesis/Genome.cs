using Project10.Sources.Domain;
using System;
using System.Collections.Generic;

namespace Project10.Sources.Genesis
{
    public class Genome
    {
        public enum DNA_KEYS { View = 0 }
        public Dictionary<DNA_KEYS, DNA_STRAND> DNA = new Dictionary<DNA_KEYS, DNA_STRAND>();
        public Genome() { }

        internal void Learn(ExperienceResult xp)
        {
            foreach (var mod in xp.Modifiers)
                if (DNA.ContainsKey(mod.Key))
                    DNA[mod.Key].Weight += mod.Value;
        }
    }
}
