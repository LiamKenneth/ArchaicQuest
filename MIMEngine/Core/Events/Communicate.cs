using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using MIMEngine.Core.PlayerSetup;

    public class Communicate
    {
        public static void Say(string message, Player player, Player recipientPlayer = null)
        {
            if (recipientPlayer == null)
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You say " + message);
                HubContext.getHubContext.Clients.AllExcept(player.HubGuid).addNewMessageToPage(player.Name + " says " + message);
            }
            else
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You say to " + recipientPlayer.Name + " " + message);
                HubContext.getHubContext.Clients.Client(recipientPlayer.HubGuid).addNewMessageToPage(player.Name + " says to you " + message);
                HubContext.getHubContext.Clients.All.addNewMessageToPage(player.Name + "says to " + recipientPlayer.Name + " " + message);
            }
        }
    }
}
