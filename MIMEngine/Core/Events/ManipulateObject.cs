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


        public static object FindObject(Room room, Player player, string command, string thingToFind, string objectTypeToFind = "")
        {
            /*
             *  sayto geof good morning
             *  say to geof good morning <- nah
             *  > geof hello
             *  get 'long sword'
             *  get sword
             *  get sword chest
             *  get 2.dagger
             *  
             *  drop potion
             *  give dagger geof
             *  put dagger chest
             *  
             *  open north
             *  lock chest
             *  
             * */


            // gets if it;s 2.sword or not and returns the item

            //checks for spaces
            //get sword bag - text after the 1st space is the container
            int indexOfSpaceInUserInput = thingToFind.IndexOf(" ");
            int lastIndexOfSpaceInUserInput = thingToFind.LastIndexOf(" ");
            string item = string.Empty;
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
            if (itemContainer != null)
            {
                 findContainer = FindNth(itemContainer);
                 nthContainer = findContainer.Key;
                 comntainerToFind = findContainer.Value;
            }

            Item foundItem = null;

            List<Item> roomItems = room.items;
            List<Item> playerInv = player.Inventory;

            if (objectTypeToFind == "item")
            {
                if (itemContainer != null)
                {
                    //search room items 1st
                    foundItem = (nth == -1) ? roomItems.Find(x => x.name.ToLower().Contains(itemToFind))
                                        : roomItems.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                    if (foundItem != null) { return foundItem; }

                    if (foundItem == null)
                    {
                        foundItem = (nth == -1) ? playerInv.Find(x => x.name.ToLower().Contains(itemToFind))
                                          : playerInv.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                        if (foundItem != null)
                        {
                            return foundItem;
                        }
                        else
                        {
                            HubContext.SendToClient("You don't see that item here and you are not carrying such an item", player.HubGuid);
                        }


                    }
                }
                else
                {
                    //look in container

                }
            }
            else if (objectTypeToFind == "inventory")
            {
                foundItem = (nth == -1) ? playerInv.Find(x => x.name.ToLower().Contains(itemToFind))
                                        : playerInv.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(nth - 1).FirstOrDefault();

                if (foundItem != null)
                {
                    return foundItem;
                }
                else
                {
                    HubContext.SendToClient("You are not carrying a " + itemToFind, player.HubGuid);
                }
            }
       




            //var item = (nth == -1) ? roomData.items.Find(x => x.name.ToLower().Contains(commandOptions))
            //                           : roomData.items.FindAll(x => x.name.ToLower().Contains(item))
            //                                 .Skip(n - 1)
            //                                 .FirstOrDefault();




            //var roomItems = room.items;
            //Item roomContainer = null;
            //int indexOfSpaceInUserInput = userInput.IndexOf(" ");
            //int lastIndexOfSpaceInUserInput = userInput.LastIndexOf(" ");
            //itemToFind = string.Empty;
            //var itemContainer = string.Empty;

            //if (indexOfSpaceInUserInput > 0 && lastIndexOfSpaceInUserInput == -1)
            //{
            //    itemToFind = userInput.Substring(0, indexOfSpaceInUserInput);
            //}
            //else
            //{
            //    itemToFind = userInput;
            //}

            //if (lastIndexOfSpaceInUserInput != -1)
            //{
            //    itemToFind = userInput.Substring(0, indexOfSpaceInUserInput);
            //    itemContainer = userInput.Substring(lastIndexOfSpaceInUserInput).TrimStart();
            //}

            return null;
        }

    /// <summary>
    /// Adds item from room to player inventory
    /// </summary>
    /// <param name="room">Room Object</param>
    /// <param name="player">Player Object</param>
    /// <param name="userInput">Text user entered</param>
    public static void GetItem(Room room, Player player, string userInput, string commandKey)
    {
        //no check for 2.sword

        var foundObject = FindObject(room, player, userInput, commandKey);

        var roomItems = room.items;
        Item roomContainer = null;
        int indexOfSpaceInUserInput = userInput.IndexOf(" ");
        int lastIndexOfSpaceInUserInput = userInput.LastIndexOf(" ");
        string itemToFind = string.Empty;
        var itemContainer = string.Empty;

        if (indexOfSpaceInUserInput > 0 && lastIndexOfSpaceInUserInput == -1)
        {
            itemToFind = userInput.Substring(0, indexOfSpaceInUserInput);
        }
        else
        {
            itemToFind = userInput;
        }

        if (lastIndexOfSpaceInUserInput != -1)
        {
            itemToFind = userInput.Substring(0, indexOfSpaceInUserInput);
            itemContainer = userInput.Substring(lastIndexOfSpaceInUserInput).TrimStart();
        }

        Item item = null;

        if (string.IsNullOrEmpty(itemContainer))
        {
            item = roomItems.FirstOrDefault(x => x.name.ToLower().Contains(itemToFind));
        }
        else
        {
            roomContainer = roomItems.FirstOrDefault(x => x.name == itemToFind && x.actions.container == true);

        }

        if (item != null)
        {
            room.items.Remove(item);
            item.location = "Inventory";
            player.Inventory.Add(item);

            HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You pick up a " + item.name);
            HubContext.getHubContext.Clients.AllExcept(player.HubGuid).addNewMessageToPage(player.Name + " picks up a " + item.name);

            //save to cache

        }
        else
        {
            HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("There is no " + itemToFind + " here to pick up.");
            HubContext.getHubContext.Clients.AllExcept(player.HubGuid).addNewMessageToPage(player.Name + " is looking for a " + itemToFind + " to pick up.");
        }


    }
}
}
