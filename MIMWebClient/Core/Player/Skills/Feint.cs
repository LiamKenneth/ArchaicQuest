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

    public class Feint : Skill
    {
        private static bool _taskRunnning = false;
        public static Skill FeintSkill { get; set; }

        public static Skill FeintAb()
        {


            var skill = new Skill
            {
                Name = "Feint",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 20,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                MovesCost = 10,
                UsableFromStatus = "Standing",
                Syntax = "Feint <Target>"
            };


            var help = new Help
            {
                Syntax = "Feint <Victim>",
                HelpText = "Feint can be used to start a fight or during a fight, " +
                           "you can only Feint your primary target." +
                           " During combat only Feint is needed to Feint your target to inflict damage",
                DateUpdated = "18/01/2017"

            };

            skill.HelpText = help;


            return skill;


        }

        public void StartFeint(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, FeintAb().Name);

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
                player.ActiveSkill = FeintAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't Feint them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < FeintAb().MovesCost)
                {


                    context.SendToClient("You are too tired to use Feint.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= FeintAb().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Feint"));
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
                    HubContext.Instance.SendToClient("You don't see an opportunity to Feint.",
                        player.HubGuid);
                    PlayerSetup.Player.LearnFromMistake(player, FeintAb(), 250);


                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

        private int FeintSuccess(PlayerSetup.Player attacker, PlayerSetup.Player target)
        {
            var success = attacker.Dexterity - target.Wisdom + target.Dexterity + (attacker.Level - target.Level) * 2;

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

            if (attacker.ManaPoints < FeintAb().MovesCost)
            {
                context.SendToClient("You are too tired to feint.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }
 

            var die = new PlayerStats();

            bool alive = Fight2.IsAlive(attacker, target);

            if (alive)
            {

                var skillSuccess = FeintSuccess(attacker, target);
                int chanceOfFeint = die.dice(1, 100);

                if (skillSuccess > chanceOfFeint)
                {

                    var weapon =
                        attacker.Inventory.FirstOrDefault(
                            x => x.name == attacker.Equipment.Wielded && x.location == Item.Item.ItemLocation.Worn || x.name == attacker.Equipment.Wielded && x.location == Item.Item.ItemLocation.Wield);

                    int damage;
                    if (weapon == null)
                    {
                        damage = Helpers.Rand(1, attacker.Level / 3 + 1);
                    }
                    else
                    {

                        damage = Helpers.Rand(1, weapon.stats.damMax * 2);
                    }

                   

                    var calcDamage = Skill.Damage(damage, target);

                    var damageText = Fight2.DamageText(calcDamage);

                    target.HitPoints -= calcDamage;

                    HubContext.Instance.SendToClient(
                        "Your feint sends " + Helpers.ReturnName(target, attacker, null).ToLower() + " the wrong way. Your attack " + damageText.Value.ToLower() +
                        " " + Helpers.ReturnName(target, attacker, null).ToLower() + " [" + calcDamage + "]"+ Fight2.ShowStatus(target),
                        attacker.HubGuid);

                   
                    HubContext.Instance.SendToClient("<span style=color:cyan>" + Helpers.ReturnName(target, attacker, null) + " " + Fight2.ShowMobHeath(target) + "</span>", attacker.HubGuid);

 
                    HubContext.Instance.SendToClient(
                        $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} feint leaves you wrong footed, their attack {damageText.Value.ToLower()} you! [{calcDamage}]</span>",
                        target.HubGuid);
 

                    foreach (var player in room.players)
                    {
                        if (player != attacker && player != target)
                        {

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, target, null) + " tricks " + Helpers.ReturnName(target, attacker, null) + " leaving them wrong footed. " + Helpers.ReturnName(attacker, target, null) + " attack " + damageText.Value.ToLower() + Helpers.ReturnName(target, attacker, null), target.HubGuid);

                        }


                    }

                    Fight2.IsDead(attacker, target, room);



                }
                else
                {
                    var attackerMessage = "Your feint <span style='color:olive'>fails</span> to trick " +
                                             Helpers.ReturnName(target, attacker, null);

                    var targetMessage = Helpers.ReturnName(attacker, target, null) +
                                           "tries to feint their attack.";

                    var observerMessage = Helpers.ReturnName(attacker, target, null) +
                                             "tries to feint their attack. But " +
                                             Helpers.ReturnName(target, attacker, null) + " see's through it.";


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
 
