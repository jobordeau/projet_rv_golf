using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    public class GameManager
    {
        public LinkedList<Player> Players { get; }

        public GameManager(){Players = new LinkedList<Player>();}

        private void AddWin(Player player)
        {
            player.NbWin++;
        }

       public void AddPlayer(Player player)
       {
            Players.AddLast(player);
       }


        public void Reset(LinkedList<Player> players)
        {
            if (players == null)
            {
                throw new Exception("La liste des joueurs est vide");
            }

            foreach (var player in players)
            {
                player.Reset();
            }
        }

    }
}
