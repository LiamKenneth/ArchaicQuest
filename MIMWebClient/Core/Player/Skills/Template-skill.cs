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

    public class DoSkill : Skill
    {

        public static Skill ShieldBashSkill { get; set; }
        public static Skill ShieldBashAb()
        {

            if (ShieldBashSkill != null)
            {
                return ShieldBashSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Shield Bash",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 14,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Passive command",
                    HelpText = new Help()
                    {
                        HelpText = "Shield Bash help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                ShieldBashSkill = skill;
            }

            return ShieldBashSkill;


        }

        public void CreateSkill(Skill skill)
        {
            var newSkill = new Skill
            {
                Name = skill.Name,
                CoolDown = skill.CoolDown,
                Delay = skill.Delay,
                LevelObtained = skill.LevelObtained,
                Proficiency = 1,
                MaxProficiency = skill.MaxProficiency,
                Passive = skill.Passive,
                UsableFromStatus = skill.UsableFromStatus,
                Syntax = skill.Syntax,
                ManaCost = skill.ManaCost,
                HelpText = new Help()
                {
                    HelpText = skill.HelpText.HelpText,
                    DateUpdated = new DateTime().ToShortDateString() // date added or edited
                }
            };

            //save new skill to db
        }



        public void StartSkill(IHubContext context, PlayerSetup.Player player, string skillName, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, skillName);

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

            var getSkill = null //Get skill

            // Required Component for skill?
            if (getSkill.RequiresComponent)
            {
                if (hasPlayerGotCmponet(getSkill.RequiredComponent))
                {
                    context.SendToClient(getSkill.RequiresComponentErrorMessage, player.HubGuid);
                    return;
                }
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
                player.ActiveSkill = ShieldBashAb();
            }




            if (_target != null)
            {


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient($"You can't {skillName} them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < getSkill().MovesCost)
                {


                    context.SendToClient($"You are too tired to use {skillName}.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= getSkill().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals(skillName));
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
                    PlayerSetup.Player.LearnFromMistake(player, GetSkillTarget().AB, 250);



                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

        private int SkillSuccess(PlayerSetup.Player attacker, PlayerSetup.Player target)
        {
            var sizeMod = attacker.SizeCategory < target.SizeCategory ? 18 : 10;
            var stunChance = ((int)attacker.SizeCategory - (int)target.SizeCategory) * sizeMod;

            stunChance += 10;

            stunChance += attacker.Weight / 250;
            stunChance -= target.Weight / 200;
            stunChance += attacker.Strength;
            stunChance -= (target.Dexterity * 4) / 3;

            var hasHaste = attacker.Effects?.FirstOrDefault(
                                    x => x.Name.Equals("Haste", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (hasHaste)
            {
                chance += 10;
            }

            var targetHasHaste = target.Effects?.FirstOrDefault(
                               x => x.Name.Equals("Haste", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (targetHasHaste)
            {
                chance -= 30;
            }

            chance += attacker.Level - target.Level;

            return stunChance;
        }

        private async Task DoSkill(IHubContext context, PlayerSetup.Player attacker, PlayerSetup.Player target, Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < ShieldBashAb().MovesCost)
            {
                context.SendToClient("You are too tired to use {skillName}.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }

            var die = new PlayerStats();

            var dam = die.dice(1, 6);

            var ToBash = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(ShieldBashAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95);

            int chance = die.dice(1, 100);

            bool alive = Fight2.IsAlive(attacker, target);

            var bashChance = BashSuccess(attacker, target);
            var bashStunRand = Helpers.Rand(1, 100);
            var isBashSuccess = bashChance >= bashStunRand ? true : false;


            if (alive)
            {
                if (ToBash > chance && isBashSuccess)
                {
                    var damage = Helpers.Rand(2, 4 * (int)attacker.SizeCategory + bashChance / 12);

                    var damageText = Fight2.DamageText(damage);

                    if (Fight2.IsAlive(attacker, target))
                    {
                        HubContext.Instance.SendToClient(
                            "<span style='color:cyan'>You slam into " + Helpers.ReturnName(target, attacker, null).ToLower() + ", and send " + Helpers.ReturnName(target, attacker, null).ToLower() + " flying!</span>",
                            attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            $"Your shield bash {damageText.Value.ToLower()} {Helpers.ReturnName(target, attacker, null).ToLower()} [{dam}]", attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(target, attacker, null) + " " + Fight2.ShowMobHeath(target), attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            $"<span style='color:cyan'>{Helpers.ReturnName(attacker, target, null)} sends you sprawling with a powerful shield bash!</span>",
                            target.HubGuid);


                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(attacker, target, null) + "'s shield bash " + damageText.Value.ToLower() +
                            " you [" + dam + "]", target.HubGuid);


                        foreach (var player in room.players)
                        {
                            if (player != attacker && player != target)
                            {

                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + " sends " + Helpers.ReturnName(target, attacker, null) + " sprawling with a powerful shield bash.", target.HubGuid);




                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + "'s shield bash " + damageText.Value.ToLower() +
                                    " " + Helpers.ReturnName(target, attacker, null), player.HubGuid);


                            }


                        }


                        target.StunDuration = 10000;
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
                            attackerMessage = "Your shield bash <span style='color:olive'>misses</span> " +
                                              Helpers.ReturnName(target, attacker, null);

                            targetMessage = Helpers.ReturnName(attacker, target, null) + "'s shield bash <span style='color:olive'>misses</span> you ";

                            observerMessage = Helpers.ReturnName(attacker, target, null) + "'s   <span style='color:olive'>misses</span> " +
                                              Helpers.ReturnName(target, attacker, null);
                        }
                        else if (rand > 1 && rand <= 2)
                        {
                            attackerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>dodges</span> your shield bash.";

                            targetMessage = "You <span style='color:olive'>dodge</span> " + Helpers.ReturnName(attacker, target, null) + "'s shield bash";

                            observerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>dodges</span> " + Helpers.ReturnName(attacker, target, null) + "'s shield bash.";
                        }
                        else
                        {
                            attackerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>parries</span> your shield bash";

                            targetMessage = "You <span style='color:olive'>parry</span> " + Helpers.ReturnName(attacker, target, null) + "'s shield bash";

                            observerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>parries</span> " + Helpers.ReturnName(attacker, target, null) + "'s shield bash";
                        }

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

            }

            //Bash / stun player


            Score.ReturnScoreUI(target);


            PlayerSetup.Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);




            attacker.ActiveSkill = null;

        }

    }

}
