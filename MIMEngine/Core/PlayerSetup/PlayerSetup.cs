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
        string name;
        string sex;
        string selectedClass;
        int strength;
        int dexterity;
        int constitution;
        int wisdom;
        int intelligence;
        int charisma;

        public static void CharacterSetup(string name, string sex, string selectedClass, int strength, int dexterity, int constitution, int wisdom, int intelligence, int charisma)
        {


            PlayerSetup createPlayer = new PlayerSetup();

            createPlayer.name = name;
            createPlayer.sex = sex;
            createPlayer.selectedClass = selectedClass;
            createPlayer.strength = strength;
            createPlayer.dexterity = dexterity;
            createPlayer.constitution = constitution;
            createPlayer.wisdom = wisdom;
            createPlayer.intelligence = intelligence;
            createPlayer.charisma = charisma;

            string json = JsonConvert.SerializeObject(createPlayer, Formatting.Indented);
            File.WriteAllText(@"c:\person.json", json);
        }
    }
}
