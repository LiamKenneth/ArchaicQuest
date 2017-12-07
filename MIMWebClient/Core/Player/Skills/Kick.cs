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
                MovesCost = 10,
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

        public void StartKick(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, KickAb().Name);

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
                player.ActiveSkill = KickAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't kick them as they are dead.", player.HubGuid);
                    return;

                }

                if (player.MovePoints < KickAb().MovesCost)
                {


                    context.SendToClient("You are too tired to use kick.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= KickAb().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Kick"));
                if (skill == null)
                {
                    player.ActiveSkill = null;
                    return;
                }

                var skillProf = skill.Proficiency;

                if (skillProf >= chanceOfSuccess)
                {
                    Task.Run(() => DoSkill(context, player, _target, room));
                }
                else
                {
                    player.ActiveSkill = null;
                    HubContext.Instance.SendToClient("You don't see an opportunity to kick.",
                        player.HubGuid);
                    PlayerSetup.Player.LearnFromMistake(player, KickAb(), 250);


                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

        private int KickSuccess(PlayerSetup.Player attacker, PlayerSetup.Player target)
        {
            var success = 2 * attacker.Dexterity - target.Dexterity + (attacker.Level - target.Level) * 2;

            if (attacker.Effects.FirstOrDefault(x => x.Name.Equals("Haste")) != null)
            {
                success += 10;
            }

            if (target.Effects.FirstOrDefault(x => x.Name.Equals("Haste")) != null)
            {
                success -= 25;
            }

            if (success <= 0)
            {
                success = 1;
            }

            return success;
        }

        private async Task DoSkill(IHubContext context, PlayerSetup.Player attacker, PlayerSetup.Player target, Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < KickAb().MovesCost)
            {
                context.SendToClient("You are too tired to kick.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }
 

            var die = new PlayerStats();

            bool alive = Fight2.IsAlive(attacker, target);

            if (alive)
            {

                var skillSuccess = KickSuccess(attacker, target);
                int chanceOfKick = die.dice(1, 100);

                if (skillSuccess > chanceOfKick)
                {
                    HubContext.Instance.SendToClient(
                        "<span style='color:cyan'>You kick " +
                        Helpers.ReturnName(target, attacker, null).ToLower() + ".</span>",
                        attacker.HubGuid);

 //no dam

                    HubContext.Instance.SendToClient(
                        $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} kicks you!</span>",
                        target.HubGuid);
 

                    foreach (var player in room.players)
                    {
                        if (player != attacker && player != target)
                        {

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, target, null) + " kicks " +
                                Helpers.ReturnName(target, attacker, null) + ".", target.HubGuid);

                        }


                    }

                }
                else
                {
                    var attackerMessage = "Your kick <span style='color:olive'>misses</span> " +
                                             Helpers.ReturnName(target, attacker, null);

                    var targetMessage = Helpers.ReturnName(attacker, target, null) +
                                           "'s kick <span style='color:olive'>misses</span> you ";

                    var observerMessage = Helpers.ReturnName(attacker, target, null) +
                                             "'s kick <span style='color:olive'>misses</span> " +
                                             Helpers.ReturnName(target, attacker, null);


                    HubContext.Instance.SendToClient(attackerMessage, attacker.HubGuid);
                    HubContext.Instance.SendToClient(targetMessage, target.HubGuid);

                    foreach (var player in room.players)
                    {
                        if (player != attacker && player != target)
                        {
                            HubContext.Instance.SendToClient(
                                observerMessage, player.HubGuid);
                        }
                    }


                }


            }


            Score.ReturnScoreUI(target);

            PlayerSetup.Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);


            attacker.ActiveSkill = null;

        }
    }
}
 
