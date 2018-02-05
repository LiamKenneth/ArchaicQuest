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

    public class CureBlindness : Skill
    {
        private static bool _taskRunnning = false;
        public static CureBlindness CureBlindnessSkill { get; set; }

        public static CureBlindness CureBlindnessAb()
        {


            var skill = new CureBlindness
            {
                Name = "Cure Blindness",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                ManaCost = 10,
                UsableFromStatus = "Standing",
                Syntax = "cure blindness <Target>"
            };


            var help = new Help
            {
                Syntax = "cure blindness <Victim>",
                HelpText = "",
                DateUpdated = "18/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }

        public void StartCureBlindness(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, CureBlindnessAb().Name);

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
                player.ActiveSkill = CureBlindnessAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't cast cure blindness on them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < CureBlindnessAb().MovesCost)
                {


                    context.SendToClient("You are too tired to cast cure blindness.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.ManaPoints -= CureBlindnessAb().ManaCost;

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

            if (attacker.ManaPoints < CureBlindnessAb().ManaCost)
            {
              context.SendToClient("You attempt to draw energy but fail", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }


            var chanceOfSuccess = Helpers.Rand(1, 100);
            var skill = attacker.Skills.FirstOrDefault(x => x.Name.Equals("Cure Blindness"));
            if (skill == null)
            {
                attacker.ActiveSkill = null;
                return;
            }

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {
                var isBlind = Effect.HasEffect(target, "Blindess");

                if (!isBlind)
                {
                    HubContext.Instance.SendToClient($"{target.Name} is not blind.", attacker.HubGuid);

                }
                else
                {

                    if (target != attacker)
                    {
                        HubContext.Instance.SendToClient($"{target.Name} looks better.", attacker.HubGuid);
                    }

                    HubContext.Instance.SendToClient("You regain the ability to see.", target.HubGuid);
                }


            }
            else
            {
                attacker.ActiveSkill = null;
                HubContext.Instance.SendToClient("You lost your concerntration.",
                    attacker.HubGuid);
                PlayerSetup.Player.LearnFromMistake(attacker, CureBlindnessAb(), 250);

                Score.ReturnScoreUI(attacker);
            }


            Score.ReturnScoreUI(target);

            PlayerSetup.Player.SetState(attacker);

            attacker.ActiveSkill = null;

        }
    }
}
 
