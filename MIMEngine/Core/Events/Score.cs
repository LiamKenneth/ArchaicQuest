using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using MIMEngine.Core.PlayerSetup;

    using Newtonsoft.Json.Linq;

    class Score
    {

        public static PlayerSetup PlayerData { get; set; }

        public static void ReturnScore(PlayerSetup playerData)
        {
            string scoreTest = "Score:\r\n Name: " + PlayerData.name + " Race: " + PlayerData.race;

            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<MIMHub>();
            //hubContext.Clients.Client(playerData.hubGuid).SendToClient(scoreTest, true);
        }
    }
}
