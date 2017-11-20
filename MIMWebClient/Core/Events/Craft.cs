using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MIMWebClient.Core.Player;
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
        Carve,
        Knitting,
        Forage
    }
    public class CraftMaterials
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class CraftFailMessage
    {
        public string Message { get; set; }
        public bool BreakMaterial { get; set; }
        public bool DamageMaterial { get; set; }
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

        public List<CraftFailMessage> FailureMessages { get; set; }

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


        public static void CraftItem(PlayerSetup.Player player, Room.Room room, string craftItem, CraftType craftType)
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


        public static async Task CanCraftAsync(PlayerSetup.Player player, Room.Room room, string craftItem, CraftType craftType)
        {


            if (string.IsNullOrEmpty(craftItem) && craftType == CraftType.Craft)
            {
                HubContext.Instance.SendToClient("What do you want to craft?", player.HubGuid);
                return;
            }

            //if (string.IsNullOrEmpty(craftItem))
            //{
            //    HubContext.Instance.SendToClient("What do you want to " + craftType + "?", player.HubGuid);
            //    return;
            //}


         
            var findCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.ToLower().Contains(craftItem.ToLower()));

            var hasCraft = player.CraftingRecipes.FirstOrDefault(x => x.ToLower().Contains(craftItem.ToLower()));

            if (hasCraft == null && findCraft == null)
            {
                HubContext.Instance.SendToClient("You don't know how to craft that.", player.HubGuid);
                return;
            }

            if (string.IsNullOrEmpty(hasCraft))
            {
                HubContext.Instance.SendToClient("You don't know how to do that.", player.HubGuid);
                return;
            }



            bool hasMaterials = false;
            if (craftType ==  CraftType.Craft && findCraft.CraftCommand == CraftType.Craft)
            {
                hasMaterials = findCraft != null && HasAllMaterials(player, findCraft.Materials, hasCraft);

 

            }
            else if (craftType == CraftType.Chop && findCraft.CraftCommand == CraftType.Chop)
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
            else if (craftType == CraftType.Cook && findCraft.CraftCommand == CraftType.Cook)
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
            else if (craftType == CraftType.Brew && findCraft.CraftCommand == CraftType.Brew)
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
            else if (craftType == CraftType.Forge && findCraft.CraftCommand == CraftType.Forge)
            {
                hasMaterials = findCraft != null &&
                               room.items.FirstOrDefault(
                                   x => x.name.Equals("Furnace", StringComparison.CurrentCultureIgnoreCase)) != null && HasAllMaterials(player, findCraft.Materials, hasCraft); ;

                if (!hasMaterials)
                {
                    HubContext.Instance.SendToClient("You don't have all the required materials.", player.HubGuid);
                    return;
                }
            }
            else if (craftType == CraftType.Carve && findCraft.CraftCommand == CraftType.Carve)
            {
                hasMaterials = findCraft != null && HasAllMaterials(player, findCraft.Materials, hasCraft);


                if (room.items.Count == 0 || room.items.FirstOrDefault(
                        x => x.name.Equals("Carpentry work bench", StringComparison.CurrentCultureIgnoreCase)) == null)
                {
                    HubContext.Instance.SendToClient("You need to be at a Carpentry work bench.", player.HubGuid);
                    return;
                }
            }
            else if (craftType == CraftType.Knitting && findCraft.CraftCommand == CraftType.Knitting)
            {
                hasMaterials = findCraft != null && HasAllMaterials(player, findCraft.Materials, hasCraft); ;


                if (room.items.Count == 0 || room.items.FirstOrDefault(
                        x => x.name.Equals("Knitting desk", StringComparison.CurrentCultureIgnoreCase)) == null)
                {
                    HubContext.Instance.SendToClient("You need to be at a Knitting desk.", player.HubGuid);
                    return;
                }
            }

            if (!hasMaterials)
            {
                if (findCraft.CraftCommand != craftType)
                {
                    HubContext.Instance.SendToClient("You can't do that. try " + findCraft.CraftCommand, player.HubGuid);
                    return;
                }

                return;
            }

            await CraftItem(player, room, findCraft);
 
        }

        public static async Task CraftItem(PlayerSetup.Player player, Room.Room room, Craft craftItem)
        {

            //add skill check here
            var getSkill = player.Skills.FirstOrDefault(x => x.Alias.Equals(craftItem.CraftCommand.ToString()));

            if (getSkill == null)
            {
                HubContext.Instance.SendToClient("You don't know how to do that", player.HubGuid);
                return;
            }


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


         
            var successChance = ShowSkills.CraftSuccess(getSkill.Points);

            if (getSkill.Points >= successChance)
            {
                //success
                if (player.ActiveSkill != null)
                {
                    HubContext.Instance.SendToClient("wait till you have finished " + player.ActiveSkill.Name,
                        player.HubGuid);
                    return;
                }

                player.ActiveSkill = getSkill;

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
                            var item = player.Inventory.FirstOrDefault(x => x.name.ToLower()
                                .Contains(materials.Name.ToLower()));

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

                player.ActiveSkill = null;

            }
            else
            {
 
                var failMessage = craftItem.FailureMessages[Helpers.Rand(1, craftItem.FailureMessages.Count)];

                HubContext.Instance.SendToClient(failMessage.Message,
                    player.HubGuid);

                if (failMessage.BreakMaterial)
                {
                    var item = player.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(craftItem
                        .Materials[Helpers.Rand(1, craftItem.Materials.Count)].Name.ToLower()));

                    player.Inventory.Remove(item);

                }

                var xp = new Experience();



                if (getSkill != null)
                {
                    if (player.ChosenCraft == craftItem.CraftCommand.ToString())
                    {
                        getSkill.Points += Helpers.Rand(1, 10);


                        HubContext.Instance.SendToClient("<p class='roomExit'>You learn from your mistakes and gain 100 experience points.</p>",
                            player.HubGuid);

                        player.Experience += 100;

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(player.ChosenCraft) && getSkill.Points <= 99)
                        {

                            HubContext.Instance.SendToClient("<p class='roomExit'>You learn from your mistakes and gain 100 experience points.</p>",
                                player.HubGuid);

                          player.Experience += 100;


                            getSkill.Points +=  Helpers.Rand(1, 10);

                            if (getSkill.Points > 99)
                            {
                                var rankWarning = ShowSkills.checkRank(getSkill.Points, player);
                                HubContext.Instance.SendToClient(rankWarning,
                                    player.HubGuid);

                                getSkill.Points = 99;
                            }

                        }
                        else if (getSkill.Points <= 99)
                        {
                            HubContext.Instance.SendToClient("<p class='roomExit'>You learn from your mistakes and gain 100 experience points.</p>",
                                player.HubGuid);

                            player.Experience += 100;


                            getSkill.Points += Helpers.Rand(1, 10);

                            if (getSkill.Points > 99)
                            {
                              
                                getSkill.Points = 99;
                            }
                        }

                    }
                 
                }

                Score.ReturnScoreUI(player);


                xp.GainLevel(player);


            }
        }


        }
    }
