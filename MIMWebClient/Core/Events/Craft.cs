﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MIMWebClient.Core.World.Crafting;

namespace MIMWebClient.Core.Events
{

    public enum CraftType
    {
        Craft,
        Chop,
        Cook,
        Brew,
        Smith,
        Forge,
        Carve
    }
    public class CraftMaterials
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }


    public class Craft
    {
        public string Name { get; set; }

        /// <summary>
        /// required Materials
        /// </summary>
        public List<CraftMaterials> Materials { get; set; }

        public List<string> CraftingEmotes { get; set; }

        public CraftType CraftCommand { get; set; } = CraftType.Craft;


        public string StartMessage { get; set; }

        public string Description { get; set; }

        public string SuccessMessage { get; set; }

        public string FaliureMessage { get; set; }

        public Item.Item CreatesItem { get; set; }
        /// <summary>
        /// otherwise player inv
        /// </summary>
        public bool CraftAppearsInRoom { get; set; }

        /// <summary>
        /// duration in ms
        /// </summary>
        public int Duration { get; set; }

        public int MoveCost { get; set; }


        public static void CraftItem(PlayerSetup.Player player, Room.Room room, string craftItem, string craftType = null)
        {
            CanCraftAsync(player, room, craftItem, craftType);
        }

        public static void CraftList(PlayerSetup.Player player)
        {
            if (player.CraftingRecipes == null || player.CraftingRecipes.Count == 0)
            {
                HubContext.Instance.SendToClient("You don't know how to craft anything.", player.HubGuid);
                return;
            }

            var craftlist = "<table><thead><tr><td>Craft Item</td><td>Required</td></tr></thead><tbody>";

            HubContext.Instance.SendToClient("<p>You can craft:</p>", player.HubGuid);

            foreach (var craft in player.CraftingRecipes)
            {
                var canCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.Equals(craft));
                var required = string.Empty;

                if (canCraft?.Materials != null)
                {
                    foreach (var materials in canCraft.Materials)
                    {
                        required += materials.Name + " x"+ materials.Count + ",";
                    }

                    craftlist += "<tr><td>" + canCraft.Name + "</td><td>" + required + "</td></tr>";
                }
            }
            craftlist += "</tbody></table>";

            HubContext.Instance.SendToClient(craftlist, player.HubGuid);
        }

        public static Boolean HasAllMaterials(PlayerSetup.Player player, List<CraftMaterials> materialList, string craftItem)
        {
            foreach (var material in materialList)
            {
                if (player.Inventory.Count(x => x.name.ToLower().Contains(material.Name.ToLower())) < material.Count || player.Inventory.Count(x => x.name.ToLower().Contains(material.Name.ToLower())) == 0)
                {
                    HubContext.Instance.SendToClient("You don't have all the materials required to craft " + Helpers.ReturnName(null, null, craftItem).ToLower() + ".", player.HubGuid);
                    return false;
                }
            }

            return true;
        }


        public static async Task CanCraftAsync(PlayerSetup.Player player, Room.Room room, string craftItem, string craftType)
        {


            if (string.IsNullOrEmpty(craftItem) && craftType == null)
            {
                HubContext.Instance.SendToClient("What do you want to craft?", player.HubGuid);
                return;
            }

            if (string.IsNullOrEmpty(craftItem))
            {
                HubContext.Instance.SendToClient("What do you want to " + craftType + "?", player.HubGuid);
                return;
            }


         
            var findCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.ToLower().Contains(craftItem.ToLower()));

            var hasCraft = player.CraftingRecipes.FirstOrDefault(x => x.ToLower().Contains(craftItem.ToLower()));

            if (hasCraft == null && findCraft == null && craftType == null)
            {
                HubContext.Instance.SendToClient("You don't know how to craft that.", player.HubGuid);
                return;
            }

            if (string.IsNullOrEmpty(hasCraft))
            {
                HubContext.Instance.SendToClient("You don't know how to do that.", player.HubGuid);
                return;
            }
 

            if ((findCraft == null || findCraft.CraftCommand == CraftType.Chop) && craftType == null)
            {
                HubContext.Instance.SendToClient("You can't craft that.", player.HubGuid);
                return;
            }

            if (findCraft.CraftCommand == CraftType.Craft)
            {
                HubContext.Instance.SendToClient("You can't " + craftType + " that.", player.HubGuid);
                return;
            }
 

            bool hasMaterials = false;
            if (craftType == null && findCraft.CraftCommand == CraftType.Craft)
            {
                hasMaterials = findCraft != null && HasAllMaterials(player, findCraft.Materials, hasCraft);

 

            }
            else if (craftType == "chop" && findCraft.CraftCommand == CraftType.Chop)
            {
                hasMaterials = findCraft != null &&
                               room.items.FirstOrDefault(
                                   x => x.name.Equals("Chopping block", StringComparison.CurrentCultureIgnoreCase)) !=
                               null &&
                               player.Inventory.FirstOrDefault(x => x.weaponType == Item.Item.WeaponType.Axe) != null;

                if (!hasMaterials)
                {
                    HubContext.Instance.SendToClient("You don't have all the required materials.", player.HubGuid);
                    return;
                }
            }
            else if (craftType == "cook" && findCraft.CraftCommand == CraftType.Cook)
            {
                var hasIngredients = false;
                foreach (var item in room.items)
                {
                    if (findCraft != null && item.description.room.Contains("fire") || findCraft != null && item.description.room.Contains("stove"))
                    {
                       hasIngredients = true;
                        hasMaterials = true;
                    }
                    
                }
                

                if (!hasIngredients)
                {
                    HubContext.Instance.SendToClient("You don't have all the required ingredients.", player.HubGuid);
                    return;
                }
            

               
            }
            else if (craftType == "brew" && findCraft.CraftCommand == CraftType.Brew)
            {
                var hasIngredients = false;
                foreach (var item in room.items)
                {
                    if (findCraft != null && item.name.Contains("Alchemists work bench"))
                    {
                        hasIngredients = true;
                        hasMaterials = true;
                    }

                }


                if (!hasIngredients)
                {
                    HubContext.Instance.SendToClient("You don't have all the required components.", player.HubGuid);
                    return;
                }



            }
            else if (craftType == "forge" && findCraft.CraftCommand == CraftType.Forge)
            {
                hasMaterials = findCraft != null &&
                               room.items.FirstOrDefault(
                                   x => x.name.Equals("Furnace", StringComparison.CurrentCultureIgnoreCase)) != null;

                if (!hasMaterials)
                {
                    HubContext.Instance.SendToClient("You don't have all the required materials.", player.HubGuid);
                    return;
                }
            }
            else if (craftType == "carve" && findCraft.CraftCommand == CraftType.Carve)
            {
                hasMaterials = findCraft != null &&
                               room.items.FirstOrDefault(
                                   x => x.name.Equals("Carpentry work bench", StringComparison.CurrentCultureIgnoreCase)) != null;

                if (!hasMaterials)
                {
                    HubContext.Instance.SendToClient("You don't have all the required materials.", player.HubGuid);
                    return;
                }
            }

            if (!hasMaterials)
            {
               // HubContext.Instance.SendToClient("You don't have all the required materials.", player.HubGuid);
                return;
            }

            await CraftItem(player, room, findCraft);
 
        }

        public static async Task CraftItem(PlayerSetup.Player player, Room.Room room, Craft craftItem)
        {

            if (player.MovePoints < craftItem.MoveCost)
            {
                if (craftItem.CraftCommand == CraftType.Chop || craftItem.CraftCommand == CraftType.Brew)
                {
                    HubContext.Instance.SendToClient("You are too tired to make " + Helpers.ReturnName(null, null, craftItem.CreatesItem.name).ToLower() + ".", player.HubGuid);
                    return;
                }
 


                HubContext.Instance.SendToClient("You are too tired to make " + Helpers.ReturnName(null, null, craftItem.Name).ToLower() + ".", player.HubGuid);
                return;
            }

            player.MovePoints -= craftItem.MoveCost;

            var oldRoom = room;
            HubContext.Instance.SendToClient(craftItem.StartMessage, player.HubGuid);

            await Task.Delay(1500);

            foreach (var emote in craftItem.CraftingEmotes)
            {
                HubContext.Instance.SendToClient(emote, player.HubGuid);


                await Task.Delay(2000);
            }

            HubContext.Instance.SendToClient(craftItem.SuccessMessage, player.HubGuid);


            if (craftItem.Materials != null)
            {

                foreach (var materials in craftItem.Materials)
                {

                    for (var i = 0; i < materials.Count; i++)
                    {
                        var item = player.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(materials.Name.ToLower()));

                        player.Inventory.Remove(item);
                    }
                }
            }

            if (craftItem.CraftAppearsInRoom)
            {
                room.items.Add(craftItem.CreatesItem);
            }

            if (!craftItem.CraftAppearsInRoom)
            {
                player.Inventory.Add(craftItem.CreatesItem);
            }


            Score.UpdateUiInventory(player);

            Cache.updateRoom(room, oldRoom);

        }
    }
}