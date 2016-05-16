using MIMEngine.Core.Room;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMHubServer
{
    public class RoomManager : MimHubServer
    {

        //public void addToRoom(int roomId, JObject roomJson, object objectToAdd, string type)
        //{

        //    string region = (string)roomJson["region"];
        //    string area = (string)roomJson["area"];
        //    int areaId = (int)roomJson["areaId"];
        //    bool clean = (bool)roomJson["clean"];
        //    string modified = (string)roomJson["modified"];
        //    string title = (string)roomJson["title"];
        //    string description = (string)roomJson["description"];
        //    string terrain = (string)roomJson["terrain"];
        //    BsonDocument keywords =  roomJson["keywords"].ToBsonDocument();
        //    BsonDocument exits = roomJson["exits"].ToBsonDocument();
        //    BsonArray players =  (BsonArray)roomJson["players"].ToJson();
        //    BsonArray mobs = (BsonArray)roomJson["mobs"].ToJson();
        //    BsonArray items = (BsonArray)roomJson["items"].ToJson();
        //    BsonArray corpses = (BsonArray)roomJson["corpses"].ToJson();

        //    var roomData = new Room(region, area, areaId, clean, title, description, terrain, keywords, exits, players, mobs, items, corpses);

 

        //    Console.WriteLine("hello");


        //}

        public void removeFromRoom(int roomId, JObject roomJson, object objectToRemove)
        {

        }
    }
}
