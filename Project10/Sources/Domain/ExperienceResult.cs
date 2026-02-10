using System;
using System.Collections.Generic;
using static Project10.Sources.Genesis.Genome;

namespace Project10.Sources.Domain
{
    public class ExperienceResult
    {
        public Dictionary<DNA_KEYS, double> Modifiers = new Dictionary<DNA_KEYS, double>();

        public ExperienceResult(bool collided)
        {
            //Modifiers[(int)DNA_KEYS.View] = collided ? -0.2 : +0.5;
        }
    }
}
