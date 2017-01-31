using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.PlayerSetup;
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

            Assert.That(rand, Is.InRange(1, 2));
            Assert.That(rand, Is.Not.InRange(3, 4));
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

        [Test]
        public void ShouldReturnArticleForName()
        {
            var cat = new Player {Name = "Cat", KnownByName = false};
            var mollyCat = new Player { Name = "Molly", KnownByName = true};
            var apple = "apple";
            var sword = "sword";

            var returnedName = MIMWebClient.Core.Helpers.ReturnName(cat, null);
            var returnedName2 = MIMWebClient.Core.Helpers.ReturnName(mollyCat, null);
            var objName = MIMWebClient.Core.Helpers.ReturnName(null, apple);
            var objName2 = MIMWebClient.Core.Helpers.ReturnName(null, sword);

            Assert.That(returnedName, Is.EqualTo("a Cat"));
            Assert.That(returnedName2, Is.EqualTo("Molly"));
            Assert.That(objName2, Is.EqualTo("A sword"));
            Assert.That(objName, Is.EqualTo("An apple"));
        }
    }
}
