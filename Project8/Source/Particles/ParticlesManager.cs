using Microsoft.Xna.Framework.Graphics;
using Project8.Source.Entities;
using System.Collections.Generic;

namespace Project8.Source.Particles
{
    public class ParticleManager
    {
        public List<Particle> Particles;
        public ParticleManager()
        {
            Particles = new List<Particle>();
        }
        public void Update()
        {
            var list = new List<Particle>(Particles);
            foreach (var particle in list)
            {
                if (particle.Exists)
                    particle.Update();
                else
                    Particles.Remove(particle);
            }
        }
        public void Draw(GraphicsDevice graphics)
        {
            var list = new List<Particle>(Particles);
            foreach (var particle in list)
                if (particle.Exists)
                    particle.Draw(graphics);
        }
    }
}
