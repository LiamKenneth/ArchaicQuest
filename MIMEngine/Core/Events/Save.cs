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
    using MongoDB.Driver.Linq;

    using Player = MIMEngine.Core.PlayerSetup.Player;

    public static class Save
    {
        private const string DbServer = "mongodb://localhost:27017";

        public static async Task SavePlayer(Player player)
        {
            const string ConnectionString = DbServer;

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Player>("Player");

             await collection.InsertOneAsync(player);            
          
        }

        public static async Task UpdatePlayer(Player player)
        {
            const string ConnectionString = DbServer;

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Player>("Player");

            await collection.ReplaceOneAsync<Player>(x => x._id == player._id, player);

            HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("The gods take note of your progress");


        }

        public static async Task<Player> GetPlayer(string name, string password)
        {
            const string ConnectionString = DbServer;

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Player>("Player");

            var returnPlayer = await collection.AsQueryable<Player>().SingleOrDefaultAsync(x => x.Name.Equals(name)); 

            return returnPlayer;



        }
    }
}
