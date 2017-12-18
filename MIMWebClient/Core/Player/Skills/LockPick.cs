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

    public class LockPick : Skill
    {

        public static Skill LockPickSkill { get; set; }
        public static Skill LockPickAb()
        {

            if (LockPickSkill != null)
            {
                return LockPickSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Lock Pick",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 13,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Lock Pick",
                    MovesCost = 10,
                    HelpText = new Help()
                    {
                        HelpText = "Lock Pick help text",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                LockPickSkill = skill;
            }

            return LockPickSkill;

        }

        public static int LockPickDifficulty(Item.BaseItem.lockPickDifficulty difficulty)
        {
            switch (difficulty)
            {
                case Item.BaseItem.lockPickDifficulty.Simple:
                    return 25;
                case Item.BaseItem.lockPickDifficulty.Mid:
                    return 50;
                case Item.BaseItem.lockPickDifficulty.Hard:
                    return 75;
            }

            return 1;
        }

        public static void DoLockPick(IHubContext context, PlayerSetup.Player player,  Room room, string item)
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, LockPickAb().Name);

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

            if (string.IsNullOrEmpty(item))
            {
                context.SendToClient("You need to specify an item.", player.HubGuid);
                return;
            }

            var hasItem = FindItem.Item(room.items, -1, item, Item.Item.ItemLocation.Room);


            if (hasItem == null)
            {
                context.SendToClient("Nothing here by that name.", player.HubGuid);
                return;
            }


            if (!hasItem.locked)
            {
                context.SendToClient($"{Helpers.ReturnName(null, null, hasItem.name).ToLower()} is already unlocked.", player.HubGuid);
                return;
            }


            var chanceOfSuccess = Helpers.Rand(LockPickDifficulty(hasItem.LockPick), 100);
            var skill = player.Skills.FirstOrDefault(x => x.Name.Equals("Lock Pick"));

            var skillProf = skill.Proficiency;

            if (skillProf >= chanceOfSuccess)
            {

                HubContext.Instance.SendToClient("*CLICK!*", player.HubGuid);

                //Itme stats
                HubContext.Instance.SendToClient($"You unlock {Helpers.ReturnName(null, null, hasItem.name).ToLower()}", player.HubGuid);

                hasItem.locked = false;
 
                Score.ReturnScoreUI(player);
            }
            else
            {
                //something random
                HubContext.Instance.SendToClient($"You fail to unlock {Helpers.ReturnName(null, null, hasItem.name).ToLower().ToLower()}.", player.HubGuid);

                Score.ReturnScoreUI(player);
            }
        }

    }
}
