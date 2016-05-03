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
    using MIMEngine.Core;

    public class MIMHub : Hub
    {

        public PlayerSetup PlayerData { get; set; }

        public void Welcome()
        {

            var motd = Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
            SendToClient(motd, true);


        }

        public void charSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {
            //Creates and saves player
            PlayerSetup playerData = new PlayerSetup(id, name, email, password, gender, race, selectedClass, strength, dexterity, constitution, wisdom, intelligence, charisma);
            this.PlayerData = playerData;
            playerData.SavePlayerInformation();
        }



        public void recieveFromClient(string message)
        {
            //MIMEngine.Core.PlayerSetup.PlayerAccount.Login(message);
            this.SendToClient(message);
            Command.ParseCommand(message, this.PlayerData);
            //   MIMEngine.Core.Command.commands(message);

        }

        public void getStats()
        {
            PlayerStats playerStats = new PlayerStats();

            int[] stats = playerStats.rollStats();
            Clients.Caller.setStats(stats);
        }

        public void characterSetupWizard(string value, string step)
        {

         
            if (step == "race")
            {
                var selectedRace = PlayerRace.selectRace(value);
                Clients.Caller.updateCharacterSetupWizard("race", selectedRace.name, selectedRace.help, selectedRace.imgUrl);
            }
            else if (step == "class")
            {
                var selectedClass = PlayerClass.selectClass(value);
                Clients.Caller.updateCharacterSetupWizard("class", selectedClass.name, selectedClass.description, selectedClass.imgUrl);
            }

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