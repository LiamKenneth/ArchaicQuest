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
        private static IHubProxy mimHubServer;

        public static IHubProxy MimHubServer
        {
            get
            {
                if (mimHubServer == null)
                {
                    var connection = new HubConnection("http://localhost:4000");
                    mimHubServer = connection.CreateHubProxy("MimHubServer");
                    connection.Start().Wait(); 
                }

                return mimHubServer;
            }
        }





    }
}
