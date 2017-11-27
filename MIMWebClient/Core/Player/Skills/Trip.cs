using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Core.Player.Skills
{

    public class Trip: Skill
    {

        public static Skill TripSkill { get; set; }
        public static Skill TripAb()
        {
                  
            if (TripSkill != null)
            {
               return TripSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Trip",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "trip",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "trip help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                TripSkill = skill;
            }

            return TripSkill;
            
        }

        public void StartTrip(IHubContext context, PlayerSetup.Player player, Room.Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, TripAb().Name);

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
                player.ActiveSkill = TripAb();
            }




            if (_target != null)
            {


                if (player.MovePoints < TripAb().MovesCost)
                {


                    context.SendToClient("You are too tired to use trip.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= TripAb().MovesCost;

                Score.UpdateUiPrompt(player);

               

                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Trip"));
                if (skill == null)
                {
                    player.ActiveSkill = null;
                    return;
                }

                var skillProf = skill.Proficiency;

                if (skillProf >= chanceOfSuccess)
                {
                    Task.Run(() => DoTrip(context, player, _target, room));
                }
                else
                {
                    player.ActiveSkill = null;
                    PlayerSetup.Player.LearnFromMistake(player, TripAb(), 250);

                    var gain = Helpers.Rand(1, 5);

                    skill.Proficiency += gain;

                    context.SendToClient("Your trip skill increases by " + gain + "%.", player.HubGuid);

                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient("You can't trip yourself", player.HubGuid);
            }


        }

        private async Task DoTrip(IHubContext context,  PlayerSetup.Player attacker, PlayerSetup.Player target, Room.Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < TripAb().MovesCost)
            {
                context.SendToClient("You are too tired to use trip.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }

            var die = new PlayerStats();

            var dam = die.dice(1, 6);

            var toHit = Helpers.GetPercentage(attacker.Skills.Find(x => x.Name.Equals(TripAb().Name, StringComparison.CurrentCultureIgnoreCase)).Proficiency, 95); // always 5% chance to miss
            int chance = die.dice(1, 100);


            bool alive = Fight2.IsAlive(attacker, target);
            int IsCritical = Fight2.CriticalHit(toHit, chance);

            if (alive)
            {
                if (toHit > chance)
                {

                    var damage = dam > 0 ? dam : Fight2.Damage(attacker, target, IsCritical);

                    var damageText = Fight2.DamageText(damage);

                    if (Fight2.IsAlive(attacker, target))
                    {
                        HubContext.Instance.SendToClient(
                            "Your trip " + Helpers.ReturnName(target, attacker, null).ToLower() + " to the ground, leaving them slightly dazed.",
                            attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            "Your trip " + damageText.Value.ToLower() +
                            " " + Helpers.ReturnName(target, attacker, null).ToLower() + " [" + dam + "]", attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(target, attacker, null) + " " + Fight2.ShowMobHeath(target) + "<br><br>", attacker.HubGuid);

                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(attacker, target, null) + " sends you to the ground with a well timed trip, leaving you feeling dazed." , target.HubGuid);


                        HubContext.Instance.SendToClient(
                            Helpers.ReturnName(attacker, target, null) + "'s trip " + damageText.Value.ToLower() +
                            " you [" + dam + "]", target.HubGuid);



                        foreach (var player in room.players)
                        {
                            if (player != attacker && player != target)
                            {

                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + " sends " + Helpers.ReturnName(target, attacker, null) + " to the ground with a well timed trip, leaving them loo king dazed.", target.HubGuid);




                                HubContext.Instance.SendToClient(
                                    Helpers.ReturnName(attacker, target, null) + "'s trip " + damageText.Value.ToLower() +
                                    " " + Helpers.ReturnName(target, attacker, null), player.HubGuid);


                            }


                        }


                        target.StunDuration = 20;
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
                            attackerMessage = "Your trip <span style='color:olive'>misses</span> " +
                                      Helpers.ReturnName(target, attacker, null);

                            targetMessage = Helpers.ReturnName(attacker, target, null) + "'s trip <span style='color:olive'>misses</span> you ";

                            observerMessage = Helpers.ReturnName(attacker, target, null) + "'s   <span style='color:olive'>misses</span> " +
                                              Helpers.ReturnName(target, attacker, null);
                        }
                        else if (rand > 1 && rand <= 2)
                        {
                            attackerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>dodges</span> your trip.";

                            targetMessage = "You <span style='color:olive'>dodge</span> " + Helpers.ReturnName(attacker, target, null) + "'s trip";

                            observerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>dodges</span> " + Helpers.ReturnName(attacker, target, null) + "'s trip.";
                        }
                        else
                        {
                            attackerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>parries</span> your trip";

                            targetMessage = "You <span style='color:olive'>parry</span> " + Helpers.ReturnName(attacker, target, null) + "'s trip";

                            observerMessage = Helpers.ReturnName(target, attacker, null) + " <span style='color:olive'>parries</span> " + Helpers.ReturnName(attacker, target, null) + "'s trip";
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

            //trip / stun player


            Score.ReturnScoreUI(target);


            PlayerSetup.Player.SetState(attacker);

            Fight2.PerpareToFightBack(attacker, room, target.Name, true);




            attacker.ActiveSkill = null;

        }

    }
}
