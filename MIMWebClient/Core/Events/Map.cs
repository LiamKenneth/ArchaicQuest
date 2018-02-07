using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class Map
    {
        public static string[,] ReturnMap(Room.Room currentRoom)
        {
            var grid = new string[5,5];

            //Find all rooms within 5points of player
            var rows = 5;
            var cols = 5;

            var startingCoordX = currentRoom.coords.X - 2;
            var startingCoordY = currentRoom.coords.Y + 2;
            for (int i = 0; i != rows; i++)
            {
                for (int j = 0; j != cols; j++)
                {

                    if (j == cols - 1)
                    {
                        startingCoordX = currentRoom.coords.X - 2;
                    }

                    if (i == rows)
                    {
                        startingCoordY--;
                    }

                    var getRoom =
                        Startup.SetMappedRooms.FirstOrDefault(
                            x => x.coords.X == startingCoordX && x.coords.Y == startingCoordY &&
                                 x.area == currentRoom.area);



                    grid[i, j] = buildMap(getRoom);
                    startingCoordX++;
                }
            }

            return grid;
        }

        public static string DisplayMap(Room.Room currentRoom)
        {

            var map = new StringBuilder();
            var grid = ReturnMap(currentRoom);
            var rows = 5;
            var cols = 5;
            var num = 0;

            for (int i = 0; i != rows; i++)
            {
                for (int j = 0; j != cols; j++)
                {
                    if (i == rows && j == cols)
                    {
                        map.Append("\n");
                        map.Append(grid[i, j]);
                    }
                    map.Append(grid[i, j]);

                   
                    num++;
                }
            }

            return map.ToString();
        }


        public static string buildMap(Room.Room currentRoom)
        {

            if (currentRoom == null)
            {
                return " ";
            }


 
            if (currentRoom?.exits.FirstOrDefault(x => x.name == "East") != null && currentRoom?.exits.FirstOrDefault(x => x.name == "North") != null)
            {
                return " | \n[ ]--";
            }


            if (currentRoom?.exits.FirstOrDefault(x => x.name == "East") != null && currentRoom?.exits.FirstOrDefault(x => x.name == "North") == null)
            {
                return "[ ]--";
            }


            if (currentRoom?.exits.FirstOrDefault(x => x.name == "North") != null && currentRoom?.exits.FirstOrDefault(x => x.name == "East") == null)
            {
                return " | \n" + "[ ]";
            }

            return "";
        }




    }
}


