using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
   public class PlayerClass
    {
        public string name { get; set; }
        public string description { get; set; }
        public string imgUrl { get; set; }

        public PlayerClass (string name, string description, string imgUrl)
        {
            this.name = name;
            this.description = description;
            this.imgUrl = imgUrl;
        }

        public static List<PlayerClass> setupClass()
        {
            var classList = new List<PlayerClass>();

            string fighterInfo =
                "Warriors are the hardiest of fighters. Using their great defense skills as well as multiple attacks, they make the best tanks in battle. Warriors may choose to develop into one of four subclasses when they reach level 30: Knights, Paladins, or Berserkers. (Note that only Warriors of the Barbarian race have the innate fury to become a Berserker.) Warriors require a good amount of strength.";
            string fighterImgURL = "/UI/img/human.jpg";
            string thiefInfo =
                "One of the hardest, though advantageous classes to play. Thieves master the art of deception, as well as crime. They do not earn their experience from killing like other classes, but instead make their levels by stealing from MOB's and PLAYERS. Thieves are the most frail of fighters, but their arts of deception make up for it. If you are looking for a kill and smash class, this is NOT the one to play. But if you are looking for an edge over others, try and master this class.";
            string thiefImgURL = "/UI/img/human.jpg";
            string clericInfo =
                "Clerics have mastered the art of healing. Through their great channels with the immortals above, they have an enormous variety of defensive spells to help with. There are, however, those clerics that have found their way to the darkside, and use their spells to bring forth the dead to bring havoc to the realm.";
            string clericImgURL = "/UI/img/human.jpg";

            string mageInfo = "Mages can inflict enormous amounts of damage from their wide variety of offensive spells. They are, however, extremely fraile, and can be easily defeated if attacked first. Mages are an advanced class to play, so if you are new to the mud, you might want to try another class first. Mages study the basic spells in the schools of ILLUSION, ENCHANTMENT, MENTAL, DIVINATION, ALTERATION, CONJURATION, SUMMONING, NECROMANCY, INVOCATION, EVOCATION, and ABJURATION.";
            string mageImgURL = "/UI/img/human.jpg";

            classList.Add(new PlayerClass("Fighter", fighterInfo, fighterImgURL));
            classList.Add(new PlayerClass("Theif", thiefInfo, thiefImgURL));
            classList.Add(new PlayerClass("Cleric", clericInfo, clericImgURL));
            classList.Add(new PlayerClass("Mage", mageInfo, mageImgURL));


            return classList;
        }

        public static PlayerClass selectClass(string findClass)
        {
            var selectedClass = setupClass().FirstOrDefault(x => x.name.ToLower().StartsWith(findClass.ToLower()));

            return selectedClass;
        }
    }
}
