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
                //hardcode 50% flee success rate

                if (Helpers.Rand(1, 100) >= 50)
                {

                    HubContext.Instance.SendToClient("You Flee", player.HubGuid);

                    var exit = Helpers.diceRoll.Next(room.exits.Count);

                    player.Status = PlayerSetup.Player.PlayerStatus.Standing;

                    HubContext.Instance.SendToClient(player.Name + " Flee's from combat", player.Target.HubGuid);

                    player.Target = null;
                    player.ActiveFighting = false;

                    Room.Movement.Move(player, room, room.exits[exit].name);

                }
                else
                {
                    HubContext.Instance.SendToClient("You fail to flee", player.HubGuid);
                }
            }
            else
            {
                HubContext.Instance.SendToClient("Flee from whom, you're not fighting anyone", player.HubGuid);
            }
        }
    }
}
