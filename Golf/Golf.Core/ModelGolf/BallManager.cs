using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    
    public class BallManager
    {
        private List<Ball> Balls { get; set; }
        public BallManager(LinkedList<Player> listplayer)
        {
            Balls = new List<Ball>();
            foreach(var player in listplayer)
            {
                Balls.Add(player.Ball);
            }
        }

        public void HandleBallsCollision(Ball currentBall)
        {
            foreach (var ball in Balls)
            {
                if(currentBall != ball)
                {
                    currentBall.HandleBallCollision(ball);
                }
            }
        }


        public void Draw(GameTime gameTime)
        {
            foreach(var ball in Balls)
            {
                ball.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(var ball in Balls)
            {
                ball.Update(gameTime);
            }
        }
    }
}
