using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob.Events
{
    public class Shop
    {
        public static void listItems(PlayerSetup.Player player, PlayerSetup.Player mob) {

            var itemsForSell = string.Empty;

            if (mob.itemsToSell.Count > 0)
            {
                foreach (var item in mob.itemsToSell)
                {
                    itemsForSell += item.name + "\r\n";
                }
            }
            else
            {
                itemsForSell = "Sorry I have nothing to sell you.";
            }

            //e.g Yes sure here are my wares.
            HubContext.SendToClient(mob.sellerMessage, player.HubGuid);

            //show player items
            HubContext.SendToClient(itemsForSell, player.HubGuid);
        }

        public static void buyItems(PlayerSetup.Player player, PlayerSetup.Player mob, Item.Item item)
        {

            if (mob.Shop)
            {            
                player.itemsToSell.Remove(item);
                HubContext.SendToClient("You give " + mob.Name + " your " + item.name, player.HubGuid);

                mob.itemsToSell.Add(item);
                HubContext.SendToClient(mob.Name + " gives you 100 gold", player.HubGuid);
            }
            else
            {
 
                HubContext.SendToClient("Sorry no shops here", player.HubGuid);
            }
            
 
 
        }

    }
}