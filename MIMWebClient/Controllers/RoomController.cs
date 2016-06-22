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
        public RoomController()
        {
           
                const string ConnectionString = "mongodb://localhost:27017";

                // Create a MongoClient object by using the connection string
                var client = new MongoClient(ConnectionString);

                //Use the MongoClient to access the server
                var database = client.GetDatabase("MIMDB");

                this.roomCollection = database.GetCollection<Room>("Room");
            
        }

        // GET: Room
        [HttpGet]
        public ActionResult Index()
        {
          
            var rooms = this.roomCollection.Find(_ => true).ToList();

            return this.View(rooms);
        }

        // GET: Room/Details/valston/Town/1

        public ActionResult Details(string region, string area, int areaId)
        { 

            var rooms = this.roomCollection.Find(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area)).FirstOrDefault();

            return this.View(rooms);
        }

        // GET: Room/Details/valston/Town/1
        [HttpGet]
        public ActionResult Edit(string region, string area, int areaId)
        {

            var rooms = this.roomCollection.Find(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area)).FirstOrDefault();

            return this.View(rooms);
        }

        // POST: Default/Edit/5
        [HttpPost]
        public ActionResult Edit(string region, string area, int areaId, FormCollection collection)
        {
           

            try
            {
                // TODO: Add update logic here

          //      var rooms = this.roomCollection.Find(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area)).FirstOrDefault();


                return RedirectToAction("Index");
            }
            catch
            {
                var rooms = this.roomCollection.Find(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area)).FirstOrDefault();
                return View(rooms);
            }
        }

        public IMongoCollection<Room> roomCollection { get; set; }
    }
}