using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MIMWebClient.Core.Item;
using MIMWebClient.Core.Room;

namespace MIMWebClient.Controllers.Admin.Room
{
    using MIMWebClient.Core.Item;
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
            this.itemCollection = database.GetCollection<Item>("Item");
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
                var room = this.roomCollection.Find(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area)).FirstOrDefault();

                room.region = collection["region"];
                room.area = collection["area"];
                room.areaId = Convert.ToInt32(collection["areaId"]);
                Room.Terrain terrain;
                Enum.TryParse(collection["terrain"], out terrain);
                room.terrain = terrain;

                room.title = collection["title"];

                room.description = collection["description"];

                this.roomCollection.ReplaceOne<Room>(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area), room);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var rooms = this.roomCollection.Find(x => x.areaId.Equals(areaId) && x.region.Equals(region) && x.area.Equals(area)).FirstOrDefault();
                return View(rooms);
            }
        }

        // POST: addItem
        [HttpPost]
        public void addItem(Item model)
        {
            try
            {
                this.itemCollection.InsertOne(model);
            }
            catch (Exception e)
            {

            }
        }

        // GET: Default/Create
        [HttpGet]
        public ActionResult Create()
        {
            var pageModel = new ToPage();

            var itemList = this.itemCollection.Find(_ => true).ToList();
            pageModel.itemModel.itemFlags = new List<Item.ItemFlags>();
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.antievil);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.antigood);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.antineutral);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.bless);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.container);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.dark);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.equipable);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.evil);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.glow);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.hidden);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.hum);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.invis);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.locked);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.nodrop);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.nolocate);
            pageModel.itemModel.itemFlags.Add(Item.ItemFlags.noremove);

            pageModel.itemSelect = itemList;

            return this.View(pageModel);
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult Create(FormCollection RoomData)
        {

            var newRoom = new Room();

            newRoom.region = RoomData["roomModel.title"];
            newRoom.area = RoomData["roomModel.area"];
            newRoom.areaId = Convert.ToInt32(RoomData["roomModel.areaId"]);

            newRoom.title = RoomData["roomModel.title"];
            newRoom.description = RoomData["roomModel.description"];

            Room.Terrain terrain;
            Enum.TryParse(RoomData["roomModel.terrain"], out terrain);
            newRoom.terrain = terrain;
    

            this.roomCollection.InsertOne(newRoom);

            if (ModelState.IsValid)
            {


                try
                {
                    // TODO: Add insert logic here

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        public IMongoCollection<Room> roomCollection { get; set; }
        public IMongoCollection<Item> itemCollection { get; set; }
    }
}

public class ToPage
{
    public Room roomModel { get; set; }
    public Item itemModel { get; set; }
    public Exit exitModel { get; set; }
    public List<Item> itemSelect { get; set; }

    public ToPage()
    {
        roomModel = new Room();
        itemModel = new Item();
        exitModel = new Exit();
    }
}

public class dummy
{
    public string name { get; set; }
}
