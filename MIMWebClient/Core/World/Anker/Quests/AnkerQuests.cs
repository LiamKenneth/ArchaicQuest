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
using MIMWebClient.Core.World.Items.Weapons.DaggerBasic;

namespace MIMWebClient.Core.World.Anker.Quests
{
    public class AnkerQuests
    {
        public static Quest TutorialLeatherQuest()
        {

            var quest = new Quest()
            {
                Name = "This goblin camp may have some useful items that may help you.",
                Description = "<div class='questColor'>" +
                              "<p>To find them, here are some things that you need to know:</p>" +
                              "<ul><li>Kill &lt;target&gt; - example: kill goblin.</li></ul>" +
                              "<p>After the fight you will want to check the corpse for items:</p>" +
                              "<ul><li>look in &lt;item&gt; - example: look in corpse. To list items on the corpse.</li>" +
                              "<li>get &lt;item&gt; corpse - example: get sword corpse. To get the sword from the corpse.</li></ul>" +
                              "<p>Some items may be inside containers that need opening:</p>" +
                              "<ul><li>open &lt;container&gt; - example: open chest. Will now allow to view items inside the chest when you look in to it.</li></ul>" +
                              "<p>For hidden items you should read the room description carefully and examine objects you see in the description, doing so may reveal a hidden item.</p>" +
                              "<ul><li>examine &lt;keyword&gt; - example: examine desk. May give you a more detailed description of what's on the table that's not shown in the room description.</li></ul>" +
                              "<p>If you need help on commands use the help command to get a list of them. </p>" +
                              "<p>Also you may with to <a href='https://discord.gg/nuf7FVq' target='_blank'>join the discord community</a> and give your feedback. </p>" +
                              "<p>ArchaicQuest is in development and your input and feedback will really help make this game better for everyone.</p>" +
                              "<p>Good luck.</p></div>"
                ,


                RewardXp = 500,
                RewardGold = 20,
                Type = Quest.QuestType.FindItem,
                QuestItem = new List<Item.Item>() {
                    LeatherHead.LeatherHelmet(),
                    LeatherBody.LeatherVest(),
                    LeatherLegs.LeatherLeggings(),
                    LeatherArms.LeatherSleeves(),
                    LeatherFeet.LeatherBoots(),
                    LeatherHands.LeatherGloves()
                },
                 RewardDialog = new DialogTree()
                {
                    Message = "Well done, a curved dagger appears in your inventory."
                },
                 RewardItem = DaggerBasic.BronzeCurvedDagger()
                
            };

            return quest;
        }
    }
}