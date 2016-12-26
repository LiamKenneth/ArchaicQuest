using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Mob;

namespace MIMWebClient.Core.Player
{
    public class Quest
    {
        public enum QuestType
        {
            Kill,
            Find,
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
        public Item.Item QuestItem { get; set; }
        public Item.Item PrerequisiteItem { get; set; }
        public string PrerequisiteItemEmote { get; set; }
        public PlayerSetup.Player QuestKill { get; set; }
        public string QuestDo { get; set; }
        public int RewardGold { get; set; }
        public int RewardXp { get; set; }
        public Item.Item RewardItem { get; set; }
        public DialogTree RewardDialog { get; set; }
        public bool Completed { get; set; } = false;
    }
}