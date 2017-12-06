using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Events
{
    using System.Runtime.Remoting.Channels;

    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class ManipulateObject
    {
        private static string lookItem { get; } = "item";
        private static string FindInventory { get; } = "inventory";
        private static string Findkillable { get; } = "killable";
        private static string FindAll { get; } = "all";


        /// <summary>
        /// Finds Objects in room, inventory, containers or mobs and players
        /// </summary>
        /// <param name="room">the room the player is in</param>
        /// <param name="player">the player calling the method</param>
        /// <param name="command">command used</param>
        /// <param name="thingToFind">Object to find</param>
        /// <param name="objectTypeToFind">can only be item, inventory or all</param>
        /// <returns></returns>
        public static object FindObject(Room room, Player player, string command, string thingToFind,
            string objectTypeToFind = "")
        {

            if (thingToFind == "all" && objectTypeToFind == "all")
            {
                return new KeyValuePair<Item, Item>(null, null);
            }

            string item = thingToFind;
            var itemContainer = string.Empty;
            //checks for spaces

            //get sword bag - text after the 1st space is the container
            item = FindFirstAndLast.FindFirstAndLastIndex(thingToFind).Key;
            itemContainer = FindFirstAndLast.FindFirstAndLastIndex(thingToFind).Value;

            //get Item
            var findObject = Events.FindNth.Findnth(item);
            int nth = findObject.Key;
            string itemToFind = findObject.Value;


            //get container
            var findContainer = new KeyValuePair<int, string>();
            int nthContainer = 0;
            string comntainerToFind = string.Empty;
            if (!string.IsNullOrEmpty(itemContainer))
            {
                findContainer = Events.FindNth.Findnth(itemContainer);
                nthContainer = findContainer.Key;
                comntainerToFind = findContainer.Value;
            }

            Item foundItem = null;
            Item foundContainer = null;
            Player foundMob = null;
            Player foundPlayer = null;

            List<Item> roomItems = room.items;
            List<Item> playerInv = player.Inventory;


            #region find Item searching Room and Player Inventory

            if (objectTypeToFind == lookItem && itemToFind != "all")
            {
                // if its not a container
                if (string.IsNullOrEmpty(itemContainer))
                {
                    //search room items 1st
                    //this is crappy
                    foundItem = FindItem.Item(room.items, nth, itemToFind, Item.ItemLocation.Room);

                    if (foundItem != null)
                    {
                        return new KeyValuePair<Item, Item>(null, foundItem);
                    }


                    string msgToPlayer = "You don't see " + AvsAnLib.AvsAn.Query(itemToFind).Article + " " + itemToFind +
                                         " here and you are not carrying " + AvsAnLib.AvsAn.Query(itemToFind).Article +
                                         " " + itemToFind;

                    HubContext.Instance.SendToClient(msgToPlayer, player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} rummages around for an item but finds nothing.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    return new KeyValuePair<Item, Item>(null, null);
                }
                else
                {

                    //look in room
                    foundContainer = (nthContainer == -1)
                        ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
                        : roomItems.FindAll(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
                            .Skip(nthContainer - 1)
                            .FirstOrDefault();


                    if (foundContainer != null)
                    {

                        foundItem = FindItem.Item(foundContainer.containerItems, nth, itemToFind, Item.ItemLocation.Room);

                    }

                    //return item found in container
                    if (foundItem != null || itemToFind.Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                    }
                    else
                    {

                        HubContext.Instance.SendToClient("You don't see that inside the container", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} searches around inside the container but finds nothing";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }
                    }


                }



            }
            else if (itemToFind == "all" && roomItems.Count == 0 && command == "get")
            {
                HubContext.Instance.SendToClient("There is nothing here to get", player.HubGuid);
            }
            else if (itemToFind == "all" && playerInv.Count == 0 && command == "drop" ||
                     itemToFind == "all" && playerInv.Count == 0 && command == "put")
            {
                HubContext.Instance.SendToClient("You have nothing to drop", player.HubGuid);
            }

            #endregion
            #region find item in player inventory for commands such as drop, equip, wield etc

            else if (objectTypeToFind == FindInventory)
            {

                if (string.IsNullOrEmpty(itemContainer))
                {

                    foundItem = FindItem.Item(player.Inventory, nth, itemToFind, Item.ItemLocation.Inventory);



                    if (foundItem != null || itemToFind.Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                    }
                    else
                    {

                        HubContext.Instance.SendToClient("you are not carrying such an item", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} tries to get an item but can't find it.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                    }
                }
                else
                {
                    //look in inv
                    if (itemToFind != "all")
                    {
                        foundItem = FindItem.Item(player.Inventory, nth, itemToFind, Item.ItemLocation.Inventory);
                    }

                    if (foundItem != null || itemContainer != null && itemToFind == "all")
                    {
                        //find container
                        foundContainer = (nth == -1)
                            ? roomItems.Find(x => x.name.ToLower().Contains(itemContainer))
                            : roomItems.FindAll(x => x.name.ToLower().Contains(itemContainer))
                                .Skip(nth - 1)
                                .FirstOrDefault();
                    }
                    else
                    {

                        HubContext.Instance.SendToClient("you are not carrying such an item", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} tries to get an item but can't find it.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                    }

                    //return item found in container
                    if (foundItem != null || foundContainer != null)
                    {
                        return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                    }
                    else
                    {

                        HubContext.Instance.SendToClient($"You don't see that item.", player.HubGuid);

                        //foreach (var character in room.players)
                        //{
                        //    if (player != character)
                        //    {

                        //        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} tries to get somthing from a {foundContainer.name} but can't find it.";

                        //        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        //    }
                        //}

                    }
                }
            }
            #endregion
            #region find killable mob or player

            else if (objectTypeToFind == Findkillable)
            {


                //search mob 
                foundMob = FindItem.Player(room.mobs, nth, itemToFind);

                if (foundMob != null)
                {
                    return foundMob;
                }
                //search player
                foundPlayer = FindItem.Player(room.players, nth, itemToFind);

                if (foundPlayer != null)
                {
                    return foundPlayer;
                }
                else
                {

                    var result = AvsAnLib.AvsAn.Query(itemToFind);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"You don't see {article} {itemToFind} here.", player.HubGuid);


                }


            }
            #endregion

            else if (objectTypeToFind == FindAll)
            {

                if (comntainerToFind == null)
                {

                    FindWhere.find(room, player, nth, itemToFind);
                }
                else
                {
                    //look in room
                    foundContainer = (nthContainer == -1)
                        ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
                        : roomItems.FindAll(x => x.name.ToLower().Contains(comntainerToFind) && x
                                                     .container == true).Skip(nthContainer - 1).FirstOrDefault();



                    if (foundContainer != null)
                    {
                        //inside found container
                        if (foundContainer.containerItems != null)
                        {


                            var containerItemsCount = foundContainer.containerItems.Count;

                            if (containerItemsCount == 0)
                            {
                                //TODO: Get all should not come here??
                                //dafuq? 
                                HubContext.Instance.SendToClient($"The {foundContainer.name} is empty.", player.HubGuid);

                                foreach (var character in room.players)
                                {
                                    if (player != character)
                                    {

                                        var roomMessage =
                                            $"{Helpers.ReturnName(player, character, string.Empty)} looks in a {foundContainer.name} but finds nothing.";

                                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                    }
                                }


                                return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                            }

                            foreach (var containerItem in foundContainer.containerItems.ToList())
                            {
                                
                           
                                if (containerItem.type != Item.ItemType.Gold ||
                                    containerItem.type != Item.ItemType.Silver
                                    || containerItem.type != Item.ItemType.Copper)
                                {



                                    containerItem.location = Item.ItemLocation.Inventory;
                                    player.Inventory.Add(containerItem);

                                }
                                else
                                {
                                    if (containerItem.type == Item.ItemType.Gold)
                                    {
                                        player.Gold += containerItem.count;
                                    }

                                    if (containerItem.type == Item.ItemType.Silver)
                                    {
                                        player.Silver += containerItem.count;
                                    }

                                    if (containerItem.type == Item.ItemType.Copper)
                                    {
                                        player.Copper +=containerItem.count;
                                    }

                                }

                                var result = AvsAnLib.AvsAn.Query(foundContainer.name);
                                string article = result.Article;

                                var containerResult = AvsAnLib.AvsAn.Query(foundContainer.name);
                                string containerArticle = containerResult.Article;


                                HubContext.Instance.SendToClient(
                                    $"You pick up {article} {containerItem.name} from {containerArticle} {foundContainer.name}.",
                                    player.HubGuid);

                                foreach (var character in room.players)
                                {
                                    if (player != character)
                                    {

                                        var roomMessage =
                                            $"{Helpers.ReturnName(player, character, string.Empty)} picks up {article} {containerItem.name} from {containerArticle} {foundContainer.name}.";

                                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                    }
                                }


                                foundContainer.containerItems.Remove(containerItem);
                            }
                        }
                        else
                        {

                            HubContext.Instance.SendToClient($"{foundContainer.name} doesn't contain that. ", player.HubGuid);

                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage =
                                        $"{Helpers.ReturnName(player, character, string.Empty)} searches around the {foundContainer.name} but finds nothing.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }


                            return new KeyValuePair<Item, Item>(foundContainer, foundItem);

                        }
                    }


                }

            }



            return new KeyValuePair<Item, Item>(foundContainer, foundItem);
        }

        /// <summary>
        /// Adds item from room to player inventory
        /// </summary>
        /// <param name="room">Room Object</param>
        /// <param name="player">Player Object</param>
        /// <param name="userInput">Text user entered</param>
        public static void GetItem(Room room, Player player, string userInput, string commandKey, string type)
        {

            //TODO handle container
            var currentRoom = room;
            var currentPlayer = player;
            string[] all = userInput.Split();

            if (all[0] == "all")
            {
                type = "all";
            }


            var returnedItem = (KeyValuePair<Item, Item>)FindObject(room, player, commandKey, userInput, type);
            var container = returnedItem.Key;
            var item = returnedItem.Value;


            if (all[0].Equals("all", StringComparison.InvariantCultureIgnoreCase) && all.Length == 1)
            {
                var roomItems = room.items;
                var roomItemsCount = roomItems.Count;

                for (int i = roomItemsCount - 1; i >= 0; i--)
                {
                    if (!roomItems[i].stuck)
                    {
                        //Get all Items from the room
                        if (roomItems[i].type != Item.ItemType.Gold)
                        {
                            roomItems[i].location = Item.ItemLocation.Inventory;
                            player.Inventory.Add(roomItems[i]);

                            player.Weight += roomItems[i].Weight;

                            var result = AvsAnLib.AvsAn.Query(roomItems[i].name);
                            string article = result.Article;


                            HubContext.Instance.SendToClient($"You pick up {article} {roomItems[i].name}", player.HubGuid);

                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage =
                                        $"{Helpers.ReturnName(player, character, string.Empty)} picks up {article} {roomItems[i].name}.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }


                            room.items.Remove(roomItems[i]);
                        }
                        else
                        {


                            player.Gold += roomItems[i].count;


                            var result = AvsAnLib.AvsAn.Query(roomItems[i].name);
                            string article = result.Article;
                            HubContext.Instance.SendToClient($"You pick up {roomItems[i].count} {roomItems[i].name}",
                                player.HubGuid);

                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage =
                                        $"{Helpers.ReturnName(player, character, string.Empty)} picks up {roomItems[i].count} {roomItems[i].name}.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }

                            room.items.Remove(roomItems[i]);
                        }
                    }
                    else
                    {
                        HubContext.Instance.SendToClient("You can't take that", player.HubGuid);
                    }
                }


            }
            else if (all[0].Equals("all", StringComparison.InvariantCultureIgnoreCase) && container != null)
            {

                if (container.locked == true)
                {

                    HubContext.Instance.SendToClient($"{container.name} is locked.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} tries to open the locked {container.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    return;
                }
                //get all from container
                var containerItems = container.containerItems;
                var containerCount = containerItems.Count;

                for (int i = containerCount - 1; i >= 0; i--)
                {
                    if (containerItems[i].type != Item.ItemType.Gold)
                    {

                        containerItems[i].location = Item.ItemLocation.Inventory;
                        player.Inventory.Add(containerItems[i]);

                        var result = AvsAnLib.AvsAn.Query(containerItems[i].name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient(
                            $"You get {article} {containerItems[i].name} from the {container.name}.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} gets {article} {containerItems[i].name} from the {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }


                        containerItems.Remove(containerItems[i]);
                    }
                    else
                    {


                        player.Gold += containerItems[i].count;

                        var result = AvsAnLib.AvsAn.Query(container.name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient(
                            $"You pick up {item.count} {item.name} from {article} {container.name}.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} picks up {item.count} {item.name} from {article} {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }



                        containerItems.Remove(containerItems[i]);
                    }
                }


            }

            else if (item != null)
            {
                //get single Item       

                if (container == null)
                {

                    if (!item.stuck)
                    {

                        if (item.type != Item.ItemType.Gold)
                        {

                            room.items.Remove(item);
                            item.location = Item.ItemLocation.Inventory;
                            player.Inventory.Add(item);
            
                            var result = AvsAnLib.AvsAn.Query(item.name);
                            string article = result.Article;

                            HubContext.Instance.SendToClient($"You pick up {article} {item.name}.", player.HubGuid);

                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage =
                                        $"{Helpers.ReturnName(player, character, string.Empty)} picks up {article} {item.name}.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }

                        }
                        else
                        {
                            room.items.Remove(item);
                            player.Gold += item.count;


                            var result = AvsAnLib.AvsAn.Query(container.name);
                            string article = result.Article;

                            HubContext.Instance.SendToClient($"You pick up {item.count} {item.name}.", player.HubGuid);

                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage =
                                        $"{Helpers.ReturnName(player, character, string.Empty)} picks up {item.count} {item.name} from {article} {container.name}.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }

                        }
                    }
                    else
                    {
                        HubContext.Instance.SendToClient("You can't take that", player.HubGuid);
                    }

                }
                else
                {
                    if (container.locked == true)
                    {
                        HubContext.Instance.SendToClient($"{container.name} is locked.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} tries to open the locked {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                        return;
                    }

                    //Get item from container

                    if (item.type != Item.ItemType.Gold)
                    {


                        container.containerItems.Remove(item);
                       item.location = Item.ItemLocation.Inventory;
                        player.Inventory.Add(item);


                        var result = AvsAnLib.AvsAn.Query(item.name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient($"You get {article} {item.name} from the {container.name}.",
                            player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} gets {article} {item.name} from the {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }


                    }
                    else
                    {
                        container.containerItems.Remove(item);

                        player.Gold += item.count;

                        var result = AvsAnLib.AvsAn.Query(container.name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient($"You get {item.count} {item.name} from {article} {container.name}.",
                            player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} gets {item.count} {item.name} from {article} {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }
                    }





                }


            }
            else
            {

                HubContext.Instance.SendToClient("There is nothing here to pick up.", player.HubGuid);

                return;
            }

            //save to cache
            Quest.CheckIfGetItemQuestsComplete(player);
            Cache.updateRoom(room, currentRoom);
            Cache.updatePlayer(player, currentPlayer);
            Score.UpdateUiInventory(player);
            Score.ReturnScoreUI(player);
            var roomdata = LoadRoom.DisplayRoom(room, player.Name);
            Score.UpdateUiRoom(player, roomdata);
        }

        /// <summary>
        /// Drops item from player inventory
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void DropItem(Room room, Player player, string userInput, string commandKey)
        {
            var currentRoom = room;
            var currentPlayer = player;
            string[] all = userInput.Split();
            var returnedItem = (KeyValuePair<Item, Item>)FindObject(room, player, commandKey, userInput, FindInventory);
            var container = returnedItem.Key;
            var item = returnedItem.Value;

            if (all[0].Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                var playerInv = player.Inventory.Where(x => x.type != Item.ItemType.Gold).ToList();
                var playerInvCount = player.Inventory.Count;

                for (int i = playerInvCount - 1; i >= 0; i--)
                {
                    playerInv[i].location = Item.ItemLocation.Room;


                    if (container == null)
                    {
                        playerInv[i].location = Item.ItemLocation.Room;
                        playerInv[i].isHiddenInRoom = false;
                        room.items.Add(playerInv[i]);

                        var result = AvsAnLib.AvsAn.Query(playerInv[i].name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient($"You drop {article} {playerInv[i].name}.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} drops {article} {playerInv[i].name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                        player.Inventory.Remove(playerInv[i]);
                    }
                    else
                    {

                        if (container.locked == true)
                        {

                            HubContext.Instance.SendToClient($"{container.name} is locked.", player.HubGuid);

                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {

                                    var roomMessage =
                                        $"{Helpers.ReturnName(player, character, string.Empty)} tries to open the locked {container.name}.";

                                    HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                                }
                            }

                            return;
                        }

                        if (container.open == false)
                        {
                            HubContext.Instance.SendToClient(
                                "You have to open the " + container.name + " before you can put something inside",
                                player.HubGuid);
                            return;
                        }

                        container.containerItems.Add(playerInv[i]);


                        var result = AvsAnLib.AvsAn.Query(playerInv[i].name);
                        string article = result.Article;

                        var containerResult = AvsAnLib.AvsAn.Query(container.name);
                        string containerArticle = containerResult.Article;

                        HubContext.Instance.SendToClient(
                            $"You drop {article} {playerInv[i].name} into {containerArticle} {container.name}.",
                            player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} drops {article} {playerInv[i].name} into {containerArticle} {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }



                        player.Inventory.Remove(playerInv[i]);
                    }


                }
            }
            else
            {
                if (item == null)
                {
                    return;
                }



                if (container == null)
                {


                    player.Inventory.Remove(item);
                    item.location = Item.ItemLocation.Room;
                    room.items.Add(item);

                    var result = AvsAnLib.AvsAn.Query(item.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"You drop {article} {item.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} drops {article} {item.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                }
                else
                {

                    if (container.locked == true)
                    {
                        HubContext.Instance.SendToClient($"{container.name} is locked.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} tries to open the locked {container.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                        return;
                    }

                    if (container.open == false)
                    {
                        HubContext.Instance.SendToClient(
                            "You have to open the " + container.name + " before you can put something inside",
                            player.HubGuid);
                        return;
                    }

                    player.Inventory.Remove(item);
                    item.location = Item.ItemLocation.Room;
                    container.containerItems.Add(item);

                    var result = AvsAnLib.AvsAn.Query(item.name);
                    string article = result.Article;

                    var containerResult = AvsAnLib.AvsAn.Query(container.name);
                    string containerArticle = containerResult.Article;

                    HubContext.Instance.SendToClient($"You put {article} {item.name} into the {container.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} puts {article} {item.name} into the {container.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                }


            }

            //save to cache
            Cache.updateRoom(room, currentRoom);
            Cache.updatePlayer(player, currentPlayer);
            Score.UpdateUiInventory(player);
            var roomdata = LoadRoom.DisplayRoom(room, player.Name);
            Score.UpdateUiRoom(player, roomdata);
        }


        /// <summary>
        /// Drops item from player inventory
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void GiveItem(Room room, Player player, string userInput, string commandKey, string type)
        {
            var currentRoom = room;
            var currentPlayer = player;
            string[] all = userInput.Split();
            var itemToGive = all[0];
            var thing = all.Last();
            var item = itemToGive;

            var foundItem =
                player.Inventory.FirstOrDefault(x => item != null && x.name.ToLower().Contains(item.ToLower()));
            var foundThing =
                room.players.FirstOrDefault(
                    x => thing != null && x.Name.StartsWith(thing, StringComparison.CurrentCultureIgnoreCase)) ??
                room.mobs.FirstOrDefault(
                    x => thing != null && x.Name.StartsWith(thing, StringComparison.CurrentCultureIgnoreCase));

            if (all[0].Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                var playerInv = player.Inventory;
                var playerInvCount = player.Inventory.Count;

                for (int i = playerInvCount - 1; i >= 0; i--)
                {
                    if (foundThing != null)
                    {
                        foundThing.Inventory.Add(playerInv[i]);


                        var result = AvsAnLib.AvsAn.Query(playerInv[i].name);
                        string article = result.Article;

                        var result2 = AvsAnLib.AvsAn.Query(foundThing.Name);
                        string article2 = result2.Article;

                        if (foundThing.KnownByName)
                        {
                            article2 = String.Empty;
                        }


                        HubContext.Instance.SendToClient(
                            $"You give {article} {playerInv[i].name} to {article2} {foundThing.Name}.", player.HubGuid);

                        HubContext.Instance.SendToClient(
                            $"{Helpers.ReturnName(player, foundThing, string.Empty)} gives {article} {playerInv[i].name} to you.",
                            foundThing.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player.Name != character.Name && character.Name != foundThing.Name)
                            {

                                var roomMessage =
                                    $"{Helpers.ReturnName(player, character, string.Empty)} gives {article} {playerInv[i].name} to {article2} {Helpers.ReturnName(foundThing, character, string.Empty)}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }

                        player.Inventory.Remove(playerInv[i]);
                      
                    }
                }
            }
            else
            {
                if (foundThing == null)
                {
                    var foundThingResult = AvsAnLib.AvsAn.Query(thing);
                    string foundThingArticle = foundThingResult.Article;

                    HubContext.Instance.SendToClient($"You don't see {foundThingArticle} {thing} here.", player.HubGuid);
                    return;
                }

                if (foundItem == null)
                {
                    var foundItemResult = AvsAnLib.AvsAn.Query(item);
                    string foundItemResultArticle = foundItemResult.Article;

                    HubContext.Instance.SendToClient($"You are not carrying {foundItemResultArticle} {item}", player.HubGuid);
                    return;
                }

                player.Inventory.Remove(foundItem);
                foundThing.Inventory.Add(foundItem);


                var result = AvsAnLib.AvsAn.Query(foundItem.name);
                string article = result.Article;

                var result2 = AvsAnLib.AvsAn.Query(foundThing.Name);
                string article2 = result2.Article;

                if (foundThing.KnownByName)
                {
                    article2 = String.Empty;
                }

                HubContext.Instance.SendToClient($"You give {article} {foundItem.name} to {article2} {foundThing.Name}.",
                    player.HubGuid);

                HubContext.Instance.SendToClient(
                    $"{Helpers.ReturnName(player, foundThing, string.Empty)} gives {article} {foundItem.name} to you.",
                    foundThing.HubGuid);

                foreach (var character in room.players)
                {
                    if (player.Name != character.Name  && character.Name != foundThing.Name)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} gives {article} {foundItem.name} to {article2} {Helpers.ReturnName(foundThing, character, string.Empty)}.";

                        HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                //check quest

                foreach (var quest in player.QuestLog)
                {
                    if (!quest.Completed && quest.Type == Quest.QuestType.FindItem)
                    {
                        //Find quest requires player to give item to the mob."Horik's Axe "Horik's  Axe"

                        if (quest.QuestGiver == foundThing.Name && quest.QuestItem?.FirstOrDefault(x => x.name.Trim().Equals(foundItem.name.Trim(), StringComparison.CurrentCultureIgnoreCase)) != null)
                        {
                            // player completed quest

                            var mobQuest = foundThing.Quest.FirstOrDefault(x => x.Id.Equals(quest.Id));
                            if (mobQuest != null)
                                HubContext.Instance.SendToClient("<span class='sayColor'>" +
                                    foundThing.Name + " says to you \"" +
                                    mobQuest.RewardDialog.Message.Replace("$playerName", player.Name) + "\"</span>",
                                    player.HubGuid,
                                    null, true);


                            var rewardItem = quest?.RewardItem;

                            if (rewardItem != null)
                            {
                                rewardItem.location = Item.ItemLocation.Inventory;

                                player.Inventory.Add(rewardItem);
                            }



                            if (quest.RewardSkill != null)
                            {
                                player.Skills.Add(quest.RewardSkill);

                                HubContext.Instance.SendToClient(
                                    $"<p class='roomExits'>You have learnt {quest.RewardSkill.Name}!</p>", player.HubGuid);


                            }


                            HubContext.Instance.SendToClient("You gain " + quest.RewardXp + " experience Points", player.HubGuid);
                            HubContext.Instance.SendToClient("You gain " + quest.RewardGold + " gold pieces", player.HubGuid);

                            player.Experience += quest.RewardXp;
                            player.Gold = quest.RewardGold;

                            //check if player level
                            var xp = new Experience();
                            xp.GainLevel(player);
                            //update ui
                            Score.UpdateUiQlog(player);
                            Score.ReturnScoreUI(player);

                            //award player
                        }

                    }
                }
            }

            //save to cache
            Cache.updateRoom(room, currentRoom);
            Cache.updatePlayer(player, currentPlayer);
            Score.UpdateUiInventory(player);
            Score.UpdateUiInventory(foundThing);
 
        }


        /// <summary>
        /// Unlock Item
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void UnlockItem(Room room, Player player, string userInput, string commandKey)
        {
            var currentRoom = room;
            var currentPlayer = player;
            string[] all = userInput.Split();
            // var lockedItem = (KeyValuePair<Item, Item>)FindObject(room, player, commandKey, userInput, FindInventory);
            // var container = lockedItem.Key;
            //  var item = lockedItem.Value;

            var findObject = Events.FindNth.Findnth(userInput);
            int nth = findObject.Key;

            Item foundItem = null;
            Exit foundExit = null;

            foundItem = FindItem.Item(room.items, nth, userInput, Item.ItemLocation.Room);



            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit?.canLock == false)
                {

                    HubContext.Instance.SendToClient("You can't unlock that", player.HubGuid);

                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canLock == false)
                {
                    HubContext.Instance.SendToClient("You can't unlock that", player.HubGuid);

                    return;
                }

                // find key matching chest lock id
                var hasKey = false;

                foreach (var key in player.Inventory)
                {
                    if (key.keyValue != null)
                    {
                        if (key.keyValue == foundItem.keyId)
                        {
                            hasKey = true;
                        }
                    }
                }

                if (hasKey == false)
                {
                    HubContext.Instance.SendToClient("You don't the right key for that", player.HubGuid);

                    return;
                }


                if (foundItem != null)
                {
                    if (foundItem.locked != true)
                    {
                        HubContext.Instance.SendToClient("It's already locked.", player.HubGuid);
                        return;
                    }
                    else
                    {

                        var result = AvsAnLib.AvsAn.Query(foundItem.name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient($"*CLICK* You unlock {article} {foundItem.name}.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"*CLICK* {Helpers.ReturnName(player, character, string.Empty)} unlocks {article} {foundItem.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }


                        foundItem.locked = false;
                        return;
                    }
                }

            }
            else
            {

                // find key matching chest lock id
                var hasKey = false;

                foreach (var key in player.Inventory)
                {
                    if (key.keyValue != null)
                    {
                        if (key.keyValue == foundExit.keyId)
                        {
                            hasKey = true;
                        }
                    }

                }

                if (hasKey == false)
                {

                    HubContext.Instance.SendToClient($"You don't have a key for that.", player.HubGuid);

                    return;
                }


                if (foundExit != null)
                {
                    if (foundExit.locked != true)
                    {
                        HubContext.Instance.SendToClient($"It is already unlocked.", player.HubGuid);

                        return;
                    }
                    else
                    {
                        var result = AvsAnLib.AvsAn.Query(foundExit.name);
                        string article = result.Article;

                        HubContext.Instance.SendToClient($"*CLICK* You unlock {article} {foundExit.name}.", player.HubGuid);

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage =
                                    $"*CLICK* {Helpers.ReturnName(player, character, string.Empty)} unlocks {article} {foundExit.name}.";

                                HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                            }
                        }


                        foundExit.locked = false;
                        return;
                    }
                }
            }

            //save to cache
            Cache.updateRoom(room, currentRoom);

        }

        /// <summary>
        /// lock Item
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void LockItem(Room room, Player player, string userInput, string commandKey)
        {
            var currentRoom = room;
            var currentPlayer = player;
            string[] all = userInput.Split();
            // var lockedItem = (KeyValuePair<Item, Item>)FindObject(room, player, commandKey, userInput, FindInventory);
            // var container = lockedItem.Key;
            //  var item = lockedItem.Value;

            var findObject = Events.FindNth.Findnth(userInput);
            int nth = findObject.Key;

            Item foundItem = null;
            Exit foundExit = null;

            foundItem = FindItem.Item(room.items, nth, userInput, Item.ItemLocation.Room);



            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit.canLock == false)
                {
                    HubContext.Instance.SendToClient("You can't lock that.", player.HubGuid);
                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canLock == false)
                {

                    HubContext.Instance.SendToClient("You can't lock that.", player.HubGuid);
                    return;
                }

                // find key matching chest lock id
                var hasKey = false;

                foreach (var key in player.Inventory)
                {
                    if (key.keyValue != null)
                    {
                        if (key.keyValue == foundItem.keyId)
                        {
                            hasKey = true;
                        }
                    }
                }

                if (hasKey == false)
                {
                    HubContext.Instance.SendToClient("You don't have a key for that.", player.HubGuid);
                    return;
                }

                if (foundItem.locked != true)
                {

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"*CLICK* You lock {article} {foundItem.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"*CLICK* {Helpers.ReturnName(player, character, string.Empty)} locks the {foundItem.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }
                    foundItem.locked = true;

                }
                else
                {

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"The {article} {foundItem.name} is already locked.", player.HubGuid);


                    return;
                }
            }
            else
            {
                // find key matching chest lock id
                var hasKey = false;

                foreach (
                    var key in
                    player.Inventory.Where(key => key.keyValue != null).Where(key => key.keyValue == foundExit.keyId))
                {
                    hasKey = true;
                }

                if (hasKey == false)
                {

                    HubContext.Instance.SendToClient($"You don't have a key for that.", player.HubGuid);


                    return;
                }

                if (foundExit.locked != true)
                {

                    var result = AvsAnLib.AvsAn.Query(foundExit.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"*CLICK* You lock {article} {foundExit.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"*CLICK* {Helpers.ReturnName(player, character, string.Empty)} locks the {foundExit.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    foundExit.locked = true;

                }
                else
                {


                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"The {article} {foundExit.name} is already locked.", player.HubGuid);

                    return;
                }
            }



            //save to cache
            Cache.updateRoom(room, currentRoom);

        }

        /// <summary>
        /// Open
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void Open(Room room, Player player, string userInput, string commandKey)
        {
            var currentRoom = room;

            var findObject = Events.FindNth.Findnth(userInput);
            int nth = findObject.Key;

            Item foundItem = null;
            Exit foundExit = null;

            foundItem = FindItem.Item(room.items, nth, userInput, Item.ItemLocation.Room);



            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit?.canOpen == false)
                {

                    HubContext.Instance.SendToClient("You can't open that.", player.HubGuid);

                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canOpen == false)
                {
                    HubContext.Instance.SendToClient("You can't open that.", player.HubGuid);

                    return;
                }

                if (foundItem.open)
                {
                    HubContext.Instance.SendToClient("It's already open.", player.HubGuid);

                    return;
                }

                if (foundItem.locked)
                {

                    HubContext.Instance.SendToClient("You need to unlock that before you can open it.", player.HubGuid);

                    //Guard check for thiefs..

                    return;
                }


                if (foundItem.open != true)
                {


                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"You open {article} {foundItem.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} opens {article} {foundItem.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }

                    foundItem.open = true;

                }
                else
                {

                    HubContext.Instance.SendToClient($"The {foundItem.name} is already open.", player.HubGuid);


                    return;
                }
            }
            else if (foundExit != null)
            {


                if (foundExit.open != true)
                {

                    if (foundExit.locked)
                    {


                        HubContext.Instance.SendToClient("You need to unlock that before you can open it", player.HubGuid);


                        return;

                    }


                    var result = AvsAnLib.AvsAn.Query(foundExit.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"You open {article} {foundExit.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} opens {article} {foundExit.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }


                    foundExit.open = true;

                }
                else
                {


                    HubContext.Instance.SendToClient($"The {foundExit.name} is already open.", player.HubGuid);


                    return;
                }
            }
            //save to cache
            Cache.updateRoom(room, currentRoom);

        }


        /// <summary>
        /// Close
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void Close(Room room, Player player, string userInput, string commandKey)
        {
            var currentRoom = room;

            var findObject = Events.FindNth.Findnth(userInput);
            int nth = findObject.Key;

            Item foundItem = null;
            Exit foundExit = null;

            foundItem = FindItem.Item(room.items, nth, userInput, Item.ItemLocation.Room);


            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit != null && foundExit.canOpen == false)
                {

                    HubContext.Instance.SendToClient("You can't close that.", player.HubGuid);

                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canOpen == false)
                {

                    HubContext.Instance.SendToClient("You can't close that.", player.HubGuid);

                    return;
                }

                if (foundItem.open == false)
                {

                    HubContext.Instance.SendToClient("It's already closed.", player.HubGuid);

                    return;
                }

                if (foundItem.locked)
                {

                    HubContext.Instance.SendToClient("You need to unlock that before you can close it.", player.HubGuid);

                    return;
                }


                if (foundItem.open)
                {

                    var result = AvsAnLib.AvsAn.Query(foundItem.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"You close {article} {foundItem.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} closes {article} {foundExit.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }



                    foundItem.open = false;

                }
                else
                {

                    HubContext.Instance.SendToClient("It's already closed.", player.HubGuid);

                    return;
                }
            }
            else if (foundExit != null)
            {
                if (foundExit.open != true)
                {

                    if (foundExit.locked)
                    {
                        //dafuq?
                        //TODO: should not be allowed to lock anything that's open without closing it 1st.

                        HubContext.Instance.SendToClient("You need to unlock that before you can close it", player.HubGuid);

                        return;
                    }



                    var result = AvsAnLib.AvsAn.Query(foundExit.name);
                    string article = result.Article;

                    HubContext.Instance.SendToClient($"You close {article} {foundExit.name}.", player.HubGuid);

                    foreach (var character in room.players)
                    {
                        if (player != character)
                        {

                            var roomMessage =
                                $"{Helpers.ReturnName(player, character, string.Empty)} closes {article} {foundExit.name}.";

                            HubContext.Instance.SendToClient(roomMessage, character.HubGuid);
                        }
                    }


                    foundExit.open = false;

                }
                else
                {


                    HubContext.Instance.SendToClient($"{foundExit.name} is already closed.", player.HubGuid);

                    return;
                }

            }
            //save to cache
            Cache.updateRoom(room, currentRoom);

        }

        /// <summary>
        /// Drink command
        /// </summary>
        /// <param name="room">room object</param>
        /// <param name="player">player object</param>
        /// <param name="userInput">text entered by user</param>
        /// <param name="commandKey">command entered</param>
        public static void Drink(Room room, Player player, string userInput, string commandKey)
        {
            var currentRoom = room;

            var findObject = Events.FindNth.Findnth(userInput);
            int nth = findObject.Key;

            Item foundItem = null;
            foundItem = FindItem.Item(room.items, nth, userInput, Item.ItemLocation.Room);


            if (foundItem == null)
            {
                HubContext.Instance.SendToClient("You can't find that to drink.", player.HubGuid);
                return;
            }

            if (foundItem.waterContainer == false)
            {
                HubContext.Instance.SendToClient("You can't drink that.", player.HubGuid);
                return;
            }

            if (foundItem.waterContainerAmount > 0)
            {
                var result = AvsAnLib.AvsAn.Query(foundItem.name);
                string article = result.Article;

                HubContext.Instance.SendToClient(
                    $"You take a gulp of {foundItem.waterContainerLiquid} from {article} {foundItem.name}.",
                    player.HubGuid);

                foundItem.waterContainerAmount--;
            }
            else
            {
                HubContext.Instance.SendToClient($"{foundItem.name} is empty.", player.HubGuid);
                return;
            }

            //save to cache
            Cache.updateRoom(room, currentRoom);
        }
    }
}
