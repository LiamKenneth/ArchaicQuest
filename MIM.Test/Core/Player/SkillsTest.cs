using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.Room;
using NUnit.Framework;

namespace MIM.Test.Core.Player
{
    class SkillsTest
    {
        [Test]
        public void Should_Confirm_Player_Has_Skill()
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
        public void Should_Confirm_Player_Does_Not_Have_Skill()
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
        public void Should_GetTarget_From_SkillCommand()
        {
            var command = "magic cat";

            var target = Skill.GetSkillTarget(command);

            Assert.That(target, Is.EqualTo("cat"));

        }

        [Test]
        public void Should_Return_Empty_string_From_SkillCommand_if_No_Target()
        {
            var command = "magic";

            var target = Skill.GetSkillTarget(command);

            Assert.That(target, Is.EqualTo(string.Empty));

        }


        [Test]
        public void Should_Find_Target_For_Skill()
        {
            var target = new MIMWebClient.Core.PlayerSetup.Player()
            {
                Name = "Liam"
            };

            var room = new Room()
            {
                players = new List<MIMWebClient.Core.PlayerSetup.Player>()
            };

            room.players.Add(target);

            var skillCommand = "magic liam"; //C magic Liam (command is not included)

            var findTarget = Skill.FindTarget(skillCommand, room);

            Assert.That(findTarget, Is.EqualTo(target));

        }


    }
}
