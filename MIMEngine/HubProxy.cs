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
            var hubConnection = new HubConnection("http://localhost:4000");
            IHubProxy hubConnectionProxy = hubConnection.CreateHubProxy("MIMHubServer");


            return hubConnectionProxy;
        }

  
    }
}
