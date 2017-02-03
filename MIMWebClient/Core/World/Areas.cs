using System.Collections.Generic;

namespace MIMWebClient.Core.World
{
    using MIMWebClient.Core.Room;

    public static class Areas
    {
        public static List<Room> ListOfRooms()
        {
            var areas = new List<Room>

            {
                 Anker.Anker.VillageSquare(),
                Anker.Anker.SquareWalkOutsideTavern(),
                Anker.Anker.SquareWalkOutsideStables(),
                Anker.Anker.RedLionStables(),
                Anker.Anker.SquareWalkCommerceCorner(),
                Anker.Anker.SquareWalkEastOfCentre(),
                Anker.Anker.SquareWalkEntrance(),
                Anker.Anker.SquareWalkSouthWestOfCentre(),
                Anker.Anker.SquareWalkWestOfCentre(),
                Anker.Anker.SquareWalkSouthOfCentre(),
                Anker.Anker.DrunkenSailor(),
                Anker.Anker.GeneralStore(),
                Anker.Anker.MetalMedley(),
                Anker.Anker.VillageHall(),
                Anker.Anker.VillageHallEntrance(),
                Anker.Anker.VillageHallEldersRoom(),
                Anker.Anker.TempleRoad(),
                Anker.Anker.TempleRoad2(),
                Anker.Anker.TempleEntrance()
            };

            return areas;
        } 
    }
}