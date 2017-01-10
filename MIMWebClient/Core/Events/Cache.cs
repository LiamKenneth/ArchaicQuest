using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient;

namespace MIMWebClient.Core.Events
{

    using Room;
    using PlayerSetup;
    using System.Collections.Concurrent;
    using MIMWebClient.Hubs;
    public class Cache
    {
 
        /// <summary>
        /// Gets room cache
        /// </summary>
        /// <returns>returns room Cache</returns>
        private static ConcurrentDictionary<Tuple<string,string,int>, Room> getRoomCache()
        {
            return MIMHub._AreaCache;
        }

        /// <summary>
        /// Gets player cache
        /// </summary>
        /// <returns>returns player Cache</returns>
        private static ConcurrentDictionary<string, Player> getPlayerCache()
        {
            return MIMHub._PlayerCache;
        }

        /// <summary>
        /// Gets Mob cache
        /// </summary>
        /// <returns>returns Mob Cache</returns>
        private static ConcurrentDictionary<string, Player> getMobCache()
        {
            return MIMHub._ActiveMobCache;
        }



        /// <summary>
        /// Update room cache
        /// </summary>
        /// <param name="newRoom">The new room data</param>
        /// <param name="oldRoom">The old room data</param>
        public static bool updateRoom(Room newRoom, Room oldRoom)
        {

            var updateOldRoom = new Tuple<string, string, int>(oldRoom.region, oldRoom.area, oldRoom.areaId);

            if (MIMHub._AreaCache.TryUpdate(updateOldRoom, newRoom, oldRoom))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the room which the player is in
        /// </summary>
        /// <param name="player">Player that is in the room you wish to get</param>
        /// <returns></returns>
        public static Room getRoom(Player player)
        {
            Room roomData;

            var getRoom = new Tuple<string, string, int>(player.Region, player.Area, player.AreaId);

            MIMHub._AreaCache.TryGetValue(getRoom, out roomData);

            return roomData;
        }

        /// <summary>
        /// Updates player in the cache
        /// </summary>
        /// <param name="newPlayer">The new player data</param>
        /// <param name="oldPlayer">The old player data</param>
        public static bool updatePlayer(Player newPlayer, Player oldPlayer)
        {

            if (MIMHub._PlayerCache.TryUpdate(oldPlayer.HubGuid, newPlayer, oldPlayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns player from cache
        /// </summary>
        /// <param name="playerId">Player Hub ID to get from cache</param>
        /// <returns>PlayerData</returns>
        public static Player getPlayer(string playerId)
        {
            Player playerData;
            MIMHub._PlayerCache.TryGetValue(playerId, out playerData);

            return playerData;
        }

        public static List<Player> ReturnPlayers()
        {
           var players = MIMHub._PlayerCache.Values.ToList();

            return players;
        }

        public static List<Player> ReturnMobs()
        {
            var mobs = MIMHub._PlayerCache.Values.ToList();

            return mobs;
        }

        public static List<Room> ReturnRooms()
        {
            var rooms = MIMHub._AreaCache.Values.ToList();

            return rooms;
        }

    }
}
