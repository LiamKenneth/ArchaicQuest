using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class CheckEvent
    {
        public enum EventType
        {
            Say,
            Wake,
            Sleep,
            Move,
            Fight,
            Wear,
            Remove,
            Get,
            Drop

        }

        public static void FindEvent(EventType eventName, PlayerSetup.Player player, string option)
        {

            var room =
                Cache.ReturnRooms()
                    .FirstOrDefault(
                        x =>
                            x.area.Equals(player.Area) && x.areaId.Equals(player.AreaId) &&
                            x.region.Equals(player.Region));

            if (room == null)
            {
                //logerror
                return;
            }

            if (eventName.Equals(EventType.Wear))
            {
                EvokeEvent(room, player, option);
            }

        }

        private static void EvokeEvent(Room.Room room, PlayerSetup.Player player, string option)
        {

            if (!string.IsNullOrEmpty(room.EventWear))
            {
                var triggerEvent = room.EventWear;

                Event.ParseCommand(triggerEvent, player, null, room, option, "player");
            }

            foreach (var mob in room.mobs)
            {
                if (!string.IsNullOrEmpty(mob.EventWear))
                {
                    var triggerEvent = mob.EventWear;

                    Event.ParseCommand(triggerEvent, player, mob, room, option, "player");
                }
            }
        }

    }

}