using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Collections.Concurrent;

    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class LoadRoom
    {
        public string Region { get; set; }
        public string Area { get; set; }
        public int id { get; set; }


        public Room LoadRoomFile()
        {
            const string connectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Room>("Room");

            Room room = collection.AsQueryable<Room>().SingleOrDefault(x => x.areaId == this.id && x.area == Area && x.region == Region);


            if (room != null)
            {
                return room;
            }

            throw new Exception("No Room found in the Database for areaId: " + id);
        }



        public static string DisplayRoom(Room room)
        {

            var roomJson = room;

            string roomTitle = room.title;
            string roomDescription = room.description;

            var exitList = string.Empty;
            foreach (var exit in room.exits)
            {
                exitList += exit.name + " ";

            };

            var itemList = string.Empty;
            foreach (var item in room.items)
            {
                itemList += item.name + " ";
            }

            var playerList = string.Empty;
            foreach (var item in room.players)
            {
                playerList += item.Name + " ";
            }


            ////GEt Mobs
            //var roomMobs = string.Empty;
            //var mobList = roomJson["mobs"];

            //if (mobList.Any())
            //{
            //    foreach (var mob in mobList)
            //    {
            //        roomMobs += mob["name"] + "\r\n";
            //    }
            //}
            //else
            //{
            //    mobList = string.Empty;
            //}

            ////GEt players
            //var roomPlayers = string.Empty;
            //var playerList = roomJson["players"];

            //if (playerList.Any())
            //{
            //    foreach (var player in playerList)
            //    {
            //        if (player["name"] != null)
            //        {
            //            roomPlayers += player["name"] + "\r\n";
            //        }
            //        else
            //        {
            //            roomPlayers += player["n"] + "\r\n";
            //        }

            //    }
            //}
            //else
            //{
            //    playerList = string.Empty;
            //}



            string displayRoom = roomTitle + "\r\n" + roomDescription + "\r\n Exits: " + exitList + "Items: " + itemList + " " + playerList;

            return displayRoom;

        }

        public static void ReturnRoom(Player player, Room room, string commandOptions = "", string keyword = "")
        {

            Room roomData = room;




            if (string.IsNullOrEmpty(commandOptions) && keyword == "look")
            {
                var roomInfo = DisplayRoom(roomData);
                HubProxy.MimHubServer.Invoke("SendToClient", roomInfo);
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

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = roomDescription.look;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = roomDescription.examine;
                    }
                    else if (keyword.StartsWith("touch"))
                    {
                        descriptionText = roomDescription.touch;
                    }
                    else if (keyword.StartsWith("smell"))
                    {
                        descriptionText = roomDescription.smell;
                    }
                    else if (keyword.StartsWith("taste"))
                    {
                        descriptionText = roomDescription.taste;
                    }

                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + roomDescription.name);
                    }

                }
                else if (itemDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = itemDescription.description.look;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = itemDescription.description.exam;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + roomDescription.name);
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
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + roomDescription.name);
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
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke(
                            "SendToClient",
                            "You can't do that to a " + roomDescription.name);
                    }
                }
                else
                {
                    HubProxy.MimHubServer.Invoke("SendToClient", "You don't see that here.");
                }

            }
        }
    }
}
