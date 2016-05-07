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

    using Newtonsoft.Json.Linq;

    public class PlayerSetup
    {
        [JsonProperty("id")]
        public string HubGuid;    
        [JsonProperty("e")]
        public string Email;
        [JsonProperty("p")]
        public string Password;

        // General Info
        [JsonProperty("n")]
        public string Name;
        [JsonProperty("g")]
        public string Gender;
        [JsonProperty("r")]
        public string Race;
        [JsonProperty("sc")]
        public string SelectedClass;
        [JsonProperty("lvl")]
        public int Level;
        [JsonProperty("ali")]
        public int AlignmentScore;
        [JsonProperty("xp")]
        public int Experience;
        [JsonProperty("tnl")]
        public int ExperienceToNextLevel;
        [JsonProperty("hp")]
        public int HitPoints;
        [JsonProperty("mhp")]
        public int MaxHitPoints;
        [JsonProperty("mp")]
        public int ManaPoints;
        [JsonProperty("mmp")]
        public int MaxManaPoints;
        [JsonProperty("mvp")]
        public int MovePoints;
        [JsonProperty("mmvp")]
        public int MaxMovePoints;

        [JsonProperty("in")]
        public object [] Inventory;

        //Game stats
        [JsonProperty("ex")]
        public int Explored;
        [JsonProperty("hr")]
        public int HitRoll;
        [JsonProperty("dr")]
        public int DamRoll;
        [JsonProperty("wi")]
        public int Wimpy;
        [JsonProperty("hrs")]
        public int Hours;
        [JsonProperty("we")]
        public int Weight;
        [JsonProperty("mwe")]
        public int MaxWeight;
        [JsonProperty("st")]
        public int Status;

        //Kills
        [JsonProperty("mk")]
        public int MobKills;
        [JsonProperty("md")]
        public int MobDeaths;
        [JsonProperty("pk")]
        public int Pkills;
        [JsonProperty("pd")]
        public int PkDeaths;
        [JsonProperty("pkp")]
        public int PkPoints;

        //Money
        [JsonProperty("gp")]
        public int Gold;
        [JsonProperty("sp")]
        public int Silver;
        [JsonProperty("cp")]
        public int Copper;

        // attributes
        [JsonProperty("as")]
        public int Strength;
        [JsonProperty("ad")]
        public int Dexterity;
        [JsonProperty("ac")]
        public int Constitution;
        [JsonProperty("aw")]
        public int Wisdom;
        [JsonProperty("ai")]
        public int Intelligence;
        [JsonProperty("ach")]
        public int Charisma;

        //location
        [JsonProperty("re")]
        public string Region;
        [JsonProperty("ar")]
        public int Area;
        [JsonProperty("ari")]
        public int AreaId;

        //Equipment
        [JsonProperty("efl")]
        public object Floating;
        [JsonProperty("eli")]
        public object Light;
        [JsonProperty("eh")]
        public object Head;
        [JsonProperty("efa")]
        public object Face;
        [JsonProperty("ee")]
        public object Eyes;
        [JsonProperty("ele")]
        public object LeftEar;
        [JsonProperty("ere")]
        public object RightEar;
        [JsonProperty("en")]
        public object Neck;
        [JsonProperty("ec")]
        public object Cloak;
        [JsonProperty("ea")]
        public object AboutBody;
        [JsonProperty("eb")]
        public object Body;
        [JsonProperty("ew")]
        public object Waist;
        [JsonProperty("elw")]
        public object LeftWrist;
        [JsonProperty("erw")]
        public object RightWrist;
        [JsonProperty("elh")]
        public object LeftHand;
        [JsonProperty("erh")]
        public object RightHand;
        [JsonProperty("elf")]
        public object LeftFinger;
        [JsonProperty("erf")]
        public object RightFinger;
        [JsonProperty("el")]
        public object Legs;
        [JsonProperty("ef")]
        public object Feet;

        public PlayerSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
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
            this.Inventory = null;
            this.Explored = 1;
            this.HitRoll = 1;
            this.DamRoll = 1;
            this.Wimpy = 10;
            this.Hours = 0;
            this.Weight = 0;
            this.MaxWeight = 70; // class to workout
            this.Status = 1; // enum property? 1 standing

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
            this.Area = 0;
            this.AreaId = 0;

            //Equipment
            this.Floating = "Nothing";
            this.Light = "Nothing";
            this.Head = "Nothing";
            this.Face = "Nothing";
            this.Eyes = "Nothing";
            this.LeftEar = "Nothing";
            this.RightEar = "Nothing";
            this.Neck = "Nothing";
            this.Cloak = "Nothing";
            this.AboutBody = "Nothing";
            this.Body = "Nothing";
            this.Waist = "Nothing";
            this.LeftWrist = "Nothing";
            this.RightWrist = "Nothing";
            this.LeftHand = "Nothing";
            this.RightHand = "Nothing";
            this.LeftFinger = "Nothing";
            this.RightFinger = "Nothing";
            this.Legs = "Nothing";
            this.Feet = "Nothing";


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
    }
}
