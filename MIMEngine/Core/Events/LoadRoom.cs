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



            string displayRoom = roomTitle + "\r\n" + roomDescription + "\r\n Exits: " + exitList + "Items: " + itemList;

            return displayRoom;

        }

        public static void ReturnRoom(Room room, string commandOptions = "", string keyword = "")
        {

            if (string.IsNullOrEmpty(commandOptions) && keyword == "look")
            {
                var roomInfo = DisplayRoom(room);
                HubProxy.MimHubServer.Invoke("SendToClient", roomInfo);
            }
            else
            {
                var description = room.keywords.Find(x => x.name.Contains(commandOptions));

                if (description != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look"))
                    {
                        descriptionText = description.look;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = description.examine;
                    }
                    else if (keyword.StartsWith("touch"))
                    {
                        descriptionText = description.touch;
                    }
                    else if (keyword.StartsWith("smell"))
                    {
                        descriptionText = description.smell;
                    }
                    else if (keyword.StartsWith("taste"))
                    {
                        descriptionText = description.taste;
                    }

                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", descriptionText);
                    }
                    else
                    {
                        HubProxy.MimHubServer.Invoke("SendToClient", "You can't do that to a " + description.name);
                    }

                }
                else
                {
                    HubProxy.MimHubServer.Invoke("SendToClient", "You can't do that");
                }

            }
        }
    }
}
