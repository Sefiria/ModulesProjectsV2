using System.Collections.Generic;
using System.Linq;

namespace Tooling
{
    public class DynamicArray<T> where T : struct
    {
        public Dictionary<int[], T?> Data;
        public int Dimensions { get; private set; }

        public T? this[params int[] coordinates]
        {
            get
            {
                if (Dimensions != coordinates.Length) return null;
                return Data.FirstOrDefault(d => d.Key == coordinates).Value;
            }
            set
            {
                if (Dimensions != coordinates.Length) return;
                Data[coordinates] = value;
            }
        }

        public DynamicArray(int dimensions)
        {
            Dimensions = dimensions;
        }
    }
}
