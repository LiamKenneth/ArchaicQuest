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


        public enum lockPickDifficulty
        {
            Easy,
            Simple,
            Mid,
            Hard,
            Impossible
        }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonIgnore]
        public int count { get; set; } = 0;
        public string name { get; set; }
        public List<string> keywords { get; set; }
        public bool open { get; set; } = true;
        public bool canOpen{ get; set; }
        public bool canLock { get; set; }
        /// <summary>
        /// Key id must match key value
        /// </summary>
        public string keyId { get; set; }
        /// <summary>
        /// Key value must match key id
        /// </summary>
        public string keyValue { get; set; }
        /// <summary>
        /// Lock pick difficulty
        /// Easy 1- 100
        /// Simple 25 - 100
        /// Mid 50 - 100
        /// Hard 75- 100
        /// impossible 100 -100
        /// </summary>
        public lockPickDifficulty LockPick { get; set; } = lockPickDifficulty.Simple;
        public bool hidden { get; set; }
        /// <summary>
        /// Wear is this item? Room, Inventory, worn
        /// </summary>
        public Item.ItemLocation? location { get; set; }
        public bool equipable { get; set; }
        public Item.EqSlot slot { get; set; }

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
        public int waterContainerAmount { get; set; }
        public int waterContainerMaxAmount { get; set; }
        public string waterContainerLiquid { get; set; } = "water";
        public bool locked { get; set; }
        /// <summary>
        /// Cannont remove item
        /// </summary>
        public bool stuck { get; set; }

    }
}
