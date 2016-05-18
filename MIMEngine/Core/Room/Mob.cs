using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Room
{
    public class Mob
    {
        // General Info
        [JsonProperty("n")]
        public string Name;

        [JsonProperty("g")]
        public string Gender;

        [JsonProperty("de")]
        public string Description;

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
        public object[] Inventory;

        //Game stats
        [JsonProperty("hr")]
        public int HitRoll;

        [JsonProperty("dr")]
        public int DamRoll;

        [JsonProperty("wi")]
        public int Wimpy;

        [JsonProperty("we")]
        public int Weight;

        [JsonProperty("mwe")]
        public int MaxWeight;

        [JsonProperty("st")]
        public int Status;

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
        public string Area;

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
    }
}
