using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.World.Anker.Mobs;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Arms;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Body;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Feet;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Head;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Legs;
using MIMWebClient.Core.World.Items.Armour.HeavyArmour.FullPlate.Hands;

namespace MIMWebClient.Core.World.Anker
{
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using MIMWebClient.Core.World.Items.Weapons.Sword.Long;

    public static class AnkerFarm
    {
  
        //public static Room AnkerLaneGate()
        //{
        //    var room = new Room
        //    {
        //        region = "Anker",
        //        area = "Anker",
        //        areaId = 42,
        //        title = "Arched gate of Anker",
        //        description = "<p>The dirt road leads through the grey stoned archway which towers over the road. The archway signals the start of Anker as there is no gate to stop intruders from entering. Some engravings and carved into the stone. The Anker farm is to the east, further in the distance you can make out a forest of some kind. The western path through the archway leads to Anker.</p>",

        //        //Defaults
        //        exits = new List<Exit>(),
        //        items = new List<Item.Item>(),
        //        mobs = new List<Player>(),
        //        terrain = Room.Terrain.Field,
        //        keywords = new List<RoomObject>()
        //        {
        //            new RoomObject()
        //            {
        //                name = "engravings",
        //                look = "The engraving reads: \"Welcome to Anker\"",
        //                examine = "Looking closely at the engraving you see: \"Welcome to Anker\" underneath in small writing it reads: \"Malleus and Gamia was here XVI-X-MMXVII\"."
        //            }
        //        },
        //        corpses = new List<Player>(),
        //        players = new List<Player>(),
        //        fighting = new List<string>(),
        //        clean = true,
                

        //    };




        //    #region exits


        //    // Create Exits
        //    var east = new Exit
        //    {
        //        name = "East",
        //        area = "Anker",
        //        region = "Anker",
        //        areaId = 48,
        //        keywords = new List<string>(),
        //        hidden = false,
        //        locked = false
        //    };

        //    // Create Exits
        //    var west = new Exit
        //    {
        //        name = "West",
        //        area = "Anker",
        //        region = "Anker",
        //        areaId = 40,
        //        keywords = new List<string>(),
        //        hidden = false,
        //        locked = false
        //    };




        //    #endregion
        //    room.exits.Add(east);
        //    room.exits.Add(west);



        //    return room;
        //}

    }
}