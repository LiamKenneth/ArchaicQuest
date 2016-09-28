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
            Player foundMob = null;
            Player foundPlayer = null;

            List<Item> roomItems = room.items;
            List<Item> playerInv = player.Inventory;
            List<Player> mobList = room.mobs;
            List<Player> playerList = room.players;

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

                }
                else
                {

                    //look in room
                    var foundContainer = (nthContainer == -1) ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
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
                Item foundContainer = null;
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
                    var foundContainer = (nthContainer == -1) ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.container == true)
                                          : roomItems.FindAll(x => x.name.ToLower().Contains(comntainerToFind) && x
                      .container == true).Skip(nthContainer - 1).FirstOrDefault();



                    if (foundContainer != null)
                    {
                        //inside found container
                        if (foundContainer.containerItems != null)
                        {


                            var containerItemsCount = foundContainer.containerItems.Count;

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


                                BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You pick up a " + foundContainer.containerItems[i].name, player.Name + " picks up a " + foundContainer.containerItems[i].name);

                                foundContainer.containerItems.Remove(foundContainer.containerItems[i]);
                            }
                        }
                        else
                        {

                            BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You don't see that inside the container", player.Name + " searches around inside the container but finds nothing");


                            return null;
                        }
                    }


                }

            }



            return new KeyValuePair<Item, Item>(null, null);
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

            if (all[0].Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {

                var returnedItem = (KeyValuePair<Item, Item>)FindObject(room, player, commandKey, userInput, type);
                var container = returnedItem.Key;
                var item = returnedItem.Value;


                if (container == null)
                {

                    var roomItems = room.items;
                    var roomItemsCount = roomItems.Count;

                    for (int i = roomItemsCount - 1; i >= 0; i--)
                    {
                        if (roomItems[i].type != Item.ItemType.Gold || roomItems[i].type != Item.ItemType.Silver
                            || roomItems[i].type != Item.ItemType.Copper)
                        {



                            roomItems[i].location = Item.ItemLocation.Inventory;
                            player.Inventory.Add(roomItems[i]);

                        }
                        else
                        {
                            if (roomItems[i].type == Item.ItemType.Gold)
                            {
                                player.Gold += roomItems[i].count;
                            }

                            if (roomItems[i].type == Item.ItemType.Silver)
                            {
                                player.Silver += roomItems[i].count;
                            }

                            if (roomItems[i].type == Item.ItemType.Copper)
                            {
                                player.Copper += roomItems[i].count;
                            }

                        }

                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You pick up a " + roomItems[i].name, player.Name + " picks up a " + roomItems[i].name);

                        room.items.Remove(roomItems[i]);
                    }

                }
                else
                {

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
            }
            else
            {

                KeyValuePair<Item, Item> returnedItem = (KeyValuePair<Item, Item>)FindObject(room, player, commandKey, userInput, type);

                var container = returnedItem.Key;
                var item = returnedItem.Value;

                if (container == null)
                {
                    if (item == null)
                    {
                        return;
                    }

                    room.items.Remove(item);
                    item.location = Item.ItemLocation.Inventory;
                    player.Inventory.Add(item);



                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You pick up a " + item.name, player.Name + " picks up a " + item.name);

                }
                else
                {
                    if (item == null)
                    {
                        return;
                    }


                    container.containerItems.Remove(item);
                    container.location = Item.ItemLocation.Inventory;
                    player.Inventory.Add(item);


                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You get a " + item.name + " from a " + container.name, player.Name + " gets a " + item.name + " from a " + container.name);

                }


                //save to cache
                Cache.updateRoom(room, currentRoom);
                Cache.updatePlayer(player, currentPlayer);

            }

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
                        room.items.Add(playerInv[i]);

                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You drop a " + playerInv[i].name, player.Name + " drops a " + playerInv[i].name);
                    }
                    else
                    {
                        container.containerItems.Add(playerInv[i]);

                        BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You drop a " + playerInv[i].name + " into a " + container.name, player.Name + " drops a " + playerInv[i].name + " into a " + container.name);
                    }

                    player.Inventory.Remove(playerInv[i]);
                }
            }
            else
            {
                if (item == null)
                {
                    return;
                }

                player.Inventory.Remove(item);
                item.location = Item.ItemLocation.Room;

                if (container == null)
                {
                    room.items.Add(item);

                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You drop  a " + item.name, player.Name + " drops  a " + item.name);
                }
                else
                {
                    container.containerItems.Add(item);

                    BroadcastPlayerAction.BroadcastPlayerActions(player.HubGuid, player.Name, room.players, "You put a " + item.name + " inside the " + container.name, player.Name + " puts a " + item.name + " inside the " + container.name);
                }

                //save to cache
                Cache.updateRoom(room, currentRoom);
                Cache.updatePlayer(player, currentPlayer);
            }
        }
    }
}
