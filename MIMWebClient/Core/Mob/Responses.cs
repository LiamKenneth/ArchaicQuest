using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob
{
    public class Responses
    {
        /// <summary>
        /// Match Dialog tree ID
        /// </summary>
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }
        public int QuestId { get; set; } = 0;
        public List<string> Keyword { get; set; }
        public string MatchPhrase { get; set; }
        public string Response { get; set; }
        public bool? GiveQuest { get; set; } = false;
        public bool? GivePrerequisiteItem { get; set; } = false;
        public List<string> ShowIfOnQuest { get; set; } = new List<string>();
        public int ShowIfLevelUpTo { get; set; } = 0;
        public bool ShowIfEvil { get; set; } = false;
        public bool ShowIfGood { get; set; } = false;
        public bool ShowIfNeutral { get; set; } = false;


    }
}