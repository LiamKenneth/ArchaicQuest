using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Mob.Events
{
    public class Shop
    {
        public static void listItems(PlayerSetup.Player player, Room.Room room)
        {

            var itemsForSell = "<table><thead><tr><td>Item</td><td>Price</td></tr></thead><tbody>";
            var mob = room.mobs.FirstOrDefault(x => x.Shop.Equals(true));

            if (mob != null)
            {

                if (mob.itemsToSell.Count > 0)
                {



                    foreach (var item in mob.itemsToSell)
                    {
                        itemsForSell +=  "<tr><td>" + item.name + "</td> <td>" + item.worth + " GP</td></tr>";
                    }

                    itemsForSell += "</tbody></table>";
                }
                else
                {
                    HubContext.SendToClient("Sorry I have nothing to sell you.", player.HubGuid);
                    return;
                }

                //e.g Yes sure here are my wares.
                HubContext.SendToClient(mob.Name + " says to you " + mob.sellerMessage, player.HubGuid);

                //show player items
                HubContext.SendToClient(itemsForSell, player.HubGuid);

            }
            else
            {
                HubContext.SendToClient("There is no merchant here", player.HubGuid);
            }
        }

        public static void buyItems(PlayerSetup.Player player, Room.Room room, string itemName)
        {

            var mob = room.mobs.FirstOrDefault(x => x.Shop.Equals(true));

            if (mob != null)
            {


                if (mob.Shop)
                {

                    if (string.IsNullOrEmpty(itemName))
                    {
                        HubContext.SendToClient("Buy? Buy what?", player.HubGuid);
                        return;
                    }

                    var itemToBuy = mob.itemsToSell.FirstOrDefault(x => x.name.ToLower().Contains(itemName.ToLower()));

                    if (itemToBuy != null)
                    {

                    

                        itemToBuy.location = Item.Item.ItemLocation.Inventory;
                        player.Inventory.Add(itemToBuy);
                        HubContext.SendToClient(
                            "You buy " + AvsAnLib.AvsAn.Query(itemToBuy.name) + itemToBuy.name + " from " + mob.Name,
                            player.HubGuid);
                        HubContext.broadcastToRoom(
                            player.Name + " buys " + AvsAnLib.AvsAn.Query(itemToBuy.name) + itemToBuy.name + " from " +
                            mob.Name, room.players, player.HubGuid, true);
                        Score.UpdateUiInventory(player);
                        //deduct gold
                    }

                }
                else
                {

                    HubContext.SendToClient("Sorry I don't sell that", player.HubGuid);
                }

            }

        }

    }
}