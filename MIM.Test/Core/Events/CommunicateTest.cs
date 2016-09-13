using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIM.Test.Core.Events
{
    class CommunicateTest
    {
        [Test]
        [Ignore("Not implemented")]
        public void ShouldSayToAllInRoom()
        {
            //var RoomCache = MIMWebClient.Core.Events.Cache.getRoom
            //var isConDict = RoomCache.try
            //Assert.That(expectedResult, Is.EqualTo(15));

            var mockPlayer = new MIMWebClient.Core.PlayerSetup.Player("test", "Liam", "liam@email.com", "123", "Male", "Human", "Mage", 10, 13, 16, 17, 20, 15);
            var mockRoom = new MIMWebClient.Core.Room.Room();



            /// MIMWebClient.Core.Events.Communicate.Say("Hello", mockPlayer, mockRoom);
 
          
        }
    }
}
