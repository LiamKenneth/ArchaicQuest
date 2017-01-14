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
               
                var inventoryItems = new StringBuilder();;
                inventoryItems.Append("You are carrying:").AppendLine();

                foreach (var item in player.Inventory.Where(x => x.location == Item.ItemLocation.Inventory))
                {
                   
                    inventoryItems.Append(item.name).AppendLine();
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
