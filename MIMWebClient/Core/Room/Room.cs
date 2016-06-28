using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public string region { get; set; }

        public string area { get; set; }

        public int areaId { get; set; }

        public bool clean { get; set; }

        public string modified { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public Terrain terrain { get; set; }

         public List<RoomObject> keywords { get; set; }
         public List<Exit> exits { get; set; }

        public List<PlayerSetup.Player> players { get; set; }
        public List<string> fighting { get; set; }
        public List<Mob> mobs { get; set; }
         public List<Item.Item> items { get; set; }
         public List<PlayerSetup.Player> corpses { get; set; }

       
       
    }

  
}
