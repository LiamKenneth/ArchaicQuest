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

        public static void ReturnInventory(ItemContainer inventory, Player player)
        {
            if (inventory != null && inventory.Count > 0)
            {

                var itemList = new StringBuilder();
                itemList.Append("You are carrying:").AppendLine();

                foreach(var item in inventory.List((x => x.location == Item.ItemLocation.Inventory))){
                    itemList.AppendLine(item);
                }               

                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(itemList.ToString());
            }
            else
            {
                HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage("You are not carrying anything.");
            }
        }
    }
}
