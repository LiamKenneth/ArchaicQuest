using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using MIMEngine;
using MIMEngine.Core;
using MIMEngine.Core.PlayerSetup;
using Owin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMHubServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Server...");

            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:4000";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class MimHubServer : Hub
    {
        private static ConcurrentDictionary<string, PlayerSetup> _cache = new ConcurrentDictionary<string, PlayerSetup>();
        public static PlayerSetup PlayerData { get; set; }

        public void Welcome()
        {
 

            var motd = Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
            //  SendToClient(motd, true);            
        }

        public void charSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {

            //Creates and saves player
            PlayerData = new PlayerSetup(id, name, email, password, gender, race, selectedClass, strength, dexterity, constitution, wisdom, intelligence, charisma);

            PlayerData.SavePlayerInformation();

            //Players.addPlayer(id, PlayerData);

            _cache.TryAdd(id, PlayerData);




        }



        public void recieveFromClient(string message, String playerGuid)
        {
            

     
            this.SendToClient(message);

            PlayerSetup PlayerData;
            if (_cache.TryGetValue(playerGuid, out PlayerData))
            {
                Command.ParseCommand(message, PlayerData);
            }

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
