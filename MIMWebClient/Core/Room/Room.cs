using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Item;

namespace MIMWebClient.Core.Room
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    [BsonIgnoreExtraElements]
    public class Room
    {

        //        INSIDE          |   0  | Weather doesn't show.
        //CITY            |   1  | 
        //FIELD           |   2  | 
        //FOREST          |   3  | 
        //HILLS           |   4  | 
        //MOUNTAIN        |   5  |
        //WATER_SWIM      |   6  | PC needs aqua_breath/fly/float or boat
        //WATER_NOSWIM    |   7  | PC needs aqua_breath/fly/float or boat
        //UNDERWATER      |   8  | PC needs aqua_breath
        //AIR             |   9  |
        //DESERT          |  10  |
        //UNKNOWN         |  11  |
        //OCEANFLOOR      |  12  | PC needs aqua_breath.Can dig here.
        //UNDERGROUND

        public enum Terrain
        {
            Inside, //no weather
            City,
            Field,
            Forest,
            Hills,
            Mountain,
            Water,
            Underwater,
            Air,
            Desert,
            Underground //no weather

        }

        public enum RoomType
        {
            Standard,
            Shop,
            Guild 

        }

        [BsonRepresentation(BsonType.ObjectId)]
        public Guid _id { get; set; }
        public string region { get; set; }
        public string area { get; set; }
        public int areaId { get; set; }
        public RoomType type { get; set; } = RoomType.Standard;
        [BsonIgnore]
        public bool visited { get; set; }
        public Coordinates coords { get; set; } = new Coordinates();
        public bool clean { get; set; }
        public string modified { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string updateMessage { get; set; }
        public Terrain terrain { get; set; }
        public bool InstantRePop { get; set; }
        public List<RoomObject> keywords { get; set; }
        public List<Exit> exits { get; set; }
        public List<PlayerSetup.Player> players { get; set; }
        public List<string> fighting { get; set; }
        public List<PlayerSetup.Player> mobs { get; set; }
        public List<Item.Item> items { get; set; }
        public List<Item.Item> ForageItems { get; set; } = new List<Item.Item>();
        public List<PlayerSetup.Player> corpses { get; set; }
        public bool containsCamp { get; set; }
        public List<string> Emotes { get; set; } = new List<string>();
        [BsonElement("eoe")]
        public string EventOnEnter { get; set; }
        [BsonElement("eow")]
        public string EventWake { get; set; }
        [BsonElement("eowe")]
        public string EventWear { get; set; }
        [BsonElement("eod")]
        public string EventDeath { get; set; }
        [BsonElement("eol")]
        public string EventLook { get; set; }


    }


}
