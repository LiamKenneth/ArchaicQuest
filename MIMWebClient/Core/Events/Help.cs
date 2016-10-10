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

        /// <summary>
        /// List of available help files. Help is added to skills, spells and their related classes
        /// Anything generic will be added to the help class
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Help> HelpList()
        {
            var helpList = new Dictionary<string, Help>
            {
                {"Help", GeneralHelp() },
                {"Punch", Punch.PunchAb().HelpText}
            };

            return helpList;
        }

        /// <summary>
        /// Displays Help text to User
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="player"></param>
        public static void ShowHelp(string keyword, PlayerSetup.Player player)
        {
            var help = HelpList().FirstOrDefault(x => x.Key.StartsWith(keyword, StringComparison.InvariantCultureIgnoreCase));

            if (help.Value == null)
            {
                HubContext.SendToClient("No help found for '" + keyword + "'. The gods will be notified.", player.HubGuid);
                //Logs missing help
                return;
            }

            var helpText = help.Value.Syntax + " " + help.Value.HelpText;

            HubContext.SendToClient(helpText, player.HubGuid);
        }

        /// <summary>
        /// General Help to MIM
        /// </summary>
        /// <returns></returns>
        public static Help GeneralHelp()
        {
           var generalHelp = new Help {HelpText = "Welcome to MIM here are a list of commands to get you started...."};

           return generalHelp;
        }
    }
}