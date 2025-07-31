using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7.Source.Arcade
{
    public interface IScene
    {
        string Name { get; }
        void Initialize();
        void Update();
        void Draw(GraphicsDevice graphics);
        void Dispose();
    }
}
