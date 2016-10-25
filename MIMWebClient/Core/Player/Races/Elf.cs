using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Player.Classes.Reclasses;
using MIMWebClient.Core.Player.Skills;

namespace MIMWebClient.Core.Player.Races
{
    public class Elf : PlayerRace
    {

        public static PlayerRace ElfRace()
        {


            var elf = new PlayerRace()
            {
                Name = "Elf",
                ExperienceModifier = 300,
                HelpText = new Help(),
                MaxStr = 16,
                MaxDex = 20,
                MaxCon = 16,
                MaxInt = 25,
                MaxWis = 21,
                MaxCha = 20,


            };

            elf.HelpText.HelpText = "Elves are slightly taller than humans, but have a much lighter build. " +
                "They lack the strength and stamina of the other races, but are far more agile," +
                " both in body and mind. Elves are superb mages and thieves, but have at best fair" +
                " talent as warriors or priests. Elves resist charm spells most effectively," +
                " due to their magical nature. They may see in the dark with infravision.";

            


            return elf;


        }
    }
}