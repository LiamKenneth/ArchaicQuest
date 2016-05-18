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
    using MIMEngine.Core.Events;
    using MIMEngine.Core.Room;
    using MongoDB.Bson;
    using Newtonsoft.Json.Linq;

    class Server
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
        public static ConcurrentDictionary<string, Player> _PlayerCache = new ConcurrentDictionary<string, Player>();
        public static ConcurrentDictionary<int, Room> _AreaCache = new ConcurrentDictionary<int, Room>();
       // public static ConcurrentDictionary<int, string> _RoomCache = new ConcurrentDictionary<int, string>();

        public static Player PlayerData { get; set; }


        public void Welcome()
        {
            var motd = Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
            //  SendToClient(motd, true);            
        }

        #region input from user
        public void recieveFromClient(string message, String playerGuid)
        {

            this.SendToClient(message);

            Player PlayerData;
            Room RoomData;
            _PlayerCache.TryGetValue(playerGuid, out PlayerData);
            _AreaCache.TryGetValue(0, out RoomData);

            Command.ParseCommand(message, PlayerData, RoomData);

        }
        #endregion

        #region load and display room
        public string ReturnRoom(string id)
        {
            Player player;

            if (_PlayerCache.TryGetValue(id, out player))
            {
                string room = string.Empty;
                LoadRoom roomJSON = new LoadRoom();

                roomJSON.Region = player.Region;
                roomJSON.Area = player.Area;
                roomJSON.id = player.AreaId;

                Room roomData;


                if (_AreaCache.TryGetValue(roomJSON.id, out roomData))
                {


                    room = LoadRoom.DisplayRoom(roomData);

                }
                else
                {

                    roomData = roomJSON.LoadRoomFile();
                    _AreaCache.TryAdd(roomJSON.id, roomData);
                    room = LoadRoom.DisplayRoom(roomData);

                }

                return room;
            }

            return null;
        }

        public void loadRoom(string id)
        {

            string roomData = ReturnRoom(id);

            this.Clients.Caller.addNewMessageToPage(roomData, true);

        }

        #endregion

        #region send data to player

        public void SendToClient(string message)
        {
            Clients.All.addNewMessageToPage(message);
        }

        public void SendToClient(string message, bool caller)
        {
            Clients.Caller.addNewMessageToPage(message);
        }
        #endregion


        #region  Character Wizard & Setup

        public void charSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {

            //Creates and saves player
            PlayerData = new Player(id, name, email, password, gender, race, selectedClass, strength, dexterity, constitution, wisdom, intelligence, charisma);

            PlayerData.SavePlayerInformation();

            JObject playerJson = PlayerData.ReturnPlayerInformation();

            _PlayerCache.TryAdd(id, PlayerData);

            loadRoom(id);
            //add player to room
            Room roomData = null;
            _AreaCache.TryGetValue(PlayerData.AreaId, out roomData);

            MIMEngine.Core.Room.PlayerManager.AddPlayerToRoom(roomData, PlayerData);
            Movement.EnterRoom(PlayerData);

           // addToRoom(PlayerData.AreaId, roomData, PlayerData, "player");


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

        #endregion
    }
}
