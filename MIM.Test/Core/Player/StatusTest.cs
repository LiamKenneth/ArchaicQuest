using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePlayer = MIMWebClient.Core.PlayerSetup.Player;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.Player;

namespace MIM.Test.Core.Player
{
    class StatusTest
    {
        // More tests should be written and this one tidied up
        [Test]
        public void StandShouldKeepStatusIfAlreadyStanding()
        {
            Mock<MIMWebClient.Core.IHubContext> context = new Mock<MIMWebClient.Core.IHubContext>();

            CorePlayer player = new CorePlayer
            {
                Status = CorePlayer.PlayerStatus.Standing
            };
            Room room = new Room();

            Status.StandPlayer(context.Object, player, room);

            Assert.That(player.Status, Is.EqualTo(CorePlayer.PlayerStatus.Standing));
            context.Verify(m => m.SendToClient("You are standing already.", player.HubGuid, null, false, false, false));
        }

      
        [Test]
        public void StatusShouldChangeToRest()
        {
           var context = new Mock<MIMWebClient.Core.IHubContext>();

            CorePlayer player = new CorePlayer
            {
                Status = CorePlayer.PlayerStatus.Standing
            };
            Room room = new Room();
            room.players = new List<CorePlayer>();

            Status.RestPlayer(context.Object, player, room);

            Assert.That(player.Status, Is.EqualTo(CorePlayer.PlayerStatus.Resting));
            context.Verify(m => m.SendToClient("You sit down and rest.", player.HubGuid, null, false, false, false)); //Mind blown, this is pure magic :)
        }
    }
}
