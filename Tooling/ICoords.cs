using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public interface ICoords
    {
        float X { get; set; }
        float Y { get; set; }
    }

    internal class ICoordsInstance : ICoords
    {
        public float X { get; set; }
        public float Y { get; set; }
        public ICoordsInstance(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
    public static class ICoordsFactory
    {
        public static ICoords Create(float x, float y) => new ICoordsInstance(x, y);
    }
}
