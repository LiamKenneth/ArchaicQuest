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

                foreach(var item in ItemContainer.List(inventory.Where(x => x.location == Item.ItemLocation.Inventory), true)){
                    itemList.AppendLine(item);
                }               

                HubContext.Instance.AddNewMessageToPage(player.HubGuid, itemList.ToString());
            }
            else
            {
                HubContext.Instance.AddNewMessageToPage(player.HubGuid, "You are not carrying anything.");
            }
        }
    }
}
