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
    public class GameManager
    {
        public LinkedList<Player> Players { get; }
        public List<Level> Levels { get; }
        public Space Space { get; }
        private MiniGolf game;
        public Level MainLevel { get; private set; }
        public Player MainPlayer { get; private set; }
        private int nbPlayer;
        public int LevelIndex { get; private set; } = 1;
        public bool Loading { get; private set; } = false;
        public bool Ended { get; private set; } = false;
        private int numberOfLevels = 4;
        public int NbHits { get; set; } = 0;


        public GameManager(MiniGolf game)
        {
            this.game = game;
            Players = new LinkedList<Player>();
            nbPlayer = 0;
            

            //Creating and configuring space
            Space = new Space();
            Space.ForceUpdater.Gravity = new Vector3(-0, -9.81f, 0);

        }

       public void AddPlayer(Player player)
       {
            Players.AddLast(player);
            nbPlayer++;
       }

       public void LoadGame(int levelIndex)
       {
           if (nbPlayer != 0)
           {
               MainPlayer = Players.First.Value;
               MainLevel = new Level(game, levelIndex);
               LoadLevel(MainLevel);
               LoadBall();
           }
       }

       private void LoadBall()
       {
            foreach(Player player in Players)
            {
                player.Ball.Load(Space, game);
            }
       }

       private void LoadLevel(Level level)
       {
            level.Load(Space, game);
       }

       public void UnloadGame()
       {
           game.Components.Clear();
           foreach (Player player in Players)
           {
               player.ReloadBall(Space);
           }
       }

        public void LoadNextLevel()
        {
            Loading = true;
            // move to the next level
            LevelIndex += 1;
            MainPlayer.AddScore(NbHits);

            if (LevelIndex % numberOfLevels == 0)
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
