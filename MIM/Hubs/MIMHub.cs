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


        public void createPlayerName(string playerName)
        {
         
            if (!string.IsNullOrEmpty(playerName) && playerName.Length >= 3)
            {
                Clients.All.addNewMessageToPage("Thanks, {0} what class will you want to be?", playerName);

            }
            else
            {
                SendToClient("You must enter a name with atleast 3 characters");
            }
        }

        public void createPlayerClass(string playerClass)
        {

            if (!string.IsNullOrEmpty(playerClass) && playerClass.Length >= 3)
            {
                Clients.All.addNewMessageToPage("yep all good");

            }
            else
            {
                SendToClient("You must enter a name with atleast 3 characters");
            }
        }

        public void recieveFromClient(string message)
        {
            //MIMEngine.Core.PlayerSetup.PlayerAccount.Login(message);
            SendToClient(message);
            bool commandParser = false;
            bool loggedIn = false;

            if (commandParser && loggedIn)
            {
                //check command
            }
            else
            {
                createPlayerName(message);
                // sign player up
            }
           
           
        }


        public void SendToClient(string message)
        {
            Clients.All.addNewMessageToPage(message);
        }

        public void SendToClient(string message, bool caller)
        {
            Clients.Caller.addNewMessageToPage(message);
        }
    }
}