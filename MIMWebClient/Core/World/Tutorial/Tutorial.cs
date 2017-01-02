using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Tutorial
    {
        public static async Task Intro(PlayerSetup.Player player, Room.Room room)
        {
            var npc = room.mobs.FirstOrDefault(x => x.Name.Equals("Wilhelm"));

            HubContext.SendToClient(npc.Name + " says to you I don't think we have much further to go, " + player.Name, player.HubGuid);

            await Task.Delay(2000);

            HubContext.SendToClient("You hear a twig snap in the distance", player.HubGuid);

            await Task.Delay(2000);

            HubContext.SendToClient(npc.Name + " looks at you with a face of terror and dread", player.HubGuid);

            await Task.Delay(3000);

            HubContext.SendToClient(npc.Name + " says to you did you hear that?" + player.Name, player.HubGuid);

            /*
             *  add quest to player?
             *  
             *  show dialogue options
             *  yes / no
             *  regardless of what is picked proceed to nect step
             *  if nothing is picked repeat
             */
           

            
        }
    }
}