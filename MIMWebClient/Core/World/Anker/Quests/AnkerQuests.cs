using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;

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
                
                RewardDialog = new DialogTree()
                {
                    Message = "a weapon appears in your inventory"
                },
                
            };

            return quest;
        }
    }
}