using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8.Source.Entities
{
    public class Collectible : Entity
    {
        public Collectible(string name = null) : base(name)
        {
        }

        public void Collected()
        {
            Exists = false;
        }
    }
}
