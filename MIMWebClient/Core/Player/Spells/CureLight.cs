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

    public class CureLight : Skill
    {
        private static bool _taskRunnning = false;
        public static CureLight CureLightSkill { get; set; }

        public static CureLight CureLightAb()
        {


            var skill = new CureLight
            {
                Name = "Cure Light",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                ManaCost = 10,
                UsableFromStatus = "Staning",
                Syntax = "cure light <Target>"
            };


            var help = new Help
            {
                Syntax = "cure light <Victim>",
                HelpText = "",
                DateUpdated = "18/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }

        public void StartCureLight(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, CureLightAb().Name);

            if (hasSkill == false)
            {
                context.SendToClient("You don't know that skill.", player.HubGuid);
                return;
            }

            var canDoSkill = Skill.CanDoSkill(player);

            if (!canDoSkill)
            {
                return;
            }

            if (string.IsNullOrEmpty(target))
            {
                target = player.Name;
            }

            var _target = Skill.FindTarget(target, room) ?? player;


            if (player.ActiveSkill != null)
            {

                context.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = CureLightAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't cast cure light on them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < CureLightAb().MovesCost)
                {


                    context.SendToClient("You are too tired to cast cure light.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.ManaPoints -= CureLightAb().ManaCost;

                if (player.ManaPoints < 0)
                {
                    player.ManaPoints = 0;
                }

                Score.UpdateUiPrompt(player);


                 Task.Run(() => DoSkill(context, player, _target, room));
               
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }
 
        private async Task DoSkill(IHubContext context, PlayerSetup.Player attacker, PlayerSetup.Player target, Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < CureLightAb().ManaCost)
            {
              context.SendToClient("You attempt to draw energy but fail", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);

                if (attacker.ManaPoints < 0)
                {
                    attacker.ManaPoints = 0;
                }
                return;
            }


            var chanceOfSuccess = Helpers.Rand(1, 100);
            var skill = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Cure Light"));
            if (skill == null)
            {
                attacker.ActiveSkill = null;
                return;
            }

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {
                var heal = Helpers.Rand(1, 8) / 3;

                target.HitPoints += heal;

                if (target.HitPoints > target.MaxHitPoints)
                {
                    target.HitPoints = target.MaxHitPoints;
                }

                if (target != attacker)
                {
                    HubContext.Instance.SendToClient($"{target.Name} looks better.", attacker.HubGuid);
                }
             
                HubContext.Instance.SendToClient("You feel better.",  target.HubGuid);


            }
            else
            {
                attacker.ActiveSkill = null;
                HubContext.Instance.SendToClient("You lost your concerntration.", attacker.HubGuid);
                PlayerSetup.Player.LearnFromMistake(attacker, CureLightAb(), 250);

                Score.ReturnScoreUI(attacker);
            }


            Score.ReturnScoreUI(target);

            PlayerSetup.Player.SetState(attacker);

            attacker.ActiveSkill = null;

        }
    }
}
 
