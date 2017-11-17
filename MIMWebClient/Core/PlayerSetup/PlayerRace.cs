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
        public int str { get; set; }
        public int dex { get; set; }
        public int con { get; set; }
        public int inte { get; set; }
        public int wis { get; set; }
        public int cha { get; set; }
        public string help { get; set; }
        public string imgUrl { get; set; }

  
        public static List<PlayerRace> GenerateRace()
        {
            var listOfRace = new List<PlayerRace>();

            string humanHelp = "<p>Humans are highly adaptable and the most common race in the world. They come in a wide range of skin, eye and hair colours as well as different shapes and sizes.<p><p>Stats:<br/>STR: 60, INT: 60, WIS: 60, DEX: 60, CON: 60 </p>";
 
            string humanImgURL =  "/UI/img/human.jpg";

            string elfHelp = "<p>Elves are shorter and slimmer than humans, they are also more in tune with nature and magic. They have an innate ability of Sneaking, Infrasion and resistance to charm spells.</p> <p>Stats:<br/>STR: 35, INT: 82, WIS: 82, DEX: 69, CON: 40 </p> ";

            string ElfImgURL = "/UI/img/elf.jpg";

            string darkElfHelp = "<p>Dark Elves are identical to their elven brethren except their skin ranges from dark pale blue to black. They too have an innate ability of Sneaking, Infrasion and resistance to charm spells.</p> <p>Stats:<br/>STR: 35, INT: 81, WIS: 81, DEX: 71, CON: 55 </p> ";


            string darkElfImgURL = "/UI/img/darkElf.jpg";

            string dwarfHelp = "<p>Dwarves are a short muscular humanoids who prefer the mountains and the underdark where they enjoy digging for gold. A lot of dwarves do venture out of the caves and can be found in human settlements in the local tavern with a mug of Ale. They are powerful Warriors and Clerics</p> <p>Stats:<br/>STR: 62, INT: 52, WIS: 79, DEX: 50, CON: 64 </p> ";

            string DwarfImgURL = "/UI/img/dwarf.jpg";

            string mauHelp = "<p>Mau, Cat like humanoid race. Info coming soon</p> <p>Stats:<br/>STR: 30, INT: 55, WIS: 70, DEX: 92, CON: 45 </p> ";

            string TlalocHelp = "<p>Tlaloc, Reptilian lizard like humanoid race. Info coming soon</p> <p>Stats:<br/>STR: 51, INT: 80, WIS: 67, DEX: 46, CON: 61 </p> ";

            listOfRace.Add(new PlayerRace()
            {
                name = "Human",
                str = 60,
                inte = 60,
                wis = 60,
                dex = 60,
                con = 60,
                cha = 60,
                help = humanHelp,
                imgUrl = humanImgURL
            });

            listOfRace.Add(new PlayerRace()
            {
              name = "Elf",
              str = 35,
              inte = 82,
              wis = 82,
              dex = 69,
              con = 40,
              cha = 70,
              help = elfHelp,
              imgUrl = ElfImgURL
            });

            listOfRace.Add(new PlayerRace()
            {
                name = "Dark Elf",
                str = 35,
                inte = 81,
                wis = 81,
                dex = 71,
                con = 42,
                cha = 55,
                help = darkElfHelp,
                imgUrl = darkElfImgURL
            });

            listOfRace.Add(new PlayerRace()
            {
                name = "Dwarf",
                str = 62,
                inte = 52,
                wis = 79,
                dex = 50,
                con = 64,
                cha = 55,
                help = dwarfHelp,
                imgUrl = darkElfImgURL
            });

            listOfRace.Add(new PlayerRace()
            {
                name = "Mau",
                str = 30,
                inte = 55,
                wis = 70,
                dex = 92,
                con = 45,
                cha = 62,
                help = mauHelp,
                imgUrl = darkElfImgURL
            });

            listOfRace.Add(new PlayerRace()
            {
                name = "Tlaloc",
                str = 51,
                inte = 80,
                wis = 67,
                dex = 46,
                con = 61,
                cha = 50,
                help = TlalocHelp,
                imgUrl = darkElfImgURL
            });



            return listOfRace;
        }

        public static PlayerRace selectRace(string race)
        {

            var result =  GenerateRace().FirstOrDefault(x => x.name.ToLower().Contains(race.ToLower()));

            return result;
        }
    

}
}
