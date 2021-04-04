using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysicsDemos;
using BEPUutilities;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// The manager of our game
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// The current players
        /// </summary>
        public LinkedList<Player> Players { get; }
        /// <summary>
        /// The current Bepu space
        /// </summary>
        public Space Space { get; }
        /// <summary>
        /// The current game
        /// </summary>
        private MiniGolf _game;
        /// <summary>
        /// The current level
        /// </summary>
        public Level MainLevel { get; private set; }
        /// <summary>
        /// The current player
        /// </summary>
        public Player MainPlayer { get; private set; }
        /// <summary>
        /// The number of players
        /// </summary>
        private int _nbPlayer;
        /// <summary>
        /// The index of the current level
        /// </summary>
        public int LevelIndex { get; private set; } = 1;
        /// <summary>
        /// The state of the game if loading
        /// </summary>
        public bool Loading { get; private set; } = false;
        /// <summary>
        /// The state of the game if ended
        /// </summary>
        public bool Ended { get; private set; } = false;
        /// <summary>
        /// Number of levels in our game
        /// </summary>
        private int _numberOfLevels = 4;
        /// <summary>
        /// Number of hits
        /// </summary>
        public int NbHits { get; set; } = 0;

        /// <summary>
        /// Constructor of the manager
        /// </summary>
        /// <param name="game">The current game</param>
        public GameManager(MiniGolf game)
        {
            this._game = game;
            Players = new LinkedList<Player>();
            _nbPlayer = 0;
            
            //Creating and configuring space
            Space = new Space();
            Space.ForceUpdater.Gravity = new Vector3(-0, -9.81f, 0);
        }

        /// <summary>
        /// Adding a player to the list of player
        /// </summary>
        /// <param name="player">the player to add</param>
       public void AddPlayer(Player player)
       {
            Players.AddLast(player);
            _nbPlayer++;
       }

        /// <summary>
        /// Loading the game with the level and the ball
        /// </summary>
        /// <param name="levelIndex">Current level index</param>
       public void LoadGame(int levelIndex)
       {
           if (_nbPlayer != 0)
           {
               MainPlayer = Players.First.Value;
               MainLevel = new Level(_game, levelIndex);
               LoadLevel(MainLevel);
               LoadBall();
           }
       }

        /// <summary>
        /// Method for loading the ball
        /// </summary>
       private void LoadBall()
       {
            foreach(Player player in Players)
            {
                player.Ball.Load(Space, _game);
            }
       }

        /// <summary>
        /// Method for loading the level
        /// </summary>
        /// <param name="level">Level to load</param>
       private void LoadLevel(Level level)
       {
            level.Load(Space, _game);
       }
        /// <summary>
        /// Unloading the game
        /// </summary>
       public void UnloadGame()
       {
           _game.Components.Clear();
           foreach (Player player in Players)
           {
               player.ReloadBall(Space);
           }
       }
        /// <summary>
        /// Method in order to load the next level
        /// </summary>
        public void LoadNextLevel()
        {
            Loading = true;
            // Move to the next level
            LevelIndex += 1;
            MainPlayer.AddScore(NbHits);

            if (LevelIndex % _numberOfLevels == 0)
            {
                Ended = true;
                return;
            }

            // Unloads the content for the current level before loading the next one.
            UnloadGame();
            LoadGame(LevelIndex);


            NbHits = 0;
            Loading = false;
        }
    }
}
