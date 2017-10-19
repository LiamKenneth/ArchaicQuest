using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MIMWebClient.Core.World.Crafting;

namespace MIMWebClient.Core.Events
{
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

            HubContext.Instance.SendToClient("<p>You can craft:</p>", player.HubGuid);

            foreach (var craft in player.CraftingRecipes)
            {
                var canCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.Equals(craft));
                var required = string.Empty;

                if (canCraft != null)
                {
                    foreach (var materials in canCraft.Materials)
                    {
                        required += materials.Name + " ";
                    }

                    HubContext.Instance.SendToClient("<p>" + canCraft.Name + ", Required: " + required + "</p>", player.HubGuid);
                }
            }
        }

        public static Boolean HasAllMaterials(PlayerSetup.Player player, List<CraftMaterials> materialList)
        {
            foreach (var material in materialList)
            {
                if (player.Inventory.Count(x => x.name.Contains(material.Name)) <= material.Count || player.Inventory.Count(x => x.name.Contains(material.Name)) == 0)
                {
                    HubContext.Instance.SendToClient("You don't have all the materials required to craft " + Helpers.ReturnName(null, null, material.Name) + ".", player.HubGuid);
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
            else if ((string.IsNullOrEmpty(craftItem) && craftType == "chop"))
            {
                HubContext.Instance.SendToClient("What do you want to chop?", player.HubGuid);
                return;
            }

            var findCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.ToLower().Contains(craftItem.ToLower()));

            var hasCraft = player.CraftingRecipes.FirstOrDefault(x => x.ToLower().Contains(craftItem.ToLower()));

            if (hasCraft == null && craftType == null)
            {
                HubContext.Instance.SendToClient("You don't know how to craft that.", player.HubGuid);
                return;
            }
            else if ((string.IsNullOrEmpty(hasCraft) && craftType == "chop"))
            {
                HubContext.Instance.SendToClient("You don't know how to do that.", player.HubGuid);
                return;
            }

            bool hasMaterials = false;
            if (craftType == null)
            {
                hasMaterials = findCraft != null && HasAllMaterials(player, findCraft.Materials);
            }
            else if (craftType == "chop")
            {
                hasMaterials = findCraft != null &&
                               room.items.FirstOrDefault(
                                   x => x.name.Equals("Chopping block", StringComparison.CurrentCultureIgnoreCase)) !=
                               null &&
                               player.Inventory.FirstOrDefault(x => x.weaponType == Item.Item.WeaponType.Axe) != null;
            }

            if (!hasMaterials)
            {
                HubContext.Instance.SendToClient("You don't have everything you need to make that.", player.HubGuid);
                return;
            }

            await CraftItem(player, room, findCraft);
 
        }

        public static async Task CraftItem(PlayerSetup.Player player, Room.Room room, Craft craftItem)
        {

            if (player.MovePoints < craftItem.MoveCost)
            {
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


                await Task.Delay(1500);
            }

            HubContext.Instance.SendToClient(craftItem.SuccessMessage, player.HubGuid);


            foreach (var materials in craftItem.Materials)
            {

                for (var i = 0; i < materials.Count; i++)
                {
                    var item = player.Inventory.FirstOrDefault(x => x.name.Equals(materials.Name));

                    player.Inventory.Remove(item);
                }
            }

            if (craftItem.CraftAppearsInRoom)
            {
                room.items.Add(craftItem.CreatesItem);
            }


            Cache.updateRoom(room, oldRoom);

        }
    }
}