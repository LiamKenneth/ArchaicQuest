using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Room
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    [BsonIgnoreExtraElements]
   public class Room
    {
        public string _id { get; set; }

        public string region { get; set; }

        public string area { get; set; }

        public int areaId { get; set; }

        public bool clean { get; set; }

        public string modified { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public string terrain { get; set; }

         public BsonDocument keywords { get; set; }
         public List<Exit> exits { get; set; }

        public List<PlayerSetup.PlayerSetup> players { get; set; }
         public BsonArray mobs { get; set; }
         public List<Item.Item> items { get; set; }
         public BsonArray corpses { get; set; }

 
        //public Room(string region, string area, int areaId, bool clean, string title, string description, string terrain, BsonDocument keywords, List<Exit> exits, List<PlayerSetup.PlayerSetup> players, BsonArray mobs, BsonArray items, BsonArray corpses)
        //{

        //    this.region = region;
        //    this.area = area;
        //    this.areaId = areaId;
        //    this.clean = clean;
        //    this.title = title;
        //    this.description = description;
        //    this.terrain = terrain;
        //    this.keywords = keywords;
        //    this.exits = exits;
        //    this.players = players;
        //    this.mobs = mobs;
        //    this.items = items;
        //    this.corpses = corpses;

        //}

        //public JObject returnRoomJSON()
        //{
        //    string json = this.ToJson();
        //    JObject room = JObject.Parse(json);
        //    return room;
        //}
    }

  
}
