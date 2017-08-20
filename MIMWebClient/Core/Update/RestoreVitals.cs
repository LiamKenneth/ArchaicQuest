using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MIMWebClient.Core.Events;
using MongoDB.Bson;

namespace MIMWebClient.Core.Update
{
    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using MIMWebClient.Core.World.Anker;

    public static class RestoreVitals
    {

        public static void UpdatePlayers()
        {
            var context = HubContext.getHubContext;
            var players = Cache.ReturnPlayers();

            if (players.Count == 0)
            {
                return;
            }

            // I suspect this is the reason the update failed
            // player dropped out and invalidated the loop
            // .toList should solve it if that's the issue
            foreach (var player in players.ToList())
            {
               

                try
                {

                    if (player.Status != Player.PlayerStatus.Fighting)
                    {
                        UpdateHp(player, context);

                        UpdateMana(player, context);

                        UpdateEndurance(player, context);
                    }

                    UpdateAffects(player, context);
 
                }
                catch (Exception ex)
                {
                    var log = new Error.Error
                    {
                        Date = DateTime.Now,
                        ErrorMessage = ex.InnerException.ToString(),
                        MethodName = "KickIdlePlayers"
                    };

                    Save.LogError(log);

                }

            }
        }

        public static void UpdateRooms()
        {
            var context = HubContext.getHubContext;
            var rooms = Cache.ReturnRooms();

            if (rooms.Count == 0)
            {
                return;
            }

            try
            {
                
                foreach (var room in rooms)
                {
                    foreach (var mob in room.mobs.ToList())
                    {
                        UpdateHp(mob, context);
                        UpdateMana(mob, context);
                        UpdateEndurance(mob, context);
                        UpdateAffects(mob, context);
                    }
                 
                    
                    #region add Mobs back

                 
                        if (room.corpses.Count > 0)
                        {
                            // decay corpse

                            foreach (var corpse in room.corpses.ToList())
                            {

                          

                            if (corpse.Type.Equals(Player.PlayerTypes.Player))
                                {
                                    continue;
                                }

                                var mobRoomOrigin =
                                    rooms.Find(
                                        x =>
                                            x.areaId == corpse.Recall.AreaId && x.area == corpse.Recall.Area &&
                                            x.region == corpse.Recall.Region);
                                var originalArea = World.Areas.ListOfRooms().FirstOrDefault(x =>
                                    x.area == mobRoomOrigin.area && x.areaId == mobRoomOrigin.areaId &&
                                    x.region == mobRoomOrigin.region);

                                if (originalArea != null)
                                {

                                // potential bug with mobs that have the same name but only one carries an item
                                // finding the origianl mob will probbaly match for one but not the other so the
                                // mob with the other item never gets loaded.
                                // potential way round it. random loot drops for mobs that don't have a name and are genric ie. rat
                                // mobs like village idiot have set items
                                    var originalMob = originalArea.mobs.Find(x => x.Name == corpse.Name);

                                if (originalMob != null)
                                {
                                    room.items.Remove(room.items.Find(x => x.name.Contains(originalMob.Name)));
                                    room.corpses.Remove(room.corpses.Find(x => x.Name.Equals(originalMob.Name)));

                                    room.mobs.Add(originalMob);
                                }
                              
                                }
                            }

                        }
                    

                    #endregion

                    #region add Items back

                    for (int j = World.Areas.ListOfRooms().Count - 1; j >= 0; j--)
                    {
                        if (World.Areas.ListOfRooms()[j].area == room.area &&
                            World.Areas.ListOfRooms()[j].areaId == room.areaId &&
                            World.Areas.ListOfRooms()[j].region == room.region)
                        {

                            for (int k = World.Areas.ListOfRooms()[j].items.Count - 1; k >= 0; k--)
                            {
                                var itemAlreadyThere =
                                    room.items.Find(x => x.name.Equals(World.Areas.ListOfRooms()[j].items[k].name));

                                if (itemAlreadyThere == null)
                                {
                                    room.items.Add(World.Areas.ListOfRooms()[j].items[k]);
                                }

                                if (itemAlreadyThere?.container == true)
                                {


                                    for (int l = World.Areas.ListOfRooms()[j].items[k].containerItems.Count - 1; l >= 0; l--)
                                    {

                                        var containerItemAlreadyThere =
                                            itemAlreadyThere.containerItems.Find(
                                                x =>
                                                    x.name.Equals(
                                                        World.Areas.ListOfRooms()[j].items[k].containerItems[l].name));

                                        if (containerItemAlreadyThere == null)
                                        {
                                            itemAlreadyThere.containerItems.Add(
                                                World.Areas.ListOfRooms()[j].items[k].containerItems[l]);
                                        }

                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(room.updateMessage))
                    {
                        foreach (var player in room.players)
                        {
                            HubContext.SendToClient(room.updateMessage, player.HubGuid);
                        }
                    }
                }





            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "Room update fucked"
                };

                Save.LogError(log);

            }
        }



        public static void UpdateHp(PlayerSetup.Player player, IHubContext context)
        {
            try
            {
                if (player == null)
                {
                    return;
                }

                if (player.HitPoints <= player.MaxHitPoints)
                {

                    var die = new Helpers();
                    var maxGain = player.Constitution;


                    if (player.Status == Player.PlayerStatus.Fighting)
                    {
                        maxGain = maxGain / 4;
                    }

                    if (player.Status == Player.PlayerStatus.Sleeping)
                    {
                        maxGain = maxGain * 2;
                    }


                    if (player.Status == Player.PlayerStatus.Resting)
                    {
                        maxGain = (maxGain * 2) / 2;
                    }


                    player.HitPoints += die.dice(1, 1, maxGain);

                    if (player.HitPoints > player.MaxHitPoints)
                    {
                        player.HitPoints = player.MaxHitPoints;
                    }

                    if (player.Type == Player.PlayerTypes.Player)
                    {
                        if (player.HubGuid == null)
                        {
                            return;
                        }

                        context.Clients.Client(player.HubGuid).updateStat(player.HitPoints, player.MaxHitPoints, "hp");
                    }

                }

            }
            catch (Exception ex)
            {

                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "updateHP"
                };

                Save.LogError(log);

            }

        }

        public static void UpdateMana(PlayerSetup.Player player, IHubContext context)
        {

            try
            {

                if (player == null)
                {
                    return;
                }


                if (player.ManaPoints <= player.MaxHitPoints)
                {

                    var die = new Helpers();
                    var maxGain = player.Intelligence;

                    if (player.Status == Player.PlayerStatus.Fighting)
                    {
                        maxGain = maxGain / 4;
                    }

                    if (player.Status == Player.PlayerStatus.Sleeping)
                    {
                        maxGain = maxGain * 2;
                    }


                    if (player.Status == Player.PlayerStatus.Resting)
                    {
                        maxGain = (maxGain * 2) / 2;
                    }

                    player.ManaPoints += die.dice(1, 1, maxGain);

                    if (player.ManaPoints > player.MaxManaPoints)
                    {
                        player.ManaPoints = player.MaxManaPoints;
                    }


                    if (player.Type == Player.PlayerTypes.Player)
                    {
                        if (player.HubGuid == null)
                        {
                            return;
                        }

                        context.Clients.Client(player.HubGuid)
                            .updateStat(player.ManaPoints, player.MaxManaPoints, "mana");

                    }
                }
            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "UpdateMana"
                };

                Save.LogError(log);

            }

        }

        public static void UpdateAffects(PlayerSetup.Player player, IHubContext context)
        {
            try
            {
                if (player?.Affects == null)
                {
                    return;
                }

                foreach (var af in player.Affects.ToList())
                {

                    if (af.Duration == 0 || af.Duration <= 0)
                    {
                        

                        // put in method? or change way we handle invis
                        if (af.Name == "Invis")
                        {
                            player.invis = false;
                        }

                        if (af.Name == "Chill Touch")
                        {
                            player.Equipment.Wielded = "Nothing";
                            var chillTouch = player.Inventory.FirstOrDefault(x => x.name.Equals("Chill Touch"));
                            player.Inventory.Remove(chillTouch);
                        }

                        if (af.AffectLossMessageRoom != null)
                        {
                            HubContext.SendToClient(af.AffectLossMessagePlayer, player.HubGuid);
                        }

                        if (af.AffectLossMessageRoom != null)
                        {
                            var room = Cache.getRoom(player);

                            foreach (var character in room.players.ToList())
                            {
                                if (player != character && character.HubGuid != null)
                                {
                                    HubContext.SendToClient(
                                        Helpers.ReturnName(player, character, string.Empty) + " " +
                                        af.AffectLossMessageRoom, character.HubGuid);

                                }
                            }

                        }

                        player.Affects.Remove(af);
                    }
                    else
                    {
                        af.Duration -= 1;
                    }
                 

                }
            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "Update Affects"
                };

                Save.LogError(log);
            }
        }

