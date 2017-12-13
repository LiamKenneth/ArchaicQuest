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

    public class Disarm : Skill
    {

        public static Skill DisarmSkill { get; set; }
        public static Skill DisarmAb()
        {

            if (DisarmSkill != null)
            {
                return DisarmSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Disarm",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 15,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    MovesCost = 10,
                    Passive = true,
                    UsableFromStatus = "Fighting",
                    Syntax = "Disarm <target>",
                    HelpText = new Help()
                    {
                        HelpText = "disarm help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                DisarmSkill = skill;
            }

            return DisarmSkill;

        }

        public void StartDisarm(IHubContext context, PlayerSetup.Player player, Room room)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, DisarmAb().Name);

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

             var target = player.Target?.Name;
             

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
                player.ActiveSkill = DisarmAb();
            }


            if (_target != null)
            {

                var weapon = _target.Inventory.FirstOrDefault( x => x.name == _target.Equipment.Wielded && x.location == Item.Item.ItemLocation.Worn || x.name == player.Equipment.Wielded && x.location == Item.Item.ItemLocation.Wield);

                if (weapon == null)
                {
                    HubContext.Instance.SendToClient(
                     Helpers.ReturnName(_target, player, null).ToLower() + " has no weapon to disarm.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;
                }


                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't disarm them as they are dead.", player.HubGuid);
                    player.ActiveSkill = null;
                    return;

                }

                if (player.MovePoints < DisarmAb().MovesCost)
                {


                    context.SendToClient("You are too tired to disarm.", player.HubGuid);
                    player.ActiveSkill = null;

                    return;
                }

                player.MovePoints -= DisarmAb().MovesCost;

                Score.UpdateUiPrompt(player);



                var chanceOfSuccess = Helpers.Rand(1, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Disarm"));
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
                    HubContext.Instance.SendToClient($"You fail to disarm {Helpers.ReturnName(_target, player, string.Empty)}.",
                        player.HubGuid);
                    PlayerSetup.Player.LearnFromMistake(player, DisarmAb(), 250);


                    Score.ReturnScoreUI(player);
                }
            }
            else if (_target == null)
            {
                context.SendToClient("You need to be in combat before using disarm.", player.HubGuid);
                player.ActiveSkill = null;
            }


        }

        private int DisarmSuccess(PlayerSetup.Player attacker, PlayerSetup.Player target)
        {
            var success = 2 * attacker.Strength - target.Strength + (attacker.Level - target.Level) * 2;

            if (attacker.Effects.FirstOrDefault(x => x.Name.Equals("Haste")) != null)
            {
                success += 10;
            }

            if (target.Effects.FirstOrDefault(x => x.Name.Equals("Haste")) != null)
            {
                success -= 25;
            }

            if (attacker.SizeCategory > target.SizeCategory)
            {
                success += 10;
            }

            if (target.SizeCategory > attacker.SizeCategory)
            {
                success -= 25;
            }

            if (success <= 0)
            {
                success = 1;
            }

            return success;
        }

        private  void DoDisarm(Player player, Room room)
        {
           

            var weapon = player.Inventory.FirstOrDefault(x => x.name == player.Equipment.Wielded && x.location == Item.Item.ItemLocation.Worn || x.name == player.Equipment.Wielded && x.location == Item.Item.ItemLocation.Wield);

            if (weapon != null)
            {
              
                player.Inventory.Remove(weapon);
                weapon.location = Item.Item.ItemLocation.Room;
                room.items.Add(weapon);
                player.Equipment.Wielded = "Nothing";
            }
        }

        private async Task DoSkill(IHubContext context, PlayerSetup.Player attacker, PlayerSetup.Player target, Room room)
        {

            attacker.Status = PlayerSetup.Player.PlayerStatus.Busy;

            await Task.Delay(500);

            if (attacker.ManaPoints < DisarmAb().MovesCost)
            {
                context.SendToClient("You are too tired to disarm.", attacker.HubGuid);
                attacker.ActiveSkill = null;
                PlayerSetup.Player.SetState(attacker);
                return;
            }


            var die = new PlayerStats();

            bool alive = Fight2.IsAlive(attacker, target);

            if (alive)
            {
                var weapon = target.Inventory.FirstOrDefault(
                        x => x.name == attacker.Equipment.Wielded && x.location == Item.Item.ItemLocation.Worn || x.name == attacker.Equipment.Wielded && x.location == Item.Item.ItemLocation.Wield);

                if (weapon == null)
                {
                    HubContext.Instance.SendToClient(
                     Helpers.ReturnName(target, attacker, null).ToLower() + " has no weapon to disarm.", attacker.HubGuid);
                    attacker.ActiveSkill = null;
                    return;
                }

                var skillSuccess = DisarmSuccess(attacker, target);
                int chanceOfDisarm = die.dice(1, 100);

                if (skillSuccess > chanceOfDisarm)
                {



                    HubContext.Instance.SendToClient(
                        "You disarm " + Helpers.ReturnName(target, attacker, null).ToLower() + "!", attacker.HubGuid);

                    HubContext.Instance.SendToClient(
                        $"{Helpers.ReturnName(attacker, target, null)} disarms you!",
                        target.HubGuid);

                      DoDisarm(target, room);


                    foreach (var player in room.players)
                    {
                        if (player != attacker && player != target)
                        {

                            HubContext.Instance.SendToClient(
                                Helpers.ReturnName(attacker, target, null) + " disarms " +
                            Helpers.ReturnName(target, attacker, null) + "!", target.HubGuid);

                        }


                    }


                }
                else
                {
                    var attackerMessage = "You try to disarm " + Helpers.ReturnName(target, attacker, null) + " but fail.";

                    var targetMessage = Helpers.ReturnName(attacker, target, null) + " tries to disarm you.";

                    var observerMessage = Helpers.ReturnName(attacker, target, null) +
                                             " tries to disarm " +
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

            Score.ReturnScoreUI(attacker);

            attacker.ActiveSkill = null;
            PlayerSetup.Player.SetState(attacker);
        }

    }
}
