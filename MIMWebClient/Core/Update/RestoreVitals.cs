using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Update
{

    using MIMWebClient.Core.PlayerSetup;
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

            foreach (var player in players)
            {

                UpdateHp(player, context);
                UpdateMana(player, context);
                UpdateEndurance(player, context);

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

                    for (int i = room.mobs.Count - 1; i >= 0; i--)
                    {

                        UpdateHp(room.mobs[i], context);
                        UpdateMana(room.mobs[i], context);
                        UpdateEndurance(room.mobs[i], context);

                    }
 
                    if (room.corpses.Count > 0)
                    {

                        for (int i = room.corpses.Count - 1; i >= 0; i--)
                        {
                            for (int j = World.Areas.ListOfRooms().Count - 1; j >= 0; j--)
                            {

                                for (int k = World.Areas.ListOfRooms()[j].mobs.Count - 1; k >= 0; k--)
                                {

                                    var originalMob = World.Areas.ListOfRooms()[j].mobs[k];
                                    var originalRoom = World.Areas.ListOfRooms()[j];
                                    var corpse = room.corpses[i];

                                    if (originalMob.Name == corpse.Name)
                                    {

                                        //put mob back to start position
                                        var roomToReset =
                                            rooms.Find(
                                                x =>
                                                    x.areaId == originalRoom.areaId && x.area == originalRoom.area &&
                                                    x.region == originalRoom.region);

                                        roomToReset.mobs.Add(originalMob);
                                        room.corpses.Remove(corpse);

                                    }
                                }
                            }
                         
                        }

                        

                    }
                }

                //now for items


                // add missing items
                // add mob if found in corpse list

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
        }



        public static void UpdateHp(PlayerSetup.Player player, IHubContext context)
        {
            try
            {

                if (player.HitPoints <= player.MaxHitPoints)
                {

                    var die = new Helpers();
                    var maxGain = player.Constitution;


                    if (player.Status == Player.PlayerStatus.Fighting)
                    {
                        maxGain = maxGain/2;
                    }

                    if (player.Status == Player.PlayerStatus.Sleeping)
                    {
                        maxGain = maxGain*2;
                    }


                    if (player.Status == Player.PlayerStatus.Resting)
                    {
                        maxGain = (maxGain*2)/2;
                    }


                    player.HitPoints += die.dice(1, 1, maxGain);

                    if (player.HitPoints > player.MaxHitPoints)
                    {
                        player.HitPoints = player.MaxHitPoints;
                    }

                    if (player.Type == Player.PlayerTypes.Player)
                    {
                        context.Clients.Client(player.HubGuid).updateStat(player.HitPoints, player.MaxHitPoints, "hp");
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

        }

        public static void UpdateMana(PlayerSetup.Player player, IHubContext context)
        {

            try
            {
                if (player.ManaPoints <= player.MaxHitPoints)
                {

                    var die = new Helpers();
                    var maxGain = player.Intelligence;

                    if (player.Status == Player.PlayerStatus.Fighting)
                    {
                        maxGain = maxGain / 2;
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

                    if (player.ManaPoints > player.MaxHitPoints)
                    {
                        player.ManaPoints = player.MaxHitPoints;
                    }


                    if (player.Type == Player.PlayerTypes.Player)
                    {
                        context.Clients.Client(player.HubGuid)
                            .updateStat(player.ManaPoints, player.MaxManaPoints, "mana");

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

        }

        public static void UpdateEndurance(PlayerSetup.Player player, IHubContext context)
        {

            try
            {
                if (player.MovePoints <= player.MaxMovePoints)
                {

                    var die = new Helpers();
                    var maxGain = player.Dexterity;

                    if (player.Status == Player.PlayerStatus.Fighting)
                    {
                        maxGain = maxGain / 2;
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

                        context.Clients.Client(player.HubGuid)
                            .updateStat(player.MovePoints, player.MaxMovePoints, "endurance");
                    }
                }
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

        }


    }
}