using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Events
{
    public class Flee
    {
        enum Exits
        {
            North = 1,
            East = 2,
            South = 3,
            West = 4
        };
        public static void fleeCombat(PlayerSetup.Player player, Room.Room room)
        {
            if (player.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                HubContext.SendToClient("You Flee", player.HubGuid);

               //var exit = room.exits.Find(x => x.name.Equals("north", StringComparison.InvariantCultureIgnoreCase));

                player.Status = PlayerSetup.Player.PlayerStatus.Standing;
                HubContext.SendToClient(player.Name + " Flee's from combat", player.Target.HubGuid);

                Room.Movement.Move(player, room, "North");
            }
            else
            {
                HubContext.SendToClient("Flee from whom, you're not fighting anyone", player.HubGuid);
            }
        }
    }
}
