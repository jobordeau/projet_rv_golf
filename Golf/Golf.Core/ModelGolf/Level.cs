using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    public class Level
    {

        public Model ModelLevel { get; }
        public Model ModelFinish { get; }

        public Level(Game game, string modelName)
        {

            ModelLevel = game.Content.Load<Model>(modelName);

        }

    }
}
