using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Core.Player.Skills
{

    public class Steal : Skill
    {

        public static Skill StealSkill { get; set; }
        public static Skill StealAb()
        {

            if (StealSkill != null)
            {
                return StealSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Steal",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 11,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Steal",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "Steal help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                StealSkill = skill;
            }

            return StealSkill;

        }

        public static void DoSteal(IHubContext context, PlayerSetup.Player player, Room.Room room, string stealItemFrom)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, StealAb().Name);

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
            string[] splitString = stealItemFrom.Split(' ');

            string target;
            string item;

            if (splitString.Length > 1)
            {
                target = splitString[0].Trim();
                item = splitString[1].Trim();
            }
            else
            {
                context.SendToClient("What do you want to steal?", player.HubGuid);
                return;
            }

          

            var _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }

            if (_target != null)
            {

                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't steal from them as they are dead.", player.HubGuid);
                    return;

                }
 
                var chanceOfSuccess = Helpers.Rand(_target.Wisdom, 100);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Steal"));

                if (skill == null)
                {
                    return;
                }

                var skillProf = skill.Proficiency;

                if (skillProf >= chanceOfSuccess)
                {
                    HubContext.Instance.SendToClient($"You swipe {Helpers.ReturnName(null, null, item).ToLower()} from {Helpers.ReturnName(_target,player,string.Empty)}.", player.HubGuid);

                    if (_target.Inventory.Count == 0)
                    {
                        HubContext.Instance.SendToClient($"You see nothing of value carried by {Helpers.ReturnName(_target, player, string.Empty)}.",
                            player.HubGuid);

                        return;
                    }

                    var getItem = _target.Inventory.FirstOrDefault(x => x.name.ToLower().Contains(item.ToLower()));

                    if (getItem != null)
                    {
                        player.Inventory.Add(getItem);
                        _target.Inventory.Remove(getItem);
                        Score.ReturnScoreUI(player);
                        Score.ReturnScoreUI(_target);
                    }
                }
                else
                {

            

                    HubContext.Instance.SendToClient($"You fail to steal from {Helpers.ReturnName(_target, player, string.Empty)}.",
                        player.HubGuid);

                    HubContext.Instance.SendToClient($"{Helpers.ReturnName(player, _target, string.Empty)} tries to steal something from you.",
                        _target.HubGuid);

                    PlayerSetup.Player.LearnFromMistake(player, StealAb(), 250);

                    Fight2.PerpareToFightBack(_target, room, player.Name);


                    Score.ReturnScoreUI(player);
                }
            }
            else
            {
                context.SendToClient($"You can't find anything known as '{target}' here.", player.HubGuid);
                player.ActiveSkill = null;
            }


 
        }

    }
}
