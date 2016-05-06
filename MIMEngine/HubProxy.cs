using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine
{
    class HubProxy
    {

        public static IHubProxy HubContext()
        {
            var connection = new HubConnection("http://localhost:4000");
            IHubProxy MimHubServer = connection.CreateHubProxy("MimHubServer");

            connection.Start().Wait();

            return MimHubServer;
        }

  
    }
}
