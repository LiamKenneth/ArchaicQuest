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

    public class Kick : Skill
    {
        private static bool _taskRunnning = false;
        public static Skill KickSkill { get; set; }

        public static void StartKick(Player attacker, Room room)
        {

            //TODO: Fix His to be gender specific
            //TODO: Fist? what if it's a paw?

            if (!_taskRunnning && attacker.Target != null)
            {
                // find target if not in fight
                HubContext.SendToClient("You pull your leg back", attacker.HubGuid);
                HubContext.SendToClient(Helpers.ReturnName(attacker, null) + " pulls " + Helpers.ReturnHisOrHers(attacker.Gender) + " leg back ready to kick at you.", attacker.HubGuid,
                    attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom(
                    Helpers.ReturnName(attacker, null) + " pulls " + Helpers.ReturnHisOrHers(attacker.Gender) +" leg back ready to kick at " + Helpers.ReturnName(attacker.Target, null),
                    room.players, attacker.HubGuid, true);

                Task.Run(() => DoKick(attacker, room));

            }
            else
            {
                if (attacker.Target == null)
                {
                    HubContext.SendToClient("You stop your kick", attacker.HubGuid);
                    return;
                }

                HubContext.SendToClient("You are already trying to kick", attacker.HubGuid);

            }

        }

        private static async Task DoKick(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(2000);


            //get attacker strength
            var die = new PlayerStats();
            var dam = die.dice(1, attacker.Strength);
            var toHit = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals("Kick", StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss
            int chance = die.dice(1, 100);

            Fight2.ShowAttack(attacker, attacker.Target, room, toHit, chance, KickAb(), dam);


            _taskRunnning = false;
            attacker.Status = Player.PlayerStatus.Fighting;

        }

        public static Skill KickAb()
        {


            var skill = new Skill
            {
                Name = "Kick",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "Kick <Target>"
            };


            var help = new Help
            {
                Syntax = "Kick <Victim>",
                HelpText = "Kick can be used to start a fight or during a fight, " +
                           "you can only kick your primary target." +
                           " During combat only kick is needed to kick your target to inflict damage",
                DateUpdated = "18/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
