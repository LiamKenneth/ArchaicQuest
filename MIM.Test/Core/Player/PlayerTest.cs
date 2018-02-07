using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;

namespace MIM.Test.Core.Player
{
    class PlayerTest
    {
        [Test]
        public void ShouldMap()
        {
            var room = new Room();

            room.coords.X = 0;
            room.coords.Y = 0;
            room.area = "Anker";
            var map = Map.DisplayMap(room);
 
            Console.WriteLine(map);
            Console.Write(map);



            Assert.That(map, Is.EqualTo("     \n     [ ]"));
            // Assert.That(pc.Inventory.Count, Is.EqualTo(0));

        }

    }
}
