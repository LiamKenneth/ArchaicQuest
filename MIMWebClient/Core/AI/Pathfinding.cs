using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.World;

namespace MIMWebClient.Core.AI
{
 
    public class Pathfinding
    {
        public string Name { get; set; }
        public int AreaId { get; set; }

        private static List<Pathfinding> list { get; set; } = new List<Pathfinding>();

        public List<Pathfinding> FindPath2(string destName, int destId, string regionName, PlayerSetup.Player mob, Room.Room room)
        {

        

            var dest =  SearchExits(room, destId);

             

            return dest;
        }

        public static List<Pathfinding>  SearchExits(Room.Room room, int destId)
        {
          

            foreach (var exit in room.exits)
            {
                if (exit.areaId == destId)
                {

                    var path1 = new Pathfinding()
                    {
                        Name = exit.name,
                        AreaId = exit.areaId
                    };

                    list.Add(path1);

                    return list;

                }

                var path = new Pathfinding()
                {
                    Name = exit.name,
                    AreaId = exit.areaId
                };

                var getRoom = new LoadRoom()
                {
                    Area = exit.area,
                    id = exit.areaId,
                    Region = exit.region
                };

                list.Add(path);

                var getNewRoom = getRoom.LoadRoomFile();
                if (getNewRoom != null)
                {
                    SearchExits(getNewRoom, destId);
                }
              
            }

            return list;

        }

        //   public List<Pathfinding> FindPath(string destName, int destId, string regionName, PlayerSetup.Player mob, Room.Room room)
        //   {


        ////starting path?
        //       var list = new List<Pathfinding>();
        //       var prevStep = mob.AreaId;
        //       var areass = Startup.ReturnRooms.OrderByDescending(i => i.areaId == mob.AreaId).ToList();
        //       foreach (var searchRoom in areass)

        //           foreach (var exit in searchRoom.exits.OrderBy(x => Helpers.Rand(0, searchRoom.exits.Count)).ToList())
        //           {
        //               if (prevStep == exit.areaId)
        //               {
        //                   continue;
        //               }

        //               if (list.Count(x => x.AreaId == exit.areaId) == 1)
        //               {
        //                   continue;
        //               }

        //               prevStep = exit.areaId;

        //               var path = new Pathfinding()
        //               {
        //                   Name = exit.name,
        //                   AreaId = exit.areaId
        //               };

        //               list.Add(path);
        //               if (exit.areaId == destId)
        //               {
        //                   return list;
        //               }

        //               var anExit = SearchRooms(destId, exit, searchRoom);

        //                   foreach (var ae in anExit)
        //                   {

        //                       if (ae.Key != null)
        //                       {



        //                       if (prevStep == ae.Value)
        //                       {
        //                           continue;
        //                       }

        //                       if (list.Count(x => x.AreaId == ae.Value) == 1)
        //                       {
        //                           continue;
        //                       }

        //                       prevStep = ae.Value;


        //                       var path1 = new Pathfinding()
        //                       {
        //                           Name = ae.Key,
        //                           AreaId = ae.Value
        //                       };

        //                           list.Add(path1);
        //                       }
        //                   }



        //           }

        //       return list;
        //   }


        public Dictionary<string, int> SearchRooms(int destId, Room.Exit exit, Room.Room room)
        {

            var newExit = new Dictionary<string, int>();
            var getNewRoom = Startup.ReturnRooms.FirstOrDefault(x => x.area == exit.area && x.areaId == exit.areaId);

            if (getNewRoom != null)
            {
                foreach (var newRoomExit in getNewRoom.exits.OrderBy(x => Helpers.Rand(0, room.exits.Count)).ToList())
                {
                     
                    if (newRoomExit.areaId == destId)
                    {
                        newExit.Add(newRoomExit.name, newRoomExit.areaId);
                        return newExit;
                    }
                    newExit.Add(newRoomExit.name, newRoomExit.areaId);
                    return newExit;
                }
            }

            return null;
        }
    }
}