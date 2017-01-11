using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob
{
    public class DialogTree
    {
        /// <summary>
        /// ID = MobName + int e.g Modo1
        /// </summary>
        public string Id { get; set; }
        public int? QuestId { get; set; }
        public string Message { get; set; }
        public string MatchPhrase { get; set; }
        public bool? GiveQuest { get; set; } = false;
        public bool? GivePrerequisiteItem { get; set; } = false;
        public List<Responses> PossibleResponse { get; set; }
        public bool? DoAction { get; set; } = false;
        public string ShowIfOnQuest { get; set; } = String.Empty;
        public int ShowIfLevelUpTo { get; set; } = 0;

    }
}