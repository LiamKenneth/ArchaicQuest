using MIMEngine.Core.PlayerSetup;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    public class Save
    {

        public async static Task savePlayer(Player player)
        {
            const string connectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Player>("Player");

             await collection.InsertOneAsync(player);            
          
        }

        public async static Task UpdatePlayer(Player player)
        {
            const string connectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Player>("Player");

            await collection.ReplaceOneAsync<Player>(x => x._id == player._id, player);
                
        }

        //public async static Task GetPlayer(string name, string password)
        //{
        //    const string connectionString = "mongodb://localhost:27017";

        //    // Create a MongoClient object by using the connection string
        //    var client = new MongoClient(connectionString);

        //    //Use the MongoClient to access the server
        //    var database = client.GetDatabase("MIMDB");

        //    var collection = database.GetCollection<Player>("Player");

        //    Player player = collection.FindAsync<Player>(x => x.Name == name && x.Password == password, new FindOptions<Player> { Limit = 1 });
 

        //}
    }
}
