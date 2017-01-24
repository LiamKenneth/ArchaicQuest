using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MIM.Test
{
    class Helpers
    {
        [Test]
        public void ShouldReturnCountOfDiceRollsbetweenOneAndSix()
        {
            var helper = new MIMWebClient.Core.Helpers();

            var die = helper.dice(1, 1, 6);

            Assert.That(die, Is.GreaterThanOrEqualTo(1));
            Assert.That(die, Is.LessThanOrEqualTo(6));
        }

        [Test]
        public void ShouldReturnRandomNumber()
        {
            var rand = MIMWebClient.Core.Helpers.Rand(1, 2);

            Assert.That(rand, Is.GreaterThanOrEqualTo(1));
            Assert.That(rand, Is.LessThanOrEqualTo(2));
            Assert.That(rand, Is.Not.GreaterThanOrEqualTo(3));
            Assert.That(rand, Is.Not.LessThanOrEqualTo(0));
        }

        [Test]
        public void ShouldReturnPercentage()
        {
            var percentage = MIMWebClient.Core.Helpers.GetPercentage(50, 95);
        
             Assert.That(percentage, Is.EqualTo(53));
        }

        [Test]
        public void ShouldReturnOneIfPercentageLessThanZero()
        {
            var percentage = MIMWebClient.Core.Helpers.GetPercentage(-5, 95);

            Assert.That(percentage, Is.EqualTo(1));
        }

        [Test]
        public void ShouldReturnRandomString()
        {
            var strings = new string[] {"hello", "world"};

            var randomString = MIMWebClient.Core.Helpers.RandomString(strings);

           // Assert.That(randomString, Is.EqualTo("hello") || Is.EqualTo("world"));
        }
    }
}
