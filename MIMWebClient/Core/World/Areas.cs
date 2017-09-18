using System.Collections.Generic;

namespace MIMWebClient.Core.World
{
    using MIMWebClient.Core.Room;
    using MIMWebClient.Core.World.Tutorial;

    public static class Areas
    {
        public static List<Room> ListOfRooms()
        {
            var areas = new List<Room>

            {
                  Ambush.TutorialRoom1(),
               Ambush.TutorialRoom2(),
                Ambush.TutorialLostInTheWoods(),
                Ambush.TutorialLostInTheWoods2(),
                Ambush.TutorialLostInTheWoods3(),
                Ambush.TutorialLostInTheWoods4(),
                Ambush.TutorialLostInTheWoods5(),
                 Ambush.TutorialLostInTheWoods6(),
                Ambush.TutorialGoblinCamp(),
                Ambush.TutorialGoblinCampTentNorth(),
                Ambush.TutorialGoblinCampTentSouth(),
                Awakening.TempleOfTyr(),
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
                Anker.Anker.TempleRoad3(),
                Anker.Anker.TempleEntrance(),
                Anker.Anker.PathToTheSquare(),
                Anker.Anker.AnkerLane(),
                Anker.Anker.AnkerLaneWest21(),
                Anker.Anker.AnkerLaneWest25(),
                Anker.Anker.AnkerLaneWest37(),
                Anker.Anker.AnkerLaneEast22(),
                Anker.Anker.AnkerLaneEast23(),
                Anker.Anker.AnkerLaneEast24(),
                Anker.Anker.AnkerHome(),
                Anker.Anker.AnkerHome2(),
                Anker.Anker.AnkerHome3(),
                Anker.Anker.AnkerHome4(),
                Anker.Anker.AnkerHome5(),
                Anker.Anker.AnkerHome6(),
                Anker.Anker.AnkerHome31(),
                Anker.Anker.AnkerHome32(),
                Anker.Anker.AnkerHome33(),
                Anker.Anker.AnkerHome34(),
                Anker.Anker.AnkerHome35(),
                Anker.Anker.AnkerHome36(),
            };

            return areas;
        }


        public static List<Room> ListOfRoomsCanTeleport()
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
                Anker.Anker.TempleRoad3(),
                Anker.Anker.TempleEntrance(),
                Anker.Anker.PathToTheSquare(),
                Anker.Anker.AnkerLane(),
                Anker.Anker.AnkerLaneWest21(),
                Anker.Anker.AnkerLaneWest25(),
                Anker.Anker.AnkerLaneWest37(),
                Anker.Anker.AnkerLaneEast22(),
                Anker.Anker.AnkerLaneEast23(),
                Anker.Anker.AnkerLaneEast24(),
                Anker.Anker.AnkerHome(),
                Anker.Anker.AnkerHome2(),
                Anker.Anker.AnkerHome3(),
                Anker.Anker.AnkerHome4(),
                Anker.Anker.AnkerHome5(),
                Anker.Anker.AnkerHome6(),
                Anker.Anker.AnkerHome31(),
                Anker.Anker.AnkerHome32(),
                Anker.Anker.AnkerHome33(),
                Anker.Anker.AnkerHome34(),
                Anker.Anker.AnkerHome35(),
                Anker.Anker.AnkerHome36(),
            };

            return areas;
        }
    }
}