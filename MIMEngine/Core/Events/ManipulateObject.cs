using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Runtime.Remoting.Channels;

    using MIMEngine.Core.Item;
    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    public class ManipulateObject
    {

        public static KeyValuePair<int, string> FindNth(string thingToFind)
        {

            int containsNth = thingToFind.IndexOf('.');
            var itemToFind = thingToFind;
            if (containsNth != -1)
            {
                containsNth = Convert.ToInt32(thingToFind.Substring(0, containsNth));
                itemToFind = thingToFind.Substring(thingToFind.LastIndexOf('.') + 1);
            }

            return new KeyValuePair<int, string>(containsNth, itemToFind);

        }

        /// <summary>
        /// Finds Objects!!
        /// </summary>
        /// <param name="room">the room the player is in</param>
        /// <param name="player">the player calling the method</param>
        /// <param name="command">command used</param>
        /// <param name="thingToFind">Object to find</param>
        /// <param name="objectTypeToFind">can only be item, inventory or all</param>
        /// <returns></returns>
        public static object FindObject(Room room, Player player, string command, string thingToFind, string objectTypeToFind = "")
        {
            /*words to stripout
             * 
             *  from / in / inside / the  ?
            /*
             *  sayto geof good morning
             *  say to geof good morning <- nah
             *  > geof hello
             *  
             *  get 'long sword' / "long sword" / long_sword
             *  get sword -done
             *  get sword chest - done
             *  get 2.dagger -done
             *  
             *  drop potion
             *  give dagger geof
             *  put dagger chest - done
             *  
             *  open north
             *  lock chest
             *  
             * */


            // gets if it;s 2.sword or not and returns the item

            string item = thingToFind;
            //checks for spaces

          
            //get sword bag - text after the 1st space is the container
            int indexOfSpaceInUserInput = item.IndexOf(" ", StringComparison.Ordinal);
            int lastIndexOfSpaceInUserInput = item.LastIndexOf(" ", StringComparison.Ordinal);

            var itemContainer = string.Empty;

            if (indexOfSpaceInUserInput > 0 && lastIndexOfSpaceInUserInput == -1)
            {
                item = thingToFind.Substring(0, indexOfSpaceInUserInput);
            }


            if (lastIndexOfSpaceInUserInput != -1)
            {
                item = thingToFind.Substring(0, indexOfSpaceInUserInput);
                itemContainer = thingToFind.Substring(lastIndexOfSpaceInUserInput).TrimStart();
            }

            //get Item
            var findObject = FindNth(item);
            int nth = findObject.Key;
            string itemToFind = findObject.Value;

            //get container
            KeyValuePair<int, string> findContainer = new KeyValuePair<int, string>();
            int nthContainer = 0;
            string comntainerToFind = string.Empty;
            if (!string.IsNullOrEmpty(itemContainer))
            {
                findContainer = FindNth(itemContainer);
                nthContainer = findContainer.Key;
                comntainerToFind = findContainer.Value;
            }

            Item foundItem = null;
            Mob foundMob = null;
            Player foundPlayer = null;

            List<Item> roomItems = room.items;
            List<Item> playerInv = player.Inventory;
            List<Mob> mobList = room.mobs;
            List<Player> playerList = room.players;

            #region find Item searching Room and Player Inventory
            if (objectTypeToFind == "item")
            {
                // if its not a container
                if (string.IsNullOrEmpty(itemContainer))
                {
                    //search room items 1st
                    foundItem = (nth == -1) ? roomItems.Find(x => x.name.ToLower().Contains(itemToFind))
                                        : roomItems.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundItem != null) { return foundItem; }

                    //search player inv 1st
                    foundItem = (nth == -1) ? playerInv.Find(x => x.name.ToLower().Contains(itemToFind))
                                    : playerInv.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundItem != null)
                    {
                        return foundItem;
                    }
                    else
                    {
                        HubContext.SendToClient("You don't see that item here and you are not carrying such an item", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " rummages around for an item but finds nothing", room.players, player.HubGuid, true);
                    }
                }
                else
                {

                    //look in room
                    foundItem = (nthContainer == -1) ? roomItems.Find(x => x.name.ToLower().Contains(comntainerToFind) && x.actions
                    .container == true)
                                        : roomItems.FindAll(x => x.name.ToLower().Contains(comntainerToFind) && x.actions
                    .container == true).Skip(nthContainer - 1).FirstOrDefault();

                    if (foundItem == null)
                    {
                        //look in inventory
                        foundItem = (nthContainer == -1) ? playerInv.Find(x => x.name.ToLower().Contains(itemToFind) && x.actions
                  .container == true)
                                  : playerInv.FindAll(x => x.name.ToLower().Contains(itemToFind) && x.actions
                  .container == true).Skip(nth - 1).FirstOrDefault();
                    }


                    if (foundItem != null)
                    {
                        //inside found container
                        if (foundItem.containerItems != null)
                        {
                            foundItem = (nth == -1)
                                            ? foundItem.containerItems.Find(x => x.name.ToLower().Contains(itemToFind))
                                            : foundItem.containerItems.FindAll(
                                                x => x.name.ToLower().Contains(itemToFind))
                                                  .Skip(nth - 1)
                                                  .FirstOrDefault();
                        }
                        else
                        {
                            HubContext.SendToClient("You don't see that inside the container", player.HubGuid);
                            HubContext.broadcastToRoom(player.Name + " searches around inside the container but finds nothing", room.players, player.HubGuid, true);

                            return null;
                        }
                    }
                    else
                    {
                        HubContext.SendToClient("You don't see that container here and you are not carrying such an item", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " searches for a container but finds nothing", room.players, player.HubGuid, true);
                        return null;
                    }

                    //return item found in container
                    if (foundItem != null)
                    {
                        return foundItem;
                    }
                    else
                    {
                        HubContext.SendToClient("You don't see that item inside the container", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " searches around inside the container but finds nothing", room.players, player.HubGuid, true);

                    }
                }


            }
            #endregion
            #region find item in player inventory for commands such as drop, equip, wield etc
            else if (objectTypeToFind == "inventory")
            {
                if (string.IsNullOrEmpty(itemContainer))
                {

                    foundItem = (nth == -1) ? playerInv.Find(x => x.name.ToLower().Contains(itemToFind))
                                    : playerInv.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundItem != null)
                    {
                        return foundItem;
                    }
                    else
                    {
                        HubContext.SendToClient("you are not carrying such an item", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " tries to get an item but can't find it.", room.players, player.HubGuid, true);
                    }
                }
                else
                {
                    //look in container
                    foundItem = (nthContainer == -1) ? playerInv.Find(x => x.name.ToLower().Contains(comntainerToFind))
                                        : playerInv.FindAll(x => x.name.ToLower().Contains(comntainerToFind)).Skip(nthContainer - 1).FirstOrDefault();
                    if (foundItem != null)
                    {
                        //inside found container
                        foundItem = (nth == -1)
                                        ? foundItem.containerItems.Find(x => x.name.ToLower().Contains(itemToFind))
                                        : foundItem.containerItems.FindAll(x => x.name.ToLower().Contains(itemToFind))
                                              .Skip(nth - 1)
                                              .FirstOrDefault();
                    }
                    else
                    {
                        HubContext.SendToClient("You are not carrying such an item", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " tries to get an item but can't find it.", room.players, player.HubGuid, true);
                    }

                    //return item found in container
                    if (foundItem != null)
                    {
                        return foundItem;
                    }
                    else
                    {
                        HubContext.SendToClient("You don't see that item inside the container", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " tries to get an item from a container but can't find it.", room.players, player.HubGuid, true);
                    }
                }
            }
            #endregion
            #region find killable mob or player
            else if (objectTypeToFind == "killable")
            {


                //search mob 
                foundMob = (nth == -1) ? mobList.Find(x => x.Name.ToLower().Contains(itemToFind))
                                      : mobList.FindAll(x => x.Name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                if (foundMob != null)
                {
                    return foundMob;
                }
                //search player
                foundPlayer = (nth == -1) ? playerList.Find(x => x.Name.ToLower().Contains(itemToFind))
                                    : playerList.FindAll(x => x.Name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                if (foundPlayer != null)
                {
                    return foundPlayer;
                }
                else
                {
                    HubContext.SendToClient("you don't see " + itemToFind + " here", player.HubGuid);
                    HubContext.broadcastToRoom(player.Name + " tries to kill x but can't find them.", room.players, player.HubGuid, true);
                }


            }
            #endregion
            else if (objectTypeToFind == "all")
            {

                    //general stuff? sayto command? whsper? cast

                    //search room items 1st
                    foundItem = (nth == -1) ? roomItems.Find(x => x.name.ToLower().Contains(itemToFind))
                                    : roomItems.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundItem != null) { return foundItem; }

                    //search player inventory
                    foundItem = (nth == -1) ? playerInv.Find(x => x.name.ToLower().Contains(itemToFind))
                                    : playerInv.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundItem != null)
                    {
                        return foundItem;
                    }


                    //search mob
                    foundMob = (nth == -1) ? mobList.Find(x => x.Name.ToLower().Contains(itemToFind))
                                   : mobList.FindAll(x => x.Name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundMob != null)
                    {
                        return foundMob;
                    }

                    //search players
                    foundPlayer = (nth == -1) ? playerList.Find(x => x.Name.ToLower().Contains(itemToFind))
                                   : playerList.FindAll(x => x.Name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundPlayer != null)
                    {
                        return foundPlayer;
                    }
                    else
                    {
                        HubContext.SendToClient("You don't see anything by that name here", player.HubGuid);
                        HubContext.broadcastToRoom(player.Name + " something something...", room.players, player.HubGuid, true);
                    }

                }
            
            

            return null;
        }

        /// <summary>
        /// Adds item from room to player inventory
        /// </summary>
        /// <param name="room">Room Object</param>
        /// <param name="player">Player Object</param>
        /// <param name="userInput">Text user entered</param>
        public static void GetItem(Room room, Player player, string userInput, string commandKey, string type)
        {
  
            Item foundItem = (Item)FindObject(room, player, commandKey, userInput, type);   

            if (foundItem != null)
            {
                room.items.Remove(foundItem);
                foundItem.location = "Inventory";
                player.Inventory.Add(foundItem);

                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You pick up a " + foundItem.name);
                HubContext.getHubContext.Clients.AllExcept(player.HubGuid).addNewMessageToPage(player.Name + " picks up a " + foundItem.name);

                //save to cache

            }
            else
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("There is no x here to pick up.");
                HubContext.getHubContext.Clients.AllExcept(player.HubGuid).addNewMessageToPage(player.Name + " is looking for a x to pick up.");
            }


        }
    }
}
