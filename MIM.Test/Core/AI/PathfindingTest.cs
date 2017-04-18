using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MIMWebClient.Core.AI;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World;
using NUnit.Framework;

namespace MIM.Test.Core.AI
{
    class PathfindingTest
    {
        [Test]
        public void ShouldFindDestination()
        {
            var roomSetUp = new BreadthFirstSearch();

            var list = roomSetUp.AssignCoords();


            var modo = list.FirstOrDefault(x => x.areaId == 2);
            var modoActual = new Coordinates()
            {
                X = 0,
                Y = 4,
                Z = 0
            };

            var church = list.FirstOrDefault(x => x.areaId == 18);
            var churchActual = new Coordinates()
            {
                X = 6,
                Y = 2,
                Z = 0
            };

            var elder = list.FirstOrDefault(x => x.areaId == 13);
            var elderctual = new Coordinates()
            {
                X = 4,
                Y = 2,
                Z = 0
            };

            foreach (var i in list.OrderBy(x => x.areaId))
            {
                Console.WriteLine("roomID " + i.areaId + " (" + i.coords.X + ", " + i.coords.Y + ")");
            }


            Assert.That(modo.coords.X, Is.EqualTo(modoActual.X));
            Assert.That(modo.coords.Y, Is.EqualTo(modoActual.Y));

            Assert.That(church.coords.X, Is.EqualTo(churchActual.X));
            Assert.That(church.coords.Y, Is.EqualTo(churchActual.Y));

            Assert.That(elder.coords.X, Is.EqualTo(elderctual.X));
            Assert.That(elder.coords.Y, Is.EqualTo(elderctual.Y));
        }

        [Test]
        public void ShouldShowAMap()
        {
              var map = Map.GenerateGrid();
 
                Console.WriteLine(map);
           
            Assert.That(map, !Is.Empty);
        }
    }
}
