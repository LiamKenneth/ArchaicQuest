using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Player
{
    using MIMEngine.Core.Item;

    public class Inventory
    {

        public static void ReturnInventory(List<Item> inventory)
        {
            if (inventory != null)
            {
                int inventoryCount = inventory.Count;
                var inventoryItems = new StringBuilder();
                for (int i = 0; i < inventoryCount; i++)
                {
                    inventoryItems.Append(inventory[i].name).AppendLine();
                }

                HubContext.getHubContext.Clients.All.addNewMessageToPage(inventoryItems.ToString());
            }
            else
            {
                HubContext.getHubContext.Clients.All.addNewMessageToPage("You are not carrying anything.");
            }
        }
    }
}
