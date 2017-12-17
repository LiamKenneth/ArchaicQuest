using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.PlayerSetup;

namespace MIMWebClient.Core.Player.Skills
{

    public class Peak : Skill
    {

        public static Skill PeakSkill { get; set; }
        public static Skill PeakAb()
        {

            if (PeakSkill != null)
            {
                return PeakSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Peak",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 10,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Peak",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "Peak help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                PeakSkill = skill;
            }

            return PeakSkill;

        }

        public static void DoPeak(IHubContext context, PlayerSetup.Player player, Room.Room room, string target)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, PeakAb().Name);

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

            if (_target != null)
            {

                if (_target.HitPoints <= 0)
                {
                    context.SendToClient("You can't kick them as they are dead.", player.HubGuid);
                    return;

                }
 
                var chanceOfSuccess = Helpers.Rand(1, _target.Intelligence);
                var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Peak"));

                if (skill == null)
                {
                    return;
                }

                var skillProf = skill.Proficiency;

                if (skillProf >= chanceOfSuccess)
                {
                    HubContext.Instance.SendToClient($"You peak at {_target.Name}.",
                        player.HubGuid);

                    if (_target.Inventory.Count == 0)
                    {
                        HubContext.Instance.SendToClient($"You see nothing of value carried by {Helpers.ReturnName(_target, player, string.Empty)}.",
                            player.HubGuid);

                        return;
                    }

                    HubContext.Instance.SendToClient($"{_target.Name} is carrying:",
                        player.HubGuid);

                    var itemList = new StringBuilder();
                   
                    foreach (var item in ItemContainer.List(_target.Inventory.Where(x => x.location == Item.Item.ItemLocation.Inventory), true))
                    {
                        itemList.AppendLine(item);
                    }

                    HubContext.Instance.AddNewMessageToPage(player.HubGuid, itemList.ToString());
                }
                else
                {
                    HubContext.Instance.SendToClient($"You fail to peak at {Helpers.ReturnName(_target, player, string.Empty)}.",
                        player.HubGuid);

                    HubContext.Instance.SendToClient($"{Helpers.ReturnName(player, _target, string.Empty)} tries to peak at your inventory.",
_target.HubGuid);

                    PlayerSetup.Player.LearnFromMistake(player, PeakAb(), 250);


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
