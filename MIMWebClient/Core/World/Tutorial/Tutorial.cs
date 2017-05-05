using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World.Anker.Mobs;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Clothing.Legs;
using MIMWebClient.Core.World.Items.Clothing.ClothingBody;
using Cache = MIMWebClient.Core.Events.Cache;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Tutorial
    {

        public static void setUpTut(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            Task.Run(async () => await Intro(player, room, step, calledBy));
        }

        public static async Task Intro(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {

            try
            {
                var npc = room.mobs.FirstOrDefault(x => x.Name.Equals("Wilhelm"));

                if (string.IsNullOrEmpty(step))
                {

                    HubContext.SendToClient(
                        npc.Name + " says to you \"I don't think we have much further to go " + player.Name + ".\"",
                        player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient("You hear a twig snap in the distance.", player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient(npc.Name + " looks at you with a face of terror and dread.", player.HubGuid);

                    await Task.Delay(3000);

                    HubContext.SendToClient(npc.Name + " says to you \"did you hear that " + player.Name + ".\"",
                        player.HubGuid);

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
                var hasDagger = player.Inventory.FirstOrDefault(x => x.name.Contains("dagger"));
                if (step.Equals("yes", StringComparison.CurrentCultureIgnoreCase) && hasDagger == null)
                {

                    await Task.Delay(1500);

                    HubContext.SendToClient("You look around but see nothing.", player.HubGuid);

                    await Task.Delay(1500);

                    HubContext.SendToClient("Suddenly a Goblin yells AARGH-tttack!!", player.HubGuid);

                    await Task.Delay(3000);

                    HubContext.SendToClient("You hear movement all around you.", player.HubGuid);

                    await Task.Delay(1500);

                    HubContext.SendToClient(npc.Name + " says to you \"here take this dagger " + player.Name + ".\"",
                        player.HubGuid);

                    var weapon = npc.Inventory.FirstOrDefault(x => x.name.Contains("dagger"));

                    if (weapon != null)
                    {
                        player.Inventory.Add(weapon);
                    }


                    HubContext.SendToClient(npc.Name + " gives you a blunt dagger.", player.HubGuid);

                    Score.UpdateUiInventory(player);

                    await Task.Delay(1500);

                    HubContext.SendToClient(
                        npc.Name +
                        " says to you \"it's nothing special but it will help you. I believe the way to Ester is all north from here.\"",
                        player.HubGuid);

                    await Task.Delay(3000);

                    HubContext.SendToClient("You hear movement getting closer.", player.HubGuid);

                    await Task.Delay(3000);

                    HubContext.SendToClient("Suddenly 5 Goblins emerge from the bushes and fan out in a semi circle.",
                        player.HubGuid);

                    await Task.Delay(3000);

                    HubContext.SendToClient(
                        npc.Name + " yells \"GO " + player.Name + ", I'll hold them off. RUN! Run now to the North.\"",
                        player.HubGuid);

                    while (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)) != null)
                    {
                        await Task.Delay(30000);

                        if (room.players.FirstOrDefault(x => x.Name.Equals(player.Name)) != null)
                        {
                            HubContext.SendToClient(
                                npc.Name + " yells \"GO\", " + player.Name +
                                " \"I'll hold them off. RUN! Run now to the North.\"", player.HubGuid);

                            HubContext.SendToClient(
                                "<p class='RoomExits'>[Hint] Type north or n for short to move north away from the ambush</p>",
                                player.HubGuid);
                        }

                    }


                }

                if (step.Equals("Attack") && calledBy.Equals("mob"))
                {
                    //blah blah
                }

            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "willhelm"
                };

                Save.LogError(log);
            }
        }

        public static void setUpRescue(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            Task.Run(async () => await AwakeningRescue(player, room, step, calledBy));
        }

        public static void setUpAwakening(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            Task.Run(async () => await Awakening(player, room, step, calledBy));

        }

        public static async Task AwakeningRescue(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {

            //give player quest
            var findLance = new Quest()
            {
                Id = 3,
                Name = "Find and greet Lance",
                Description =
             "Mortem has asked me to go find Lance the village elder who can be found in the main square, From the temple leave south and follow the hill path in to town." +
             "<p class='RoomExits'>[Hint] Type greet lance to greet the Elder once you have found him</p>",
                QuestGiver = "Mortem",
                QuestFindMob = Lance.VillageElderLance().Name,
                Type = Quest.QuestType.FindMob,
                RewardXp = 250,
                QuestHint = "<h5>Hint:</h5><p>Lance is here, type greet Lance to interact with him.</p>",
                QuestTrigger = Lance.VillageElderLance().Name,
                RewardDialog = new DialogTree()
                {
                    Message = "Yes I am Lance, well met $playerName",
                    ShowIfOnQuest = "Find and greet Lance"
                }
            };

            try
            {
                //to stop task firing twice
                if (player.QuestLog.FirstOrDefault(x => x.Name.Equals("Find and greet Lance")) != null)
                {
                    return;
                }

                var npc = room.mobs.FirstOrDefault(x => x.Name.Equals("Mortem"));

                if (npc == null) return;

                if (step.Equals("wake", StringComparison.CurrentCultureIgnoreCase))
                {

                    //remove player from tutorial room
                    var oldRoom = Cache.ReturnRooms()
                        .FirstOrDefault(
                            x => x.area.Equals("Tutorial") && x.areaId.Equals(1) && x.region.Equals("Tutorial"));

                    if (oldRoom != null && oldRoom.players.Contains(player))
                    {
                        PlayerManager.RemovePlayerFromRoom(oldRoom, player);
                    }


                    HubContext.SendToClient(npc.Name + " says \"Ah you are awake!\"", player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient(
                        npc.Name + " says \"You were in a bad way when we found you, I didn't think you would wake.\"",
                        player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient(npc.Name + " Gets a pair of trousers and a shirt and hands them to you",
                        player.HubGuid);

                    player.Inventory.Add(ClothingBody.PlainTop());
                    player.Inventory.Add(ClothingLegs.PlainTrousers());

                    Score.UpdateUiInventory(player);

                    await Task.Delay(2000);

                    HubContext.SendToClient(npc.Name + " says \"Wear them, you can't walk around naked I am afraid.\"",
                        player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient(
                        "<p class='RoomExits'>[Hint] To view items you are carrying type Inventory or i for short</p>",
                        player.HubGuid);

                    HubContext.SendToClient(npc.Name + " smiles at you.", player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient(
                        "<p class='RoomExits'>[Hint] Type wear trousers and wear shirt or alternatively wear all</p>",
                        player.HubGuid);

                }

                if (step != null && step.Contains("plain"))
                {

                    if (player.QuestLog.FirstOrDefault(x => x.Name.Equals("Find and greet Lance")) != null)
                    {
                        return;
                    }



                    if (player.Equipment.Body.Equals(ClothingBody.PlainTop().name) &&
                        !player.Equipment.Legs.Equals(ClothingLegs.PlainTrousers().name))
                    {
                        HubContext.SendToClient(
                            npc.Name + " says \"It fits well, don't forget to wear the trousers too.\"",
                            player.HubGuid);

                        await Task.Delay(2000);

                        return;

                    }

                    if (player.Equipment.Legs.Equals(ClothingLegs.PlainTrousers().name) &&
                        !player.Equipment.Body.Equals(ClothingBody.PlainTop().name))
                    {


                        HubContext.SendToClient(npc.Name + " says \"It fits well, don't forget to wear the top too.\"",
                            player.HubGuid);

                        await Task.Delay(2000);

                        return;

                    }

                    if (player.Equipment.Legs.Equals(ClothingLegs.PlainTrousers().name) &&
                        player.Equipment.Body.Equals(ClothingBody.PlainTop().name))
                    {

                        HubContext.SendToClient(
                            npc.Name +
                            " says \"Excellent, I have one request for you and that is to speak to Lance the Elder of the village.\"",
                            player.HubGuid);

                        HubContext.SendToClient(
                            npc.Name +
                            " says \"He wants to know if you remember anything about the attack that may help him? We have been raided a few times of late.\"",
                            player.HubGuid);

                        HubContext.SendToClient(
                            npc.Name +
                            " says \"You will found him in the Square of Anker just leave south and follow the hill path in to town you can't miss the Square.\"",
                            player.HubGuid);




                        player.QuestLog.Add(findLance);




                        HubContext.SendToClient(
                            "New Quest added: Find and greet Lance. Type qlog to be reminded about quest information.",
                            player.HubGuid);


                        HubContext.SendToClient(
                            npc.Name +
                            " waves to you, may Tyr bless you.",
                            player.HubGuid);
                    }

                }
            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "mortem"
                };

                Save.LogError(log);
            }
        }


        //TODO: find bug causing this task to fire randomly
        // ran twice when casting armor spell on cat! -_-

        // c armor cat

        //Your hands start to glow as you begin chanting the armour spell

        //You place your hands upon Black and White cat engulfing them in a white protective glow.

        //You feel better as a wave of warth surrounds your body <-- task

        //Someone says to you, you should be feeling better now, wake when you are ready <-- task

        public static async Task Awakening(PlayerSetup.Player player, Room.Room room, string step, string calledBy)
        {
            //to stop task firing twice
            if (player.QuestLog.FirstOrDefault(x => x.Name.Equals("Find and greet Lance")) != null)
            {
                return;
            }


            player.Status = PlayerSetup.Player.PlayerStatus.Sleeping;

            await Task.Delay(5000);

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

                //fix for random wake message hint showing

                var playerInRoom =
                    Cache.ReturnRooms()
                        .FirstOrDefault(
                            x => x.area.Equals("Tutorial") && x.areaId.Equals(1) && x.region.Equals("Tutorial"))
                        .players.FirstOrDefault(x => x.Name.Equals(player.Name));


                //well this does not work
                if (playerInRoom != null)
                {
                    await Task.Delay(3000);

                    HubContext.SendToClient("You feel better as a wave of warmth surrounds your body", player.HubGuid);

                    await Task.Delay(2000);

                    HubContext.SendToClient("Someone says to you \"You should be feeling better now, wake when you are ready.\"", player.HubGuid);



                    await Task.Delay(2000);

                    HubContext.SendToClient("<p class='RoomExits'>[Hint] Type wake to wake up</p>", player.HubGuid);
                }

 
       

                //loops forever because room.players does not get updated when player leaves the ambush room
                // so this always tries to fire the messages below. as to why it sometimes it shows and sometimes does not, I have no idea.
                while (playerInRoom !=null && playerInRoom.Area.Equals("Tutorial"))
                {
                    await Task.Delay(30000); // <-- is this the cause && the check below is not working

                    playerInRoom =
                  Cache.ReturnRooms()
                      .FirstOrDefault(
                          x => x.area.Equals("Tutorial") && x.areaId.Equals(1) && x.region.Equals("Tutorial"))
                      .players.FirstOrDefault(x => x.Name.Equals(player.Name));

                    if (playerInRoom == null)
                    {
                        return;
                    }

                    if (playerInRoom.Status != PlayerSetup.Player.PlayerStatus.Standing)

                    {
                        HubContext.SendToClient("You feel better as a wave of warmth surrounds your body", player.HubGuid);

                        await Task.Delay(2000);

                        HubContext.SendToClient("Someone says to you \"you should be feeling better now, wake when you are ready.\"", player.HubGuid);

                        await Task.Delay(2000);

                        HubContext.SendToClient("<p class='RoomExits'>[Hint] Type wake to wake up</p>", player.HubGuid);



                    }

                }



            }





        }
    }
}