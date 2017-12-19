using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player.Skills
{
    using System.Runtime.CompilerServices;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Lore : Skill
    {

        public static Skill LoreSkill { get; set; }
        public static Skill LoreAb()
        {

            if (LoreSkill != null)
            {
                return LoreSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Lore",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 8,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Lore",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "Lore help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                LoreSkill = skill;
            }

            return LoreSkill;

        }

        public static void DoLore(IHubContext context, PlayerSetup.Player player, string item)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, LoreAb().Name);

            if (hasSkill == false)
            {
                context.SendToClient("You don't know that skill.", player.HubGuid);
                return;
            }

            var canDoSkill = Skill.CanDoSkill(player);

            if (!canDoSkill)
            {
                return;
            }

            if (string.IsNullOrEmpty(item))
            {
                context.SendToClient("You need to specify an item.", player.HubGuid);
                return;
            }

            var hasItem = player.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(item.ToLower()));

            if (hasItem == null)
            {
                context.SendToClient("You don't have such an item.", player.HubGuid);
                return;
            }


            var chanceOfSuccess = Helpers.Rand(1, 100);
            var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Lore"));

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {

                //Itme stats
                HubContext.Instance.SendToClient($"You inspect {Helpers.ReturnName(null, null, hasItem.name)}, twisting and turning it as you do so.", player.HubGuid);

                var itemInfo = new StringBuilder();

                itemInfo.Append("<Table>")
                .Append($"<tr><td>Name:</td><td>{hasItem.name}</td></tr>")
                .Append($"<tr><td>Type:</td><td>{hasItem.type}</td></tr>")
                .Append($"<tr><td>Min Level:</td><td>{hasItem.stats?.minUsageLevel}</td></tr>");

                if (hasItem.type == Item.Item.ItemType.Weapon)
                {
                    itemInfo.Append($"<tr><td>Damage:</td><td>{hasItem.stats.damMin} - {hasItem.stats.damMax}</td></tr>")
                    .Append("<tr><td>Damage Type:</td><td>");

                    foreach (var dt in hasItem.damageType)
                    {
                        itemInfo.Append($"{dt}");
                    }

                    itemInfo.Append("</td></tr>");
                }

                if (hasItem.type == Item.Item.ItemType.Armour)
                {
                    itemInfo.Append($"<tr><td>Armor:</td><td>{hasItem.ArmorRating}</td></tr>");
                    itemInfo.Append($"<tr><td>Armor Type:</td><td>{hasItem.armourType}</td></tr>");
 
                }

                itemInfo.Append($"<tr><td>EQ slot:</td><td>{hasItem.eqSlot}</td></tr>");


                if (hasItem.itemFlags != null)
                {

                    itemInfo.Append("<tr><td>Flags:</td><td>");

                    foreach (var flags in hasItem.itemFlags)
                    {
                        itemInfo.Append($"{flags}");
                    }

                    itemInfo.Append("</td></tr>");
                }

               

                itemInfo.Append($"<tr><td>Weight:</td><td>{hasItem.Weight}</td></tr>")
                    .Append($"<tr><td>Condition:</td><td>{hasItem.Condition}</td></tr>")
                    .Append($"<tr><td>QuestItem:</td><td>{hasItem.QuestItem}</td></tr>")
                    .Append($"<tr><td>Worth:</td><td>{hasItem.Gold}</td></tr></table>");

                HubContext.Instance.SendToClient(itemInfo.ToString(), player.HubGuid);

                Score.UpdateUiAffects(player);
                Score.ReturnScoreUI(player);
            }
            else
            {
                //something random
                HubContext.Instance.SendToClient($"You inspect { Helpers.ReturnName(null, null, hasItem.name)} but you have no idea what's special about it.", player.HubGuid);

                Score.ReturnScoreUI(player);
            }
        }

    }
}
