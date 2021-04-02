using System;
using System.Collections;
using System.Collections.Generic;
using BEPUphysics;
using Microsoft.Xna.Framework;
using Vector3 = BEPUutilities.Vector3;
namespace Golf.Core.ModelGolf
{
    public class Player
    {
        public string Name { get; set; }
        public List<int> Score { get; set; }
        public Ball Ball { get; set; }
        public Player(IServiceProvider isServiceProvider,MiniGolf game, string name, string ball_model_name, Vector3 position )
        {
            Name = name;
            Score = new List<int>();
            Ball = new Ball(game, ball_model_name, position);
        }

        public void AjouterScore(int score)
        {
            Score.Add(score);
        }

        public void ReloadBall(Space space)
        {
            Ball.RemoveFromSpace(space);
            Ball = new Ball(Ball);

        }
        
    }
}
