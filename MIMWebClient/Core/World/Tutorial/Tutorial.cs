using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Tutorial
    {

        public static void  setUpTut(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            Task.Run(() => Intro(player, room, step, calledBy));
        }

        public static async Task Intro(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {

            var npc = room.mobs.FirstOrDefault(x => x.Name.Equals("Wilhelm"));

            if (string.IsNullOrEmpty(step))
            {     

                HubContext.SendToClient(npc.Name + " says to you I don't think we have much further to go, " + player.Name, player.HubGuid);

                await Task.Delay(2000);

                HubContext.SendToClient("You hear a twig snap in the distance", player.HubGuid);

                await Task.Delay(2000);

                HubContext.SendToClient(npc.Name + " looks at you with a face of terror and dread", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient(npc.Name + " says to you did you hear that?" + player.Name, player.HubGuid);

                HubContext.SendToClient("<p class='RoomExits'>[Hint] Type say yes</p>", player.HubGuid);

                /*
                 *  add quest to player?
                 *  
                 *  show dialogue options
                 *  yes / no
                 *  regardless of what is picked proceed to nect step
                 *  if nothing is picked repeat
                 */



            }

            if (step.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
            {

                await Task.Delay(1500);

                HubContext.SendToClient("You look around but see nothing", player.HubGuid);

                await Task.Delay(1500);

                HubContext.SendToClient("Suddenly a Goblin yells AARGH-tttack!!", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient("You hear movement all around you", player.HubGuid);

                HubContext.SendToClient(npc.Name + " says to you here take this dagger" + player.Name, player.HubGuid);

                var weapon = npc.Inventory.FirstOrDefault(x => x.name.Contains("dagger"));

                if (weapon != null)
                {
                    player.Inventory.Add(weapon);
                }


                HubContext.SendToClient(npc.Name + " gives you a blunt dagger" + player.Name, player.HubGuid);

                await Task.Delay(1500);

                HubContext.SendToClient(npc.Name + " says to you it's nothing special but it will help you. I belive the way to Ester is all north from here." + player.Name, player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient("You hear movement getting closer", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient(npc.Name + " says to you you must get that letter to Cromwell." + player.Name, player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient("Suddenly 5 Goblins emerge from the bushes and fan out in a semi circle", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient(npc.Name + " yells GO, " + player.Name + " I'll hold them off. RUN! Run now to the North", player.HubGuid);

                while (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)) != null)
                {
                    await Task.Delay(30000);
                    HubContext.SendToClient(npc.Name + " yells GO, " + player.Name + " I'll hold them off. RUN! Run now to the North", player.HubGuid);

                    HubContext.SendToClient("<p class='RoomExits'>[Hint] Type north or n for short to move north away from the ambush</p>", player.HubGuid);
                }


            }

            if (step.Equals("Attack") && calledBy.Equals("mob"))
            {
                //blah blah
            }

            
        }
    }
}