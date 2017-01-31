using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Player.Skills;
using MIMWebClient.Core.Room;
using NUnit.Framework;

namespace MIM.Test.Core.Player.Spells
{
    class MagicMissileTest
    {

        [Test]
        public void ShouldCastMagicMissile()
        {
            //mock player
            var player = new MIMWebClient.Core.PlayerSetup.Player()
            {
                Name = "Liam"
            };
            //give spell to player
            var magicMissle = MagicMissile.MagicMissileAb();
           
            player.Skills.Add(magicMissle);

            //mock target
            var target = new MIMWebClient.Core.PlayerSetup.Player()
            {
                Name = "target"
            };

            //mock room
            var room = new Room();
            room.players = new List<MIMWebClient.Core.PlayerSetup.Player>();
            room.players.Add(player);
            room.players.Add(target);


            Assert.DoesNotThrow(() => MagicMissile.StartMagicMissile(player, room, ""));

        }
    }
}
