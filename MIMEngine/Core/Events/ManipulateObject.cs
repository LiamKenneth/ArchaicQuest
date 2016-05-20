using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using MIMEngine.Core.Item;
    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    public class ManipulateObject
    {
        /// <summary>
        /// Adds item from room to player inventory
        /// </summary>
        /// <param name="room">Room Object</param>
        /// <param name="player">Player Object</param>
        /// <param name="userInput">Text user entered</param>
        public static void GetItem(Room room, Player player, string userInput)
        {
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
