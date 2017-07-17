using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Arms;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Body;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Feet;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Hands;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Head;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Legs;

namespace MIMWebClient.Core.World.Anker.Quests
{
    public class AnkerQuests
    {
        public static Quest TutorialLeatherQuest()
        {

            var quest = new Quest()
            {
                Name = "Get Equipped!",
                Description = "Find and wear all the peices of leather armour.",
                RewardXp = 500,
                Type = Quest.QuestType.FindItem,
                QuestItem = new List<Item.Item>()
                {
                    LeatherHead.LeatherHelmet(),
                    LeatherBody.LeatherVest(),
                    LeatherLegs.LeatherLeggings(),
                    LeatherArms.LeatherSleeves(),
                    LeatherFeet.LeatherBoots(),
                    LeatherHands.LeatherGloves()
                },
                 RewardDialog = new DialogTree()
                {
                    Message = "a weapon appears in your inventory"
                },
                
            };

            return quest;
        }
    }
}