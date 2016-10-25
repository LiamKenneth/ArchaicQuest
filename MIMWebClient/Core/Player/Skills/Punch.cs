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

    public class Punch : Skill
    {
        private static bool _taskRunnning = false;
        public static Skill PunchSkill { get; set; }

        public static void StartPunch(Player attacker, Room room)
        {
 
            //TODO: Fix His to be gender specific
            //TODO: Fist? what if it's a paw?

            if (!_taskRunnning)
            {

                HubContext.SendToClient("You clench your fist and pull your arm back", attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " Pulls his arm back aiming a punch at you.", attacker.HubGuid,
                    attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom(
                    attacker.Name + " clenches his fist and pulls his arm back aiming for " + attacker.Target.Name,
                    room.players, attacker.HubGuid, true);


                Task.Run(() => DoPunch(attacker, room));

            }
            else
            {
                HubContext.SendToClient("You are already trying to punch", attacker.HubGuid);
 
            }

        }

        private static async Task DoPunch(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;

        
             await Task.Delay(5000);

        
            //get attacker strength
            var die = new PlayerStats();
            var dam = die.dice(1, attacker.Strength);
            var toHit = 0.5 * 95; // always 5% chance to miss
            int chance = die.dice(1, 100);


            if (toHit > chance)
            {
                //HIt, but what about defenders ability to block and dodge?

                HubContext.SendToClient("Your punch hits", attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " punch hits you", attacker.HubGuid, attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom( attacker.Name + " punches " + attacker.Target.Name, room.players, attacker.HubGuid, true);

                //find target and hurt them, not yourself!!
                attacker.Target.HitPoints -= dam;
            }
            else
            {
                HubContext.SendToClient("You swing a punch at " + attacker.Target.Name + " but miss", attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " swings a punch at you but misses", attacker.HubGuid, attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom(attacker.Name + " swings at " + attacker.Target.Name + " but misses", room.players, attacker.HubGuid, true);
            }

            _taskRunnning = false;
            attacker.Status = Player.PlayerStatus.Fighting;

        }

        public static Skill PunchAb()
        {

 
                var skill = new Skill
                {
                    Name = "Punch",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Passive = false,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    UsableFromStatus = "Standing",
                    Syntax = "Punch <Target>"
                };


            var help = new Help
            {
                Syntax = "Punch <Victim>",
                HelpText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                           "Duis at pulvinar velit, et eleifend purus." +
                           " Vivamus venenatis lorem eu magna lobortis," +
                           " at cursus felis venenatis. Proin pretium orci vel consequat tempus.",
                DateUpdated = "10/10/2016"

            };

            skill.HelpText = help;


            return  skill;
 
 
        }
    }
}
