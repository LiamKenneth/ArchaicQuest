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

        public static void StartMagicMissile(Player attacker, Room room, string target = "")
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

            if (target != "")
            {
                var theTarget = string.Empty;
                var hasQuotes = target.Contains("'\"");

                if (hasQuotes)
                {
                    theTarget = target.Substring(target.LastIndexOf('"') + 1);

                    if (string.IsNullOrEmpty(theTarget))
                    {
                        theTarget = target.Substring(target.LastIndexOf('\'') + 1);
                    }
                }
                else
                {
                    theTarget = target.Substring(target.LastIndexOf(' ') + 1);
                }

                var foundTarget = Fight2.FindTarget(room, theTarget);

                if (foundTarget == null)
                {
                    return;;
                }

                attacker.Target = foundTarget;
            }

            var isAttackerFighting = room.fighting.FirstOrDefault(x => x.Equals(attacker.HubGuid));

            if (isAttackerFighting == null)
            {
                room.fighting.Add(attacker.HubGuid);
            }

           
            // null issue here ussing c 'magic missile' cat
            // code correctly found cat as a target
            // unsure on null, did flee from combat then reinit combat
            var isDefenderFighting = room.fighting.FirstOrDefault(x => x.Equals(attacker.Target.HubGuid));

            if (isDefenderFighting == null)
            {
                room.fighting.Add(attacker.Target.HubGuid);
            }

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

            var castingTextAttacker = ballCount == 1  ? "A red crackling energy ball hurls from your hands straight at " +  Helpers.ReturnName(attacker.Target, null) : ballCount + " red crackling energy balls hurl from your hands in a wide arc closing in on " + Helpers.ReturnName(attacker.Target, null);

            var castingTextDefender = ballCount == 1 ? Helpers.ReturnName(attacker, null) + " hurls a red crackling energy ball straight towards you." 
                :  Helpers.ReturnName(attacker, null) + " launches " + ballCount + " red crackling energy balls from " + Helpers.ReturnHisOrHers(attacker.Gender) +"  hands in a wide arc closing in on you";


            var castingTextRoom = ballCount == 1 ? Helpers.ReturnName(attacker, null) + " hurls a red crackling energy ball straight towards " + Helpers.ReturnName(attacker.Target, null)  + "."
              : Helpers.ReturnName(attacker, null) + " launches " + ballCount + " red crackling energy balls from " + Helpers.ReturnHisOrHers(attacker.Gender) + "  hands in a wide arc closing in on" + Helpers.ReturnName(attacker.Target, null);

          

            //level dependant but for testing launch 4 balls
            HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
            HubContext.SendToClient(castingTextDefender, attacker.Target.HubGuid);
            HubContext.SendToAllExcept(castingTextRoom, room.fighting, room.players);

            for (int i = 0; i < ballCount; i++)
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
