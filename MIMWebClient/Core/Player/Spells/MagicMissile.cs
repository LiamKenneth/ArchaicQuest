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


            //Check if player has spell
            var hasSpell =  Skill.CheckPlayerHasSkill(attacker, MagicMissileAb().Name);

            if (hasSpell == false)
            {
                HubContext.SendToClient("You don't know that spell.", attacker.HubGuid);
                return;
            }


            var canDoSkill = Skill.CanDoSkill(attacker);

            if (!canDoSkill)
            {
                return;
            }

            attacker.Status = Player.PlayerStatus.Busy;

            var foundTarget = Skill.FindTarget(target, room);

            if (foundTarget != null && attacker.Target == null && target != "")
            {
              
                Fight2.PerpareToFight(attacker, room, foundTarget.Name, true);
          
            }

    
            if (!_taskRunnning && attacker.Target != null)
            {

                if (attacker.ManaPoints < MagicMissileAb().ManaCost)
                {
                    HubContext.SendToClient("You clasp your hands together but fail to form any energy", attacker.HubGuid);

                    var excludePlayerInBroadcast = new List<string> {attacker.HubGuid};

                    HubContext.SendToAllExcept($"{Helpers.ReturnName(attacker, attacker.Target, null)} clasps {Helpers.ReturnHisOrHers(attacker, attacker.Target)} hands together but fails to form any energy", excludePlayerInBroadcast, room.players);
                    
                    return;
                }

                attacker.ManaPoints -= MagicMissileAb().ManaCost;

                Score.UpdateUiPrompt(attacker);

                HubContext.SendToClient("A red ball begins swirling between your hands as you begin chanting magic missle", attacker.HubGuid);

                HubContext.SendToClient($"A red ball begins swirling between {Helpers.ReturnName(attacker, attacker.Target, null)} 's hands as they begin chanting magic missle", attacker.HubGuid, attacker.Target.HubGuid, false, true);

                HubContext.broadcastToRoom($"A red ball begins swirling between {Helpers.ReturnName(attacker, attacker.Target, null)} 's hands as they begin chanting magic missle", room.players, attacker.HubGuid, true);

                Task.Run(() => DoMagicMissile(attacker, room));

            }
            else
            {
                if (attacker.Target == null)
                {
                    HubContext.SendToClient("Cast magic missile at whom?", attacker.HubGuid);
                    return;
                }

                HubContext.SendToClient("You are trying to cast magic missle", attacker.HubGuid);

            }

        }

        private static async Task DoMagicMissile(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(1000);


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
            else if (attacker.Level >= 20)
            {
                ballCount = 5;
            }

            var castingTextAttacker = ballCount == 1  ? "A red crackling energy ball hurls from your hands straight at " +  Helpers.ReturnName(attacker.Target, attacker, null) : ballCount + " red crackling energy balls hurl from your hands in a wide arc closing in on " + Helpers.ReturnName(attacker.Target, attacker, null);

            var castingTextDefender = ballCount == 1 ? Helpers.ReturnName(attacker, attacker.Target, null) + " hurls a red crackling energy ball straight towards you." 
                :  Helpers.ReturnName(attacker, attacker.Target, null) + " launches " + ballCount + " red crackling energy balls from " + Helpers.ReturnHisOrHers(attacker, attacker.Target) +"  hands in a wide arc closing in on you";


            var castingTextRoom = ballCount == 1 ? Helpers.ReturnName(attacker, attacker.Target, null) + " hurls a red crackling energy ball straight towards " + Helpers.ReturnName(attacker.Target, attacker, null)  + "."
              : Helpers.ReturnName(attacker, attacker.Target, null) + " launches " + ballCount + " red crackling energy balls from " + Helpers.ReturnHisOrHers(attacker, attacker.Target) + "  hands in a wide arc closing in on" + Helpers.ReturnName(attacker.Target, attacker, null);

          

            //level dependant but for testing launch 4 balls
            HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
            HubContext.SendToClient(castingTextDefender, attacker.Target.HubGuid);
            HubContext.SendToAllExcept(castingTextRoom, room.fighting, room.players);

            for (int i = 0; i < ballCount; i++)
            {
                var dam = die.dice(1, 4);
                var toHit = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(MagicMissileAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss
                int chance = die.dice(1, 100);
                Fight2.ShowAttack(attacker, attacker.Target, room, toHit, chance, MagicMissileAb(), dam);
            }
           


            _taskRunnning = false;
            Score.ReturnScoreUI(attacker);
            Player.SetState(attacker);

        }

        public static Skill MagicMissileAb()
        {


            var skill = new Skill
            {
                Name = "Magic Missile",
                SpellGroup = SpellGroupType.Invocation,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast magic missile <Target>"
            };


            var help = new Help
            {
                Syntax = "cast magic missile <target>",
                HelpText = "Does 1d4 magic damage, missiles increase with every 5 levels up to a maximum of 5 at level 20 " +
                           "it can be used in combat and out of combat to initiate combat.",
                DateUpdated = "30/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
