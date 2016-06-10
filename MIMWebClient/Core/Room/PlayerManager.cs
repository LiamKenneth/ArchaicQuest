using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Room
{
    using MIMWebClient.Core.PlayerSetup;

    public static class PlayerManager
    {

       public static void AddPlayerToRoom(Room room, Player player)
       {
           room.players.Add(player);
       }

        public static void RemovePlayerFromRoom(Room room, Player player)
        {
            var playerToRemove = room.players.Find(p => p.Name == player.Name);

            if (playerToRemove != null)
            {
                room.players.Remove(playerToRemove);
            }
            
        }

    }
}
