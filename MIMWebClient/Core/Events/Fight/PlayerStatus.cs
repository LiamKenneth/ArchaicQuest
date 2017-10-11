using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events.Fight
{
    public class PlayerStatus
    {

        public bool CheckPlayerStatus(PlayerSetup.Player player)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                HubContext.Instance.SendToClient("You can't do that while asleep.", player.HubGuid);
            }

            if (player.Status == PlayerSetup.Player.PlayerStatus.Dead)
            {
                HubContext.Instance.SendToClient("You can't do that as a Ghost.", player.HubGuid);
            }

           
            return false;
        }
    }
}