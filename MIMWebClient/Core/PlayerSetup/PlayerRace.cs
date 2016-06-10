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

            //help text to replace just testing
            string humanHelp = "Humans are the most common race in the world, and make up the majority of adventurers. Although they have no special talents like the other races, they are more versatile, being skilled in all seven classes. Humans have no special skills, immunities or resistances but also have no vulnerabilities.\r\n";
 
            string humanImgURL =  "/UI/img/human.jpg";

            string elfHelp = "Elves are slightly taller than humans, but have a much lighter build. They lack the strength and stamina of the other races, but are far more agile, both in body and mind. Elves are superb mages and thieves, but have at best fair talent as warriors or priests. Elves resist charm spells most effectively, due to their magical nature. They may see in the dark with infravision.";

            string ElfImgURL = "/UI/img/elf.jpg";

            string darkElfHelp = "Drow are the most feared of all the elves. They live underground in great cities and practice the darker side of magic and combat. Drow are very magical in nature and thus tend to be very intelligent, but due to their weak constitution they are vulnerable to disease. Since the drow spend most of their lives beneath the surface they are very vulnerable to bright lights, so much that a candle hurts their eyes and the sun can almost blind them. Because of this only the few drow that are able to withstand the burning rays of the sun chose to venture to the surface world.";


            string darkElfImgURL = "/UI/img/darkElf.jpg";

            string dwarfHelp = "Dwarves are short, stocky demi-humans, known for foul temper and great stamina. Dwarves have high strength and constitution, but poor dexterity. They are not as smart as humans, but are usually wiser due to their long life spans. Dwarves make excellent fighters and clerics, but are very poor mages or thieves. Dwarves are resistant to magic, poison and disease, but cannot swim, and so are very vulnerable to drowning. They can also see in the dark with infravision.";

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
