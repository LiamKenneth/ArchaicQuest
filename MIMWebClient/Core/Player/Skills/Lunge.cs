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

    public class Lunge: Skill
    {

        public static Skill LungeSkill { get; set; }
        public static Skill LungeAb()
        {
                  
            if (LungeSkill != null)
            {
               return LungeSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Lunge",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Lunge <target>",
                    MovesCost = 40,
                    HelpText = new Help()
                    {
                        HelpText = "lunge help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                LungeSkill = skill;
            }

            return LungeSkill;
            
        }


        public void StartLunge(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, LungeAb().Name);

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
                player.ActiveSkill = LungeAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't lunge at them as they are dead.", player.HubGuid);
                    return;

                }

                if (player.MovePoints < LungeAb().MovesCost)
                {


                    context.SendToClient("You are too tired to lunge.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= LungeAb().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Lunge"));
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
                    HubContext.Instance.SendToClient("You can't find an opportunity to lunge.",
                        player.HubGuid);
                    PlayerSetup.Player.LearnFromMistake(player, LungeAb(), 250);


                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

        private int LungeSuccess(PlayerSetup.Player attacker, PlayerSetup.Player target)
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

            if (attacker.ManaPoints < LungeAb().MovesCost)
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

                var skillSuccess = LungeSuccess(attacker, target);
                int chanceOfKick = die.dice(1, 100);

                if (skillSuccess > chanceOfKick)
                {
                    var weapon =
                        attacker.Inventory.FirstOrDefault(
                            x => x.name == attacker.Equipment.Wielded && x.location == Item.Item.ItemLocation.Worn);

                    if (weapon == null)
                    {
                       HubContext.Instance.SendToClient("You need a weapon to lunge", attacker.HubGuid);
                        attacker.ActiveSkill = null;
                        return;
                    }

                    var damage = Helpers.Rand(weapon.stats.damMax / 2, weapon.stats.damMax + weapon.stats.damRoll * 2);

                    var calcDamage = Skill.Damage(damage, target);

                    var damageText = Fight2.DamageText(calcDamage);

                    target.HitPoints -= calcDamage;

                    HubContext.Instance.SendToClient(
                        "Your lunge" + " " + damageText.Value.ToLower() +
                        " " + Helpers.ReturnName(target, attacker, null).ToLower() + " [" + calcDamage + "]" + Fight2.ShowStatus(target),
                        attacker.HubGuid);


                    HubContext.Instance.SendToClient("<span style=color:cyan>" + Helpers.ReturnName(target, attacker, null) + " " + Fight2.ShowMobHeath(target) + "</span>", attacker.HubGuid);


                    HubContext.Instance.SendToClient(
                        $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} lunge {damageText.Value.ToLower()} you! [{calcDamage}]</span>",
                        target.HubGuid);


                    foreach (var player in room.players)
                    {
                        if (player != attacker && player != target)
                        {

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, target, null) + " lunge " + damageText.Value.ToLower() + " " +
                            Helpers.ReturnName(target, attacker, null) + ".", target.HubGuid);

                        }


                    }

                    Fight2.IsDead(attacker, target, room);



                }
                else
                {
                    var attackerMessage = "Your lunge <span style='color:olive'>misses</span> " +
                                             Helpers.ReturnName(target, attacker, null);

                    var targetMessage = Helpers.ReturnName(attacker, target, null) +
                                           "'s lunge <span style='color:olive'>misses</span> you ";

                    var observerMessage = Helpers.ReturnName(attacker, target, null) +
                                             "'s lunge <span style='color:olive'>misses</span> " +
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
