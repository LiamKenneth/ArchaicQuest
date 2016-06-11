using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIMWebClient.Controllers.Admin.Room
{
    using MIMWebClient.Core.Room;

    using MongoDB.Driver;

    public class RoomController : Controller
    {
        // GET: Admin
        [HttpGet]
        public ActionResult Index()
        {
            const string ConnectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Room>("Room");

            var rooms = collection.Find(_ => true).ToList();

            return this.View(rooms);
        }

        // GET: Admin/Details
        [HttpGet]
        public ActionResult Details(int areaid, string area)
        {
            const string ConnectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("MIMDB");

            var collection = database.GetCollection<Room>("Room");

            var rooms = collection.Find(x => x.area.Equals(area) && x.areaId.Equals(areaid)).FirstOrDefault();

            return this.View(rooms);
        }
    }
}