using Microsoft.Xna.Framework;
using Project7.Source.Entities;

namespace Project7
{
    public partial class Game1 : Game
    {
        public EntityManager EntityManager;

        void LoadUpdate()
        {
            EntityManager = new EntityManager();

            for(int n=0;n<20; n++)
                EntityFactory.CreateRabbit();
        }
        void Update()
        {
            EntityManager.Update();
        }
    }
}