using Microsoft.Xna.Framework;
using Project10.Helpers;
using Project10.Sources.Domain;
using Project10.Sources.Genesis;
using static Project10.Sources.Genesis.Genome;

namespace Project10.Sources.Entities
{
    public class PCA : SmartEntity
    {
        public PCA() : base()
        {
            Position = new Vector2(MAP.Width * Game1.Instance._cellSize / 2, MAP.Height * Game1.Instance._cellSize / 2);
            Radius = Game1.Instance._cellSize / 2;
            Look = new Vector2(0F, 0F);

            Genome.DNA[DNA_KEYS.View] = new DNA_STRAND { Weight = 1D };
        }

        public void Update(GameTime gameTime)
        {
            var ms = Game1.MS.Position.ToVector2();
            Vector2? targetDir = Game1.MS.IsLeftDown ? ms - RenderPosition : null;
            ExperienceResult xp;

            xp = this.Move(targetDir);
            Genome.Learn(xp);
        }


        public void SpriteDraw(GameTime gameTime)
        {
            DrawHelper.DrawCircleLook(RenderPosition, Look, Radius, 4F, 2F, Color.Cyan);
        }
        public void ShaderDraw(GameTime gameTime)
        {
            DrawHelper.DrawCircle(Game1.Instance.basicEffect, RenderPosition, Radius, 8, Color.Cyan);
        }
    }
}
