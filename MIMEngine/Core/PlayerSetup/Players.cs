using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    public static class Players
    {
        private static ConcurrentDictionary<string, PlayerSetup> concurrentDictionary;

        public static ConcurrentDictionary<string, PlayerSetup> ListOfPlayers
        {
            get
            {
                if (concurrentDictionary == null)
                {
                    return concurrentDictionary = new ConcurrentDictionary<string, PlayerSetup>();
                }
                else
                {
                    return concurrentDictionary;
                }
            }
         
        }
        



        public static void addPlayer(string playerGuid, PlayerSetup playerData)
        {

            ListOfPlayers.TryAdd(playerGuid, playerData);
        }

        public static  void removePlayer(string playerGuid)
        {
            PlayerSetup playerData;

            ListOfPlayers.TryRemove(playerGuid, out playerData);
        }

        public static ConcurrentDictionary<string, PlayerSetup> returnPlayer()
        {
            return ListOfPlayers;
        }

        public static PlayerSetup returnPlayer(string playerGuid)
        {
            PlayerSetup playerData;

            if (ListOfPlayers.TryGetValue(playerGuid, out playerData))
            {
                return playerData;
            }
            else
            {
                throw new Exception("Player does not exist with Guid " + playerGuid);
            }
        }


    }
}
