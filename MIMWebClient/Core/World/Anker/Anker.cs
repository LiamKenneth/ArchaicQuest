using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Anker
    {
        public Room AnkerArea()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 0,
                title = "Market Sqaure",
                description = "Description here",
                mobs = new List<Player>()
            };

            var cat = new Player { Name = "Black and White cat" };

            room.mobs.Add(cat);



            return room;
        }

       
    }
}