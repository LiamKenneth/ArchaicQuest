using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.World.Anker.Scripts
{
    public class VilliageIdiot
    {

        public static void Annoy(PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room)
        {
            HubContext.SendToClient("HI THERE!!", player.HubGuid);

            Follow.MobStalk(mob, player, room);
 
        }
    }
}