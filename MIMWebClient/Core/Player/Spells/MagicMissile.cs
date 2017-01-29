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

    public class MagicMissile : Skill
    {
        private static bool _taskRunnning = false;
        public static Skill MagicMissilekill { get; set; }

        public static void StartMagicMissile(Player attacker, Room room)
        {

            var spell =
               attacker.Skills.FirstOrDefault(
                   x =>
                       x.Name.Equals("Magic Missile") &&
                       x.LevelObtained <= attacker.Level);

            if (spell == null)
            {
                HubContext.SendToClient("You don't know that spell.", attacker.HubGuid);
                return;
            }

            //TODO: Fix His to be gender specific
            //TODO: Fist? what if it's a paw?

            if (!_taskRunnning && attacker.Target != null)
            {
                // find target if not in fight
                HubContext.SendToClient("A red ball begins swirling between your hands as you begin chanting magic missle", attacker.HubGuid);
                HubContext.SendToClient("A red ball begins swirling between " + Helpers.ReturnName(attacker, null) + " hands " + Helpers.ReturnHisOrHers(attacker.Gender) + " as they begin chanting magic missle", attacker.HubGuid,
                    attacker.Target.HubGuid, false, true);
                HubContext.broadcastToRoom("A red ball begins swirling between " +
                    Helpers.ReturnName(attacker, null) + " hands " + Helpers.ReturnHisOrHers(attacker.Gender) +" as they begin chanting magic missle " + Helpers.ReturnName(attacker.Target, null),
                    room.players, attacker.HubGuid, true);

                Task.Run(() => DoMagicMissile(attacker, room));

            }
            else
            {
                if (attacker.Target == null)
                {
                    HubContext.SendToClient("The energy disapates as you stop your chant", attacker.HubGuid);
                    return;
                }

                HubContext.SendToClient("You are trying to cast magic missle", attacker.HubGuid);

            }

        }

        private static async Task DoMagicMissile(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(2000);


            //get attacker strength
            var die = new PlayerStats();

            var ballCount = 1;

            if (attacker.Level == 1)
            {
                ballCount = 1;
            }        
            else if (attacker.Level <= 5)
            {
                ballCount = 2;
            }
            else if (attacker.Level <= 10)
            {
                ballCount = 3;
            }
            else if (attacker.Level <= 15)
            {
                ballCount = 4;
            }
            else if (attacker.Level <= 20)
            {
                ballCount = 5;
            }
         
            

            //level dependant but for testing launch 4 balls
            HubContext.SendToClient("4 red crackling energy balls hurl from your hands in a wide spread closing in your target", attacker.HubGuid);
            for (int i = 0; i < 4; i++)
            {
                var dam = die.dice(1, 4);
                var toHit = 52; //Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals("Kick", StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss
                int chance = die.dice(1, 100);
                Fight2.ShowAttack(attacker, attacker.Target, room, toHit, chance, MagicMissileAb(), dam);
            }
           


            _taskRunnning = false;
            attacker.Status = Player.PlayerStatus.Fighting;

        }

        public static Skill MagicMissileAb()
        {


            var skill = new Skill
            {
                Name = "Magic Missile",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast magic missile <Target>"
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
