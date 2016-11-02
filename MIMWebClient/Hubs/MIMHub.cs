using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using MIMWebClient.Core.PlayerSetup;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Player;
using MIMWebClient.Core;
using MIMWebClient;

namespace MIMWebClient.Hubs
{
    using Castle.Core.Logging;

    public class MIMHub : Hub
    {
        public static ConcurrentDictionary<string, Player> _PlayerCache = new ConcurrentDictionary<string, Player>();
        public static ConcurrentDictionary<int, Room> _AreaCache = new ConcurrentDictionary<int, Room>();
        public static ConcurrentDictionary<string, Player> _ActiveMobCache = new ConcurrentDictionary<string, Player>();

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


            Player PlayerData;
            Room RoomData;
            _PlayerCache.TryGetValue(playerGuid, out PlayerData);
            _AreaCache.TryGetValue(PlayerData.AreaId, out RoomData);

            HubContext.SendToClient("<p style='color:#999'>" + message + "<p/>", PlayerData.HubGuid);

            Command.ParseCommand(message, PlayerData, RoomData);


        }
        #endregion

        #region load and display room
        public static Room getRoom(string playerId)
        {
            Player player;

            if (_PlayerCache.TryGetValue(playerId, out player))
            {

                var RoomData = new LoadRoom
                {
                    Region = player.Region,
                    Area = player.Area,
                    id = player.AreaId
                };


                Room getRoomData = null;
                if (_AreaCache.TryGetValue(RoomData.id, out getRoomData))
                {

                    return getRoomData;

                }
                else
                {
                    getRoomData = RoomData.LoadRoomFile();
                    _AreaCache.TryAdd(RoomData.id, getRoomData);


                    return getRoomData;
                }
            }

            return null;
        }

        public string ReturnRoom(string id)
        {
            Player player;

            if (_PlayerCache.TryGetValue(id, out player))
            {
                string room = string.Empty;
                var roomJSON = new LoadRoom();

                roomJSON.Region = player.Region;
                roomJSON.Area = player.Area;
                roomJSON.id = player.AreaId;

                Room roomData;

                if (_AreaCache.TryGetValue(roomJSON.id, out roomData))
                {


                    room = LoadRoom.DisplayRoom(roomData, player.Name);

                }
                else
                {

                    roomData = roomJSON.LoadRoomFile();
                    _AreaCache.TryAdd(roomJSON.id, roomData);
                    room = LoadRoom.DisplayRoom(roomData, player.Name);

                }

                return room;
            }

            return null;
        }

        public void SaveRoom(Room room)
        {

            _AreaCache.TryAdd(room.areaId, room);


        }

        public void loadRoom(Player playerData, string id)
        {

            string roomData = ReturnRoom(id);

            this.Clients.Caller.addNewMessageToPage(roomData, true);
               Score.UpdateUiRoom(playerData, roomData);

        }

        #endregion

        #region send data to player
        public void SendToClient(string message)
        {
            Clients.All.addNewMessageToPage(message);
        }

        public void SendToClient(string message, string id)
        {
            Clients.Client(id).addNewMessageToPage(message);
        }
        #endregion


        #region  Character Wizard & Setup

        public void charSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {

            //Creates and saves player
            PlayerData = new Player();

            PlayerData.HubGuid = id;
            PlayerData.Name = name;
            PlayerData.Email = email;
            PlayerData.Password = password;
            PlayerData.Gender = gender;
            PlayerData.Race = race;
            PlayerData.SelectedClass = selectedClass;
            PlayerData.Strength = strength;
            PlayerData.Constitution = constitution;
            PlayerData.Dexterity = dexterity;
            PlayerData.Wisdom = wisdom;
            PlayerData.Intelligence = intelligence;
            PlayerData.Charisma = charisma;


            // PlayerData.SavePlayerInformation();

            _PlayerCache.TryAdd(id, PlayerData);

            loadRoom(PlayerData, id);
            //add player to room
            Room roomData = null;
            _AreaCache.TryGetValue(PlayerData.AreaId, out roomData);

            MIMWebClient.Core.Room.PlayerManager.AddPlayerToRoom(roomData, PlayerData);
            Movement.EnterRoom(PlayerData, roomData);

            Save.SavePlayer(PlayerData);

            // addToRoom(PlayerData.AreaId, roomData, PlayerData, "player");
            Score.ReturnScoreUI(PlayerData);
            Score.UpdateUiPrompt(PlayerData);
            Score.UpdateUiInventory(PlayerData);

        }

        public void Login(string id, string name, string password)
        {

            var player = Save.GetPlayer(name);

            if (player != null)
            {
                //update hubID
                player.HubGuid = id;

                //check for duplicates
                var alreadyLogged = _PlayerCache.FirstOrDefault(x => x.Value.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

                if (alreadyLogged.Value != null)
                {

                    if (alreadyLogged.Value.Name == name)
                    {

                        var oldPlayer = alreadyLogged.Value;
                        _PlayerCache.TryRemove(alreadyLogged.Value.HubGuid, out oldPlayer);

                        _AreaCache.FirstOrDefault(x => x.Value.players.Remove(oldPlayer));

                        //update room cache

                        SendToClient("You have been logged in elsewhere, goodbye", alreadyLogged.Value.HubGuid);
                        SendToClient("Kicking off your old connection", id);
                        HubContext.getHubContext.Clients.Client(alreadyLogged.Value.HubGuid).quit();

                    }
                }

                _PlayerCache.TryAdd(id, player);

                this.loadRoom(player, player.HubGuid);

                //add player to room
                Room roomData = null;
                _AreaCache.TryGetValue(player.AreaId, out roomData);

                PlayerManager.AddPlayerToRoom(roomData, player);
                Movement.EnterRoom(player, roomData);

                //  Prompt.ShowPrompt(player);

                Score.ReturnScoreUI(player);
                Score.UpdateUiPrompt(player);
                Score.UpdateUiInventory(player);
            }
            else
            {
                //something went wrong
            }




        }

        public void getStats()
        {
            var playerStats = new PlayerStats();

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

        public bool validateChar(string id, string name, string password)
        {
            var validateChar = new ValidateChar();

            var valid = validateChar.CharacterisValid(id, name, password);

            return valid;

        }

        public void getChar(string hubId, string name)
        {
            var player = Cache.getPlayer(hubId);

            if (player == null)
            {
                return;
            }

            Score.ReturnScoreUI(player);
        }
    }
}
