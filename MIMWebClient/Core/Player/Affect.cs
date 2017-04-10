using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player
{

    /// <summary>
    /// When to check and use affects:
    /// Fighting : haste, weaken, blind
    /// 
    /// </summary>
    public class Affect
    {
        public string Name { get; set; }
        public int Duration { get; set; }

        public static void Show(PlayerSetup.Player player)
        {
            if (player.Affects != null)
            {
                HubContext.SendToClient("You are affected by the following affects:", player.HubGuid);

                foreach (var affect in player.Affects)
                {
                    HubContext.SendToClient(affect.Name + " (" + affect.Duration + ") ticks ", player.HubGuid);
                }
            }
            else
            {
                HubContext.SendToClient("You are not affected by anything", player.HubGuid);
            }
        }

    }
}