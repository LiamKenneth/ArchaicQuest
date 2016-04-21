using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
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

        public PlayerRace(string name, int str, int dex, int con, int inte, int wis, int cha, string help)
        {
            this.name = name;
            this.str = str;
            this.dex = dex;
            this.con = con;
            this.inte = inte;
            this.wis = wis;
            this.cha = cha;
            this.help = help;
            

        }


        public static List<PlayerRace> GenerateRace()
        {
            var listOfRace = new List<PlayerRace>();

            //help text to replace just testing
            string humanHelp = "Humans are the most common race in the world, and make up the majority of adventurers. Although they have no special talents like the other races, they are more versatile, being skilled in all seven classes. Humans have no special skills, immunities or resistances but also have no vulnerabilities.\r\n";

            string elfHelp = "Elves are slightly taller than humans, but have a much lighter build. They lack the strength and stamina of the other races, but are far more agile, both in body and mind. Elves are superb mages and thieves, but have at best fair talent as warriors or priests. Elves resist charm spells most effectively, due to their magical nature. They may see in the dark with infravision.";
 
            listOfRace.Add(new PlayerRace("Human", 0, 0, 0, 0, 0, 0, humanHelp));
            listOfRace.Add(new PlayerRace("Elf", 0, 0, -2, 2, 0, 0, elfHelp));

            return listOfRace;
        }

        public static PlayerRace selectRace(string race)
        {

            var result =  GenerateRace().FirstOrDefault(x => x.name.ToLower().Contains(race.ToLower()));

            return result;
        }
    

}
}
