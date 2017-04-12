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
        //Contains all the areas in the game
        private static List<Room.Room> AreaList { get; set; } = Areas.ListOfRooms();
        private List<Room.Room> CompletedRooms { get; set; } = new List<Room.Room>();

        /// <summary>
        /// search all rooms in AreaList assigning coords then checking neighbours 
        /// and repeating until AreaList is empty
        /// 
        /// Completed rooms are added to CompletedRooms and removed from AreaList
        /// 
        /// </summary>
        /// <returns>Returns CompletedRooms which contains the list of rooms that have coordinates set</returns>
        public List<Room.Room> AssignCoords()
        {
            var startingLoc = AreaList.FirstOrDefault(x => x.areaId == 0);
            if (startingLoc == null) return null;

            SetCoords(startingLoc, new Coordinates());

            /*
             * Takes the 1st room and search all 4 exits
             * and assigns them coordinates adding them
             * to the completed list
             */
            ProcessRoom(startingLoc);

            /*
             * Now completedList has areas, loop areaList looking for rooms
             * that have exits that lead to one of the rooms in the completed list
             */
            while (AreaList.Count > 0)
            {
                var getRoom = AreaList.Last();

                //Check if getRoom has a neighbour in the completed List
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
                    /*
                     * if nothing found remove the room from areaList
                     * and add back to the top of the Area:ist
                     */
                    AreaList.Remove(getRoom);
                    AreaList.Insert(0, getRoom);
                }
            }

            //Once all rooms in areaList have been processed return the list of completed rooms
            return CompletedRooms;
        }

        /// <summary>
        /// Searches the completed list of rooms and 
        /// returns the room that is an exit of getRoom (AreaList.Last())
        /// </summary>
        /// <param name="getRoom">The last room from AreaList</param>
        /// <returns>The room next to getRoom</returns>
        public Room.Room GetNeighbour(Room.Room getRoom)
        {
            foreach (var connectingNode in getRoom.exits)
            {
                var connectedRoom = CompletedRooms.FirstOrDefault(x => x.areaId.Equals(connectingNode.areaId));
                if (connectedRoom != null)
                {
                    return connectedRoom;
                }
            }

            return null;

        }

        /// <summary>
        /// Returns the Exit from getRoom that connects it to it's neighbour
        /// </summary>
        /// <param name="getRoom">The last room from AreaList</param>
        /// <param name="neighbour">Room that is adjacent to getRoom</param>
        /// <returns>Returns the Exit from getRoom that connects it to it's neighbour</returns>
        public Exit GetNeighbourExit(Room.Room getRoom, Room.Room neighbour)
        {
            return getRoom.exits.FirstOrDefault(x => x.areaId == neighbour.areaId);

        }
        /// <summary>
        /// Takes the current cordinates, the direction of travel and if the direction should be reversed
        /// then updates and return the new coordinates
        /// </summary>
        /// <param name="coords">Coordinates class of X,Y,Z</param>
        /// <param name="direction">This comes from Exit.Name and is a cardinal direction</param>
        /// <param name="opposite">The foreach loop is searching backwards so this must be set to true.
        /// </param>
        /// <returns>
        /// Correct set of Coordinates 
        /// </returns>
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

        /// <summary>
        /// loops through the exits of getRoom looking for adjacent rooms in AreaList
        /// then assigns coords to getRoom relating to the adjacent room
        /// 
        /// For example: 
        /// getRoom is (0,0,0) and has an exit which leads to areaId 1
        ///
        /// nextRoom is a room that has the matching areaId of one of getRoom's exits
        /// in this case 1.
        /// 
        /// So the coordinates it will assign to nextRoom is (1,0,0)
        /// 
        /// Because GetNewCoord(getRoom.coords, exit.name);
        /// would look like this:
        /// GetNewCoord((0,0,0), "North");
        /// 
        /// Once the coord has been given to a room it adds it to the completed 
        /// list and removes it from area list
        /// 
        /// </summary>
        /// <param name="getRoom">Last room from area list</param>
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

        /// <summary>
        /// Takes a direction and a coordinate axis and updates the value
        /// if it's opposite it decrements North and increments south for example
        /// </summary>
        /// <param name="opposite">if searching backwards set true</param>
        /// <param name="direction">The direction of the exit</param>
        /// <param name="coord">The coord to edit. Either X,Y or Z</param>
        /// <returns>updated coordinate axis</returns>
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

        /// <summary>
        /// Updates the room coordinates 
        /// </summary>
        /// <param name="room">The room that needs coordinates set</param>
        /// <param name="coords">The coordinates to set to the room</param>
        public static void SetCoords(Room.Room room, Coordinates coords)
        {
            if (room != null)
            {
                room.coords = coords;
            }
        }
    }
}