using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob
{
    public class Responses
    {
        public string QuestionId { get; set; }
        public List<string> Keyword { get; set; }
        public string Response { get; set; }
    }
}