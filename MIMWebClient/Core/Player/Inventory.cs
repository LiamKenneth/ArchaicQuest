using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player
{
    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.PlayerSetup;

    public class Inventory
    {

        public static void ReturnInventory(List<Item> inventory, Player player)
        {
            if (inventory != null)
            {
                int inventoryCount = inventory.Count(i => i.location.Equals(Item.ItemLocation.Inventory));
                var inventoryItems = new StringBuilder();;
                inventoryItems.Append("You are carrying:").AppendLine();
                for (int i = 0; i < inventoryCount; i++)
                {
                    //should group items with same name?
                    inventoryItems.Append(inventory[i].name).AppendLine();
                }

                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(inventoryItems.ToString());
            }
            else
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You are not carrying anything.");
            }
        }
    }
}
