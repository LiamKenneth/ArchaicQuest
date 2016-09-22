using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            const string ConnectionString = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

            var collection = database.GetCollection<Room>("Room");



            Room room = collection.Find(x => x.areaId == this.id && x.area == Area && x.region == Region).FirstOrDefault();

            
            if (room != null)
            {
                return room;
            }

            throw new Exception("No Room found in the Database for areaId: " + id);
        }



        public static string DisplayRoom(Room room, string playerName)
        {

            string roomTitle = room.title;
            string roomDescription = room.description;

            var exitList = string.Empty;
            foreach (var exit in room.exits)
            {
                exitList += exit.name + " ";
            }

            var itemList = string.Empty;
            foreach (var item in room.items)
            {
                var result = AvsAnLib.AvsAn.Query(item.name);
                string article = result.Article;

                itemList += "<p class='roomItems'>" + article + " " + item.name + " is on the floor here.<p>";
            }

            var playerList = string.Empty;
            foreach (var item in room.players)
            {
                if (item.Name != playerName)
                {
                    if (item.Status == Player.PlayerStatus.Standing)
                    {
                        playerList += item.Name + " is here\r\n";
                    }
                    else if (item.Status == Player.PlayerStatus.Fighting)
                    {
                        playerList += item.Name + " is fighting " + item.Target.Name +"\r\n";
                    }
                    else if (item.Status == PlayerSetup.Player.PlayerStatus.Ghost)
                    {
                        playerList += item.Name + "(Ghost) (Translucent) (Invis)\r\n";
                    }

                }
               
            }

            var mobList = string.Empty;
            foreach (var item in room.mobs)
            {
                var result = AvsAnLib.AvsAn.Query(item.Name);
                string article = result.Article;

                mobList += "<p class='roomItems'>" + article + " " + item.Name + " is here.<p>";
            }

            var corpseList = string.Empty;
            foreach (var item in room.corpses)
            {
                corpseList += " The corpse of " + item.Name + " is here.";
            }



            string displayRoom = "<p class='roomTitle'>" + roomTitle + "<p><p class='roomDescription'>" + roomDescription + "</p> <p class='RoomExits'>[ Exits: " + exitList.ToLower() + " ]</p>" + itemList + corpseList + "\r\n" + playerList + "\r\n" + mobList;

            return displayRoom;

        }

        public static void ReturnRoom(Player player, Room room, string commandOptions = "", string keyword = "")
        {

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


                var roomDescription = roomData.keywords.Find(x => x.name.ToLower().Contains(commandOptions));

                var itemDescription = (n == -1)
                                          ? roomData.items.Find(x => x.name.ToLower().Contains(commandOptions))
                                          : roomData.items.FindAll(x => x.name.ToLower().Contains(item))
                                                .Skip(n - 1)
                                                .FirstOrDefault();

                var mobDescription = roomData.mobs.Find(x => x.Name.ToLower().Contains(commandOptions));

                var playerDescription = roomData.players.Find(x => x.Name.ToLower().Contains(commandOptions));


                //Returns descriptions for important objects in the room
                if (roomDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;
                    if (keyword.StartsWith("look"))
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
                        HubContext.SendToClient("You can't do that to a " + roomDescription.name, player.HubGuid);
                    }

                }
                else if (itemDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;

                    if(keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase)) {

                        if (itemDescription.container == true)
                        {
                            if (itemDescription.containerItems.Count > 0)
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " and see:", player.HubGuid);

                                foreach (var containerItem in itemDescription.containerItems)
                                {
                                    HubContext.SendToClient(containerItem.name, player.HubGuid);
                                }
                            }
                            else
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " but it is empty", player.HubGuid);
                            }
                           
                            HubContext.broadcastToRoom(player.Name + " looks in a " + itemDescription.name, room.players, player.HubGuid, true);
                        }
                        else
                        {
                            HubContext.SendToClient(itemDescription.name  + " is not a container", player.HubGuid);
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
                            HubContext.SendToClient("You can't do that to a " + itemDescription.name, player.HubGuid);
                        }
                    }
                }
                else if (mobDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = mobDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + mobDescription.Name, player.HubGuid);
                    }
                }
                else if (playerDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = playerDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + playerDescription.Name, player.HubGuid);
                    }
                }
                else
                {
                    HubContext.SendToClient("You don't see that here.", player.HubGuid);
                }

            }
        }
    }
}
