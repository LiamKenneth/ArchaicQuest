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
            string humanHelp = "Humans are the most varied of the races in ArchaicQuest. " +
                               "They come in different shapes, sizes, skin, hair and eye colour " +
                               "They don't excell in any perticualy role but are good at most things " +
                               "making them average at all classes." +
                               " Humans are reccomended for beginners.";
 
            string humanImgURL =  "/UI/img/human.jpg";

            string elfHelp = "Elves are slightly smaller and slimmer than Humans making them more agile but weaker";

            string ElfImgURL = "/UI/img/elf.jpg";

            string darkElfHelp = "Drow info";


            string darkElfImgURL = "/UI/img/darkElf.jpg";

            string dwarfHelp = "Dwarf info";

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
