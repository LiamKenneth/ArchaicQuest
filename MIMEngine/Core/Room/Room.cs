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

         public List<RoomObject> keywords { get; set; }
         public List<Exit> exits { get; set; }

        public List<PlayerSetup.Player> players { get; set; }
         public List<Mob> mobs { get; set; }
         public List<Item.Item> items { get; set; }
         public List<PlayerSetup.Player> corpses { get; set; }

       
    }

  
}
