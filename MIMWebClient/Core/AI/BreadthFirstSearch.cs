using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World;

namespace MIMWebClient.Core.AI
{
    public class BreadthFirstSearch
    {

        private static List<Room.Room> AreaList { get; set; } = Areas.ListOfRooms();
        private List<Room.Room> CompletedRooms { get; set; } = new List<Room.Room>();

        /// <summary>
        /// search all rooms assigning coords then checking neighbours
        /// it will assign rooms as visited to not check them again
        /// </summary>
        public List<Room.Room> AssignCoords()
        {
            var startingLoc = AreaList.FirstOrDefault(x => x.areaId == 0);
            if (startingLoc == null) return null;

            SetCoords(startingLoc, new Coordinates());

            ProcessRoom(startingLoc);

            while (AreaList.Count > 0)
            {
                var getRoom = AreaList.Last();
                var getNeighbour = GetNeighbour(getRoom);

                if (getNeighbour != null)
                {
                    var getExitToNeighbour = GetNeighbourExit(getRoom, getNeighbour);

                    if (getExitToNeighbour != null)
                    {
                        SetCoords(getRoom, GetNewCoord(getNeighbour.coords, getExitToNeighbour.name, true));
                    }

                    ProcessRoom(getRoom);
                }
                else
                {
                    AreaList.Remove(getRoom);
                    AreaList.Insert(0, getRoom);
                }
            }
            return CompletedRooms;
        }

        public Room.Room GetNeighbour(Room.Room getRoom)
        {
            return CompletedRooms.FirstOrDefault(x =>
            {
                var lastOrDefault = getRoom.exits.LastOrDefault();
                return lastOrDefault != null && x.areaId == lastOrDefault.areaId;
            });

        }

        public Exit GetNeighbourExit(Room.Room getRoom, Room.Room neighbour)
        {
            return getRoom.exits.FirstOrDefault(x => x.areaId == neighbour.areaId);

        }

        public Coordinates GetNewCoord(Coordinates coords, string direction, bool opposite = false)
        {
            var newCoords = new Coordinates
            {
                X = coords.X,
                Y = coords.Y,
                Z = coords.Z
            };

            switch (direction)
            {
                case "North":
                    newCoords.Y = SetCoord(opposite, direction, newCoords.Y);
                    break;
                case "East":
                    newCoords.X = SetCoord(opposite, direction, newCoords.X);
                    break;
                case "South":
                    newCoords.Y = SetCoord(opposite, direction, newCoords.Y);
                    break;
                case "West":
                    newCoords.X = SetCoord(opposite, direction, newCoords.X);
                    break;
                case "Up":
                    newCoords.Z = SetCoord(opposite, direction, newCoords.Z);
                    break;
                case "Down":
                    newCoords.Z = SetCoord(opposite, direction, newCoords.Z);
                    break;
            }

            return newCoords;

        }

        public void ProcessRoom(Room.Room getRoom)
        {

            foreach (var exit in getRoom.exits)
            {
                var nextRoom = AreaList.FirstOrDefault(x => x.areaId == exit.areaId);
                var nextRoomCoords = GetNewCoord(getRoom.coords, exit.name);

                if (nextRoom != null && nextRoom.visited == false)
                {
                    SetCoords(nextRoom, nextRoomCoords);
                    nextRoom.visited = true;
                    CompletedRooms.Add(nextRoom);
                    AreaList.Remove(nextRoom);
                }
            }

            getRoom.visited = true;
            CompletedRooms.Add(getRoom);
            AreaList.Remove(getRoom);

        }

        public static int SetCoord(bool opposite, string direction, int coord)
        {

            if (opposite)
            {

                if (direction == "North" || direction == "East" || direction == "Up")
                {
                    return --coord;
                }

                // else direction == "South" || direction == "West" || direction == "Down"
                return ++coord;

            }

            if (direction == "North" || direction == "East" || direction == "Up")
            {
                return ++coord;
            }

            //direction == "South" || direction == "West" || direction == "Down")

            return --coord;
        }

        public static void SetCoords(Room.Room room, Coordinates coords)
        {
            if (room != null)
            {
                room.coords = coords;
            }
        }

 

    }
}