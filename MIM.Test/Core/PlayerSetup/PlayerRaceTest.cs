using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIM.Test.Core.PlayerSetup
{
    class PlayerRaceTest
    {
        [Test]
        public void ShouldCreateHumanRace()
        {

            var playerRace =  MIMWebClient.Core.PlayerSetup.PlayerRace.selectRace("Human");
 

            Assert.That(playerRace.name, Is.EqualTo("Human"));

        }

        [Test]
        public void ShouldCreateElfRace()
        {

            var playerRace = MIMWebClient.Core.PlayerSetup.PlayerRace.selectRace("Elf");


            Assert.That(playerRace.name, Is.EqualTo("Elf"));

        }

        [Test]
        public void ShouldCreateDarkElfRace()
        {

            var playerRace = MIMWebClient.Core.PlayerSetup.PlayerRace.selectRace("Dark Elf");


            Assert.That(playerRace.name, Is.EqualTo("Dark Elf"));

        }

        [Test]
        public void ShouldCreateDwarfRace()
        {

            var playerRace = MIMWebClient.Core.PlayerSetup.PlayerRace.selectRace("Dwarf");


            Assert.That(playerRace.name, Is.EqualTo("Dwarf"));

        }
    }
}
