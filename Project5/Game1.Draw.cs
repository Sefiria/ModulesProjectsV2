using Microsoft.Xna.Framework;
using System.Linq;
using Tooling;

namespace Project5
{
    public partial class Game1 : Game
    {
        private void InitDraw()
        {
        }

        private void DefineMap()
        {
        }

        private void draw(GameTime gameTime)
        {
            Chains.ForEach(c => c.Draw(gameTime));
        }
    }
}
