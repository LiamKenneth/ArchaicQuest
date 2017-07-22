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
                {"Help", HelpStart() },
                {"Help start", HelpStart() },
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
            var help = HelpList().FirstOrDefault(x => x.Key.ToLower().Contains(keyword.ToLower()));

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

        /// <summary>
        /// General Help to MIM
        /// </summary>
        /// <returns></returns>
        public static Help HelpStart()
        {
            var generalHelp = new Help { HelpText =
                                                    "<h2 style='text-align:center'>Commonly used commands</h2>" +
                                                    "<h2>Movement</h2><ul>" +
                                                    "<li>north, east, south, west, up, down</li>" +
                                                    "<li>open, close, lock, unlock, look, look in <thing>, look\\examine <thing></li>" +
                                                    "<li>sleep, wake stand, rest</li></ul>" +
                                                    "<h2>Objects</h2><ul>" +
                                                    "<li>get, put, drop, give</li>" +
                                                    "<li>wield, wear, remove, inventory, equipment</li>" +
                                                    "<li>list, buy, sell, drink</li></ul>" +
                                                    "<h2>Communication</h2><ul>" +
                                                    "<li>say, say to</li>" +
                                                    "<li>newbie, ooc, gossip</li>" +
                                                    "<li>emote</li></ul>" +
                                                    "<h2>Combat</h2><ul>" +
                                                    "<li>kill, cast, flee, skills, spells</li></ul>" +
                                                    "<h2>Game info</h2>" +
                                                    "<ul><li>who</li></ul>" +
                                                    "<h2>Rules</h2>" +
                                                    "<p>Rules here are simple, don't ruin the fun for other people or abuse any bugs.</p>" +
                                                    "<h2>Help Getting started</h2>" +
                                                    "<p>Completing the tutorial     should be simple to do, if you find yourself in Anker wondering what to do next. I sugest you head to Ester and enroll in the adventure acaademy you will learn a lot more and be given many quests to complete. Hopefully you will make a few new friends along the way. Remember to read and examine everything. Many secrets are here to be found.</p>"

            };

            return generalHelp;
        }
    }
}