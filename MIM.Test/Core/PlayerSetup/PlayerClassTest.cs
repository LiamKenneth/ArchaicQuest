using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIM.Test.Core.PlayerSetup
{
    class PlayerClassTest
    {

        [Test]
        public void ShouldCreateFighterClass()
        {

            var playerClass = MIMWebClient.Core.PlayerSetup.PlayerClass.selectClass("Fighter");


            Assert.That(playerClass.name, Is.EqualTo("Fighter"));

        }

        [Test]
        public void ShouldCreateTheifClass()
        {

            var playerClass = MIMWebClient.Core.PlayerSetup.PlayerClass.selectClass("Theif");


            Assert.That(playerClass.name, Is.EqualTo("Theif"));

        }

        [Test]
        public void ShouldCreateClericClass()
        {

            var playerClass = MIMWebClient.Core.PlayerSetup.PlayerClass.selectClass("Cleric");


            Assert.That(playerClass.name, Is.EqualTo("Cleric"));

        }

        [Test]
        public void ShouldCreateMageClass()
        {

            var playerClass = MIMWebClient.Core.PlayerSetup.PlayerClass.selectClass("Mage");


            Assert.That(playerClass.name, Is.EqualTo("Mage"));

        }


    }
}
