using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.PlayerSetup
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

            string fighterInfo = "Warriors are able to use any weapon and armour effectively along \r\n   side their wide range of lethal and defensive combat skills. \r\n   They have no need for mana, relying on their sheer strength and endurance alone \r\n   to overcome opponents.\r\n   \r\n   Important attributes for Warriors are Strength, Dexterity and Constitution\r\n   \r\n   Every race can train to be an effective warrior. \r\n   \r\n   For beginners we recommend you pick a Human Warrior.\r\n";
            string fighterImgURL = "/UI/img/human.jpg";
            string thiefInfo =
                "   \r\n   Rogues are masters at the arts of remaining hidden and delivering devastating blows\r\n   from the shadows before fleeing into the darkness once more. They are strong in combat but \r\n   can\'t handle the same amount of damage as a warrior.\r\n   \r\n   They are also skilled lock and pocket pickers, can set or disarm traps and know how to apply poison \r\n   to their blade.\r\n   \r\n   Rogues are a versatile class.\r\n   \r\n   Important attributes for Mages are Dexterity, Constitution and Strength\r\n   \r\n   Every race can train to be an rogue but Halfings are one of the best due to their small size. \r\n";
            string thiefImgURL = "/UI/img/human.jpg";
            string clericInfo =
                "   Cleric power comes from the gods they worship, stronger the devotion, stronger the power,\r\n   Clerical spells focus on healing and preserving life rather than destroy it but don\'t be fooled\r\n   clerics know powerful offensive spells to rival any mage. They can also wear any armour just like a warrior.\r\n   \r\n    Important attributes for Mages are Wisdom, Intelligence and Constitution\r\n   \r\n   Every race can train to be a cleric but Dwarfs are one of the best. \r\n";
            string clericImgURL = "/UI/img/human.jpg";

            string mageInfo = "  Mages are the most feared across the realm due to their devastating spells and power.\r\n   The road to such power is a hard, slow journey. Mages struggle more than other classes in melee combat because\r\n   They spent years studying magic and how to hurl a ball of fire towards their opponent instead of training for physical combat.\r\n   This makes mages relatively weak at the beginning of their training but this changes however when a they have mastered the arts of magic.\r\n   \r\n   \r\n   Important attributes for Mages are Intelligence, Wisdom and Dexterity\r\n   \r\n   Every race can train to be a mage but Elves are the best.\r\n";
            string mageImgURL = "/UI/img/human.jpg";

            classList.Add(new PlayerClass("Fighter", fighterInfo, fighterImgURL));
            classList.Add(new PlayerClass("Thief", thiefInfo, thiefImgURL));
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
