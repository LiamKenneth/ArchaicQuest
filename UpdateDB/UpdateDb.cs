using MIMWebClient.Core.Room;
using MIMWebClient.Core.World.Anker;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.World.Tutorial;

namespace UpdateDB
{
    class UpdateDb
    {
        public IMongoCollection<Room> roomCollection { get; set; }



        static void Main(string[] args)
        {
            //TODO: Clean up :)

            const string ConnectionString = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

            var roomCollection = database.GetCollection<Room>("Room");

            Console.WriteLine("Cleaning DB");
            database.DropCollection("Room");
            Console.WriteLine("Compiling Areas.");

            var areaSpeed = new Stopwatch();
            areaSpeed.Start();

            var areas = new List<Room>
            {
                Ambush.TutorialRoom1(),
                Ambush.TutorialRoom2(),
                Awakening.TempleOfTyr(),
                Anker.VillageSquare(),
                Anker.SquareWalkOutsideTavern(),
                Anker.SquareWalkOutsideStables(),
                Anker.RedLionStables(),
                Anker.SquareWalkCommerceCorner(),
                Anker.SquareWalkEastOfCentre(),
                Anker.SquareWalkEntrance(),
                Anker.SquareWalkSouthWestOfCentre(),
                Anker.SquareWalkWestOfCentre(),
                Anker.SquareWalkSouthOfCentre(),
                Anker.DrunkenSailor(),
                Anker.GeneralStore(),
                Anker.MetalMedley(),
                Anker.VillageHall(),
                Anker.VillageHallEntrance(),
                Anker.VillageHallEldersRoom(),
                Anker.TempleRoad(),
                Anker.TempleRoad2(),
                Anker.TempleEntrance(),
                Anker.PathToTheSquare(),
                Anker.AnkerLane(),
                Anker.AnkerLaneWest21(),
                Anker.AnkerLaneWest25(),
                Anker.AnkerLaneWest37(),
                Anker.AnkerLaneEast22(),
                Anker.AnkerLaneEast23(),
                Anker.AnkerLaneEast24(),
            };
            areaSpeed.Stop();
            Console.WriteLine("Compiling Areas Completed in {0}ms.", areaSpeed.Elapsed.Milliseconds);

            //check areas for duplicate ids


            Console.WriteLine("Adding Area's to Database");
            var addSpeed = new Stopwatch();
            addSpeed.Start();
            foreach (var area in areas)
            {
                Console.WriteLine("Added {0}", area.title);

                roomCollection.InsertOne(area);
            }
            addSpeed.Stop();

            Console.WriteLine("Adding Area's to Database Completed in {0}ms.", addSpeed.Elapsed.Milliseconds);

            Console.ReadLine();
        }
    }
}
