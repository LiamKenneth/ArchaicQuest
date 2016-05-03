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
        [JsonProperty("id")]
        public string hubGuid;
        [JsonProperty("n")]
        public string name;
        [JsonProperty("e")]
        public string email;
        [JsonProperty("p")]
        public string password;
        [JsonProperty("g")]
        public string gender;
        [JsonProperty("r")]
        public string race;
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


        public PlayerSetup(string id, string name, string email, string password, string gender, string race, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {
            this.hubGuid = id;
            this.name = name;
            this.email = email;
            this.password = password;
            this.gender = gender;
            this.race = race;
            this.selectedClass = selectedClass;
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = constitution;
            this.wisdom = wisdom;
            this.intelligence = intelligence;
            this.charisma = charisma;


          
        }

        public void SavePlayerInformation()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(AppDomain.CurrentDomain.RelativeSearchPath + "/" + this.name + ".json", json);
        }
    }
}
