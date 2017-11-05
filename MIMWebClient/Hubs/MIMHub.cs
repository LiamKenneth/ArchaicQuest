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
using MIMWebClient.Controllers;
using MIMWebClient.Core.AI;
using MIMWebClient.Core.World.Crafting;
using MIMWebClient.Core.World.Crafting.Smithing;
using MIMWebClient.Core.World.Tutorial;

namespace MIMWebClient.Hubs
{
    using Castle.Core.Logging;

    public class MIMHub : Hub
    {
        public static ConcurrentDictionary<string, Player> _PlayerCache = new ConcurrentDictionary<string, Player>();
        public static ConcurrentDictionary<Tuple<string, string, int>, Room> _AreaCache = new ConcurrentDictionary<Tuple<string, string, int>, Room>();
        public static ConcurrentDictionary<string, Player> _ActiveMobCache = new ConcurrentDictionary<string, Player>();

        public static Player PlayerData { get; set; }


        public void Welcome(string id)
        {
            var motd = Data.loadFile("/motd");
            // Call the broadcastMessage method to update clients.
                SendToClient(motd, id);            
        }


        #region updateDescription
        public void updateDescription(string description, String playerGuid)
        {


            Player PlayerData;
         
            _PlayerCache.TryGetValue(playerGuid, out PlayerData);


            Score.UpdateDescription(PlayerData, description);
            //save desc in db
            //update desc UI


        }
        #endregion


        #region input from user
        public void recieveFromClient(string message, String playerGuid)
        {


            Player PlayerData;
            Room RoomData;
            _PlayerCache.TryGetValue(playerGuid, out PlayerData);



            var room = new Tuple<string, string, int>(PlayerData.Region, PlayerData.Area, PlayerData.AreaId);




            if (_AreaCache.TryGetValue(room, out RoomData))
            {
                _AreaCache.TryGetValue(room, out RoomData);


            }
            else
            {

                var RoomLoadData = new LoadRoom
                {
                    Region = PlayerData.Region,
                    Area = PlayerData.Area,
                    id = PlayerData.AreaId
                };
                RoomData = RoomLoadData.LoadRoomFile();
                _AreaCache.TryAdd(room, RoomData);



            }

            HubContext.Instance.SendToClient("<p style='color:#999'>" + message + "<p/>", PlayerData.HubGuid);


            Command.ParseCommand(message, PlayerData, RoomData);




        }
        #endregion

        #region load and display room
        public static Room getRoom(Player Thing)
        {
            Player player;

            if (Thing.HubGuid != null)
            {
                if (_PlayerCache.TryGetValue(Thing.HubGuid, out player))
                {

                    var RoomData = new LoadRoom
                    {
                        Region = player.Region,
                        Area = player.Area,
                        id = player.AreaId
                    };


                    Room getRoomData = null;

                    var room = new Tuple<string, string, int>(RoomData.Region, RoomData.Area, RoomData.id);


                    if (_AreaCache.TryGetValue(room, out getRoomData))
                    {

                        return getRoomData;

                    }
                    else
                    {
                        getRoomData = RoomData.LoadRoomFile();
                        _AreaCache.TryAdd(room, getRoomData);


                        return getRoomData;
                    }
                }
            }
            else
            {
                //mob
                var mob = Thing;

                var RoomData = new LoadRoom
                {
                    Region = mob.Region,
                    Area = mob.Area,
                    id = mob.AreaId
                };


                Room getRoomData = null;

                var room = new Tuple<string, string, int>(RoomData.Region, RoomData.Area, RoomData.id);


                if (_AreaCache.TryGetValue(room, out getRoomData))
                {

                    return getRoomData;

                }
                else
                {
                    getRoomData = RoomData.LoadRoomFile();
                    _AreaCache.TryAdd(room, getRoomData);


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


                var findRoomData = new Tuple<string, string, int>(roomJSON.Region, roomJSON.Area, roomJSON.id);

                if (_AreaCache.TryGetValue(findRoomData, out roomData))
                {


                    room = LoadRoom.DisplayRoom(roomData, player.Name);

                }
                else
                {

                    roomData = roomJSON.LoadRoomFile();
                    _AreaCache.TryAdd(findRoomData, roomData);
                    room = LoadRoom.DisplayRoom(roomData, player.Name);

                }

                return room;
            }

            return null;
        }

        public void SaveRoom(Room room)
        {

            var saveRoom = new Tuple<string, string, int>(room.region, room.area, room.areaId);

            _AreaCache.TryAdd(saveRoom, room);


        }

        public void loadRoom(Player playerData, string id)
        {

            string roomData = ReturnRoom(id);

            this.Clients.Caller.addNewMessageToPage(roomData, true);

            var room = getRoom(playerData);
            //start tut
            Tutorial.setUpTut(playerData, room, string.Empty, string.Empty);
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
            try
            {
                Clients.Client(id).addNewMessageToPage(message);
            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "Send to Client"
                };

                Save.LogError(log);

            }
        }
        #endregion


        #region  Character Wizard & Setup

