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
using MIMWebClient.Core.Player.Skills;

namespace MIM.Test.Core.Player
{
    class TripTest
    {

 
        // More tests should be written and this one tidied up
        //[Test]
        //public   void TripShouldHurtPlayer()
        //{
        //    Mock<MIMWebClient.Core.IHubContext> context = new Mock<MIMWebClient.Core.IHubContext>();

        //    CorePlayer player = new CorePlayer
        //    {
        //        Name = "Attacker",
        //        Status = CorePlayer.PlayerStatus.Standing,
        //        SelectedClass = "Fighter",
        //        Skills = new List<Skill>(),
                
        //    };

        //    CorePlayer target = new CorePlayer
        //    {
        //        Name = "Defender",
        //        Status = CorePlayer.PlayerStatus.Standing,
        //        SelectedClass = "Fighter",
        //        Skills = new List<Skill>()
        //    };

        //    var skill = Trip.TripAb();
        //    skill.Proficiency = 100;
        //    player.Skills.Add(skill);

        //    Room room = new Room();
        //    room.players = new List<CorePlayer>();
        //    room.players.Add(player);
        //    room.players.Add(target);

        //    new Trip().StartTrip(context.Object, player, room, "trip defender");

        //    Assert.That(player.Status, Is.EqualTo(CorePlayer.PlayerStatus.Standing));
        //    context.Verify(m => m.SendToClient("You are standing already.", player.HubGuid, null, false, false, false));
        //}

        [Test]
        public   void TripShouldFailIfNotLearned()
        {

            #region setup
            Mock<MIMWebClient.Core.IHubContext> context = new Mock<MIMWebClient.Core.IHubContext>();

            CorePlayer player = new CorePlayer
            {
                Name = "Attacker",
                Status = CorePlayer.PlayerStatus.Standing,
                SelectedClass = "Fighter",
                Skills = new List<Skill>(),

            };



            CorePlayer target = new CorePlayer
            {
                Name = "Defender",
                Status = CorePlayer.PlayerStatus.Standing,
                SelectedClass = "Fighter",
                Skills = new List<Skill>()
            };

 

            Room room = new Room();
            room.players = new List<CorePlayer>();

            room.players.Add(player);
            room.players.Add(target);

            #endregion setup

            new Trip().StartTrip(context.Object, player, room, "trip defender");

            context.Verify(m => m.SendToClient("You don't know that skill.", player.HubGuid, null, false, false, false));
        }


        [Test]
        public void TripShouldFailIfAsleep()
        {

            #region setup
            Mock<MIMWebClient.Core.IHubContext> context = new Mock<MIMWebClient.Core.IHubContext>();

            CorePlayer player = new CorePlayer
            {
                Name = "Attacker",
                Status = CorePlayer.PlayerStatus.Standing,
                SelectedClass = "Fighter",
                Skills = new List<Skill>(),
                

            };

            player.Status = CorePlayer.PlayerStatus.Sleeping;

            CorePlayer target = new CorePlayer
            {
                Name = "Defender",
                Status = CorePlayer.PlayerStatus.Standing,
                SelectedClass = "Fighter",
                Skills = new List<Skill>()
            };

            var skill = Trip.TripAb();
                skill.Proficiency = 100;
                player.Skills.Add(skill);

            Room room = new Room {players = new List<CorePlayer> {player, target}};


            #endregion setup

            new Trip().StartTrip(context.Object, player, room, "trip defender");

            context.Verify(m => m.SendToClient("You can't do that while asleep.", player.HubGuid, null, false, false, false));
 
        }

    }
}
