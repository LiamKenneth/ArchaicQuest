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
        public List<string> Keyword { get; set; }
        public string MatchPhrase { get; set; }
        public string Response { get; set; }
    }
}