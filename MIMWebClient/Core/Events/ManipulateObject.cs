using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static object FindObject(Room room, Player player, string command, string thingToFind, string objectTypeToFind = "")
        {

            if (thingToFind == "all" && objectTypeToFind == "all")
            {
              return  new KeyValuePair<Item, Item>(null, null);
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
                    foundItem = FindItem.Item(room.items, nth, itemToFind);

                    if (foundItem != null) { return new KeyValuePair<Item, Item>(null, foundItem); }


                    string msgToPlayer = "You don't see " + AvsAnLib.AvsAn.Query(itemToFind) + " " + itemToFind + " here and you are not carrying " + AvsAnLib.AvsAn.Query(itemToFind) + " " + itemToFind;
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, msgToPlayer, player.Name + " rummages around for an item but finds nothing");
                    return new KeyValuePair<Item, Item>(null, null);
                }
                else
                {

                    //look in room
                     foundContainer = (nthContainer == -1) ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
                                          : roomItems.FindAll(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true).Skip(nthContainer - 1).FirstOrDefault();


                    if (foundContainer != null)
                    {

                        foundItem = FindItem.Item(foundContainer.containerItems, nth, itemToFind);

                    }

                    //return item found in container
                    if (foundItem != null || itemToFind.Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                    }
                    else
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't see that inside the container", player.Name + " searches around inside the container but finds nothing");
                    }


                }



            }
            else if (itemToFind == "all" && roomItems.Count == 0 && command == "get")
            {
                HubContext.SendToClient("There is nothing here to get", player.HubGuid);
            }
            else if (itemToFind == "all" && playerInv.Count == 0 && command == "drop" || itemToFind == "all" && playerInv.Count == 0 && command == "put")
            {
                HubContext.SendToClient("You have nothing to drop", player.HubGuid);
            }

            #endregion
            #region find item in player inventory for commands such as drop, equip, wield etc
            else if (objectTypeToFind == FindInventory)
            {
              
                if (string.IsNullOrEmpty(itemContainer))
                {

                    foundItem = FindItem.Item(player.Inventory, nth, itemToFind);



                    if (foundItem != null || itemToFind.Equals("all", StringComparison.OrdinalIgnoreCase))
                    {
                        return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                    }
                    else
                    {

                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "you are not carrying such an item", player.Name + " tries to get an item but can't find it.");

                    }
                }
                else
                {
                    //look in inv
                    if (itemToFind != "all")
                    {
                        foundItem = FindItem.Item(player.Inventory, nth, itemToFind);
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
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "you are not carrying such an item", player.Name + " tries to get an item but can't find it.");

                    }

                    //return item found in container
                    if (foundItem != null || foundContainer != null)
                    {
                        return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                    }
                    else
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't see that item inside the container", player.Name + " tries to get an item from a container but can't find it.");

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

                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "you don't see " + itemToFind + " here", player.Name + " tries to kill x but can't find them.");

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
                     foundContainer = (nthContainer == -1) ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
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
                                //TODO: Get all should not come here
                                BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "The " + foundContainer.name + " is empty", player.Name + " looks in " + foundContainer.name + " but finds nothing");

                                return new KeyValuePair<Item, Item>(foundContainer, foundItem);
                            }

                            for (int i = containerItemsCount - 1; i >= 0; i--)
                            {
                                if (foundContainer.containerItems[i].type != Item.ItemType.Gold || foundContainer.containerItems[i].type != Item.ItemType.Silver
                                    || foundContainer.containerItems[i].type != Item.ItemType.Copper)
                                {



                                    foundContainer.containerItems[i].location = Item.ItemLocation.Inventory;
                                    player.Inventory.Add(foundContainer.containerItems[i]);

                                }
                                else
                                {
                                    if (foundContainer.containerItems[i].type == Item.ItemType.Gold)
                                    {
                                        player.Gold += foundContainer.containerItems[i].count;
                                    }

                                    if (foundContainer.containerItems[i].type == Item.ItemType.Silver)
                                    {
                                        player.Silver += foundContainer.containerItems[i].count;
                                    }

                                    if (foundContainer.containerItems[i].type == Item.ItemType.Copper)
                                    {
                                        player.Copper += foundContainer.containerItems[i].count;
                                    }

                                }


                                BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You pick up a " + foundContainer.containerItems[i].name + " from a " + foundContainer.name, player.Name + " picks up a " + foundContainer.containerItems[i].name + " from a " + foundContainer.name);

                                foundContainer.containerItems.Remove(foundContainer.containerItems[i]);
                            }
                        }
                        else
                        {

                            BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't see that inside the container", player.Name + " searches around inside the container but finds nothing");
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
                        roomItems[i].location = Item.ItemLocation.Inventory;
                        player.Inventory.Add(roomItems[i]);

                        BroadcastPlayerAction.BroadcastPlayerActions(
                            player.HubGuid,
                            player.Name,
                            room.players,
                            "You pick up a " + roomItems[i].name,
                            player.Name + " picks up a " + roomItems[i].name);
                        room.items.Remove(roomItems[i]);
                    }
                    else
                    {
                        HubContext.SendToClient("You can't take that", player.HubGuid);
                    }
                }


            }
            else if (all[0].Equals("all", StringComparison.InvariantCultureIgnoreCase) && container != null)
            {

                if (container.locked == true)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "Container is locked", player.Name + " container is locked");

                    return;
                }
                //get all from container
                var containerItems = container.containerItems;
                var containerCount = containerItems.Count;

                for (int i = containerCount - 1; i >= 0; i--)
                {
                    containerItems[i].location = Item.ItemLocation.Inventory;
                    player.Inventory.Add(containerItems[i]);

                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You get a " + containerItems[i].name + " from a " + container.name, player.Name + " get a " + containerItems[i].name + " from a " + container.name);

                    containerItems.Remove(containerItems[i]);
                }


            }

            else if (item != null)
            {
                //get single Item       

                if (container == null)
                {

                    if (!item.stuck)
                    {
                        room.items.Remove(item);
                        item.location = Item.ItemLocation.Inventory;
                        player.Inventory.Add(item);


                        BroadcastPlayerAction.BroadcastPlayerActions(
                            player.HubGuid,
                            player.Name,
                            room.players,
                            "You pick up a " + item.name,
                            player.Name + " picks up a " + item.name);
                    }
                    else
                    {
                        HubContext.SendToClient("You can't take that", player.HubGuid);
                    }

                }
                else
                {
                    if (container.locked == true)
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "Container is locked", player.Name + " container is locked");

                        return;
                    }

                    //Get item from container
                    container.containerItems.Remove(item);
                    container.location = Item.ItemLocation.Inventory;
                    player.Inventory.Add(item);


                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You get a " + item.name + " from a " + container.name, player.Name + " gets a " + item.name + " from a " + container.name);

                }


            }
            else
            {
                BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "There is nothing here to pick up", player.Name + " searches for something to pick up");
                return;
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
                var playerInv = player.Inventory;
                var playerInvCount = player.Inventory.Count;

                for (int i = playerInvCount - 1; i >= 0; i--)
                {
                    playerInv[i].location = Item.ItemLocation.Room;


                    if (container == null)
                    {
                        playerInv[i].location = Item.ItemLocation.Room;
                        playerInv[i].isHiddenInRoom = false;
                       room.items.Add(playerInv[i]);
                        

                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You drop a " + playerInv[i].name, player.Name + " drops a " + playerInv[i].name);

                        player.Inventory.Remove(playerInv[i]);
                    }
                    else
                    {

                        if (container.locked == true)
                        {
                            BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "Container is locked", player.Name + " container is locked");

                            return;
                        }

                        if (container.open == false)
                        {
                            HubContext.SendToClient("You have to open the " + container.name + " before you can put something inside", player.HubGuid);
                            return;
                        }

                        container.containerItems.Add(playerInv[i]);

                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You drop a " + playerInv[i].name + " into a " + container.name, player.Name + " drops a " + playerInv[i].name + " into a " + container.name);

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

                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You drop  a " + item.name, player.Name + " drops  a " + item.name);
                }
                else
                {

                    if (container.locked == true)
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "Container is locked", player.Name + " container is locked");

                        return;
                    }

                    if (container.open == false)
                    {
                        HubContext.SendToClient("You have to open the " + container.name + " before you can put something inside", player.HubGuid);
                        return;
                    }

                    player.Inventory.Remove(item);
                    item.location = Item.ItemLocation.Room;
                    container.containerItems.Add(item);

                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You put a " + item.name + " inside the " + container.name, player.Name + " puts a " + item.name + " inside the " + container.name);
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

            foundItem = FindItem.Item(room.items, nth, userInput);

           

            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit.canLock == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't lock that", player.Name + " tries to lock the " + foundItem.name);
                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canLock == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't lock that", player.Name + " tries to lock the " + foundItem.name);
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
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't have a key for that", player.Name + " tries to unlock the " + foundItem.name + " without the key");
                    return;
                }


                if (foundItem != null)
                {
                    if (foundItem.locked != true)
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "IT is already unlocked", player.Name + " tries to unlock the..." + foundItem.name);
                        return;
                    }
                    else
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You unlock the " + foundItem.name, player.Name + " unlocks the..." + foundItem.name);
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
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't have a key for that", player.Name + " tries to unlock the " + foundExit.name + " without the key");
                    return;
                }


                if (foundExit != null)
                {
                    if (foundExit.locked != true)
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "IT is already unlocked", player.Name + " tries to unlock the..." + foundExit.name);
                        return;
                    }
                    else
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You unlock " + foundExit.name, player.Name + " unlocks the..." + foundExit.name);
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

            foundItem = FindItem.Item(room.items, nth, userInput);

          

            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit.canLock == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't lock that", player.Name + " tries to lock the " + foundItem.name);
                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canLock == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't lock that", player.Name + " tries to lock the " + foundItem.name);
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
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't have a key for that", player.Name + " tries to lock the " + foundItem.name + " without the key");
                    return;
                }

                if (foundItem.locked != true)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You lock the chest", player.Name + " locks the " + foundItem.name);
                    foundItem.locked = true;
                     
                }
                else
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "the chest is already locked  " + foundItem.name, player.Name + " tries to locked the already locked chest" + foundItem.name);

                    return;
                }
            }
            else
            {
                // find key matching chest lock id
                var hasKey = false;

                foreach (var key in player.Inventory.Where(key => key.keyValue != null).Where(key => key.keyValue == foundExit.keyId))
                {
                    hasKey = true;
                }

                if (hasKey == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't have a key for that", player.Name + " tries to lock the " + foundExit.name + " without the key");
                    return;
                }

                if (foundExit.locked != true)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You lock the chest", player.Name + " locks the " + foundExit.name);
                    foundExit.locked = true;
                    
                }
                else
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "the chest is already locked  " + foundExit.name, player.Name + " tries to locked the already locked chest" + foundExit.name);

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

            foundItem = FindItem.Item(room.items, nth, userInput);



            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit.canOpen == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't open that", player.Name + " tries to open the " + foundExit.name);
                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canOpen == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't open that", player.Name + " tries to open the " + foundItem.name);
                    return;
                }

                if (foundItem.open)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "It's already open", player.Name + " tries to open the " + foundItem.name + "but it's already open");
                    return;
                }

                if (foundItem.locked)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You need to unlock that before you can open it", player.Name + " tries to open the " + foundItem.name + "without a key");
                    return;
                }


                if (foundItem.open != true)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "You open the " + foundItem.name,
                        player.Name + " open the " + foundItem.name);
                    foundItem.open = true;

                }
                else
                {

                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "the" + foundItem.name + " is already open",
                        player.Name + " tries to open the already open chest" + foundItem.name);

                    return;
                }
            }
            else if (foundExit != null)
            {


                if (foundExit.open != true)
                {

                    if (foundExit.locked)
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(
                            player.HubGuid,
                            player.Name,
                            room.players,
                            "You need to unlock that before you can open it",
                            player.Name + " tries to open the " + foundExit.name + "without a key");
                        return;
                    }

                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "You open the " + foundExit.name,
                        player.Name + " open the " + foundExit.name);
                    foundExit.open = true;

                }
                else
                {
                   
                        BroadcastPlayerAction.BroadcastPlayerActions(
                            player.HubGuid,
                            player.Name,
                            room.players,
                            "the " + foundExit.name + "is already open ",
                            player.Name + " tries to open the already open chest" + foundExit.name);
                    
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

            foundItem = FindItem.Item(room.items, nth, userInput);


            if (foundItem == null)
            {
                foundExit = FindItem.Exit(room.exits, nth, userInput);

                if (foundExit.canOpen == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't close that", player.Name + " tries to close the " + foundExit.name);
                    return;
                }
            }

            if (foundItem != null)
            {

                if (foundItem.canOpen == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You can't close that", player.Name + " tries to close the " + foundItem.name);
                    return;
                }

                if (foundItem.open == false)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "It's already closed", player.Name + " tries to close the " + foundItem.name + " which is already closed");
                    return;
                }

                if (foundItem.locked)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You need to unlock that before you can close it", player.Name + " tries to close the " + foundItem.name + "without a key");
                    return;
                }


                if (foundItem.open)
                {
                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "You close the chest",
                        player.Name + " close the " + foundItem.name);
                    foundItem.open = false;

                }
                else
                {

                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "the " + foundItem.name + " is already close  " + foundItem.name,
                        player.Name + " tries to close the already close chest" + foundItem.name);

                    return;
                }
            }
            else if (foundExit != null)
            {
                if (foundExit.open != true)
                {

                    if (foundExit.locked)
                    {
                        BroadcastPlayerAction.BroadcastPlayerActions(
                            player.HubGuid,
                            player.Name,
                            room.players,
                            "You need to unlock that before you can close it",
                            player.Name + " tries to close the " + foundExit.name + "without a key");
                        return;
                    }

                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "You close " + foundExit.name,
                        player.Name + " closes the " + foundExit.name);
                    foundExit.open = false;

                }
                else
                {

                    BroadcastPlayerAction.BroadcastPlayerActions(
                        player.HubGuid,
                        player.Name,
                        room.players,
                        "the " + foundExit.name + " is already closed  " + foundExit.name,
                        player.Name + " tries to close the already closed chest" + foundExit.name);
                    return;
                }

            }
            //save to cache
            Cache.updateRoom(room, currentRoom);

        }
    
}
}
