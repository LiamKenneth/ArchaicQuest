using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MIMEngine;

namespace MIM
{
    public class MIMHub : Hub
    {

        public void Welcome()
        {
            
            var motd = MIMEngine.Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
            Clients.Caller.addNewMessageToPage(motd);

            Clients.Caller.addNewMessageToPage("Greetings, what is your name?");
        }

        public void Send(string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.addNewMessageToPage(message);
        }
    }
}