using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.Common;
using System;
using static Project7.Source.Arcade.scenes.space.Enums;
using System.Collections.Generic;

namespace Project7.Source.Arcade.scenes.space
{
    public class BaseShip : EntitySpace
    {
        public Dictionary<PowerUps, int> PowerUps = new Dictionary<PowerUps, int>();

        public BaseShip() : base()
        {
        }
        public virtual void AddPowerUp(Enums.PowerUps kind)
        {
            if (PowerUps.ContainsKey(kind))
                PowerUps[kind]++;
            else
                PowerUps[kind] = 1;
        }
    }
}
