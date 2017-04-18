using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MIMWebClient.Core.AI;

namespace MIMWebClient.Core.Events
{
    public static class Map
    {
        public static List<Room.Room> _roomList { get; set;} = new List<Room.Room>();


        public static string GenerateGrid()
        {
            if (_roomList.Count == 0)
            {
                var roomSetUp = new BreadthFirstSearch();
                Map._roomList = roomSetUp.AssignCoords();
            }

            var buildMap = new StringBuilder();

            var xAxisLength = _roomList.Max(i => i.coords.X) + 1;
            var yAxisLength = _roomList.Max(i => i.coords.Y) + 1;

            for (int x = -20; x < xAxisLength; x++)
            {
                for (int y = -20; y < yAxisLength; y++)
                {
                    Map.DrawMap(x, y, buildMap);
                }
                buildMap.Append("\r\n");
            }

            return buildMap.ToString();
        }

        private static bool IsRoom(int x, int y)
        {
            return Map._roomList.Any(c => c.coords.X.Equals(x) && c.coords.Y.Equals(y));
        }

        private static string DrawRoom()
        {
            return "o";
        }

        private static string DrawHorizontalConnector()
        {
            return "-";
        }

        private static string DrawVerticalConnector()
        {
            return "|";
        }

        private static string DrawRightAngleConnector()
        {
            return "/";
        }

        private static string DrawLeftAngleConnector()
        {
            return "\\";
        }
        private static void DrawMap(int x, int y, StringBuilder buildMap)
        {

            var room = Map._roomList.FirstOrDefault(c => c.coords.X.Equals(x) && c.coords.Y.Equals(y));
 

            if (IsRoom(x, y))
                buildMap.Append(DrawRoom());
            else if (IsRoom(x, y - 1) && IsRoom(x, y + 1))
                buildMap.Append(DrawHorizontalConnector());
            else if (IsRoom(x - 1, y) && IsRoom(x + 1, y))
                buildMap.Append(DrawVerticalConnector());
            else if (y % 2 == 0)
                buildMap.Append(" ");
            else
                buildMap.Append(" ");
        }
    }
}