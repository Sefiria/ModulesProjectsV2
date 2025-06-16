using Microsoft.Xna.Framework;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using Tooling;

namespace Project6
{
    public partial class Game1 : Game
    {
        MongoService mongodb;

        private void InitUpdate()
        {
            mongodb = new MongoService();

            KB.OnKeyPressed += KB_OnKeyPressed;
            MS.OnButtonPressed += MS_OnButtonPressed;
            MS.OnButtonDown+= MS_OnButtonDown;
        }
        private void KB_OnKeyPressed(char key)
        {
        }

        private void MS_OnButtonPressed(Tools.Inputs.MS.MouseButtons button)
        {
        }

        private void MS_OnButtonDown(Tools.Inputs.MS.MouseButtons button)
        {
        }

        private void update(GameTime gameTime)
        {
            var chunk = Chunk.CreateDefault(0, 0);
            chunk.Save(mongodb);

            chunk = mongodb.LoadChunk(0, 0);

            chunk[1, 0].Type = "Dirt";
            chunk.Save(mongodb);

            chunk = mongodb.LoadChunk(0, 0);
        }
    }
}
