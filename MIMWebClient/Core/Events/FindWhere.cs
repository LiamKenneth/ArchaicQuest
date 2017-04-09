using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class FindWhere
    {

        public static object find(Room.Room room, PlayerSetup.Player player, int findNth, string itemToFind)
        {

            //search room items 1st

           var  foundItem = FindItem.Item(room.items, findNth, itemToFind, Item.Item.ItemLocation.Room);

            if (foundItem != null) { return foundItem; }

            //search player inventory
            foundItem = FindItem.Item(player.Inventory, findNth, itemToFind, Item.Item.ItemLocation.Inventory);

            if (foundItem != null)
            {
                return foundItem;
            }

            //search mob
           var foundMob = FindItem.Player(room.mobs, findNth, itemToFind);

            if (foundMob != null)
            {
                return foundMob;
            }

            //search players
            var foundPlayer = FindItem.Player(room.players, findNth, itemToFind);

            if (foundPlayer != null)
            {
                return foundPlayer;
            }
            else
            {

                HubContext.SendToClient($"You don't see anything by that name here.", player.HubGuid);

             
            }

            return null;
        }
    }
}