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

            var room = Areas.ListOfRooms().FirstOrDefault(x => x.areaId == 2);

            var lookUpPath = new Pathfinding();

            var options = new List<List<Pathfinding>>();

            //for (int i = 0; i < 100; i++)
            //{
                options.Add(lookUpPath.FindPath2("Anker", 0, "Anker", player, room));
           
          //  }

            int min = options.Min(x => x.Count);
            var lowestValues = options.Where(x => x.Count == min);
          //  var fastesRoute = options.Where(d => d == options.Min());

            Assert.IsNotEmpty(lowestValues);
        }
    }
}
