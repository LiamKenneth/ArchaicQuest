using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    public class PlayerAccount
    {
        public static string Login(string playerName)
        {
          //  var hubClient = GlobalHost.ConnectionManager.GetHubContext<>();

//hubClient.Clients.All.addNewMessageToPage("hey");

            //hubClient.Clients.All
            if (!string.IsNullOrEmpty(playerName) && playerName.Length >= 3)
            {
                PlayerSetup.CharacterSetup();
                return null;
            }
            else
            {
                return "You must enter a name with atleast 3 characters";
            }
        }
    }
}
