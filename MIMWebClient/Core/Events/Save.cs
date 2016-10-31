using MIMWebClient.Core.PlayerSetup;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace MIMWebClient.Core.Events
{
    using MongoDB.Driver.Linq;

    using Player = MIMWebClient.Core.PlayerSetup.Player;

    public static class Save
    {
        private const string DbServer = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

        public static void SavePlayer(Player player)
        {

            try
            {
                const string ConnectionString = DbServer;

                // Create a MongoClient object by using the connection string
                var client = new MongoClient(ConnectionString);

                //Use the MongoClient to access the server
                var database = client.GetDatabase("mimdb");

                var collection = database.GetCollection<Player>("Player");

                collection.InsertOne(player);
            }
            catch(Exception e)
            {
              
            }    
          
        }

        public static void UpdatePlayer(Player player)
        {
            const string ConnectionString = DbServer;

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

            var collection = database.GetCollection<Player>("Player");

             collection.ReplaceOne<Player>(x => x._id == player._id, player);

            HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("The gods take note of your progress");


        }

        public static Player GetPlayer(string name)
        {
            const string ConnectionString = DbServer;

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

            var collection = database.GetCollection<Player>("Player");

            var cleanName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

            var returnPlayer = collection.AsQueryable<Player>().FirstOrDefault(x => x.Name.Equals(cleanName)); 

            return returnPlayer;



        }
    }
}
