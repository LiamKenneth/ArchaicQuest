﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player
{
    using Events;

    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.PlayerSetup;
    public class Equipment
    {
        public string Light { get; set; } = "Nothing";
        public string Finger { get; set; } = "Nothing";
        public string Finger2 { get; set; } = "Nothing";
        public string Neck { get; set; } = "Nothing";
        public string Neck2 { get; set; } = "Nothing";
        public string Face { get; set; } = "Nothing";
        public string Head { get; set; } = "Nothing";
        public string Torso { get; set; } = "Nothing";
        public string Legs { get; set; } = "Nothing";
        public string Feet { get; set; } = "Nothing";
        public string Hands { get; set; } = "Nothing";
        public string Arms { get; set; } = "Nothing";
        public string Body { get; set; } = "Nothing";
        public string Waist { get; set; } = "Nothing";
        public string Wrist { get; set; } = "Nothing";
        public string Wrist2 { get; set; } = "Nothing";
        public string Wielded { get; set; } = "Nothing";
        public string Shield { get; set; } = "Nothing";
        public string Held { get; set; } = "Nothing";
        public string Floating { get; set; } = "Nothing";
       
        /// <summary>
        /// Displays what the player is wearing and what slots are availaible
        /// </summary>
        /// <param name="player">The player</param>
        public static void ShowEquipment(Player player)
        {
 
            HubContext.SendToClient("You are wearing:", player.HubGuid);

            HubContext.SendToClient(DisplayEq(player, player.Equipment), player.HubGuid);
            
        }

        public static void ShowEquipmentLook(Player mob, Player viewer)
        {
            HubContext.SendToClient(DisplayEq(mob, mob.Equipment), viewer.HubGuid);
        }

        public static string ReturnEqName(Player mob, string eq)
        {
            var flags = string.Empty;
            var item = mob.Inventory.FirstOrDefault(x => x.name == eq);

            if (item == null || item.itemFlags == null)
            {
                return flags + eq;
            }
             

            foreach (var itemFlag in item.itemFlags)
            {
                switch (itemFlag)
                {
                    case Item.ItemFlags.glow:
                        flags += " (" + itemFlag + ") ";
                        break;
                    case Item.ItemFlags.hum:
                        flags += " (" + itemFlag + ") ";
                        break;
                    case Item.ItemFlags.bless:
                        flags += " (" + itemFlag + "ed) ";
                        break;
                }      

            }

            return flags + eq;
        }

        public static string DisplayEq(Player player, Equipment eq)
        {
            var displayEquipment = new StringBuilder();
            displayEquipment.Append("<table>")
               .Append("<tr><td style='width:175px;'>").Append("&lt;Used as light&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Light)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on finger&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Finger)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on finger&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Finger2)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn around neck&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Neck)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn around neck&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Neck2)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on face&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Face)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on head&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Head)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on torso&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Torso)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on legs&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Legs)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on feet&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Feet)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on hands&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Hands)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn on arms&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Arms)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn about body&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Body)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn about waist&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Waist)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn around wrist&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Wrist)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Worn around wrist&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Wrist2)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Wielded&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Wielded)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Shield&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Shield)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Held&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Held)).Append("</td></tr>")
                .Append("<tr><td>").Append("&lt;Floating nearby&gt;").Append("</td>").Append("<td>").Append(ReturnEqName(player, eq.Floating)).Append("</td></tr>").Append("</table");

            return displayEquipment.ToString();
        }

        /// <summary>
        /// Wears an item
        /// </summary>
        /// <param name="player">The Player</param>
        /// <param name="itemToWear">Item to wear</param>
        public static void WearItem(Player player, string itemToWear, bool wield = false)
        {

            var room = Cache.getRoom(player);

            if (string.IsNullOrEmpty(itemToWear))
            {
                HubContext.SendToClient("Wear what?", player.HubGuid);
                return;
            }

            var oldPlayer = player;

            if (!itemToWear.Equals("all", StringComparison.CurrentCultureIgnoreCase))
            {

                var findObject = Events.FindNth.Findnth(itemToWear);
                int nth = findObject.Key;
                string itemToFind = findObject.Value;

                var foundItem = FindItem.Item(player.Inventory, nth, itemToFind, Item.ItemLocation.Inventory);
                // player.Inventory.Find(i => i.name.ToLower().Contains(itemToWear.ToLower()));

                if (foundItem == null)
                {
                    if (wield)
                    {
                        HubContext.SendToClient("You do not have that item to wield.", player.HubGuid);
                        return;
                    }

                    HubContext.SendToClient("You do not have that item to wear.", player.HubGuid);
                    return;
                }



                foundItem.location = Item.ItemLocation.Worn;
                var slot = Enum.GetName(typeof(Item.EqSlot), foundItem.slot);

                //TODO: WTF is this?
                var eqLocation = player.Equipment.GetType().GetProperty(slot);

                if (slot == null)
                {
                    return;
                } // Log error? What the hell is eqLocation?

                var hasValue = eqLocation.GetValue(player.Equipment);

                if (hasValue.ToString() != "Nothing")
                {
                    RemoveItem(player, hasValue.ToString(), true);
                }

                eqLocation.SetValue(player.Equipment, foundItem.name);

                if (foundItem.ArmorRating != null)
                {
                    player.ArmorRating += foundItem.ArmorRating.Armour;
                }

                if (!wield || !slot.Equals(Item.EqSlot.Wielded.ToString()))
                {
                    HubContext.SendToClient("You wear " + foundItem.name, player.HubGuid);

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wears {article} {foundItem.name}";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    //Wear event

                    CheckEvent.FindEvent(CheckEvent.EventType.Wear, player, foundItem.name);
                }
                else
                {
                    HubContext.SendToClient("You wield " + foundItem.name, player.HubGuid);

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wields {article} {foundItem.name}";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }
                }
                Score.UpdateUiInventory(player);
                Score.ReturnScoreUI(player);
                Score.UpdateUiEquipment(player);
            }
            else
            {
                var listOfItemsWorn = string.Empty;
                foreach (var item in player.Inventory.Where(x => x.location.Equals(Item.ItemLocation.Inventory)))
                {
                    if (item.location == Item.ItemLocation.Inventory && item.equipable == true)
                    {
              
                        var slot = Enum.GetName(typeof(Item.EqSlot), item.slot);

                        //TODO: WTF is this?
                        if (slot != null)
                        {
                           
                            var eqLocation = player.Equipment.GetType().GetProperty(slot);


                            if (eqLocation != null)
                            {
                                var hasValue = eqLocation.GetValue(player.Equipment);

                                if (hasValue.ToString() != "Nothing")
                                {
                                    RemoveItem(player, hasValue.ToString(), true);
                                }
                            }
                            item.location = Item.ItemLocation.Worn;
                            eqLocation.SetValue(player.Equipment, item.name);

                            if (item.ArmorRating != null)
                            {
                                player.ArmorRating += item.ArmorRating.Armour;
                            }


                            listOfItemsWorn += $" {item.name}";


                            if (!wield || !slot.Equals("wield", StringComparison.CurrentCultureIgnoreCase))
                            {

                                var result = AvsAnLib.AvsAn.Query(item.name);
                                string article = result.Article;

                                HubContext.SendToClient("You wear " + article + " " + item.name, player.HubGuid);
                              
                                foreach (var character in room.players)
                                {
                                    if (player != character)
                                    {

                                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wears {article} {item.name}";

                                        HubContext.SendToClient(roomMessage, character.HubGuid);
                                    }
                                }

                            }
                            else
                            {


                                var result = AvsAnLib.AvsAn.Query(item.name);
                                string article = result.Article; 

                                HubContext.SendToClient("You wield " + article + item.name, player.HubGuid);


                                foreach (var character in room.players)
                                {
                                    if (player != character)
                                    {

                                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} wields {article} {item.name}";

                                        HubContext.SendToClient(roomMessage, character.HubGuid);
                                    }
                                }
                            }
 

                        }
                    }
                }

                CheckEvent.FindEvent(CheckEvent.EventType.Wear, player, listOfItemsWorn);
            }
            Score.UpdateUiInventory(player);
            Score.ReturnScoreUI(player);
            Cache.updatePlayer(player, oldPlayer);
            Score.UpdateUiEquipment(player);
        }

        /// <summary>
        /// Remove worn item.
        /// </summary>
        /// <param name="player">The Player</param>
        /// <param name="itemToRemove">Item to Remove</param>
        public static void RemoveItem(Player player, string itemToRemove, bool replaceWithOtherEQ = false, bool unwield = false)
        {
            var room = Cache.getRoom(player);

            if (!itemToRemove.Equals("all", StringComparison.CurrentCultureIgnoreCase))
            {
                var oldPlayer = player;
                var foundItem =
                    player.Inventory.Find(
                        i =>
                            i.name.ToLower().Contains(itemToRemove.ToLower()) &&
                            i.location.Equals(Item.ItemLocation.Worn));

                if (foundItem == null)
                {
                    if (unwield)
                    {
                        HubContext.SendToClient("You do not have that item to unwield.", player.HubGuid);
                        return;
                    }

                    HubContext.SendToClient("You are not wearing that item.", player.HubGuid);
                    return;
                }

                foundItem.location = Item.ItemLocation.Inventory;
                var value = string.Empty;
                var slot = Enum.GetName(typeof(Item.EqSlot), foundItem.slot);

                var eqLocation = player.Equipment.GetType().GetProperty(slot);

                if (eqLocation == null)
                {
                    return;
                } // Log error?


                eqLocation.SetValue(player.Equipment, "Nothing");

                if (foundItem.ArmorRating != null)
                {
                    player.ArmorRating -= foundItem.ArmorRating.Armour;
                }

                if (!unwield || !slot.Equals("wield", StringComparison.CurrentCultureIgnoreCase))
                {
                    HubContext.SendToClient("You remove " + foundItem.name, player.HubGuid);

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} removes {article} {foundItem.name}";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }
                }
                else
                {
                    HubContext.SendToClient("You unwield " + foundItem.name, player.HubGuid);

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} unwields {article} {foundItem.name}";

                            HubContext.SendToClient(roomMessage, character.HubGuid);
                        }
                    }
                }
                Score.UpdateUiInventory(player);
                Score.ReturnScoreUI(player);
                Score.UpdateUiEquipment(player);
                if (replaceWithOtherEQ)
                {
                    return; // we don't need to update the cache
                }


                Cache.updatePlayer(player, oldPlayer);

            }
            else
            {

                var oldPlayer = player;
                var wornItems = player.Inventory.Where(x => x.location.Equals(Item.ItemLocation.Worn));
                foreach (var item in wornItems)
                {

                    if (item.ArmorRating != null)
                    {
                        player.ArmorRating -= item.ArmorRating.Armour;
                    }

                    if (item.eqSlot != Item.EqSlot.Wielded)
                    {
                        HubContext.SendToClient("You remove " + item.name, player.HubGuid);

                        var result = AvsAnLib.AvsAn.Query(item.name);
                        string article = result.Article;

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} remove {article} {item.name}";

                                HubContext.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                    }
                    else
                    {
                        HubContext.SendToClient("You unwield " + item.name, player.HubGuid);

                        var result = AvsAnLib.AvsAn.Query(item.name);
                        string article = result.Article;

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} unwields {article} {item.name}";

                                HubContext.SendToClient(roomMessage, character.HubGuid);
                            }
                        }
                    }

                    item.location = Item.ItemLocation.Inventory;
                    var value = string.Empty;
                    var slot = Enum.GetName(typeof(Item.EqSlot), item.slot);

                    var eqLocation = player.Equipment.GetType().GetProperty(slot);

                    if (eqLocation == null)
                    {
                        return;
                    } // Log error?


                    eqLocation.SetValue(player.Equipment, "Nothing");


                }

                if (!wornItems.Any())
                {
                    HubContext.SendToClient("You aren't wearing anything to remove.", player.HubGuid);
                }

                Score.UpdateUiInventory(player);
                Score.ReturnScoreUI(player);
                Cache.updatePlayer(player, oldPlayer);
                Score.UpdateUiEquipment(player);
            }

        }





    }
}
