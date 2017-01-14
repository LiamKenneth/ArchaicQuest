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

            string fighterInfo =
                "Warrior info";
            string fighterImgURL = "/UI/img/human.jpg";
            string thiefInfo =
                "Thief info";
            string thiefImgURL = "/UI/img/human.jpg";
            string clericInfo =
                "Cleric info";
            string clericImgURL = "/UI/img/human.jpg";

            string mageInfo = "Mage info";
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
