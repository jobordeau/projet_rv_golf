using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    public class Player
    {
        public string Name { get; set; }
        public int NbWin { get; set; }
        public int NbHit { get; set; }

        public Ball Ball { get; set; }
        public Player(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphics,  string name)
        {
            Name = name;
            NbWin = 0;
            NbHit = 0;
            Ball = new Ball(game, spriteBatch, graphics, new Vector3(10,10,10));
        }

        public void HitBall(Vector3 hitVelocity)
        {
            Ball.Velocity = hitVelocity;
            NbHit++;
        }

        public void Reset()
        {
            NbHit = 0;
        }

        
    }
}
