using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World;

namespace MIMWebClient.Core.AI
{
    public class BreadthFirstSearch
    {
        /// <summary>
        /// search all rooms assigning coords then checking neighbours
        /// it will assign rooms as visited to not check them again
        /// </summary>
        public List<Room.Room> AssignCoords()
        {
            var areas = Areas.ListOfRooms();
            var startingLoc = areas.FirstOrDefault(x => x.areaId == 0);
            var completedRooms = new List<Room.Room>();



            if (startingLoc == null)
            {
                return null;

            }

            startingLoc.coords.X = 0;
            startingLoc.coords.Y = 0;
            startingLoc.coords.Z = 0;

            while (areas.Count > 0)
            {

                if (startingLoc.visited == false)
                {
                    //get exit id
                    foreach (var exit in startingLoc.exits)
                    {
                        var nextRoom = areas.FirstOrDefault(x => x.areaId == exit.areaId);
                        var nextRoomCoords = GetNewCoord(startingLoc.coords, exit.name);

                        if (nextRoom != null && nextRoom.visited == false)
                        {
                            nextRoom.coords.X = nextRoomCoords.X;
                            nextRoom.coords.Y = nextRoomCoords.Y;
                            nextRoom.coords.Z = nextRoomCoords.Z;

                            nextRoom.visited = true;

                            completedRooms.Add(nextRoom);
                            areas.Remove(nextRoom);
                        }
                    }

                    startingLoc.visited = true;
                    completedRooms.Add(startingLoc);
                    areas.Remove(startingLoc);
                }


                var getRoom = areas.Last();
                var getNeighbour = new Room.Room();

                foreach (var exit in getRoom.exits)
                {
                     getNeighbour = completedRooms.FirstOrDefault(x => x.areaId == exit.areaId);
                }

                if (getNeighbour != null)
                {
                    var getRoomCorrds = GetNewCoord(getNeighbour.coords, getRoom.exits[0].name, true);

                    getRoom.coords.X = getRoomCorrds.X;
                    getRoom.coords.Y = getRoomCorrds.Y;

                    foreach (var exit in getRoom.exits)
                    {
                        var nextRoom = areas.FirstOrDefault(x => x.areaId == exit.areaId);
                        var nextRoomCoords = GetNewCoord(getRoom.coords, exit.name);

                        if (nextRoom != null && nextRoom.visited == false)
                        {
                            nextRoom.coords.X = nextRoomCoords.X;
                            nextRoom.coords.Y = nextRoomCoords.Y;
                            nextRoom.coords.Z = nextRoomCoords.Z;

                            nextRoom.visited = true;

                            completedRooms.Add(nextRoom);
                            areas.Remove(nextRoom);
                        }
                    }

                    getRoom.visited = true;
                    completedRooms.Add(getRoom);
                    areas.Remove(getRoom);

                }
                else
                {
                    //1st in last out?
                    areas.Remove(getRoom);

                    areas.Insert(0, getRoom);

                }



            }

            return completedRooms;

        }

        public Coordinates GetNewCoord(Coordinates parentCoords, string direction, bool opposite = false)
        {
            var newCoords = new Coordinates();
            var dir = direction;
            //if (opposite)
            //{
            //    switch (dir)
            //    {
            //        case "North":
            //            newCoords.X = parentCoords.X;
            //            newCoords.Y = parentCoords.Y - 1;
            //            newCoords.Z = parentCoords.Z;
            //            break;
            //        case "East":
            //            newCoords.X = parentCoords.X - 1;
            //            newCoords.Y = parentCoords.Y;
            //            newCoords.Z = parentCoords.Z;
            //            break;
            //        case "South":
            //            newCoords.X = parentCoords.X;
            //            newCoords.Y = parentCoords.Y + 1;
            //            newCoords.Z = parentCoords.Z;
            //            break;
            //        case "West":
            //            newCoords.X = parentCoords.X + 1;
            //            newCoords.Y = parentCoords.Y;
            //            newCoords.Z = parentCoords.Z;
            //            break;
            //        case "Up":
            //            newCoords.X = parentCoords.X;
            //            newCoords.Y = parentCoords.Y;
            //            newCoords.Z = parentCoords.Z - 1;
            //            break;
            //        case "Down":
            //            newCoords.X = parentCoords.X;
            //            newCoords.Y = parentCoords.Y;
            //            newCoords.Z = parentCoords.Z + 1;
            //            break;
            //    }

            //     return newCoords;
           // }

            switch (dir)
            {
                case "North":
                    newCoords.X = parentCoords.X;
                    newCoords.Y = parentCoords.Y + 1;
                    newCoords.Z = parentCoords.Z;
                    break;
                case "East":
                    newCoords.X = parentCoords.X + 1;
                    newCoords.Y = parentCoords.Y;
                    newCoords.Z = parentCoords.Z;
                    break;
                case "South":
                    newCoords.X = parentCoords.X;
                    newCoords.Y = parentCoords.Y - 1;
                    newCoords.Z = parentCoords.Z;
                    break;
                case "West":
                    newCoords.X = parentCoords.X - 1;
                    newCoords.Y = parentCoords.Y;
                    newCoords.Z = parentCoords.Z;
                    break;
                case "Up":
                    newCoords.X = parentCoords.X;
                    newCoords.Y = parentCoords.Y;
                    newCoords.Z = parentCoords.Z + 1;
                    break;
                case "Down":
                    newCoords.X = parentCoords.X;
                    newCoords.Y = parentCoords.Y;
                    newCoords.Z = parentCoords.Z - 1;
                    break;
            }

            return newCoords;

        }



    }
}