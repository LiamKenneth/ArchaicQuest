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
            int nthItem = 0;
            var itemToFind = string.Empty;
            if (containsNth != -1)
            {
                nthItem = Convert.ToInt32(thingToFind.Substring(0, containsNth));
                itemToFind = thingToFind.Substring(thingToFind.LastIndexOf('.') + 1);
            }

            return new KeyValuePair<int, string>(nthItem,
       itemToFind);

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

            //int n = -1;
            //string item = string.Empty;

            //if (commandOptions.IndexOf('.') != -1)
            //{
            //    n = Convert.ToInt32(commandOptions.Substring(0, commandOptions.IndexOf('.')));
            //    item = commandOptions.Substring(commandOptions.LastIndexOf('.') + 1);
            //}


            //var roomDescription = roomData.keywords.Find(x => x.name.ToLower().Contains(commandOptions));

            //var itemDescription = (n == -1)
            //                          ? roomData.items.Find(x => x.name.ToLower().Contains(commandOptions))
            //                          : roomData.items.FindAll(x => x.name.ToLower().Contains(item))
            //                                .Skip(n - 1)
            //                                .FirstOrDefault();

            // gets if it;s 2.sword or not and returns the item
            var findNth = FindNth(thingToFind);
            int nth = findNth.Key;
            string itemToFind = findNth.Value;

            object found = null;
            if (objectTypeToFind == "player")
            {
                found = room.players.FirstOrDefault(x => x.Name.StartsWith(thingToFind));
            }

            var roomItems = room.items;
            Item roomContainer = null;
            int indexOfSpaceInUserInput = userInput.IndexOf(" ");
            int lastIndexOfSpaceInUserInput = userInput.LastIndexOf(" ");
            itemToFind = string.Empty;
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

            return x;
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
