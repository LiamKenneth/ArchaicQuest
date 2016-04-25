using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MIMEngine;
using MIMEngine.Core.PlayerSetup;
using Newtonsoft.Json;

namespace MIM
{
    public class MIMHub : Hub
    {

        public void Welcome()
        {
           // var selectedRace = PlayerRace.selectRace("hum");
            var motd = Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
           // Clients.Caller.addNewMessageToPage(selectedRace.name);
           // Clients.Caller.addNewMessageToPage(selectedRace.help);

            // Clients.Caller.createCharacter();

        }

        public void charSetup(string name, string sex, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {
            PlayerSetup player = new PlayerSetup(name, sex, selectedClass, strength, dexterity, constitution, wisdom, intelligence, charisma);
        }
        

       
        public void recieveFromClient(string message)
        {
            //MIMEngine.Core.PlayerSetup.PlayerAccount.Login(message);
            SendToClient(message);
         //   MIMEngine.Core.Command.commands(message);






        }

        public void getStats()
        {
            PlayerStats playerStats = new PlayerStats();

            int[] stats = playerStats.rollStats();
            Clients.Caller.setStats(stats);
        }

        public void pickRace(string race)
        {

            var selectedRace = PlayerRace.selectRace(race);


            Clients.Caller.updateRaceInfo(selectedRace.name, selectedRace.help);

        }

        public void loadRoom()
        {
            MIMEngine.Core.Events.LoadRoom room = new MIMEngine.Core.Events.LoadRoom();

            Clients.Caller.addNewMessageToPage(room.DisplayRoom());
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