using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.PlayerSetup
{
    using System.Runtime.CompilerServices;

    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.Player;

    using Newtonsoft.Json.Linq;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Core.Player.Skills;
    using MongoDB.Driver;

    public class Player
    {

        public enum PlayerStatus
        {
            Standing = 1,
            Resting = 2,
            Sleeping = 3,
            Fighting = 4,
            Incapitated = 5,
            Dead = 6,
            Ghost = 7,
            Busy = 8,
 
        }

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
        public PlayerStatus Status;

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

        public Player()
        {
            //this.HubGuid = id;
            this.Type = "Player";
            //this.Email = email;
            //this.Password = password;

            ////info
            //this.Name = name;
            //this.Gender = gender;
            //this.Race = race;
            //this.SelectedClass = selectedClass;
            this.Level = 1;
            this.Description = this.Description ?? "You see nothing special about them.";
            this.AlignmentScore = 0;
            this.Experience = 500;
            this.ExperienceToNextLevel = 1000; // create class to work out
            this.HitPoints = 100; // class to workout
            this.MaxHitPoints = 100;
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
            this.Status = PlayerStatus.Standing; // enum property? 1 standing
            this.Target = null;
            this.Inventory = this.Inventory ?? (this.Inventory = new List<Item>());



            var dagger = new Core.Item.Item
            {
                actions = new Core.Item.Action(),
                name = "Blunt dagger",
                eqSlot = Core.Item.Item.EqSlot.Wield,
                weaponType = Core.Item.Item.WeaponType.ShortBlades,
                stats = new Core.Item.Stats { damMin = 2, damMax = 4, minUsageLevel = 1, damRoll = 0},
                type = Core.Item.Item.ItemType.Weapon,
                equipable = true,
                attackType = Core.Item.Item.AttackType.Pierce,
                slot = "wield",
                location = Item.ItemLocation.Inventory
                
            };

            this.Inventory.Add(dagger);

            this.Skills = this.Skills = new List<Skill>();
            
            var punch = Punch.PunchAb();
            punch.Proficiency = 0.1;
      

            this.Skills.Add(punch);
            //this.Skills.Add(shortBlades);
          

        

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
            this.Region = "Anker";
            this.Area = "Anker";
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

            //this.Strength = strength;
            //this.Dexterity = dexterity;
            //this.Constitution = constitution;
            //this.Wisdom = wisdom;
            //this.Intelligence = intelligence;
            //this.Charisma = charisma;



        }

        public void SavePlayerInformation()
        {
            //string json = JsonConvert.SerializeObject(this);
            //File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + Name + ".json", json);


            const string ConnectionString = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

            var playerCollection = database.GetCollection<Player>("Player");

            playerCollection.InsertOne(this);

        }

        public JObject ReturnPlayerInformation()
        {
            JObject json = JObject.Parse(JsonConvert.SerializeObject(this));
            return json;
        }
    }
}