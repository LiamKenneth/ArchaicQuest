using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MIMWebClient.Hubs;

namespace MIMWebClient.Core.Update
{
    public class UpdateWorld
    {


        public static void Init()
        {
            Task.Run(UpdateTime);
        }

        public static void CleanRoom()
        {
            Task.Run(UpdateRoom);
        }

        public static void UpdateMob()
        {
            Task.Run(MoveMob);
        }

        public static async Task EmoteMob()
        {
            //Loop every mob in the rooms in cache.
            // only emote if player is in the room

            foreach (var room in MIMHub._AreaCache.Values)
            {
                if (room.players.Count > 0 && room.mobs.Count > 0)
                {
                    // check mob emotes

                    foreach (var mob in room.mobs)
                    {
                        if (mob.Emotes != null && mob.HitPoints > 0)
                        {
                            await Task.Delay(5000);
                            var emoteIndex = Helpers.diceRoll.Next(mob.Emotes.Count);
                            HubContext.broadcastToRoom(mob.Name + " " + mob.Emotes[emoteIndex], room.players, String.Empty);
                        }
                    }
                }
            }


        }

        public static async Task EmoteRoom()
        {
            //Loop every mob in the rooms in cache.
            // only emote if player is in the room

            foreach (var room in MIMHub._AreaCache.Values)
            {
                if (room.players.Count > 0)
                {
                    // check mob emotes

                    if (room.Emotes.Count > 0)
                    {

                        var emoteIndex = Helpers.diceRoll.Next(room.Emotes.Count);
                        HubContext.broadcastToRoom(room.Emotes[emoteIndex], room.players, String.Empty);
                    }

                }
            }


        }

        /// <summary>
        /// Wahoo! This works
        /// Now needs to update player/mob stats, spells, reset rooms and mobs. Move mobs around?
        /// Global Timer every 60 seconds? quicker timer for mob movement?
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateTime()
        {
            await Task.Delay(60000);

            Time.UpdateTIme();

            await Task.Run(EmoteMob);
            await Task.Run(EmoteRoom);
            await Task.Run(KickIdlePlayers);
            Init();
        }


        public static async Task UpdateRoom()
        {
            await Task.Delay(300000);
            RestoreVitals.UpdateRooms();

            HubContext.getHubContext.Clients.All.addNewMessageToPage("This is will update Rooms every 5 minutes and not block the game");

            CleanRoom();
        }

        public static async Task KickIdlePlayers()
        {
      
            foreach (var player in MIMHub._PlayerCache)
            {
                if (player.Value != null && player.Value.LastCommandTime.AddMinutes(1) < DateTime.UtcNow)
                {
                    HubContext.SendToClient("You disapear in the void", player.Value.HubGuid);

                    var room =
                        MIMHub._AreaCache.FirstOrDefault(
                            x =>
                                x.Value.area.Equals(player.Value.Area) && x.Value.areaId.Equals(player.Value.AreaId) &&
                                x.Value.region.Equals(player.Value.Region));

                    if (room.Value != null)
                    {
                        foreach (var players in room.Value.players)
                        {
                            HubContext.broadcastToRoom(player.Value.Name + " disapears in the void", room.Value.players,
                                player.Value.HubGuid, true);
                        }

                        //room.Value.players.Remove(player.Value);
                    }
                }

                if (player.Value != null && player.Value.LastCommandTime.AddMinutes(5) < DateTime.UtcNow)
                {
                    var room =
                        MIMHub._AreaCache.FirstOrDefault(
                            x =>
                                x.Value.area.Equals(player.Value.Area) && x.Value.areaId.Equals(player.Value.AreaId) &&
                                x.Value.region.Equals(player.Value.Region));

                    Command.ParseCommand("quit", player.Value, room.Value);
                }
            }
        }

        public static async Task MoveMob()
        {
            //await Task.Delay(5000);

            //HubContext.getHubContext.Clients.All.addNewMessageToPage("This task will update Mobs every 5 seconds and not block the game");

            //UpdateMob();
        }
    }
}