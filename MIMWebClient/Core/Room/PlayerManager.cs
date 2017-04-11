using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Room
{
    using MIMWebClient.Core.PlayerSetup;

    public static class PlayerManager
    {

        public static void AddPlayerToRoom(Room room, Player player)
        {
            var oldRoomData = room;
            if (room.players == null)
            {
                room.players = new List<Player>();
            }
            room.players.Add(player);

            var newRoomData = room;

            Cache.updateRoom(oldRoomData, newRoomData);
        }

        public static void RemovePlayerFromRoom(Room room, Player player)
        {
            var oldRoomData = room;
            var playerToRemove = room.players.Find(p => p.Name == player.Name);

            if (playerToRemove != null)
            {
                room.players.Remove(playerToRemove);
            }

            var newRoomData = room;

            Cache.updateRoom(oldRoomData, newRoomData);

        }

        public static void AddMobToRoom(Room room, Player mob)
        {
            var oldRoomData = room;
            if (room.mobs == null)
            {
                room.mobs = new List<Player>();
            }
            room.mobs.Add(mob);

            var newRoomData = room;

            Cache.updateRoom(oldRoomData, newRoomData);
        }

        public static void RemoveMobFromRoom(Room room, Player mob)
        {
            var oldRoomData = room;
            var playerToRemove = room.mobs.Find(p => p.Name == mob.Name);

            if (playerToRemove != null)
            {
                room.mobs.Remove(playerToRemove);
            }

            var newRoomData = room;

            Cache.updateRoom(oldRoomData, newRoomData);

        }




    }
}
