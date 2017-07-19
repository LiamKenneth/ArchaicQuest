using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Mob;

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
            Floating = 9,

        }

        public enum PlayerTypes
        {
            Player = 1,
            Mob = 2

        }

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        [BsonElement("hid")]
        public string HubGuid;

        [BsonElement("sd")]
        public DateTime JoinedDate;

        [BsonElement("lcd")]
        public DateTime LastCommandTime;

        [BsonElement("ty")]
        public PlayerTypes Type = PlayerTypes.Mob;

        [BsonElement("e")]
        public string Email;

        [BsonElement("de")]
        public string Description;

        [BsonElement("p")]
        public string Password;

        // General Info
        [BsonElement("n")]
        public string Name;

        [BsonElement("kn")]
        public bool KnownByName;

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

        [BsonElement("txp")]
        public int TotalExperience;

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

        [BsonElement("ask")]
        [BsonIgnoreIfNull]
        public Skill ActiveSkill { get; set; }

        [BsonElement("afig")]
        [BsonIgnoreIfNull]
        public bool ActiveFighting{ get; set; }


        [BsonElement("af")]
        public List<Affect> Affects { get; set; } = new List<Affect>();

        [BsonIgnore]
        public List<Player> Followers { get; set; } = new List<Player>();

        [BsonIgnore]
        public Player Following { get; set; }

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

        [JsonIgnore]
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

        [BsonElement("mas")]
        public int MaxStrength;

        [BsonElement("mad")]
        public int MaxDexterity;

        [BsonElement("mac")]
        public int MaxConstitution;

        [BsonElement("maw")]
        public int MaxWisdom;

        [BsonElement("mai")]
        public int MaxIntelligence;

        [BsonElement("mach")]
        public int MaxCharisma;

        //location
        [BsonElement("re")]
        public string Region;

        [BsonElement("ar")]
        public string Area;

        [BsonElement("ari")]
        public int AreaId;

        [BsonElement("rec")]
        public Recall Recall;

        [BsonElement("pra")]
        public int Practices;

        [BsonElement("tra")]
        public int Trains;

        [BsonElement("ndec")]
        public bool nonDectect = false;

        [BsonElement("inv")]
        public bool invis = false;

        [BsonElement("dinv")]
        public bool DetectInvis = false;

        [BsonElement("hidd")]
        public bool hidden = false;

        [BsonElement("dhidd")]
        public bool DetectHidden= false;

        [BsonElement("poi")]
        public bool poisoned = false;


        [BsonElement("itl")]
        public int intoxicationLevel = 0;

        [BsonElement("itml")]
        //Equal to con
        public int intoxicationMaxLevel = 12; 


        [BsonElement("qlog")]
        public List<Quest> QuestLog;

        //NPC Properties
        [BsonElement("ne")]
        [BsonIgnoreIfNull]
        public List<string> Emotes;

        //NPC Properties
        [BsonElement("nid")]
        [BsonIgnoreIfNull]
        public Guid NPCId;

        [BsonElement("nln")]
        [BsonIgnoreIfNull]
        public String NPCLongName;

        //NPC Properties
        [BsonElement("nr")]
        [BsonIgnoreIfNull]
        public bool Roam;

        //NPC Properties
        [BsonElement("nph")]
        [BsonIgnoreIfNull]
        public List<string> PathList;

        [BsonElement("nphc")]
        [BsonIgnoreIfNull]
        public int PathCount;

        //NPC Properties
        [BsonElement("na")]
        [BsonIgnoreIfNull]
        public bool Aggro;

        //NPC Properties
        [BsonElement("ng")]
        [BsonIgnoreIfNull]
        public bool Greet = false;
        //NPC Properties

        [BsonElement("ngm")]
        [BsonIgnoreIfNull]
        public string GreetMessage = String.Empty;

        //NPC Properties
        [BsonElement("ns")]
        [BsonIgnoreIfNull]
        public bool Shop;

        [BsonElement("nsm")]
        [BsonIgnoreIfNull]
        public string sellerMessage;

        //NPC Properties
        [BsonElement("nsi")]
        [BsonIgnoreIfNull]
        public List<Item> itemsToSell;

        //NPC Properties
        [BsonElement("ndia")]
        [BsonIgnoreIfNull]
        public List<Responses> Dialogue;

        //NPC Properties
        [BsonElement("ndiaT")]
        [BsonIgnoreIfNull]
        public List<DialogTree> DialogueTree;

        [BsonElement("nqu")]
        [BsonIgnoreIfNull]
        public List<Quest> Quest;

        //NPC Properties
        [BsonElement("nt")]
        [BsonIgnoreIfNull]
        public bool Trainer;

        //NPC Properties
        [BsonElement("ngu")]
        [BsonIgnoreIfNull]
        public bool Guard;

        [BsonElement("eoe")]
        [BsonIgnoreIfNull]
        public string EventOnEnter;

        [BsonElement("eow")]
        [BsonIgnoreIfNull]
        public string EventWake;

        [BsonElement("eod")]
        [BsonIgnoreIfNull]
        public string EventDeath;

        [BsonIgnoreIfNull]
        [BsonElement("eoc")]
        public Dictionary<string, string> EventOnComunicate;

        [BsonElement("eowe")]
        [BsonIgnoreIfNull]
        public string EventWear { get; set; }

        [BsonElement("nch")]
        [BsonIgnoreIfNull]
        public bool NewbieChannel = true;

        [BsonElement("gch")]
        public bool GossipChannel = true;

        [BsonElement("och")]
        public bool OocChannel = true;



        public Player()
        {
            this.Type = PlayerTypes.Player;
            this.Level = 1;
            this.Description = this.Description ?? "You see nothing special about them.";
            this.AlignmentScore = 0;
            this.TotalExperience = 0;
            this.Experience = 0;
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
       
 
            this.Skills  = new List<Skill>();
 

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
            this.Region = "Tutorial";
            this.Area = "Tutorial";
            this.AreaId = 0;

            //Eq
            this.Equipment = this.Equipment ?? (this.Equipment = new Equipment());
            this.Equipment.Floating = this.Equipment.Floating;
            this.Equipment.Head = Equipment.Head;
            this.Equipment.Face = Equipment.Face;         
            this.Equipment.Neck = Equipment.Neck;
            this.Equipment.Neck2 = Equipment.Neck2;
            this.Equipment.Body = Equipment.Body;
            this.Equipment.Waist = Equipment.Waist;         
            this.Equipment.Legs = Equipment.Legs;
            this.Equipment.Feet = Equipment.Feet;

            this.Practices = 10;
            this.Trains = 10;
            this.KnownByName = true;
           
            this.QuestLog = new List<Quest>();
            this.Quest = new List<Quest>();
            this.EventWake = "";
            this.EventOnEnter = "";
            this.EventOnComunicate = new Dictionary<string, string>();

            var recall = new Recall
            {
                Area = "Anker",
                AreaId = 0,
                Region = "Anker"
            };


            this.Recall = recall;

          
        }

        /// <summary>
        /// USed for spells and skills for now
        /// </summary>
        /// <param name="player"></param>
        public static void SetState(Player player)
        {

            if (player.Target != null)
            {
                player.Status = Player.PlayerStatus.Fighting;
            }
            else
            {
                player.Status = Player.PlayerStatus.Standing;
            }
        }

        public static void DebugPlayer(Player player)
        {
            HubContext.SendToClient("Debug:", player.HubGuid);
            HubContext.SendToClient("Player Target: " + player.Target?.Name, player.HubGuid);
            HubContext.SendToClient("Player is Fighting: " + player.ActiveFighting, player.HubGuid);
            HubContext.SendToClient("Player Has active skill: " + player.ActiveSkill?.Name, player.HubGuid);
            HubContext.SendToClient("Player status: " + player.Status, player.HubGuid);
            HubContext.SendToClient("Player items: ", player.HubGuid);

            if (player.Inventory != null)
            {
                foreach (var item in player.Inventory)
                {
                    HubContext.SendToClient(item.name + " " + item.location + " " + item.type, player.HubGuid);
                }
            }



        }

    }
}