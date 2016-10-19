using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Events
{
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Emote
    {

        public static void EmoteActionToRoom(string message, Player player)
        {
            var players = Cache.ReturnPlayers().Where(x => x.AreaId.Equals(player.AreaId) && x.Area.Equals(player.Area) && x.Region.Equals(player.Region));

            foreach (var pc in players)
            {
                HubContext.SendToClient(player.Name + " " + message, pc.HubGuid);
            }

        }

        public static void EmoteLaugh(string message, Player player, Room room)
        {
            //add standard emotes
            // malleus laughs
            // malleus laughs at you / malleus laughs at bob 
        }
    }
}
