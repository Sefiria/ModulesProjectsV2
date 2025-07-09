using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7.Source.Arcade.Common
{
    public class EntityManager
    {
        public List<Entity> Entities;
        public EntityManager()
        {
            Entities = new List<Entity>();
        }
        public void Update()
        {
            var list = new List<Entity>(Entities);
            foreach (var entity in list)
            {
                if (entity.Exists)
                    entity.Update();
                else
                    Entities.Remove(entity);
            }
        }
        public void Draw(GraphicsDevice graphics)
        {
            var list = new List<Entity>(Entities);
            foreach (var entity in list)
                if (entity.Exists)
                    entity.Draw(graphics);
        }
    }
}
