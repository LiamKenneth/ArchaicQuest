using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player.Skills;

namespace MIMWebClient.Core.Events
{
    public class Help
    {

        public string Syntax;
        public string HelpText;
        public string DateUpdated;
        public List<Help> RelatedHelpFiles;

        private static Dictionary<string, Help> HelpList()
        {
            var helpList = new Dictionary<String, Help>
            {
                {"Punch", Punch.PunchSkill.HelpText}
            };

            return helpList;
        }

        public static void ShowHelp(string keyword, PlayerSetup.Player player)
        {
            var help = HelpList().FirstOrDefault(x => x.Key.StartsWith(keyword, StringComparison.InvariantCultureIgnoreCase));

            var helpText = help.Value.Syntax + " " + help.Value.HelpText;

            HubContext.SendToClient(helpText, player.HubGuid);
        }

    }
}