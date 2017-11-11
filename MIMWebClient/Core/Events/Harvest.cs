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
            if (foundCorpse == null)
            {
                HubContext.Instance.SendToClient("No corpse found.", player.HubGuid);
                return;
            }

            var corpseName = foundCorpse.name.ToLower();
            var corpse = string.Empty;

            if (corpseName.Contains("decayed"))
            {
                corpse = corpseName.Replace("the decayed corpse of ", "");
            }
            else if (corpseName.Contains("rotting"))
            {
                corpse = corpseName.Replace("the rotting corpse of ", "");
            }
            else
            {
                corpse = corpseName.Replace("the corpse of ", "");
            }
          

            var carveMessage = "You carve " + corpse + " retrieving ";
            var bodyPart = GetBodyPart();

            HubContext.Instance.SendToClient(carveMessage + Helpers.ReturnName(null, null, bodyPart).ToLower() + ".", player.HubGuid);

            if (bodyPart == "nothing")
            {
                return;
            }

            if (foundCorpse.KnownByName)
            {
                
            }

            var bodybit = new Item.Item()
            {
                name = corpse + " " + bodyPart,
                location = Item.Item.ItemLocation.Inventory,
                slot = Item.Item.EqSlot.Held,
                eqSlot = Item.Item.EqSlot.Held,
                equipable = true,

            };

            if (foundCorpse.KnownByName)
            {
                bodybit.KnownByName = true;
            }

            player.Inventory.Add(bodybit);

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
                    return "eyeball";
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