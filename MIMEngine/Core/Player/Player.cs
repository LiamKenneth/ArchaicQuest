using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    using System.Runtime.CompilerServices;

    using MIMEngine.Core.Item;
    using MIMEngine.Core.Player;

    using Newtonsoft.Json.Linq;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    public class Player
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("hid")]
        public string HubGuid;

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

        public Player(
            string id,
            string name,
            string email,
            string password,
            string gender,
            string race,
            string selectedClass,
            int strength,
            int dexterity,
            int constitution,
            int wisdom,
            int intelligence,
            int charisma)
        {
            this.HubGuid = id;
            this.Email = email;
            this.Password = password;

            //info
            this.Name = name;
            this.Gender = gender;
            this.Race = race;
            this.SelectedClass = selectedClass;
            this.Level = 1;
            this.AlignmentScore = 0;
            this.Experience = 0;
            this.ExperienceToNextLevel = 1000; // create class to work out
            this.HitPoints = 30; // class to workout
            this.MaxHitPoints = 30;
            this.ManaPoints = 50;
            this.MaxManaPoints = 50;
            this.MovePoints = 60;
            this.MaxMovePoints = 60;
            this.Explored = 1;
            this.HitRoll = 1;
            this.DamRoll = 1;
            this.Wimpy = 10;
            this.Hours = 0;
            this.Weight = 0;
            this.MaxWeight = 70; // class to workout
            this.Status = "Standing"; // enum property? 1 standing
            this.Inventory = this.Inventory ?? (this.Inventory = new List<Item>());

            //kills
            this.MobKills = 0;
            this.MobDeaths = 0;
            this.Pkills = 0;
            this.PkDeaths = 0;
            this.PkPoints = 0;

            //Money
            this.Gold = 0;
            this.Silver = 5;
            this.Copper = 100;

            //Location
            this.Region = "Valston";
            this.Area = "Town";
            this.AreaId = 0;

            //Eq
            this.Equipment = this.Equipment ?? (this.Equipment = new Equipment());
            this.Equipment.Floating = this.Equipment.Floating;
            this.Equipment.Head = Equipment.Head;
            this.Equipment.Face = Equipment.Face;
            this.Equipment.Eyes = Equipment.Eyes;
            this.Equipment.LeftEar = Equipment.LeftEar;
            this.Equipment.RightEar = Equipment.RightEar;
            this.Equipment.Neck = Equipment.Neck;
            this.Equipment.Neck2 = Equipment.Neck2;
            this.Equipment.Cloak = Equipment.Cloak;
            this.Equipment.AboutBody = Equipment.AboutBody;
            this.Equipment.Body = Equipment.Body;
            this.Equipment.Waist = Equipment.Waist;
            this.Equipment.LeftSheath = Equipment.LeftSheath;
            this.Equipment.RightSheath = Equipment.RightSheath;
            this.Equipment.BackSheath = Equipment.BackSheath;
            this.Equipment.Back = Equipment.Back;
            this.Equipment.LeftWrist = Equipment.LeftWrist;
            this.Equipment.RightWrist = Equipment.RightWrist;
            this.Equipment.LeftHand = Equipment.LeftHand;
            this.Equipment.RightHand = Equipment.RightHand;
            this.Equipment.LeftRing = Equipment.LeftRing;
            this.Equipment.RightRing = Equipment.RightRing;
            this.Equipment.Legs = Equipment.Legs;
            this.Equipment.Feet = Equipment.Feet;
 

            //attributes

            this.Strength = strength;
            this.Dexterity = dexterity;
            this.Constitution = constitution;
            this.Wisdom = wisdom;
            this.Intelligence = intelligence;
            this.Charisma = charisma;



        }

        public void SavePlayerInformation()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + Name + ".json", json);
        }

        public JObject ReturnPlayerInformation()
        {
            JObject json = JObject.Parse(JsonConvert.SerializeObject(this));
            return json;
        }
    }
}