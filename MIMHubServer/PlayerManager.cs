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

        public static void updatePlayer(string id, Player playerDataUpdated)
        {
            Console.WriteLine("update palyer");

            Player existingPlayerData = null;
            _PlayerCache.TryGetValue(id, out existingPlayerData);

            if (existingPlayerData != null)
            {
                _PlayerCache.TryUpdate(id, playerDataUpdated, existingPlayerData);
            }
        }

        public static Player getPlayer(string id)
        {
            Player player = null;
            _PlayerCache.TryGetValue(id, out player);

            if (player != null)
            {
                return player;
            }
            throw new Exception("No such player exists");
        }

    }
}
