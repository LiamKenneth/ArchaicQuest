using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MIMWebClient.Core.Item;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Controllers.Admin.Room
{
    using Core.PlayerSetup;
    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.Room;

    using MongoDB.Driver;

    public class RoomController : Controller
    {
        public RoomController()
        {

            const string ConnectionString = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

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

            var itemFlagValues = Enum.GetValues(typeof(Item.ItemFlags)).Cast<Item.ItemFlags>();
            pageModel.itemModel.itemFlags.AddRange(itemFlagValues);

            pageModel.itemModel.damageType = new List<Item.DamageType>();

            var damageTypeValues = Enum.GetValues(typeof(Item.DamageType)).Cast<Item.DamageType>();
            pageModel.itemModel.damageType.AddRange(damageTypeValues);


            pageModel.itemSelect = itemList;

            return this.View(pageModel);
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult Create(ToPage RoomData)
        {

            var newRoom = new Room();

            newRoom.region = RoomData.roomModel.region;
            newRoom.area = RoomData.roomModel.area;
            newRoom.areaId = RoomData.roomModel.areaId;

            newRoom.title = RoomData.roomModel.title;
            newRoom.description = RoomData.roomModel.description;

            Room.Terrain terrain;
            Enum.TryParse(RoomData.roomModel.terrain.ToString(), out terrain);
            newRoom.terrain = terrain;

            newRoom.keywords = new List<RoomObject>();

            newRoom.keywords = RoomData.roomModel.keywords;


            newRoom.exits = new List<Exit>();

            if (RoomData.roomModel.exits != null)
            {
                foreach (var exit in RoomData.roomModel.exits)
                {

                    newRoom.exits.Add(exit);
                }
            }


            newRoom.items = new List<Item>();

            if (RoomData.roomModel.items != null)
            {
                foreach (var item in RoomData.roomModel.items)
                {

                    newRoom.items.Add(item);
                }

            }


            newRoom.mobs = new List<Player>();

            if (RoomData.roomModel.mobs != null)
            {

                foreach (var mob in RoomData.roomModel.mobs)
                {
              
                    newRoom.mobs.Add(mob);
                }
            }

            newRoom.corpses = new List<Player>();

            if (RoomData.roomModel.corpses != null)
            {
                foreach (var corpse in RoomData.roomModel.corpses)
                {

                    newRoom.corpses.Add(corpse);
                }
            }

            try
            {
                this.roomCollection.InsertOne(newRoom);

                return RedirectToAction("Index");
            }
            catch
            {

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
    public Player mobModel { get; set; }

    public RoomObject roomKeywords { get; set; }
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
