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
                Description = "<div class='questColor'><p>Loot the goblin camp, there is 6 pieces of leather armour hidden here that you can use.</p>" +
                              "<ul><li>Leather Helmet</li>" +
                              "<li>Leather vest</li>" +
                              "<li>Leather sleeves</li>" +
                              "<li>Leather gloves</li>" +
                              "<li>Leather leggings</li>" +
                              "<li>Leather boots</li></ul>" +
                              "<p>You will need to look and examine to find all these items.</p></div>",
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
                    Message = "a weapon appears in your inventory...test"
                },
                
            };

            return quest;
        }
    }
}