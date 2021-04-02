using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysicsDemos;
using BEPUutilities;
using Golf.Core.ModelGolf.Cam;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Vector3 = BEPUutilities.Vector3;

namespace Golf.Core.ModelGolf
{
    public class GameManager
    {
        public LinkedList<Player> Players { get; }
        public List<Player> Level { get; }
        public Space Space { get; }
        private MiniGolf game;
        public Level MainLevel { get; private set; }
        public Player MainPlayer { get; private set; }
        private int nbPlayer;


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

       public void LoadGame()
        {
            if (nbPlayer != 0)
            {
                MainPlayer = Players.First.Value;
                MainLevel = new Level(game, "StageTest");
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

    }
}
