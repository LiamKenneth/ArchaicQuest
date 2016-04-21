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

        public PlayerRace(string name, int str)
        {
            this.name = name;
            this.str = str;
            this.dex = dex;
            this.con = con;
            this.inte = inte;
            this.wis = wis;
            this.cha = cha;
            

        }


        public static List<PlayerRace> GenerateRace()
        {
            var listOfRace = new List<PlayerRace>();
            listOfRace.Add(new PlayerRace("Human", 0));

            return listOfRace;
        }

        public static PlayerRace selectRace(string race)
        {

            var result =  GenerateRace().FirstOrDefault(x => x.name.ToLower().Contains(race.ToLower()));

            return result;
        }
    

}
}
