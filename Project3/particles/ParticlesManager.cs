using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.particles
{
    public class ParticlesManager
    {
        public static ParticlesManager Instance { get; private set; }

        public List<Particle> Particles;

        public ParticlesManager()
        {
            Instance = this;
        }

        public static void AddParticle(Particle particle)
        {
            Instance.Particles.Add(particle);
        }

        public void Initialize()
        {
            Particles = new List<Particle>();
        }

        public void Update()
        {
            var list = new List<Particle>(Particles);
            foreach(var particle in list)
            {
                particle.Update();
                if (!particle.Exists)
                    Particles.Remove(particle);
            }
        }

        public void Draw()
        {
            var list = new List<Particle>(Particles);
            foreach (var particle in list)
            {
                particle.Draw();
            }
        }
    }
}
