using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Events
{
    public class Repair
    {
        public static void RepairItem(PlayerSetup.Player player, Room.Room room, string commandOptions)
        {
            var hasSkill = player.Skills.FirstOrDefault(x => x.Name.Equals("Repair"));

            if (hasSkill == null)
            {
                HubContext.Instance.SendToClient("You don't know how to do that.", player.HubGuid);
                return;
            }


            if (string.IsNullOrEmpty(commandOptions))
            {

                return;
            }

            var item = FindNth.Findnth(commandOptions);
            var foundItem = FindItem.Item(player.Inventory, item.Key, item.Value, Item.Item.ItemLocation.Inventory);
            var repairHammer =
                player.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(player.Equipment.Held.ToLower()));

            if (foundItem == null)
            {

                HubContext.Instance.SendToClient("You don't have " + Helpers.ReturnName(null, null, item.Value), player.HubGuid);
                return;
            }

            if (repairHammer != null && repairHammer.type == Item.Item.ItemType.Repair)
            {
                var chance = Helpers.Rand(1, 100);

                if (hasSkill.Proficiency >= chance)
                {
                    foundItem.Condition += Helpers.Rand(10, 25);

                    HubContext.Instance.SendToClient(Helpers.ReturnName(null, null, item.Value) + " looks to be in a better condition.", player.HubGuid);

                    if (foundItem.Condition >= 75)
                    {
                        foundItem.name.Replace("broken", String.Empty);
                        foundItem.Condition = 75;
                        HubContext.Instance.SendToClient(Helpers.ReturnName(null, null, item.Value) + " has been fully repaired.", player.HubGuid);
                        return;
                    }
                }
                else
                {
                    HubContext.Instance.SendToClient("You fail to improve the condition of " + Helpers.ReturnName(null, null, item.Value), player.HubGuid);

                    HubContext.Instance.SendToClient("You learn from your mistakes and gain 100 experience poitns", player.HubGuid);

                    hasSkill.Proficiency += Helpers.Rand(1, 5);
                    player.Experience += 100;

                    var xp = new Experience();

                    xp.GainLevel(player);
                }

                repairHammer.Uses -= 1;

                if (repairHammer.Uses <= 0)
                {
                    HubContext.Instance.SendToClient("The repair hammer falls to pieces in your hand.", player.HubGuid);
                    player.Inventory.Remove(repairHammer);
                    player.Equipment.Held = "Nothing";
                }

                Score.UpdateUiInventory(player);
                Score.ReturnScoreUI(player);

            }
            else
            {

                HubContext.Instance.SendToClient("You need to equip a repair hammer", player.HubGuid);
                return;
            }



        }

    }
}