using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Room
{
    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Mob
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("hid")]
        public string HubGuid;

        [BsonElement("ty")]
        public string Type;

        [BsonElement("e")]
        public string Email;

        [BsonElement("de")]
        public string Description;

        [BsonElement("p")]
        public string Password;

        // General Info
        [BsonElement("n")]
        public string Name;

        [BsonElement("g")]
        public string Gender;

        [BsonElement("r")]
        public string Race;

        [BsonElement("sc")]
        public string SelectedClass;

        [BsonElement("lvl")]
        public int Level;

        [BsonElement("ali")]
        public int AlignmentScore;

        [BsonElement("xp")]
        public int Experience;

        [BsonElement("tnl")]
        public int ExperienceToNextLevel;

        [BsonElement("hp")]
        public int HitPoints;

        [BsonElement("mhp")]
        public int MaxHitPoints;

        [BsonElement("mp")]
        public int ManaPoints;

        [BsonElement("mmp")]
        public int MaxManaPoints;

        [BsonElement("mvp")]
        public int MovePoints;

        [BsonElement("mmvp")]
        public int MaxMovePoints;

        [BsonElement("in")]
        public List<Item> Inventory { get; set; }

        [BsonElement("eq")]
        public Equipment Equipment { get; set; }

        [BsonElement("sk")]
        public List<Skill> Skills { get; set; }

        //Game stats
        [BsonElement("ex")]
        public int Explored;

        [BsonElement("hr")]
        public int HitRoll;

        [BsonElement("dr")]
        public int DamRoll;

        [BsonElement("wi")]
        public int Wimpy;

        [BsonElement("hrs")]
        public int Hours;

        [BsonElement("we")]
        public int Weight;

        [BsonElement("mwe")]
        public int MaxWeight;

        [BsonElement("st")]
        public string Status;

        [BsonElement("ta")]
        public Player Target;

        [BsonElement("arr")]
        public int ArmorRating;

        [BsonElement("s")]
        public int Saves;

        //Kills
        [BsonElement("mk")]
        public int MobKills;

        [BsonElement("md")]
        public int MobDeaths;

        [BsonElement("pk")]
        public int Pkills;

        [BsonElement("pd")]
        public int PkDeaths;

        [BsonElement("pkp")]
        public int PkPoints;

        //Money
        [BsonElement("gp")]
        public int Gold;

        [BsonElement("sp")]
        public int Silver;

        [BsonElement("cp")]
        public int Copper;

        // attributes
        [BsonElement("as")]
        public int Strength;

        [BsonElement("ad")]
        public int Dexterity;

        [BsonElement("ac")]
        public int Constitution;

        [BsonElement("aw")]
        public int Wisdom;

        [BsonElement("ai")]
        public int Intelligence;

        [BsonElement("ach")]
        public int Charisma;

        //location
        [BsonElement("re")]
        public string Region;

        [BsonElement("ar")]
        public string Area;

        [BsonElement("ari")]
        public int AreaId;

        //NPC Properties
        [BsonElement("ne")]
        public List<string> Emotes;

        //NPC Properties
        [BsonElement("nid")]
        public Guid NPCId;

        //NPC Properties
        [BsonElement("nr")]
        public bool Roam;

        //NPC Properties
        [BsonElement("na")]
        public bool Aggro;

        //NPC Properties
        [BsonElement("ng")]
        public bool Greet;
        //NPC Properties

        [BsonElement("ngm")]
        public string GreetMessage;

        //NPC Properties
        [BsonElement("ns")]
        public bool Shop;

        [BsonElement("nsm")]
        public string sellerMessage;

        //NPC Properties
        [BsonElement("nsi")]
        public List<Item> itemsToSell;

        //NPC Properties
        [BsonElement("nt")]
        public bool Trainer;

        //NPC Properties
        [BsonElement("ngu")]
        public bool Guard;

 
    }
}
