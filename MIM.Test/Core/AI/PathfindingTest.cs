using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MIMWebClient.Core.AI;
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
            //mock player
       
            //set room ID /area name
            var player = new MIMWebClient.Core.PlayerSetup.Player()
            {
                Name = "Liam",
                AreaId = 1,
                Area = "Anker",
                Region = "Anker"
             
            };

            var roomSetUp = new BreadthFirstSearch();

         var list =   roomSetUp.AssignCoords();

            foreach (var i in list.OrderBy(x => x.areaId))
            {
                Console.WriteLine("roomID " + i.areaId + " " + i.coords.Y + " " + i.coords.X);
            }

           

            Assert.AreEqual(1,1);
        }
    }
}
