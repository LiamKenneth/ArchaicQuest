using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public static class Anker
    {
        public static Room VillageSquare()
        {
            var room = new Room
            {
                region = "Anker",
                area = "Anker",
                areaId = 0,
                title = "Village Sqaure",
                description = "A round stone well occupies the centre of the grey cobbled square. " +
                "A large oak tree provides shade. It is frequently used by passers-by and the villages main source of fresh water " +
                "<br /> Not far from the well is the village notice board. Covered in parchments of all sizes " +
                "<br /> The local inn is to the north.",
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<Player>(),
                terrain = Room.Terrain.Field
                
            };


            var North = new Exit {
                name = "North",
                area = "Anker",
                areaId = 1,
                description = new Item.Description {
                    look = "To the north you see the inn of the drunken sailor.", //return mobs / players?
                    exam = "To the north you see the inn of the drunken sailor.",
                }
            };

            room.exits.Add(North);

            var cat = new Player { Name = "Black and White cat" };

            room.mobs.Add(cat);



            return room;
        }

       
    }
}