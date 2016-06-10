using MIMWebClient.Core.Room;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Models.Admin.Rooms
{
    public class ViewRooms
    {
        public static Room getRooms()
        {
            const string ConnectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Room>("Room");

            Room rooms = collection.Find(x => x.areaId == 1).FirstOrDefault();

            return rooms;
        }
    }
}