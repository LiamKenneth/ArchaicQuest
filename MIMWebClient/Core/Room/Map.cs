using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MIMWebClient.Core.Room
{
    public class Map
    {

        public List<Tuple<int, int>> Rooms ()
        {

            var roomList = new List<Tuple<int, int>>();

            roomList.Add(new Tuple<int, int>(0, 0));
            roomList.Add(new Tuple<int, int>(2, 0));
            roomList.Add(new Tuple<int, int>(0, 2));
            roomList.Add(new Tuple<int, int>(2, 2));
            roomList.Add(new Tuple<int, int>(0, 4));
            roomList.Add(new Tuple<int, int>(2, 4));
            roomList.Add(new Tuple<int, int>(4, 4));
            roomList.Add(new Tuple<int, int>(4, 6));
            roomList.Add(new Tuple<int, int>(4, 8));
            roomList.Add(new Tuple<int, int>(4, 10));
            roomList.Add(new Tuple<int, int>(6, 10));
            roomList.Add(new Tuple<int, int>(8, 10));
            roomList.Add(new Tuple<int, int>(8, 8));
            roomList.Add(new Tuple<int, int>(8, 6));

            return roomList;
        }




        public static void GenerateGrid(PlayerSetup.Player player)
        {
          
            var buildMap  = new StringBuilder();
            var map = new Map();

            var xAxisLength = map.Rooms().Max(i => i.Item1) + 1;
            var yAxisLength = map.Rooms().Max(i => i.Item2) + 1;


            for (int x = 0; x < xAxisLength; x++)
            {
                for (int y = 0; y < yAxisLength; y++)
                {
                    map.DrawMap(x, y, buildMap);
                }
                buildMap.Append("\r\n");
            }

            HubContext.SendToClient(buildMap.ToString(), player.HubGuid);
        }


        public bool IsRoom(int x, int y)
        {
            return Rooms().Any(c => c.Item1.Equals(x) && c.Item2.Equals(y));

        }

        public static string DrawRoom()
        {
            return "[ ]";
        }

        public static string DrawVerticalConnector()
        {
            return " | ";
        }

        public static string DrawHorizontalConnector()
        {
            return "--";
        }

        public static string DrawRightAngleConnector()
        {
            return "/";
        }

        public static string DrawLeftAngleConnector()
        {
            return "\\";
        }
        public void DrawMap (int x, int y, StringBuilder buildMap)
        {
            if (IsRoom(x, y))
            {
                buildMap.Append(DrawRoom());
            }
            else if (IsRoom(x, y - 1) && IsRoom(x, y + 1))
            {
                buildMap.Append(DrawHorizontalConnector());
            }
            else if (IsRoom(x - 1, y) && IsRoom(x + 1, y))
            {
                buildMap.Append(DrawVerticalConnector());
            }   
            else
                buildMap.Append("  ");
        }
    }
}