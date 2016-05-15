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


        public JObject LoadRoomFile()
        {
            const string connectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Room>("Room");
          
            Room room = collection.AsQueryable<Room>().SingleOrDefault(x => x.areaId == this.id);

            if (room != null)
            {
                JObject roomJson = room.returnRoomJSON();

                return roomJson;
            }

            throw new Exception("No Room found in the Database for areaId: " +  id);
        }



        public static string DisplayRoom(JObject room)
        {

            var roomJson = room;

          string roomTitle = (string)roomJson["title"];
          string roomDescription = (string)roomJson["description"];
            var roomExitObj = roomJson.Property("exits").Children();
    

          
            string exitList = null;
            foreach (var exit in roomExitObj)
            {
                if (exit["North"] != null)
                {
                    exitList += exit["North"]["name"];
                }

                if (exit["East"] != null)
                {
                    exitList += exit["East"]["name"];
                }

                if (exit["South"] != null)
                {
                    exitList += exit["South"]["name"];
                }

                if (exit["West"] != null)
                {
                    exitList += exit["West"]["name"];
                }

                if (exit["Up"] != null)
                {
                    exitList += exit["Up"]["name"];
                }

                if (exit["Down"] != null)
                {
                    exitList += exit["Down"]["name"];
                }

            }

            //GEt Room Items
            var roomItems = string.Empty;
            var itemList = roomJson["items"];

            if (itemList.Any())
            {
                foreach (var item in itemList)
                {
                    roomItems += item["name"] + "\r\n";
                }
            }

            //GEt Mobs
            var roomMobs = string.Empty;
            var mobList = roomJson["mobs"];

            if (mobList.Any())
            {
                foreach (var mob in mobList)
                {
                    roomMobs += mob["name"] + "\r\n";
                }
            }
            else
            {
                mobList = string.Empty;
            }

            //GEt players
            var roomPlayers = string.Empty;
            var playerList = roomJson["players"];

            if (playerList.Any())
            {
                foreach (var player in playerList)
                {
                    roomPlayers += player["name"] + "\r\n";
                }
            }
            else
            {
                playerList = string.Empty;
            }



            string displayRoom = roomTitle + "\r\n" + roomDescription + "\r\n" + "[ Obvious Exits:" + exitList + " ]\r\n " + roomItems + mobList + playerList;

            return displayRoom;

        }

       public static void ReturnRoom(JObject room)
       {
           var roomInfo = DisplayRoom(room);

            HubProxy.MimHubServer.Invoke("SendToClient", roomInfo);
        }
    }
}
