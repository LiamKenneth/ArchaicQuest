using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.PlayerSetup
{
    public class PlayerRace
    {

        public  string name { get; set; }
        private int str { get; set; }
        private int dex { get; set; }
        private int con { get; set; }
        private int inte { get; set; }
        private int wis { get; set; }
        private int cha { get; set; }
        public string help { get; set; }
        public string imgUrl { get; set; }

        public PlayerRace(string name, int str, int dex, int con, int inte, int wis, int cha, string help, string imgUrl)
        {
            this.name = name;
            this.str = str;
            this.dex = dex;
            this.con = con;
            this.inte = inte;
            this.wis = wis;
            this.cha = cha;
            this.help = help;
            this.imgUrl = imgUrl;

        }


        public static List<PlayerRace> GenerateRace()
        {
            var listOfRace = new List<PlayerRace>();


            //Elves are slightly taller than humans, but have a much lighter build. " +
            //    "They lack the strength and stamina of the other races, but are far more agile," +
            //    " both in body and mind. Elves are superb mages and thieves, but have at best fair" +
            //    " talent as warriors or priests. Elves resist charm spells most effectively," +
            //    " due to their magical nature. They may see in the dark with infravision.
            //help text to replace just testing
            string humanHelp = "Humans are highly adaptable and the most common race in the world. They come in a wide range of skin, eye and hair colours as well as different shapes and sizes. Most towns and cities in the realms have been built by human hands.";
 
            string humanImgURL =  "/UI/img/human.jpg";

            string elfHelp = "Elves are shorter and slimmer than humans, they are also more in tune with nature and magic. The have an innate ability of Sneaking, Infrasion and resistance to charm spells. Elfs also take great pride and care into their craftsmanship and cities. ";

            string ElfImgURL = "/UI/img/elf.jpg";

            string darkElfHelp = "\r\nDark Elves are identical to their elven brethren except their skin ranges from dark pale blue to black. They too have an innate ability of Sneaking, Infrasion and resistance to charm spells. Dark elves walk amongst the other races and despite the tales they are not all evil but some distrust is shown towards them as they are known to have a ruthless reputation.\r\n";


            string darkElfImgURL = "/UI/img/darkElf.jpg";

            string dwarfHelp = "Dwarves are a short muscular humanoids who prefer the mountains and the underdark where they enjoy digging for gold. A lot of dwarves do venture out of the caves and can be found in human settlements in the local tavern with a mug of Ale. They are powerful Warriors and Clerics\r\n";

            string DwarfImgURL = "/UI/img/dwarf.jpg";

            listOfRace.Add(new PlayerRace("Human", 0, 0, 0, 0, 0, 0, humanHelp, humanImgURL));
            listOfRace.Add(new PlayerRace("Elf", 0, 0, -2, 2, 0, 0, elfHelp, ElfImgURL));
            listOfRace.Add(new PlayerRace("Dark Elf", 1, 0, 2, -2, 0, 0, darkElfHelp, darkElfImgURL));
            listOfRace.Add(new PlayerRace("Dwarf", 1, 0, 2, -2, 0, 0, dwarfHelp, DwarfImgURL));
            return listOfRace;
        }

        public static PlayerRace selectRace(string race)
        {

            var result =  GenerateRace().FirstOrDefault(x => x.name.ToLower().Contains(race.ToLower()));

            return result;
        }
    

}
}
