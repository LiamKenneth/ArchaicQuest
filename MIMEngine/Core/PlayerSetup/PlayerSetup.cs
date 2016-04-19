using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    public class PlayerSetup
    {
        [JsonProperty("n")]
        public string name;
        [JsonProperty("s")]
        public string sex;
        [JsonProperty("sc")]
        public string selectedClass;
        [JsonProperty("as")]
        public int strength;
        [JsonProperty("ad")]
        public int dexterity;
        [JsonProperty("ac")]
        public int constitution;
        [JsonProperty("aw")]
        public int wisdom;
        [JsonProperty("ai")]
        public int intelligence;
        [JsonProperty("ach")]
        public int charisma;

        public PlayerSetup(string name, string sex, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {
            this.name = name;
            this.sex = sex;
            this.selectedClass = selectedClass;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = constitution;
            this.wisdom = wisdom;
            this.intelligence = intelligence;
            this.charisma = charisma;

            this.saveUserInformation();
        }

        public void saveUserInformation()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(AppDomain.CurrentDomain.RelativeSearchPath + "/" + this.name + ".json", json);
        }

        //public static void CharacterSetup(string name, string sex, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        //{


        //    PlayerSetup createPlayer = new PlayerSetup();

        //    createPlayer.name = name;
        //    createPlayer.sex = sex;
        //    createPlayer.selectedClass = selectedClass;
        //    createPlayer.strength = strength;
        //    createPlayer.dexterity = dexterity;
        //    createPlayer.constitution = constitution;
        //    createPlayer.wisdom = wisdom;
        //    createPlayer.intelligence = intelligence;
        //    createPlayer.charisma = charisma;

        //    string json = JsonConvert.SerializeObject(createPlayer, Formatting.Indented);
        //    File.WriteAllText(@"c:\person.json", json);
        //}
    }
}
