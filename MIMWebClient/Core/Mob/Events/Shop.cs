using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

                    Regex rgx = new Regex(@"\s[a-z][0-9]*$");
                    

                    foreach (var item in ItemContainer.List(mob.itemsToSell))
                    {
                        var cleanItemName = rgx.Replace(item, string.Empty);
                        var goldAmount = mob.itemsToSell.FirstOrDefault(x => x.name.Equals(cleanItemName)).Gold;

                        if (goldAmount <= 0)
                        {
                            goldAmount = 10;
                        }

                        itemsForSell +=  "<tr><td>" + item + "</td> <td>" + (int)(goldAmount + (goldAmount * 0.15)) + " GP</td></tr>";
                    }

                    itemsForSell += "</tbody></table>";
                }
                else
                {
                    HubContext.Instance.SendToClient("<span class='sayColor'>" + mob.Name + "says, \"Sorry I have nothing to sell you.\"</span>", player.HubGuid);
                    return;
                }

                //e.g Yes sure here are my wares.
                HubContext.Instance.SendToClient("<span class='sayColor'>" + mob.Name + " says to you \"" + mob.sellerMessage + "\"</span>", player.HubGuid);

                //show player items
                HubContext.Instance.SendToClient(itemsForSell, player.HubGuid);

            }
            else
            {
                HubContext.Instance.SendToClient("There is no merchant here", player.HubGuid);
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
                        HubContext.Instance.SendToClient("Buy? Buy what?", player.HubGuid);
                        return;
                    }

                    var itemToBuy = mob.itemsToSell.FirstOrDefault(x => x.name.ToLower().Contains(itemName.ToLower()));

                    if (itemToBuy != null)
                    {
                        var result = AvsAnLib.AvsAn.Query(itemToBuy.name);
                        string article = result.Article;

                        //Can afford
                        var cost = itemToBuy.Gold + itemToBuy.Gold * 0.15;

                        if (player.Gold >= cost)
                        {

                            itemToBuy.location = Item.Item.ItemLocation.Inventory;
                            player.Inventory.Add(itemToBuy);

                  
                            HubContext.Instance.SendToClient(
                                "You buy " + article + " " + itemToBuy.name + " from " + mob.Name + " for " + cost + " gold.",
                                player.HubGuid);
                           
                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {
                                    var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} buys {article} {itemToBuy.name} from {mob.Name}";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }

                         
                            mob.itemsToSell.Remove(itemToBuy);

                            foreach (var item in mob.itemsToSell.Where(x => x.name.Equals(itemToBuy.name)))
                            {
                                item.Gold = itemToBuy.Gold > 0 ? itemToBuy.Gold : 10;
                            }

                            Score.UpdateUiInventory(player);
                            //deduct gold

                            var value = itemToBuy.Gold > 0 ? itemToBuy.Gold : 10;

                            player.Gold -= value;

                            Score.ReturnScoreUI(player);
                        }
                        else
                        {
                            HubContext.Instance.SendToClient("You can't afford " + article + " " + itemToBuy.name, player.HubGuid);
                        }

                    }
                    else
                    {
                        HubContext.Instance.SendToClient("I don't have that item.", player.HubGuid);
                    }

                }
                else
                {

                    HubContext.Instance.SendToClient("<span class='sayColor'>" + mob.Name + " says to you \"Sorry I don't sell that\"", player.HubGuid);
                }

            }

        }

        public static void sellItems(PlayerSetup.Player player, Room.Room room, string itemName)
        {

            var mob = room.mobs.FirstOrDefault(x => x.Shop.Equals(true));

            if (mob != null)
            {


                if (mob.Shop)
                {

                    if (string.IsNullOrEmpty(itemName))
                    {
                        HubContext.Instance.SendToClient("Sell? Sell what?", player.HubGuid);
                        return;
                    }

                    var itemToSell = player.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(itemName.ToLower()));

                    if (itemToSell != null)
                    {
                        var oldPlayerInfo = player;
                        var result = AvsAnLib.AvsAn.Query(itemToSell.name);
                        string article = result.Article;

                        //Can afford


                       itemToSell.location = Item.Item.ItemLocation.Inventory;

                        mob.itemsToSell.Add(itemToSell);
 

                        var value = itemToSell.Gold > 0 ? itemToSell.Gold : 1;

                        player.Gold += value;


                        foreach (var character in room.players)
                            {
                                if (player != character)
                                {
                                    var hisOrHer = Helpers.ReturnHisOrHers(player, character);
                                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} sells {article} {itemToSell.name} to {mob.Name}.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }


                   

                        HubContext.Instance.SendToClient(
                              "You sell " + article + " " + itemToSell.name + " to " + mob.Name + " for " + value + " gold.",
                              player.HubGuid);

                        player.Inventory.Remove(itemToSell);

                        Cache.updatePlayer(oldPlayerInfo, player);


                        Score.UpdateUiInventory(player);
                    }
                    else
                    {
                        HubContext.Instance.SendToClient("You don't have that item.", player.HubGuid);
                    }


                }
                else
                {

                    HubContext.Instance.SendToClient("<span class='sayColor'>" + mob.Name + " says to you \"Sorry I don't want to buy anything.\"", player.HubGuid);
                }

            }

        }

    }
}