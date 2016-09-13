using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIM.Test.Core.PlayerSetup
{
    class PlayerStatsTest
    {

        [Test]
        public void ShouldRollStats()
        {

            var playerStats = new MIMWebClient.Core.PlayerSetup.PlayerStats();


            var stat = playerStats.rollStats();

            Assert.That(stat[0], Is.GreaterThan(6));
            Assert.That(stat.Length, Is.EqualTo(6));

        }

        [Test]
        public void ShouldRollDice()
        {

            var playerStats = new MIMWebClient.Core.PlayerSetup.PlayerStats();


            var diceRoll = playerStats.dice(2, 6);

            Assert.That(diceRoll, Is.GreaterThan(2));

        }
    }
}
