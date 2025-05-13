using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Project3.particles
{
    public class Particle : Entity
    {
        public int tile_index, size_x_in_pixels, size_y_in_pixels, scale, init_x, init_y, lifetime_in_ticks;
        public float vel_x, vel_y, vel_gravity, vel_mod_x, vel_mod_y;

        public float x, y;
        public Rectangle RndRect;

        public Particle(int tile_index, int size_x_in_pixels, int size_y_in_pixels, int scale, int lifetime_in_ticks, int init_x, int init_y, float vel_x, float vel_y, float vel_gravity = 0F, float vel_mod_x = 0F, float vel_mod_y = 0F)
        {
            this.tile_index = tile_index;
            this.size_x_in_pixels = Math.Min(Game1.tilesize, size_x_in_pixels);
            this.size_y_in_pixels = Math.Min(Game1.tilesize, size_y_in_pixels);
            this.scale = scale;
            this.lifetime_in_ticks = lifetime_in_ticks;
            x = this.init_x = init_x;
            y = this.init_y = init_y;
            this.vel_x = vel_x;
            this.vel_y = vel_y;
            this.vel_gravity = vel_gravity;
            this.vel_mod_x = vel_mod_x;
            this.vel_mod_y = vel_mod_y;

            RndRect = new Rectangle(
                tile_index * Game1.tilesize + RandomThings.arnd(0, Game1.tilesize - size_x_in_pixels),
                RandomThings.arnd(0, Game1.tilesize - size_y_in_pixels),
                size_x_in_pixels,
                size_y_in_pixels);

            ParticlesManager.AddParticle(this);
        }
        public void Update()
        {
            if (!Exists)
                return;

            x += vel_x;
            y += vel_y;
            vel_y += vel_gravity;
            vel_x += vel_mod_x;
            vel_y += vel_mod_y;
            lifetime_in_ticks--;
            if (lifetime_in_ticks == 0)
                Exists = false;
        }
        public void Draw()
        {
            if (!Exists)
                return;

            Color color = new Color(Color.White, Math.Min(byte.MaxValue, lifetime_in_ticks));
            Graphics.Graphics.Instance.DrawTexture(Game1.Instance.tex_tilemap, x, y, scale, color, RndRect);
        }
    }
}
