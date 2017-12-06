using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.World.Alchemy;
using MIMWebClient.Core.World.Cooking;
using MIMWebClient.Core.World.Crafting;
using MIMWebClient.Core.World.Crafting.Carve;
using MIMWebClient.Core.World.Crafting.Smithing;
using MIMWebClient.Core.World.Knitting;

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
            Mounted = 10,
            Stunned = 11

        }

        public enum SizeCategories
        {
            Tiny = 1,
            Small = 2,
            Medium = 3,
            Large = 4,
            Huge = 5,


        }


        public enum MobAttackTypes
        {
            Punch,
            Pound,
            Bite,
            Charge,
            Peck,
            Headbut,
        }

        public enum PlayerTypes
        {
            Player = 1,
            Mob = 2

        }

        public int Id { get; set; }

        [BsonElement("hid")]
        public string HubGuid;

        [BsonElement("sd")]
        public DateTime JoinedDate { get; set; }

        [BsonElement("lcd")]
        public DateTime LastLoginTime { get; set; }

        [BsonElement("lcd")]
        public DateTime LastCommandTime { get; set; }

        [BsonElement("ty")]
        public PlayerTypes Type { get; set; } = PlayerTypes.Mob;

        [BsonElement("e")]
        public string Email { get; set; }

        [BsonElement("de")]
        public string Description { get; set; }

        [BsonElement("p")]
        public string Password { get; set; }

        // General Info
        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("kn")]
        public bool KnownByName { get; set; }

        [BsonElement("g")]
        public string Gender { get; set; }

        public long PlayTime { get; set; } = 0;

        public long TotalPlayTime { get; set; } = 0;

        [BsonElement("r")]
        public string Race { get; set; }

        [BsonElement("sc")]
        public string SelectedClass { get; set; }

        [BsonElement("lvl")]
        public int Level { get; set; }

        [BsonElement("ali")]
        public int AlignmentScore { get; set; }

        [BsonElement("txp")]
        public int TotalExperience { get; set; }

        [BsonElement("xp")]
        public int Experience { get; set; }

        [BsonElement("tnl")]
        public int ExperienceToNextLevel { get; set; }

        [BsonElement("hp")]
        public int HitPoints { get; set; }

        [BsonElement("mhp")]
        public int MaxHitPoints { get; set; }

        [BsonElement("mp")]
        public int ManaPoints { get; set; }

        [BsonElement("mmp")]
        public int MaxManaPoints { get; set; }

        [BsonElement("mvp")]
        public int MovePoints { get; set; }

        [BsonElement("mmvp")]
        public int MaxMovePoints { get; set; }

        [BsonElement("in")]
        public ItemContainer Inventory { get; set; }

        [BsonElement("eq")]
        public Equipment Equipment { get; set; }

        [BsonElement("sk")]
        public List<Skill> Skills { get; set; }
 
        public List<string> CraftingRecipes { get; set; } = new List<string>();

        [BsonElement("ask")]
        [BsonIgnoreIfNull]
        public Skill ActiveSkill;

        [BsonElement("afig")]
        [BsonIgnoreIfNull]
        public bool ActiveFighting;


        [BsonElement("af")]
        public List<Effect> Effects { get; set; } = new List<Effect>();

        [BsonIgnore]
        public List<Player> Followers = new List<Player>();

        [BsonIgnore]
        public Player Following;

        [BsonIgnore]
        public string Pose { get; set; }

        //Game stats
        [BsonElement("ex")]
        public int Explored { get; set; }

        [BsonElement("hr")]
        public int HitRoll { get; set; }

        [BsonElement("dr")]
        public int DamRoll { get; set; }

        [BsonElement("wi")]
        public int Wimpy { get; set; }

        [BsonElement("hrs")]
        public int Hours { get; set; }

        [BsonElement("we")]
        public int Weight { get; set; }

        [BsonElement("mwe")]
        public int MaxWeight { get; set; }

        [BsonElement("st")]
        public PlayerStatus Status { get; set; }

        public int StunDuration { get; set; } = 0;

        [JsonIgnore]
        [BsonElement("ta")]
        public Player Target { get; set; }

        [BsonElement("arr")]
        public int ArmorRating { get; set; }

        public int SpellResistance { get; set; } = 0;

        public SizeCategories SizeCategory { get; set; }

        [BsonElement("s")]
        public int Saves { get; set; }

        //Kills
        [BsonElement("mk")]
        public int MobKills { get; set; }

        [BsonElement("md")]
        public int MobDeaths { get; set; }

        [BsonElement("pk")]
        public int Pkills { get; set; }

        [BsonElement("pd")]
        public int PkDeaths { get; set; }

        [BsonElement("pkp")]
        public int PkPoints { get; set; }

        //Money
        [BsonElement("gp")]
        public int Gold { get; set; }

        [BsonElement("sp")]
        public int Silver { get; set; }

        [BsonElement("cp")]
        public int Copper { get; set; }

        // attributes
        [BsonElement("as")]
        public int Strength { get; set; }

        [BsonElement("ad")]
        public int Dexterity { get; set; }

        [BsonElement("ac")]
        public int Constitution { get; set; }

        [BsonElement("aw")]
        public int Wisdom { get; set; }

        [BsonElement("ai")]
        public int Intelligence { get; set; }

        [BsonElement("ach")]
        public int Charisma { get; set; }

        [BsonElement("mas")]
        public int MaxStrength { get; set; }

        [BsonElement("mad")]
        public int MaxDexterity { get; set; }

        [BsonElement("mac")]
        public int MaxConstitution { get; set; }

        [BsonElement("maw")]
        public int MaxWisdom { get; set; }

        [BsonElement("mai")]
        public int MaxIntelligence { get; set; }

        [BsonElement("mach")]
        public int MaxCharisma { get; set; }

        //location
        [BsonElement("re")]
        public string Region { get; set; }

        [BsonElement("ar")]
        public string Area { get; set; }

        [BsonElement("ari")]
        public int AreaId { get; set; }

        [BsonElement("rec")]
        public Recall Recall { get; set; }

        [BsonElement("pra")]
        public int Practices { get; set; }

        [BsonElement("tra")]
        public int Trains { get; set; }

        [BsonElement("ndec")]
        public bool nonDectect { get; set; } = false;

        [BsonElement("inv")]
        public bool invis { get; set; } = false;

        [BsonElement("dinv")]
        public bool DetectInvis { get; set; } = false;

        [BsonElement("hidd")]
        public bool hidden { get; set; } = false;

        [BsonElement("dhidd")]
        public bool DetectHidden { get; set; } = false;


 
        public bool CanFollow { get; set; } = true;


        [BsonElement("poi")]
        public bool poisoned { get; set; } = false;


        [BsonElement("itl")]
        public int intoxicationLevel { get; set; } = 0;

        [BsonElement("itml")]
        //Equal to con
        public int intoxicationMaxLevel { get; set; } = 12;


        [BsonElement("qlog")]
        public List<Quest> QuestLog { get; set; }

        public bool IsMount { get; set; } = false;

        public Player Mount { get; set; }
  

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
        public List<Quest> Quest { get; set; }

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
        public string EventWear;

        [BsonElement("nch")]
        [BsonIgnoreIfNull]
        public bool NewbieChannel { get; set; } = true;

        [BsonElement("gch")]
        public bool GossipChannel { get; set; } = true;

        [BsonElement("och")]
        public bool OocChannel { get; set; } = true;

        public MobAttackTypes MobAttackType { get; set; }  = MobAttackTypes.Punch;

        public Stats MobAttackStats { get; set; }

        public bool MobTalkOnEnter { get; set; } = false;

        public bool PlayerIsForaging { get; set; } = false;

        public int ForageRank { get; set; } = 1;

        public string ChosenCraft { get; set; } = null;


        public Player()
        {

            this.Type = PlayerTypes.Player;
            this.Level = 1;
            this.Description = this.Description ?? "You see nothing special about them.";
            this.AlignmentScore = 0;
            this.TotalExperience = 0;
            this.Experience = 0;
            this.ExperienceToNextLevel = 1000; // create class to work out
            this.HitPoints = 32; // class to workout
            this.MaxHitPoints = 32;
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
            this.Inventory = this.Inventory ?? (this.Inventory = new ItemContainer());


            this.Skills = new List<Skill>()
            {
                //Forage.ForageAb(),
                //Chopping.ChoppingAb(),
                //Carving.CarvingAb(),
                //Crafting.CraftingAb(),
                //Brewing.BrewingAb(),
                //Cook.CookAb(),
                //Knitting.KnittingAb(),
                //Carving.CarvingAb(),
                Swim.SwimAb()
            };


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

            this.CraftingRecipes.Add(MIMWebClient.Core.World.Crafting.Crafting.CampFire().Name);
            this.CraftingRecipes.Add(MIMWebClient.Core.World.Crafting.Crafting.PineLog().Name);
            this.CraftingRecipes.Add(Recipes.SmokedChub().Name);

            this.CraftingRecipes.Add(Recipes.BoiledCarp().Name);
            this.CraftingRecipes.Add(Recipes.PeasantStew().Name);
            this.CraftingRecipes.Add(Recipes.SeasonedBream().Name);


            this.CraftingRecipes.Add(Recipes.BoiledCarp().Name);
            this.CraftingRecipes.Add(Recipes.FishStew().Name);
            this.CraftingRecipes.Add(Recipes.Bread().Name);

            this.CraftingRecipes.Add(Recipes.FriedTrout().Name);
            this.CraftingRecipes.Add(Recipes.TurtleSoup().Name);
            this.CraftingRecipes.Add(Recipes.FriedEel().Name);
            this.CraftingRecipes.Add(Recipes.FrogLegs().Name);


            this.CraftingRecipes.Add(Alchemy.BurnCream().Name);
            this.CraftingRecipes.Add(Alchemy.Antivenom().Name);
            this.CraftingRecipes.Add(Alchemy.LavenderPerfume().Name);
            this.CraftingRecipes.Add(Alchemy.Antibiotic().Name);
            this.CraftingRecipes.Add(Alchemy.Antiseptic().Name);

            this.CraftingRecipes.Add(Smithing.Lantern().Name);
            this.CraftingRecipes.Add(Smithing.CopperSword().Name);
            this.CraftingRecipes.Add(Smithing.CopperMace().Name);
            this.CraftingRecipes.Add(Smithing.CopperAxe().Name);
            this.CraftingRecipes.Add(Smithing.CopperDagger().Name);
            this.CraftingRecipes.Add(Smithing.CopperFlail().Name);

            this.CraftingRecipes.Add(Carve.WoodenRaft().Name);
            this.CraftingRecipes.Add(Carve.WoodenTorch().Name);
            this.CraftingRecipes.Add(Carve.WoodenChest().Name);

            this.CraftingRecipes.Add(MIMWebClient.Core.World.Knitting.Knitting.WoolenClothBoots().Name);
            this.CraftingRecipes.Add(MIMWebClient.Core.World.Knitting.Knitting.WoolenClothGloves().Name);
            this.CraftingRecipes.Add(MIMWebClient.Core.World.Knitting.Knitting.WoolenClothHelmet().Name);
            this.CraftingRecipes.Add(MIMWebClient.Core.World.Knitting.Knitting.WoolenClothLeggings().Name);
            this.CraftingRecipes.Add(MIMWebClient.Core.World.Knitting.Knitting.WoolenClothShirt().Name);
            this.CraftingRecipes.Add(MIMWebClient.Core.World.Knitting.Knitting.WoolenClothSleeves().Name);

        }

        public static SizeCategories ReturnCharSize(string race)
        {
                          
            if (race == "Mau" || race == "Elf" || race == "Dark Elf")
            {
                return SizeCategories.Small;
            }

            return SizeCategories.Medium;
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

        public static void UpdateMoves(Player player, int points, bool increment = true)
        {
            if (increment)
            {
                player.MovePoints += points;


                if (player.MovePoints >= player.MaxMovePoints)
                {
                    player.MovePoints = player.MaxMovePoints;
                }
            }
            else
            {
                player.MovePoints -= points;


                if (player.MovePoints <= 0)
                {
                    player.MovePoints = 0;
                }
            }
           

            Score.ReturnScoreUI(player);
        }

        public static void UpdateHitPoints(Player player, int points, bool increment = true)
        {
            if (increment)
            {
                player.HitPoints += points;


                if (player.HitPoints >= player.MaxHitPoints)
                {
                    player.HitPoints = player.MaxHitPoints;
                }
            }
            else
            {
                player.HitPoints -= points;


                if (player.HitPoints <= 0)
                {
                    player.HitPoints = 0;
                    
                }
            }


            Score.ReturnScoreUI(player);
        }
 
        public static void LearnFromMistake(Player player, Skill skill, int xpGain)
        {

            if (skill.Proficiency < 95)
            {

                var xp = new Experience();
                player.Experience += xpGain;

                xp.GainLevel(player);

                var gain = Helpers.Rand(1, 5);

                skill.Proficiency += gain;

                HubContext.Instance.SendToClient($"<span style='color:yellow'>Your {skill.Name} skill increases by {gain}%.</span>", player.HubGuid);


                HubContext.Instance.SendToClient($"<span style='color:yellow'>You learn from your mistakes and gain {xpGain} experience points</span>",
                    player.HubGuid);


            }

        }


        public static void MobAttack(Player player, Player mob, Room.Room room)
        {
            Command.ParseCommand($"kill {player.Name}", mob, room);
         
        }

        public static void SetGodmode(Player player)
        {
            player.MaxMovePoints += 3000;
            player.MovePoints += 3000;
            player.MaxHitPoints += 3000;
            player.HitPoints += 3000;
            player.MaxManaPoints += 3000;
            player.ManaPoints += 3000;

            Score.ReturnScoreUI(player);
        }

        public static void SetGold(Player player, string amount)
        {
            if (string.IsNullOrEmpty(amount))
            {
                HubContext.Instance.SendToClient("You need to add an amount of gold", player.HubGuid);
                return;
            }

            HubContext.Instance.SendToClient("You gain: " + amount + " gold", player.HubGuid);
            player.Gold += Int32.Parse(amount);

            Score.ReturnScoreUI(player);
        }

        public static void SetAC(Player player, string amount)
        {
            if (string.IsNullOrEmpty(amount))
            {
                HubContext.Instance.SendToClient("You need to add an amount", player.HubGuid);
                return;
            }

            HubContext.Instance.SendToClient("You gain: " + amount + " Armour Class", player.HubGuid);
            player.ArmorRating = Int32.Parse(amount);

            Score.ReturnScoreUI(player);
        }

        public static void DebugPlayer(Player player)
        {
            HubContext.Instance.SendToClient("Debug:", player.HubGuid);
            HubContext.Instance.SendToClient("Room Id: " + player.AreaId, player.HubGuid);
          
            HubContext.Instance.SendToClient("Player Target: " + player.Target?.Name, player.HubGuid);
            HubContext.Instance.SendToClient("Player is Fighting: " + player.ActiveFighting, player.HubGuid);
            HubContext.Instance.SendToClient("Player Has active skill: " + player.ActiveSkill?.Name, player.HubGuid);
            HubContext.Instance.SendToClient("Player status: " + player.Status, player.HubGuid);
            HubContext.Instance.SendToClient("Player items: ", player.HubGuid);

            if (player.Inventory != null)
            {
                foreach (var item in player.Inventory)
                {
                    HubContext.Instance.SendToClient(item.name + " " + item.location + " " + item.type, player.HubGuid);
                }
            }

            if (player.Target != null)
            {
                HubContext.Instance.SendToClient("target HP:  " + player.Target.HitPoints, player.HubGuid);
                HubContext.Instance.SendToClient("target status:  " + player.Target.Status, player.HubGuid);
                 HubContext.Instance.SendToClient("target active fighting:  " + player.Target.ActiveFighting, player.HubGuid);

            }



        }

    }
}