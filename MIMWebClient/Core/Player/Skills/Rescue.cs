using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player.Skills
{
    using System.Runtime.CompilerServices;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Rescue : Skill
    {

        public static Skill RescueSkill { get; set; }

        public static Skill RescueAb()
        {

            if (RescueSkill != null)
            {
                return RescueSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Rescue",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = false,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Save someone from being the target of an attack and become the new target",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                RescueSkill = skill;
            }

            return RescueSkill;

        }

        public void StartRescue(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, RescueAb().Name);

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
                context.SendToClient("Rescue whom?.", player.HubGuid);
                return;
            }

            var _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                context.SendToClient("Try fleeing instead.", player.HubGuid);
                player.ActiveSkill = null;
                return;
            }

            if (player.ActiveSkill != null)
            {

                context.SendToClient("wait till you have finished " + player.ActiveSkill.Name, player.HubGuid);
                return;

            }
            else
            {
                player.ActiveSkill = RescueAb();
            }




            if (_target != null)
            {

                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't rescue them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;
                }

                if (player.MovePoints < RescueAb().MovesCost)
                {

                    context.SendToClient("You are too tired to use Rescue.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;
                }

                if (_target.Target == null)
                {
                    context.SendToClient("They are not fighting.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;
                }


                player.MovePoints -= RescueAb().MovesCost;

                Score.UpdateUiPrompt(player);


                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Rescue"));
                if (skill == null)
                {
                    player.ActiveSkill = null;
                    return;
                }

                var skillProf = skill.Proficiency;

                if (skillProf >= chanceOfSuccess)
                {
                    Task.Run(() => DoRescue(context, player, _target, room));
                }
                else
                {
                    player.ActiveSkill = null;
                    PlayerSetup.Player.LearnFromMistake(player, RescueAb(), 250);



                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

       

        private async Task DoRescue(IHubContext context, PlayerSetup.Player attacker, PlayerSetup.Player target,
            Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < RescueAb().MovesCost)
            {
                context.SendToClient("You are too tired to use Rescue.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }

            var die = new PlayerStats();

            var ToRescue =
                Helpers.GetPercentage(
                    attacker.Skills.Find(x => x.Name.Equals(RescueAb().Name, StringComparison.CurrentCultureIgnoreCase))
                        .Proficiency, 95);

            int chance = die.dice(1, 100);

            bool alive = Fight2.IsAlive(attacker, target);


            if (alive)
            {
                if (ToRescue > chance)
                {

                    if (Fight2.IsAlive(attacker, target))
                    {
                        HubContext.Instance.SendToClient(
                            "You <span style='color:cyan'> rescue " +
                            Helpers.ReturnName(target, attacker, null).ToLower() + "</span>!",
                            attacker.HubGuid);


                        HubContext.Instance.SendToClient(
                            $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} rescues you!</span>",
                            target.HubGuid);


                        foreach (var player in room.players)
                        {
                            if (player != attacker && player != target)
                            {

                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + " rescues " +
                                    Helpers.ReturnName(target, attacker, null) + "!", target.HubGuid);

                            }
                        }
                    }

                    attacker.Target = target.Target;
                    target.Target.Target = attacker;
                    target.Target = null;
                    target.Status = Player.PlayerStatus.Standing;
                    target.ActiveFighting = false;

                }
                else
                {
                    if (Fight2.IsAlive(attacker, target))
                    {
                        var attackerMessage = "You fail to rescue " + Helpers.ReturnName(target, attacker, null);

                        var targetMessage = Helpers.ReturnName(attacker, target, null) + " fails to rescue you.";

                        var observerMessage = Helpers.ReturnName(attacker, target, null) + " fails to rescue " +
                                              Helpers.ReturnName(target, attacker, null);


                        HubContext.Instance.SendToClient(attackerMessage + " <br><br> ", attacker.HubGuid);
                        HubContext.Instance.SendToClient(targetMessage + " <br><br> ", target.HubGuid);

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

 
                attacker.ActiveSkill = null;

            }

        }
    }
}