        public void charSetup(string id, string name, string email, string password, string gender, string race, string selectedClass)
        {

            var raceStats = PlayerRace.selectRace(race);

            //Creates and saves player
            PlayerData = new Player
            {
                HubGuid = id,
                Name = name,
                Email = email,
                Password = password,
                Gender = gender,
                Race = race,
                SelectedClass = selectedClass,
                Strength = raceStats.str,
                Constitution = raceStats.con,
                Dexterity = raceStats.dex,
                Wisdom = raceStats.wis,
                Intelligence = raceStats.inte,
                Charisma = raceStats.cha,
                MaxStrength = raceStats.str,
                MaxConstitution = raceStats.con,
                MaxDexterity = raceStats.dex,
                MaxWisdom = raceStats.wis,
                MaxIntelligence = raceStats.inte,
                MaxCharisma = raceStats.cha,
                intoxicationMaxLevel = raceStats.con,
                Type = Player.PlayerTypes.Player,
                JoinedDate = DateTime.UtcNow,
                LastCommandTime = DateTime.UtcNow
            };


            //add skills to player
            var classSelected = Core.Player.Classes.PlayerClass.ClassList()
                .FirstOrDefault(x => x.Value.Name
                .Equals(selectedClass, StringComparison.CurrentCultureIgnoreCase));

            if (classSelected.Value != null)
            {
                foreach (var classSkill in classSelected.Value.Skills.Where(x => x.LevelObtained.Equals(1)))
                {
                    PlayerData.Skills.Add(classSkill);
                }
            }
            else
            {
                //well, you get no skills bro
            }


            Welcome(PlayerData.HubGuid);

            var helpMessage = "<div style='\r\n    border: 1px dashed #555;\r\n    padding: 20px;\r\n    margin-bottom: 20px; max-width:540px;\r\n'><h2 style='color:yellow; font-size:20px;'>Welcome to ArchaicQuest</h2>" +
                              "<p>Prepare yourself for a great adventure. Help can be found by typing\r\nhelp start which will tell you the basic commands in the game.\r\n\r\nIf you are struggling you can ask for help on the newbie channel by typing \r\nnewbie \'Then your message here, without the quotes\'</p>" +
                              "<p>We also have a help file system which is used by typing help <topic>. For example\r\nhelp move.\r\n\r\nA small tutorial will now teach the basics of the game, if you get stuck. Remember the newbie channel or help start and it goes without saying you have to read.</p> <p>Have fun and enjoy your time here.</p>" +
                              "\r\n\r\n<p style='color:#999;'>“A reader lives a thousand lives before he dies, said Jojen. The man who never reads lives only one.”\r\n―</p> <em style='color:#999'>George R.R. Martin, A Dance with Dragons</em></div>";

            SendToClient(helpMessage, PlayerData.HubGuid);

            _PlayerCache.TryAdd(id, PlayerData);

            loadRoom(PlayerData, id);
            //add player to room
            Room roomData = null;

            var getPlayerRoom = new Tuple<string, string, int>(PlayerData.Region, PlayerData.Area, PlayerData.AreaId);

            _AreaCache.TryGetValue(getPlayerRoom, out roomData);

            MIMWebClient.Core.Room.PlayerManager.AddPlayerToRoom(roomData, PlayerData);
            Movement.EnterRoom(PlayerData, roomData);

            PlayerData.LastLoginTime = DateTime.Now;
            PlayerData.LastCommandTime = DateTime.Now;

            Save.SavePlayer(PlayerData);

            // addToRoom(PlayerData.AreaId, roomData, PlayerData, "player");
            Score.ReturnScoreUI(PlayerData);
            Score.UpdateUiPrompt(PlayerData);
            Score.UpdateUiInventory(PlayerData);
            Score.UpdateUiEquipment(PlayerData);
            Score.UpdateUiAffects(PlayerData);
            Score.UpdateUiQlog(PlayerData);

            var discordToSay = "A new character called, " + PlayerData.Name + " has entered the realm.";

            var discordBot = new HomeController();
             discordBot.PostToDiscord(discordToSay);

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

                        Save.SavePlayer(alreadyLogged.Value);

                        var oldPlayer = alreadyLogged.Value;
                        _PlayerCache.TryRemove(alreadyLogged.Value.HubGuid, out oldPlayer);

                        _AreaCache.FirstOrDefault(x => x.Value.players.Remove(oldPlayer));

                        //update room cache

                        SendToClient("You have been logged in elsewhere, goodbye", alreadyLogged.Value.HubGuid);
                        SendToClient("Kicking off your old connection", id);
                        HubContext.Instance.Quit(alreadyLogged.Value.HubGuid);

                    }
                }

                player.LastCommandTime = DateTime.Now;
                player.LastLoginTime = DateTime.Now;

                _PlayerCache.TryAdd(id, player);

                this.loadRoom(player, player.HubGuid);

                //add player to room
                Room roomData = null;

                var getPlayerRoom = new Tuple<string, string, int>(player.Region, player.Area, player.AreaId);

                _AreaCache.TryGetValue(getPlayerRoom, out roomData);


                PlayerManager.AddPlayerToRoom(roomData, player);
                Movement.EnterRoom(player, roomData);

                //Show exits UI
                Movement.ShowUIExits(roomData, player.HubGuid);
                //  Prompt.ShowPrompt(player);

                Score.ReturnScoreUI(player);
                Score.UpdateUiPrompt(player);
                Score.UpdateUiInventory(player);
                Score.UpdateUiAffects(player);
                Score.UpdateUiEquipment(player);
                Score.UpdateUiQlog(player);
                Score.UpdateDescription(player, player.Description);

                var discordToSay = player.Name + " has entered the realm.";

                var discordBot = new HomeController();
                discordBot.PostToDiscord(discordToSay);



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
