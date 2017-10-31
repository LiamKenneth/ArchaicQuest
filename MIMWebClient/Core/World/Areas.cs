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
                Anker.Anker.TempleOfTyr(),
                Anker.Anker.TempleRoad4(),
                Anker.Anker.TempleGraveyard(),
                Anker.Anker.TempleGraveyard1(),
                Anker.Anker.TempleGraveyard2(),
                Anker.Anker.TempleGraveyard3(),
                Anker.Anker.TempleGraveyard4(),
                Anker.Anker.TempleGraveyard5(),
                Anker.Anker.TempleGraveyard6(),
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
                Anker.Anker.AnkerLaneEast40(),
                Anker.Anker.AnkerLaneGate(),
                Anker.AnkerFarm.AnkerRoad(),
                Anker.AnkerFarm.RoadThroughFarm(),
                Anker.AnkerFarm.Farm(),
                Anker.AnkerFarm.Windmill(),
                Anker.AnkerFarm.CropFields(),
                Anker.AnkerFarm.CropFields1(),
                Anker.AnkerFarm.CropFields2(),
                Anker.AnkerFarm.CropFields3(),
                Anker.AnkerFarm.AnkerRoad1(),
                Anker.AnkerFarm.AnkerRoad2(),
                Anker.AnkerFarm.AnkerRoad3(),
                Anker.AnkerFarm.RiverBank(),
                Anker.AnkerFarm.SawMill(),
                Anker.AnkerFarm.Pasture(),
                Anker.AnkerFarm.TheBridge(),
                Anker.AnkerFarm.PalusRiver(),
                Anker.AnkerFarm.PalusRiver0(),
                Anker.AnkerFarm.PalusRiver1(),
                Anker.AnkerFarm.PalusRiver2(),
                Anker.AnkerFarm.PalusRiver3(),
                Anker.AnkerFarm.HermitsHome(),
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