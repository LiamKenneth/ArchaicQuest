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

            string fighterImgURL = "/UI/img/human.jpg";
            string theifImgURL = "/UI/img/human.jpg";
            string clericImgURL = "/UI/img/human.jpg";
            string mageImgURL = "/UI/img/human.jpg";

            classList.Add(new PlayerClass("Fighter", "The Fighter Class", fighterImgURL));
            classList.Add(new PlayerClass("Theif", "The Theif Class", theifImgURL));
            classList.Add(new PlayerClass("Cleric", "The Cleric Class", clericImgURL));
            classList.Add(new PlayerClass("Mage", "The Mage Class", mageImgURL));


            return classList;
        }

        public static PlayerClass selectClass(string findClass)
        {
            var selectedClass = setupClass().FirstOrDefault(x => x.name.ToLower().StartsWith(findClass.ToLower()));

            return selectedClass;
        }
    }
}
