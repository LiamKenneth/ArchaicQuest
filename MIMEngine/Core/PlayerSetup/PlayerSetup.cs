using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    public class PlayerSetup : Hub
    {
        public static string CharacterSetup()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hub>();
            hubContext.Clients.All.Send("What Race would you like to be?");

            hubContext.Clients.All.Send("1. Human, 2. Elf");
            return null;
        }
    }
}
