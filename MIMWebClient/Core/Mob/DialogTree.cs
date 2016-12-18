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
        public string Message { get; set; }
        public string MatchPhrase { get; set; }
        public List<Responses> PossibleResponse { get; set; }
    }
}