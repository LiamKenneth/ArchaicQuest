using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class Harvest
    {
        public static void Body(PlayerSetup.Player player, Room.Room room, string commandOptions)
        {
            if (string.IsNullOrEmpty(commandOptions))
            {

                return;
            }

            var foundCorpse = room.items.FirstOrDefault(x => x.name.ToLower().Contains(commandOptions.ToLower()));
            var foundCorpseMob = room.mobs.FirstOrDefault(x => x.Name.ToLower().Contains(foundCorpse.name.ToLower().Replace("the corpse of ", "")));

            if (foundCorpse == null)
            {
                HubContext.Instance.SendToClient("No corpse found.", player.HubGuid);
                return;
            }
            var carveMessage = "You carve " +  foundCorpse.name.ToLower() + " retrieving ";
            var bodyPart = GetBodyPart();

            HubContext.Instance.SendToClient(carveMessage + Helpers.ReturnName(null, null, bodyPart).ToLower() + ".", player.HubGuid);

            if (bodyPart == "nothing")
            {
                return;
            }

            player.Inventory.Add(new Item.Item()
            {
                name = foundCorpseMob.Name + " " + bodyPart,
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Held,
                eqSlot = Item.Item.EqSlot.Held,
                equipable = true,

            });

            Score.UpdateUiInventory(player);

            if (room.items.FirstOrDefault(x => x.name.ToLower().Contains(commandOptions)) != null)
            {
                room.items.Remove(foundCorpse);
            }
           
        }

        public static string GetBodyPart()
        {
            var rand = Helpers.Rand(1, 5);

            switch (rand)
            {
                case 1:
                    return  "eyeball";
                case 2:
                    return "ear";
                case 3:
                    return "tooth";
                case 4:
                    return "rib bone";
 

            }
            return "nothing";

        }
    }
}