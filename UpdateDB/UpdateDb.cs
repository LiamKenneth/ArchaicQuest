using MIMWebClient.Core.Room;
using MIMWebClient.Core.World.Anker;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Console.WriteLine("Compiling Areas.");

            var areaSpeed = new Stopwatch();
            areaSpeed.Start();

            var areas = new List<Room>();
            areas.Add(Anker.VillageSquare());
            areas.Add(Anker.SquareWalkOutsideTavern());
            areaSpeed.Stop();
            Console.WriteLine("Compiling Areas Completed in {0}ms.", areaSpeed.Elapsed.Milliseconds);


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
