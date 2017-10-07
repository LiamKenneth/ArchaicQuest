using MIMWebClient.Core;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.World.Items.Armour.LightArmour.Leather.Body;
using MIMWebClient.Core.World.Items.Weapons.Sword.Short;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIM.Test.Core
{
    class ItemContainerTest
    {
        private ItemContainer container = new ItemContainer();
        private String vestName, swordName;

        [OneTimeSetUp]
        public void SetUp()
        {
            var vest = LeatherBody.LeatherVest();
            vestName = vest.name;

            var sword = ShortSwordBasic.RustedShortSword();
            swordName = sword.name;

            container.Add(vest);
            container.Add(sword);
            container.Add(sword);
        }

        [Test]
        public void ShouldReturnCorrectQuantity()
        {
            var expected = new List<string> { vestName, swordName + " x2" };

            Assert.That(container.List().ToList(), Is.EquivalentTo(expected));
        }

        [Test]
        public void ShouldUseArticle()
        {
            var vestArticle = AvsAnLib.AvsAn.Query(vestName).Article; ;
            var swordArticle = AvsAnLib.AvsAn.Query(swordName).Article;
            
            var expected = new List<string> { vestArticle + " " + vestName, swordArticle + " " + swordName + " x2" };

            Assert.That(container.List(true).ToList(), Is.EquivalentTo(expected));
        }
    }
}
