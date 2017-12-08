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

    public class Bash: Skill
    {

        public static Skill BashSkill { get; set; }
        public static Skill BashAb()
        {
                  
            if (BashSkill != null)
            {
               return BashSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Bash",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Bash <target>",
                    HelpText = new Help()
                    {
                        HelpText = "bash help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                BashSkill = skill;
            }

            return BashSkill;
            
        }

        public void StartBash(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, BashAb().Name);

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
                player.ActiveSkill = BashAb();
            }




            if (_target != null)
            {

  
                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't bash them as they are dead.", player.HubGuid);
                    return;

                }

                if (player.MovePoints < BashAb().MovesCost)
                {


                    context.SendToClient("You are too tired to use Bash.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= BashAb().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Bash"));
                if (skill == null)
                {
                    player.ActiveSkill = null;
                    return;
                }

                var skillProf = skill.Proficiency;

                if (skillProf >= chanceOfSuccess)
                {
                    Task.Run(() => DoBash(context, player, _target, room));
                }
                else
                {
                    player.ActiveSkill = null;
                    PlayerSetup.Player.LearnFromMistake(player, BashAb(), 250);



                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

        private bool BashSuccess(PlayerSetup.Player attacker, PlayerSetup.Player target)
        {
            var targetSaveAgainstBash = Math.Max(target.Strength, target.Dexterity);

            if (attacker.Strength == targetSaveAgainstBash)
            {
                return attacker.MovePoints > target.MovePoints;
            }

            return attacker.Strength > targetSaveAgainstBash;
        }

        private async Task DoBash(IHubContext context, PlayerSetup.Player attacker, PlayerSetup.Player target, Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < BashAb().MovesCost)
            {
                context.SendToClient("You are too tired to use Bash.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }

            var die = new PlayerStats();

            var dam = die.dice(1, 6);

            var ToBash = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(BashAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95);

            int chance = die.dice(1, 100);

            bool alive = Fight2.IsAlive(attacker, target);

            var isBashSuccess = BashSuccess(attacker, target);

            if (alive)
            {
                if (ToBash > chance && isBashSuccess)
                {
                    var damage = dam > 0 ? dam : Fight2.Damage(attacker, target, 1);

                    var damageText = Fight2.DamageText(damage);

                    if (Fight2.IsAlive(attacker, target))
                    {
                        HubContext.Instance.SendToClient(
                            "<span style='color:cyan'>You Bash " + Helpers.ReturnName(target, attacker, null).ToLower() + " to the ground, leaving them slightly dazed.</span>",
                            attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            $"Your Bash {damageText.Value.ToLower()} {Helpers.ReturnName(target, attacker, null).ToLower()} [{dam}]", attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(target, attacker, null) + " " + Fight2.ShowMobHeath(target) + "<br><br>", attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} sends you to the ground with a well timed Bash, leaving you feeling dazed.</span>",
                            target.HubGuid);


                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(attacker, target, null) + "'s Bash " + damageText.Value.ToLower() +
                            " you [" + dam + "]", target.HubGuid);


                        foreach (var player in room.players)
                        {
                            if (player != attacker && player != target)
                            {

                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + " sends " + Helpers.ReturnName(target, attacker, null) + " to the ground with a well timed Bash, leaving them looking dazed.", target.HubGuid);




                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + "'s Bash " + damageText.Value.ToLower() +
                                    " " + Helpers.ReturnName(target, attacker, null), player.HubGuid);


                            }


                        }


                        target.StunDuration = 6000;
                        target.Status = PlayerSetup.Player.PlayerStatus.Stunned;


                        target.HitPoints -= damage;

                        if (target.HitPoints < 0)
                        {
                            target.HitPoints = 0;
                        }


                        if (!Fight2.IsAlive(attacker, target))
                        {
                            Fight2.IsDead(attacker, target, room);
                        }
                    }

                }
                else
                {
                    if (Fight2.IsAlive(attacker, target))
                    {

                        //Randomly pick to output dodge, parry, miss
                        var rand = Helpers.Rand(1, 4);
                        string attackerMessage, targetMessage, observerMessage;

                        if (rand <= 1)
                        {
                            attackerMessage = "Your Bash <span style='color:olive'>misses</span> " +
                                              Helpers.ReturnName(target, attacker, null);

                            targetMessage = Helpers.ReturnName(attacker, target, null) + "'s Bash <span style='color:olive'>misses</span> you ";

                            observerMessage = Helpers.ReturnName(attacker, target, null) + "'s   <span style='color:olive'>misses</span> " +
                                              Helpers.ReturnName(target, attacker, null);
                        }
                        else if (rand > 1 && rand <= 2)
                        {
                            attackerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>dodges</span> your Bash.";

                            targetMessage = "You <span style='color:olive'>dodge</span> " + Helpers.ReturnName(attacker, target, null) + "'s Bash";

                            observerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>dodges</span> " + Helpers.ReturnName(attacker, target, null) + "'s Bash.";
                        }
                        else
                        {
                            attackerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>parries</span> your Bash";

                            targetMessage = "You <span style='color:olive'>parry</span> " + Helpers.ReturnName(attacker, target, null) + "'s Bash";

                            observerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>parries</span> " + Helpers.ReturnName(attacker, target, null) + "'s Bash";
                        }

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

            }

            //Bash / stun player


            Score.ReturnScoreUI(target);


            PlayerSetup.Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);




            attacker.ActiveSkill = null;

        }

    }
}
