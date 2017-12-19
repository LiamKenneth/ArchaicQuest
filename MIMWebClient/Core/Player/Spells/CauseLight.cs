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
                LevelObtained = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                MovesCost = 10,
                UsableFromStatus = "Fighting",
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

        public void StartKick(IHubContext context, PlayerSetup.Player player, Room room, string target = "")
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
                    context.SendToClient("You can't CauseLight them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < causeLightAb().MovesCost)
                {


                    context.SendToClient("You are too tired to use cause light.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= causeLightAb().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("cause light"));
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
                    HubContext.Instance.SendToClient("You don't see an opportunity to cause light.",
                        player.HubGuid);
                    PlayerSetup.Player.LearnFromMistake(player, causeLightAb(), 250);


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

            if (attacker.ManaPoints < causeLightAb().MovesCost)
            {
                context.SendToClient("You are too tired to cause light.", attacker.HubGuid);
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
                    var damage = Helpers.Rand(1, 8) / 3;

                    var calcDamage = Skill.Damage(damage, target);

                    var damageText = Fight2.DamageText(calcDamage);

                    target.HitPoints -= calcDamage;

                    HubContext.Instance.SendToClient(
                        "Your cause light" + " " + damageText.Value.ToLower() +
                        " " + Helpers.ReturnName(target, attacker, null).ToLower() + " [" + calcDamage + "]"+ Fight2.ShowStatus(target),
                        attacker.HubGuid);

                   
                    HubContext.Instance.SendToClient("<span style=color:cyan>" + Helpers.ReturnName(target, attacker, null) + " " + Fight2.ShowMobHeath(target) + "</span>", attacker.HubGuid);

 
                    HubContext.Instance.SendToClient(
                        $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} cause light {damageText.Value.ToLower()} you! [{calcDamage}]</span>",
                        target.HubGuid);
 

                    foreach (var player in room.players)
                    {
                        if (player != attacker && player != target)
                        {

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, target, null) + " cause light " + damageText.Value.ToLower() + " "+
                            Helpers.ReturnName(target, attacker, null) + ".", target.HubGuid);

                        }


                    }

                    Fight2.IsDead(attacker, target, room);



                }
                else
                {
                    var attackerMessage = "Your cause light <span style='color:olive'>misses</span> " +
                                             Helpers.ReturnName(target, attacker, null);

                    var targetMessage = Helpers.ReturnName(attacker, target, null) +
                                           "'s cause light <span style='color:olive'>misses</span> you ";

                    var observerMessage = Helpers.ReturnName(attacker, target, null) +
                                             "'s cause light <span style='color:olive'>misses</span> " +
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
 
