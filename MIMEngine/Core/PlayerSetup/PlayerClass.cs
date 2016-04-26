using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    class PlayerClass
    {
        public string name { get; set; }
        public string description { get; set; }

        public PlayerClass (string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public static List<PlayerClass> setupClass()
        {
            var classList = new List<PlayerClass>();

            classList.Add(new PlayerClass("Fighter", "The Fighter Class"));
            classList.Add(new PlayerClass("Theif", "The Theif Class"));
            classList.Add(new PlayerClass("Cleric", "The Cleric Class"));
            classList.Add(new PlayerClass("Mage", "The Mage Class"));

            return classList;
        }

        public static PlayerClass selectClass(string findClass)
        {
            var selectedClass = setupClass().FirstOrDefault(x => x.name.ToLower().StartsWith(findClass.ToLower()));

            return selectedClass;
        }
    }
}
