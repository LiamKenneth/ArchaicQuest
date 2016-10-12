using System.Collections.Generic;

namespace MIMWebClient.Core.World
{
    using MIMWebClient.Core.Room;

    public static class Areas
    {
        public static List<Room> ListOfRooms()
        {
            var areas = new List<Room> {Anker.Anker.VillageSquare(), Anker.Anker.DrunkenSailor(), Anker.Anker.SquareWalkOutsideTavern() };

            return areas;
        } 
    }
}