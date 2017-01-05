using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Room;
using Cache = MIMWebClient.Core.Events.Cache;

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

                HubContext.SendToClient(npc.Name + " says to you did you hear that? " + player.Name, player.HubGuid);

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

                await Task.Delay(1500);

                HubContext.SendToClient(npc.Name + " says to you here take this dagger " + player.Name, player.HubGuid);

                var weapon = npc.Inventory.FirstOrDefault(x => x.name.Contains("dagger"));

                if (weapon != null)
                {
                    player.Inventory.Add(weapon);
                }


                HubContext.SendToClient(npc.Name + " gives you a blunt dagger", player.HubGuid);

                Score.UpdateUiInventory(player);

                await Task.Delay(1500);

                HubContext.SendToClient(npc.Name + " says to you it's nothing special but it will help you. I belive the way to Ester is all north from here.", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient("You hear movement getting closer", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient(npc.Name + " says you must get that letter to Cromwell.", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient("Suddenly 5 Goblins emerge from the bushes and fan out in a semi circle", player.HubGuid);

                await Task.Delay(3000);

                HubContext.SendToClient(npc.Name + " yells GO, " + player.Name + " I'll hold them off. RUN! Run now to the North", player.HubGuid);

                while (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)) != null)
                {
                    await Task.Delay(30000);

                    if (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)) != null)
                    {
                        HubContext.SendToClient(npc.Name + " yells GO, " + player.Name + " I'll hold them off. RUN! Run now to the North", player.HubGuid);

                        HubContext.SendToClient("<p class='RoomExits'>[Hint] Type north or n for short to move north away from the ambush</p>", player.HubGuid);
                    }
                  
                }


            }

            if (step.Equals("Attack") && calledBy.Equals("mob"))
            {
                //blah blah
            }

            
        }

        public static void setUpRescue(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            Task.Run(() => AwakeningRescue(player, room, step, calledBy));
        }

        public static void setUpAwakening(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
         Task.Run(() => Awakening(player, room, step, calledBy));
        }

        public static void AwakeningRescue(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {

            var npc = room.mobs.FirstOrDefault(x => x.Name.Equals("Mortem"));

            if (npc != null)
            {
                HubContext.SendToClient(npc.Name + " says AH you are awake!", player.HubGuid);

                HubContext.SendToClient(npc.Name + " says You were in a bad way when we found you, I didn't think you would wake.", player.HubGuid);

                HubContext.SendToClient(npc.Name + " says do you remember anything?", player.HubGuid);
            }
        }

        public static async Task Awakening(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            player.Status = PlayerSetup.Player.PlayerStatus.Sleeping;

            

            if (string.IsNullOrEmpty(step))
            {

                player.Area = "Tutorial";
                player.Region = "Tutorial";
                player.AreaId = 3;

                var exit = new Exit
                {
                    area = player.Area,
                    region = player.Region,
                    areaId = player.AreaId
                };


                var templeRoom =
                    Cache.ReturnRooms()
                        .FirstOrDefault(
                            x =>
                                x.area.Equals(player.Area) && x.areaId.Equals(player.AreaId) &&
                                x.region.Equals(player.Region));

                if (templeRoom != null)
                {
                    Movement.Teleport(player, templeRoom, exit);

                }
                else
                {

                    var loadRoom = new LoadRoom
                    {
                        Area = player.Area,
                        id = player.AreaId,
                        Region = player.Region
                    };


                    var newRoom = loadRoom.LoadRoomFile();

                    Movement.Teleport(player, newRoom, exit);
                    //load from DB
                }

                await Task.Delay(3000);

                HubContext.SendToClient("You feel better as a wave of warmth surrounds your body", player.HubGuid);

                await Task.Delay(2000);

                HubContext.SendToClient("Someone says to you, You should be feeling better now, wake when you are ready", player.HubGuid);

              

                await Task.Delay(2000);

                HubContext.SendToClient("<p class='RoomExits'>[Hint] Type wake to wake up</p>", player.HubGuid);


                while (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)) != null)
                {
                    await Task.Delay(30000);

                    if (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)).Status != PlayerSetup.Player.PlayerStatus.Standing)
                    {
                        HubContext.SendToClient("You feel better as a wave of warth surrounds your body", player.HubGuid);

                        await Task.Delay(2000);

                        HubContext.SendToClient("Someone says to you, you should be feeling better now, wake when you are ready", player.HubGuid);

                        await Task.Delay(2000);

                        HubContext.SendToClient("<p class='RoomExits'>[Hint] Type wake to wake up</p>", player.HubGuid);

                       

                    }

                }



            }

        

       

        }
    }
}