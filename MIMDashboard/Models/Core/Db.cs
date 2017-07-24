using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace MIMDashboard.Models.Core
{
    public static class Db
    {
        private static IMongoDatabase _MIMDB { get; set; }

        private static void setDB()
        {
            const string ConnectionString = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
             _MIMDB = client.GetDatabase("mimdb");
 
        }

        public static IMongoDatabase GetDb()
        {
            if (_MIMDB == null)
            {
                setDB();
                return _MIMDB;
            }

            return _MIMDB;
        }
    }
}