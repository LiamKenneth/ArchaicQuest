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

    public class CauseLight : Skill
    {
        private static bool _taskRunnning = false;
        public static CauseLight causeLightSkill { get; set; }

        public static CauseLight causeLightAb()
        {


            var skill = new CauseLight
            {
                Name = "Cause Light",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                ManaCost = 10,
                UsableFromStatus = "Staning",
                Syntax = "Cause light <Target>"
            };


            var help = new Help
            {
                Syntax = "cause light <Victim>",
                HelpText = "",
                DateUpdated = "18/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }

        public void StartCauseLight(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, causeLightAb().Name);

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

            if (string.IsNullOrEmpty(target) && player.Target != null)
            {
                target = player.Target.Name;
            }

            var _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }

            if (player.ActiveSkill != null)
            {

                context.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = causeLightAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't cast cause light on them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < causeLightAb().MovesCost)
                {


                    context.SendToClient("You are too tired to cast cause light.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.ManaPoints -= causeLightAb().ManaCost;

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

            if (attacker.ManaPoints < causeLightAb().ManaCost)
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
            var skill = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Cause Light"));
            if (skill == null)
            {
                attacker.ActiveSkill = null;
                return;
            }

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {
                var damage = Helpers.Rand(1, 8) / 3;
                var toHit = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(causeLightAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss

                Fight2.ShowAttack(attacker, target, room, toHit, chanceOfSuccess, causeLightAb(), false, damage);
            }
            else
            {
                attacker.ActiveSkill = null;
                HubContext.Instance.SendToClient("You lost your concerntration.",
                    attacker.HubGuid);
                PlayerSetup.Player.LearnFromMistake(attacker, causeLightAb(), 250);

                Score.ReturnScoreUI(attacker);
            }


            Score.ReturnScoreUI(target);

            PlayerSetup.Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);


            attacker.ActiveSkill = null;

        }
    }
}
 
