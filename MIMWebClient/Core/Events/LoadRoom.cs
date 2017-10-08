using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvsAnLib;
using LiteDB;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Events
{
    using System.Collections.Concurrent;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class LoadRoom
    {
        public string Region { get; set; }
        public string Area { get; set; }
        public int id { get; set; }


        public Room LoadRoomFile()
        {
            using (var db = new LiteDatabase(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["database"])))
            {


                var col = db.GetCollection<Room>("Room");

                var room = col.Find(x => x.areaId == this.id && x.area == Area && x.region == Region).FirstOrDefault();


                if (room != null)
                {
                    return room;
                }

                throw new Exception("No Room found in the Database for areaId: " + id);
            }
        }


        /// <summary>
        /// Displays room desc, players, mobs, items and exits
        /// </summary>
        /// <param name="room"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public static string DisplayRoom(Room room, string playerName)
        {


            string roomTitle = room.title;
            string roomDescription = room.description;

            var exitList = string.Empty;
            foreach (var exit in room.exits)
            {
                exitList += exit.name + " ";
            }

            var player = room.players.FirstOrDefault(x => x.Name.Equals(playerName));

            if (player != null && player.Status == Player.PlayerStatus.Sleeping)
            {
                return "You can't see anything while asleep." ;
            }

           

            var itemList = string.Empty;

 
            foreach (var item in room.items)
            {
                if (item != null)
                {

                    if (item.itemFlags?.Contains(Item.Item.ItemFlags.invis) == true && player.DetectInvis == false || item.itemFlags?.Contains(Item.Item.ItemFlags.hidden) == true && player.DetectHidden == false)
                    {
                        continue;
                    }

                    if (!item.isHiddenInRoom)
                    {
                        var result = AvsAnLib.AvsAn.Query(item.name);
                        string article = result.Article;

                        if (!string.IsNullOrEmpty(item.description?.room))
                        {
                            if (item.count > 0)
                            {
                                itemList += $"<p class='roomItems'>({item.count}) {item.description.room}<p>";
                            }
                            else
                            {
                                itemList += $"<p class='roomItems'>{item.description.room}<p>";
                            }
                           
                        }
                        else
                        {
                            if (item.count > 0)
                            {
                                itemList += $"<p class='roomItems'>({item.count}) {article} {item.name}'s are on the floor here.<p>";
                            }
                            else
                            {
                                itemList += $"<p class='roomItems'>{article} {item.name} is on the floor here.<p>";
                            }
                               
                        }

                        
                    }
                }
            }

           
            var playerList = string.Empty;
            if (room.players != null)
            {
               

                foreach (var item in room.players)
                {
                    if (item.invis == true && player.DetectInvis == false || item.hidden == true && player.DetectHidden == false)
                    {
                        continue;
                    }

                    if (item.nonDectect == true)
                    {
                        continue;
                    }

                    if (item.Name != playerName)
                    {
                        if (item.Status == Player.PlayerStatus.Standing)
                        {

                            playerList += "<p>" + LoadRoom.ShowObjectEffects(item) + " is here.</p>";
                        }
                        else if (item.Status == Player.PlayerStatus.Fighting)
                        {

                            if (item.Target.Name == player.Name)
                            {
                                playerList += "<p>You are fighting " + item.Target.Name + "</p>";
                            }
                            else
                            {
                                playerList += "<p>" + LoadRoom.ShowObjectEffects(item) + " is fighting " + item.Target.Name +
                                              "</p>";
                            }
                        }
                        else if (item.Status == PlayerSetup.Player.PlayerStatus.Resting)
                        {
                            playerList += "<p>" + LoadRoom.ShowObjectEffects(item) + " is resting here.</p>";
                        }
                        else if (item.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
                        {
                            playerList += "<p>" + LoadRoom.ShowObjectEffects(item) + " is sleeping here.</p>";
                        }
                        else if (item.Status == PlayerSetup.Player.PlayerStatus.Mounted)
                        {
                            playerList += "<p>" + LoadRoom.ShowObjectEffects(item) + " is here riding " + Helpers.ReturnName(null, null, item.Mount.Name).ToLower() + ".</p>";
                        }
                        else
                        {
                            playerList += "<p>" + LoadRoom.ShowObjectEffects(item) + " is here.</p>";
                        }
   
                    }

                }
            }

            var mobList = string.Empty;
            if (room.mobs != null)
            {
                foreach (var item in room.mobs)
                {

                    if (item.invis == true && player.DetectInvis == false || item.hidden == true && player.DetectHidden == false)
                    {
                        continue;
                    }

                    if (item.nonDectect == true)
                    {
                        continue;
                    }


                    var result = AvsAnLib.AvsAn.Query(item.Name);
                    string article = result.Article;

                    if (item.KnownByName)
                    {
                        article = string.Empty;
                    }

                    var npcName = !string.IsNullOrEmpty(item.NPCLongName) ?item.NPCLongName : $"{item.Name} is here";

                    if (item.Status == Player.PlayerStatus.Standing)
                    {
                        mobList += "<p class='roomMob'>" + article + " " + npcName + ".<p>";
                    }
                    else if (item.Status == Player.PlayerStatus.Fighting)
                    {
                        mobList += "<p class='roomMob'>" + article + " " + item.Name + " is fighting " + item.Target.Name + ".</p>";
                    }
                    else if (item.Status == PlayerSetup.Player.PlayerStatus.Resting)
                    {
                        mobList += "<p class='roomMob'>" + article + " " + item.Name + " is resting.<p>";
                    }
                    else if (item.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
                    {
                        if (!string.IsNullOrEmpty(item.Pose))
                        {
                            mobList += "<p class='roomMob'>" + item.Pose +"<p>";
                        }
                        else
                        {
                            mobList += "<p class='roomMob'>" + article + " " + item.Name + " is sleeping.<p>";
                        }
                    }
                    else 
                    {
                        mobList += "<p class='roomMob'>" + article + " " + npcName + "<p>";
                    }
                }
            }


            string displayRoom = "<p class='roomTitle'>" + roomTitle + "<p><p class='roomDescription'>" + roomDescription + "</p> <p class='RoomExits'>[ Exits: " + exitList.ToLower() + " ]</p>" + itemList + "\r\n" + playerList + "\r\n" + mobList;
       
            return displayRoom;

        }

        public static void ReturnRoom(Player player, Room room, string commandOptions = "", string keyword = "")
        {
            var isBlind = player.Affects?.FirstOrDefault(
                       x => x.Name.Equals("Blindness", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (isBlind)
            {
                HubContext.SendToClient("You can't see a thing.", player.HubGuid);
                return;
            }

            CheckEvent.FindEvent(CheckEvent.EventType.Look, player, "look");

            Room roomData = room;

            if (string.IsNullOrEmpty(commandOptions) && keyword == "look")
            {
                var roomInfo = DisplayRoom(roomData, player.Name);
                HubContext.SendToClient(roomInfo, player.HubGuid);

            }
            else
            {

                int n = -1;
                string item = string.Empty;

                if (commandOptions.IndexOf('.') != -1)
                {
                    n = Convert.ToInt32(commandOptions.Substring(0, commandOptions.IndexOf('.')));
                    item = commandOptions.Substring(commandOptions.LastIndexOf('.') + 1);
                }

                if (roomData.keywords == null)
                {
                    roomData.keywords = new List<RoomObject>();
                }

                var roomDescription = roomData.keywords.Find(x => x.name.ToLower().Contains(commandOptions));

                if (roomData.items == null)
                {
                    roomData.items = new List<Item.Item>();
                }

                var itemDescription = (n == -1)
                                          ? roomData.items.Find(x => x.name.ToLower().Contains(commandOptions))
                                          : roomData.items.FindAll(x => x.name.ToLower().Contains(item))
                                                .Skip(n - 1)
                                                .FirstOrDefault();

                if (roomData.mobs == null)
                {
                    roomData.mobs = new List<Player>();
                }

                var mobDescription = (n == -1)
                                          ? roomData.mobs.Find(x => x.Name.ToLower().Contains(commandOptions))
                                          : roomData.mobs.FindAll(x => x.Name.ToLower().Contains(item))
                                                .Skip(n - 1)
                                                .FirstOrDefault();

                if (roomData.players == null)
                {
                    roomData.players = new List<Player>();
                }

                var playerDescription =  (n == -1)
                                          ? roomData.players.Find(x => x.Name.ToLower().Contains(commandOptions))
                                          : roomData.players.FindAll(x => x.Name.ToLower().Contains(item))
                                                .Skip(n - 1)
                                                .FirstOrDefault();

                var playerItemDescription = (n == -1)
                    ? player?.Inventory.Find(x => x.name.ToLower().Contains(commandOptions))
                    : player?.Inventory.FindAll(x => x.name.ToLower().Contains(item))
                        .Skip(n - 1)
                        .FirstOrDefault();

                var targetPlayerId = string.Empty;
                if (playerDescription != null)
                {
                    var isPlayer = playerDescription.Type.Equals("Player");

                    if (isPlayer)
                    {
                        targetPlayerId = playerDescription.HubGuid;
                    }
                }

                var roomExitDescription = roomData.exits.Find(x => x.name.ToLower().Contains(commandOptions));


                //Returns descriptions for important objects in the room
                if (roomDescription != null && keyword != "look in" && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;
                    if (keyword.Equals("look"))
                    {
                        descriptionText = roomDescription.look;
                        broadcastAction = " looks at a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = roomDescription.examine;
                        broadcastAction = " looks closely at a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("touch"))
                    {
                        descriptionText = roomDescription.touch;
                        broadcastAction = " touches closely a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("smell"))
                    {
                        descriptionText = roomDescription.smell;
                        broadcastAction = " sniffs a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("taste"))
                    {
                        descriptionText = roomDescription.taste;
                        broadcastAction = " licks a " + roomDescription.name;
                    }

                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);

                        foreach (var players in room.players)
                        {
                            if (player.Name != players.Name)
                            {
                                HubContext.SendToClient(player.Name + broadcastAction, players.HubGuid);
                            }
                        }
                    }
                    else
                    {
      
                        HubContext.SendToClient("You see nothing special about the " + roomDescription.name + ".", player.HubGuid);
                    }

                }
                else if (itemDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;

                    if (keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (itemDescription.open == false)
                        {
                            HubContext.SendToClient("You need to open the " + itemDescription.name + " before you can look inside", player.HubGuid);
                            return;
                        }

                        if (itemDescription.container == true)
                        {
                            if (itemDescription.containerItems.Count > 0)
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " and see:", player.HubGuid);


                                foreach (var containerItem in itemDescription.containerItems.List(true))
                                {
                                    HubContext.SendToClient(containerItem, player.HubGuid);
                                }
                            }
                            else
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " but it is empty", player.HubGuid);
                            }

                            
                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {
                                    
                                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} looks in a {itemDescription.name}";

                                    HubContext.SendToClient(roomMessage, character.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            HubContext.SendToClient(itemDescription.name + " is not a container", player.HubGuid);
                        }
                    }
                    else if (keyword.StartsWith("look"))
                    {
                        descriptionText = itemDescription.description.look;
                        broadcastAction = " looks at a " + itemDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = itemDescription.description.exam;
                        broadcastAction = " looks closely at a " + itemDescription.name;
                    }

                    if (!keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (!string.IsNullOrEmpty(descriptionText))
                        {
                            HubContext.SendToClient(descriptionText, player.HubGuid);

                            foreach (var players in room.players)
                            {
                                if (player.Name != players.Name)
                                {
                                    HubContext.SendToClient(player.Name + broadcastAction,
                                        players.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            var result = AvsAnLib.AvsAn.Query(itemDescription.name);
                            string article = result.Article;

                            HubContext.SendToClient("You see nothing special about " + article + " " + itemDescription.name, player.HubGuid);
                        }
                    }
                }
                else if (mobDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look") || keyword.StartsWith("examine"))
                    {
                        descriptionText = mobDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);
                     
                        Equipment.ShowEquipmentLook(mobDescription, player);
                  

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} looks at { Helpers.ReturnName(mobDescription, character, string.Empty)}";

                                HubContext.SendToClient(roomMessage, character.HubGuid);
                            }
                        }
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + mobDescription.Name, player.HubGuid);
                    }
                }
                else if (playerDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;


                    if (keyword.StartsWith("look") || keyword.StartsWith("examine"))
                    {
                        descriptionText = playerDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);
                        Equipment.ShowEquipmentLook(playerDescription, player);
                        HubContext.SendToClient(player.Name + " looks at you", playerDescription.HubGuid);
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + playerDescription.Name, player.HubGuid);
                    }
                }
                else if (playerItemDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;

                    if (keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (playerItemDescription.open == false)
                        {
                            HubContext.SendToClient("You need to open the " + playerItemDescription.name + " before you can look inside", player.HubGuid);
                            return;
                        }

                        if (playerItemDescription.container == true)
                        {
                            if (playerItemDescription.containerItems.Count > 0)
                            {
                                HubContext.SendToClient("You look into the " + playerItemDescription.name + " and see:", player.HubGuid);

                                foreach (var containerItem in playerItemDescription.containerItems.List(true))
                                {
                                    HubContext.SendToClient(containerItem, player.HubGuid);
                                }
                            }
                            else
                            {
                                HubContext.SendToClient("You look into the " + playerItemDescription.name + " but it is empty", player.HubGuid);
                            }


                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} looks in a {playerItemDescription.name}";

                                    HubContext.SendToClient(roomMessage, character.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            HubContext.SendToClient(playerItemDescription.name + " is not a container", player.HubGuid);
                        }
                    }
                    else if (keyword.StartsWith("look"))
                    {
                        descriptionText = playerItemDescription.description.look;
                        broadcastAction = " looks at a " + playerItemDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = playerItemDescription.description.exam;
                        broadcastAction = " looks closely at a " + playerItemDescription.name;
                    }

                    if (!keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (!string.IsNullOrEmpty(descriptionText))
                        {
                            HubContext.SendToClient(descriptionText, player.HubGuid);

                            foreach (var players in room.players)
                            {
                                if (player.Name != players.Name)
                                {
                                    HubContext.SendToClient(player.Name + broadcastAction,
                                        players.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            var result = AvsAnLib.AvsAn.Query(playerItemDescription.name);
                            string article = result.Article;

                            HubContext.SendToClient("You see nothing special about " + article + " " + playerItemDescription.name, player.HubGuid);
                        }
                    }
                }
                else if (roomExitDescription != null)
                {
                    HubContext.SendToClient("You look " + roomExitDescription.name, player.HubGuid);

                    var currentAreaId = player.AreaId;
                    player.AreaId = roomExitDescription.areaId;


                    var adjacentRoom = Cache.getRoom(player);
                    if (adjacentRoom == null)
                    {
                        var newRoom = new LoadRoom();

                        newRoom.Area = roomExitDescription.area;
                        newRoom.id = roomExitDescription.areaId;
                        newRoom.Region = roomExitDescription.region;

                         adjacentRoom = newRoom.LoadRoomFile();

                        //add to cache?

                    }

                  var showNextRoom = LoadRoom.DisplayRoom(adjacentRoom, player.Name);



                     HubContext.SendToClient(showNextRoom, player.HubGuid);

                    player.AreaId = currentAreaId;

                    foreach (var players in room.players)
                    {
                        if (player.Name != players.Name)
                        {
                            HubContext.SendToClient(player.Name + " looks " + roomExitDescription.name, players.HubGuid);
                        }
                    }
                }
                else
                {
                    HubContext.SendToClient("You don't see that here.", player.HubGuid);
                }

            }
        }

        public static string ShowObjectEffects(Player player)
        {
            var name = player.Type == Player.PlayerTypes.Player ? player.Name : player.NPCLongName;

            if (player.Affects?.FirstOrDefault(x => x.Name.Equals("Fly", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                name += " (Floating)";
            }

            if (player.Affects?.FirstOrDefault(x => x.Name.Equals("Hidden", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                name += " (Hidden)";
            }

            if (player.Affects?.FirstOrDefault(x => x.Name.Equals("Invis", StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                name += " (Invis)";
            }

            return name;
        }

        //public string ShowObjectEffects(Item.Item item)
        //{
        //    var name = item.name;

        //    if (item.Affects?.FirstOrDefault(x => x.Name.Equals("Hidden", StringComparison.CurrentCultureIgnoreCase)) != null)
        //    {
        //        name += " (Hidden)";
        //    }

        //    if (item.Affects?.FirstOrDefault(x => x.Name.Equals("Invis", StringComparison.CurrentCultureIgnoreCase)) != null)
        //    {
        //        name += " (Invis)";
        //    }

        //    return item.name;
        //}
    }
}
