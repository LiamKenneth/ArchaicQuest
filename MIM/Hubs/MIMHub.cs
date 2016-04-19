using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MIMEngine;
using MIMEngine.Core.PlayerSetup;

namespace MIM
{
    public class MIMHub : Hub
    {

        public void Welcome()
        {
            
            var motd = MIMEngine.Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
           // Clients.Caller.addNewMessageToPage(motd);

            Clients.Caller.createCharacter();

        }

        public void charSetup(string name, string sex, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {
            PlayerSetup player = new PlayerSetup(name, sex, selectedClass, strength, dexterity, constitution, wisdom, intelligence, charisma);
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