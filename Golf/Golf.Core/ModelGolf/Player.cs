using System;
using System.Collections;
using System.Collections.Generic;
using BEPUphysics;
using Microsoft.Xna.Framework;
using Vector3 = BEPUutilities.Vector3;
namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// Class defining a player of the game
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The name of the player
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The number of hits by course
        /// </summary>
        public List<int> Score { get; set; }
        /// <summary>
        /// The ball of the player
        /// </summary>
        public Ball Ball { get; set; }
        /// <summary>
        /// Constructor of the player
        /// </summary>
        /// <param name="game">the current game</param>
        /// <param name="name">the name of the player</param>
        /// <param name="ball_model_name">the ball model name</param>
        /// <param name="position">the starting position of the player</param>
        public Player(Game game, string name, string ballModelName, Vector3 position )
        {
            Name = name;
            Score = new List<int>();
            Ball = new Ball(game, ballModelName, position);
        }
        /// <summary>
        /// Method to add a score to a player
        /// </summary>
        /// <param name="score">the score to add</param>
        public void AddScore(int score)
        {
            Score.Add(score);
        }

        /// <summary>
        /// Reloading the ball by creating a new one
        /// </summary>
        /// <param name="space">the current space</param>
        public void ReloadBall(Space space)
        {
            Ball.RemoveFromSpace(space);
            Ball = new Ball(Ball);
        }
        
    }
}
