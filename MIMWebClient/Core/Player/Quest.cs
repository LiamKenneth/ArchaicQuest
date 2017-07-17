using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Mob;

namespace MIMWebClient.Core.Player
{
    public class Quest
    {
        public enum QuestType
        {
            Kill,
            FindItem,
            FindMob,
            Act
        }

        /* Bring me a beer
         * 
         * Go to Modo and buy a bear
         * 
         * Find
         * 
         * 1
         * 
         * beer
         * 
         */

        /* if kill use questkill to confirm kill
         * 
         * if find use questItem
         * 
         * if act use quest do??
         * 
         * 
         */
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public QuestType Type { get; set; }
        public string QuestGiver { get; set; }
        public int QuestCount { get; set; } = 1;
        /// <summary>
        /// Quest complete when player gains item
        /// </summary>
        public List<Item.Item> QuestItem { get; set; }
        public List<Item.Item> PrerequisiteItem { get; set; }
        public string PrerequisiteItemEmote { get; set; }

        /// <summary>
        /// Quest is completed when player kills mob
        /// </summary>
        public PlayerSetup.Player QuestKill { get; set; }

        /// <summary>
        /// Quest is completed when player finds mob
        /// string so match on description
        /// was on matching mob data but if player gives mob
        /// an item or change the mob in anyway 
        /// it will brake the quest x_x
        /// </summary>
        public string QuestFindMob { get; set; }


        public int RewardGold { get; set; }
        public int RewardXp { get; set; }
        public Item.Item RewardItem { get; set; }
        public DialogTree RewardDialog { get; set; }
        public string AlreadyOnQuestMessage { get; set; }
        public string QuestHint { get; set; }
        public string QuestTrigger { get; set; }
        public bool Completed { get; set; } = false;

        public static void QuestLog(PlayerSetup.Player player)
        {
            if (player.QuestLog.Count > 0)
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("Your current quests");

                foreach (var quest in player.QuestLog)
                {
                    if (!quest.Completed)
                    {
                        HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(quest.Name);
                        HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(quest.Description);
                    }
                }
            }
            else
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You have no quests.");
            }

        }


        public static void CheckIfGetItemQuestsComplete(PlayerSetup.Player player)
        {
            var quests = player.QuestLog.Where(x => x.Type == Quest.QuestType.FindItem && x.Completed == false);
            var playerInvQuestItems = player.Inventory.Where(x => x.QuestItem);
            var invQuestItems = playerInvQuestItems as IList<Item.Item> ?? playerInvQuestItems.ToList();
            foreach (var findQuest in quests)
            {
              
                var compelete = false;
                var currentItemsForQuest = new List<Item.Item>();
                foreach (var questItem in findQuest.QuestItem)
                {
                    compelete = invQuestItems.Any(x => x.name.Equals(questItem.name, StringComparison.CurrentCultureIgnoreCase));

                    if (compelete)
                    {
                        currentItemsForQuest.Add(questItem);
                    }
                }

                if (compelete && currentItemsForQuest.Count == findQuest.QuestItem.Count)
                {
                    findQuest.Completed = true;

                    HubContext.SendToClient(findQuest.RewardDialog.Message, player.HubGuid);
                    HubContext.SendToClient("You gain " + findQuest.RewardXp, player.HubGuid);
                    HubContext.SendToClient("You gain " + findQuest.RewardGold, player.HubGuid);

                    player.Experience += findQuest.RewardXp;
                    player.Gold = findQuest.RewardGold;

                    //check if player level
                    //update ui
                    Score.ReturnScoreUI(player);
                   
                }

            }
 
        }
    }
}