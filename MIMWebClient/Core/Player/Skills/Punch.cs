using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Player.Skills
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Punch
    {
        public static void StartPunch(Player attacker, Room room)
        {

            //Fight2 needs refactoring so the below skills can use the same methods for damage, damage output, working out chance to hit etc. same for spells

           

            HubContext.SendToClient("You clench your fist and pull your arm back", attacker.HubGuid);
            HubContext.SendToClient(attacker.Name + " Pulls his arm back aiming a punch at you.", attacker.HubGuid, attacker.Target.HubGuid, false, true);
            HubContext.broadcastToRoom(attacker.Name + " clenches his fist and pulls his arm back aiming for " + attacker.Target.Name, room.players, attacker.HubGuid, true);

            Task.Run(() => DoPunch(attacker, room));

        }

        public static async Task DoPunch(Player attacker, Room room)
        {
            attacker.Status = Player.PlayerStatus.Busy;

            await Task.Delay(1500);

        
            //get attacker strength
            var die = new PlayerStats();
            var dam = die.dice(1, attacker.Strength);
            var toHit = 0.5 * 95; // always 5% chance to miss
            int chance = die.dice(1, 100);


            if (toHit > chance)
            {
                HubContext.SendToClient("Your punch hits", attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " punch hits you", attacker.HubGuid,
                    attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom(
                    attacker.Name + " punches " + attacker.Target.Name,
                    room.players, attacker.HubGuid, true);

                attacker.Target.HitPoints -= dam;
            }
            else
            {
                HubContext.SendToClient("You swing a punch at " + attacker.Target.Name + " but miss", attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " swings a punch at you but misses", attacker.HubGuid, attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom(attacker.Name + " swings at " + attacker.Target.Name + " but misses", room.players, attacker.HubGuid, true);
            }

            attacker.Status = Player.PlayerStatus.Fighting;
            
        }
    }
}
