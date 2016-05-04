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
    using System.Runtime.Caching;

    public class MIMHub : Hub
    {
        ObjectCache cache = MemoryCache.Default;
        public static PlayerSetup PlayerData { get; set; }
        // public Players Players = new Players();
        //private Players player;

        //public Players Player
        //{
        //    get
        //    {
        //        if (this.player == null)
        //        {
        //            this.player = new Players();
        //            return player;
        //        }else
        //        {
        //            return player;
        //        }                
        //    }           
        //}


        public void Welcome()
        {
            var listHolder = new List<string>();
            listHolder.Add("test");
            cache.Set("test", listHolder, null);

            var motd = Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
            //  SendToClient(motd, true);            
        }

        public void charSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {
           
            //Creates and saves player
            PlayerData = new PlayerSetup(id, name, email, password, gender, race, selectedClass, strength, dexterity, constitution, wisdom, intelligence, charisma);

            PlayerData.SavePlayerInformation();

            Players.addPlayer(id, PlayerData);


        }



        public void recieveFromClient(string message, String playerGuid)
        {
            var listHolder = cache["test"] as List<string>;
            listHolder.Add(message);
            cache.Set("test", listHolder, null);
            PlayerData = Players.returnPlayer(playerGuid);
            this.SendToClient(message);
            Command.ParseCommand(message, PlayerData);


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