        public static void UpdateEndurance(PlayerSetup.Player player, IHubContext context)
        {

            try
            {
                if (player == null)
                {
                    return;
                }

                if (player.MovePoints <= player.MaxMovePoints)
                {

                    var die = new Helpers();
                    var maxGain = player.Dexterity;

                    if (player.Status == Player.PlayerStatus.Fighting)
                    {
                        maxGain = maxGain / 4;
                    }

                    if (player.Status == Player.PlayerStatus.Sleeping)
                    {
                        maxGain = maxGain * 2;
                    }


                    if (player.Status == Player.PlayerStatus.Resting)
                    {
                        maxGain = (maxGain * 2) / 2;
                    }

                    player.MovePoints += die.dice(1, 1, maxGain);

                    if (player.MovePoints > player.MaxMovePoints)
                    {
                        player.MovePoints = player.MaxMovePoints;
                    }


                    if (player.Type == Player.PlayerTypes.Player)
                    {
                        if (player.HubGuid == null)
                        {
                            return;
                        }

                        context.Clients.Client(player.HubGuid)
                            .updateStat(player.MovePoints, player.MaxMovePoints, "endurance");
                    }

                    Score.ReturnScoreUI(player);
                }
            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "Update endurance"
                };

                Save.LogError(log);

            }

        }


    }
}