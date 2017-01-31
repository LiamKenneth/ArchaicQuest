using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Player.Skills;
using NUnit.Framework;

namespace MIM.Test.Core.Player
{
    class SkillsTest
    {
        [Test]
        public void ShouldConfirmPlayerHasSkill()
        {
            var player = new MIMWebClient.Core.PlayerSetup.Player()
            {
                Name = "Liam"
            };
            //give spell to player
            var magicMissle = MagicMissile.MagicMissileAb();

            player.Skills.Add(magicMissle);

            var hasSkill = Skill.CheckPlayerHasSkill(player, magicMissle.Name);

            Assert.IsTrue(hasSkill);

        }

        [Test]
        public void ShouldConfirmPlayerDoesNotHaveSkill()
        {
            var player = new MIMWebClient.Core.PlayerSetup.Player()
            {
                Name = "Liam"
            };
            //give spell to player
            var magicMissle = MagicMissile.MagicMissileAb();

            var hasSkill = Skill.CheckPlayerHasSkill(player, magicMissle.Name);

            Assert.IsFalse(hasSkill);

        }

        [Test]
        public void ShouldGetTargetFromSkillCommand()
        {
            var command = "magic cat";

            var target = Skill.GetSkillTarget(command);

            Assert.That(target, Is.EqualTo("cat"));

        }
    }
}
