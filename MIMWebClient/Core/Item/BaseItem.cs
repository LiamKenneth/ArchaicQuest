using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Item
{
    public class BaseItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public string name { get; set; }
        public List<string> keywords { get; set; }
        public bool open { get; set; } = true;
        public bool canOpen{ get; set; }
        public bool canLock { get; set; }
        public string keyId { get; set; }
        public string keyValue { get; set; }
        public bool hidden { get; set; }
        /// <summary>
        /// Wear is this item? Room, Inventory, worn
        /// </summary>
        public Item.ItemLocation? location { get; set; }
        public bool equipable { get; set; }
        public string slot { get; set; }

        public Action actions { get; set; }
        public Description description { get; set; }
        public Stats stats { get; set; }
        public bool container { get; set; }

        /// <summary>
        ///  How many items fit in the container
        /// </summary>
        public int containerSize { get; set; }
        public bool waterContainer { get; set; }

        /// <summary>
        /// How many times a player can drink from it
        /// </summary>
        public int waterContainerSize { get; set; }
        public bool locked { get; set; }
        /// <summary>
        /// Cannont remove item
        /// </summary>
        public bool stuck { get; set; }

    }
}
