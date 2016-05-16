using MIMEngine.Core.PlayerSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMHubServer
{
    class PlayerManager : MimHubServer
    {

        public static void updatePlayer(string id, PlayerSetup playerDataUpdated)
        {
            Console.WriteLine("update palyer");

            PlayerSetup existingPlayerData = null;
            _PlayerCache.TryGetValue(id, out existingPlayerData);

            if (existingPlayerData != null)
            {
                _PlayerCache.TryUpdate(id, playerDataUpdated, existingPlayerData);
            }
        }

        public static PlayerSetup getPlayer(string id)
        {
            PlayerSetup player = null;
            _PlayerCache.TryGetValue(id, out player);

            if (player != null)
            {
                return player;
            }
            throw new Exception("No such player exists");
        }

    }
}
