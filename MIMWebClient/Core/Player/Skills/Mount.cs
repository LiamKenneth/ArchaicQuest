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

    public class Mount: Skill
    {
        private static Player _target = new Player();
        public static Skill MountSkill { get; set; }
        public static Skill MountAb()
        {
                  
            if (MountSkill != null)
            {
               return MountSkill;
            }
            else
            {
                var skill = new Skill
                {
                    Name = "Mount",
                    CoolDown = 0,
                    Delay = 0,
                    LevelObtained = 1,
                    Proficiency = 1,
                    MaxProficiency = 95,
                    Passive = true,
                    UsableFromStatus = "Standing",
                    Syntax = "Mount <Mob>",
                    HelpText = new Help()
                    {
                        HelpText = "Ability to mount a mob",
                        DateUpdated = new DateTime().ToShortDateString()
                    }
                };


                MountSkill = skill;
            }

            return MountSkill;
            
        }

        public static void StartMount(Player player, Room room, string target = "")
        {
            //Check if player has spell
            var hasSkill = Skill.CheckPlayerHasSkill(player, MountAb().Name);

            if (hasSkill == false)
            {
                HubContext.SendToClient("You don't know how to ride mounts.", player.HubGuid);
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

            _target = room.mobs.FirstOrDefault(x => x.Name.ToLower().Contains(target.ToLower()));

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }

            if ( _target != null)
            {

                if (!_target.IsMount)
                {
                    HubContext.SendToClient($"You can't mount {Helpers.ReturnName(null, null, _target.Name)}.", player.HubGuid);
                    return;
                }

                HubContext.SendToClient($"You mount {Helpers.ReturnName(null, null, _target.Name)}", player.HubGuid);

                
                foreach (var character in room.players)
                {
                    if (character != player)
                    {

                        var roomMessage =
                            $"{Helpers.ReturnName(player, character, string.Empty)} mounts {Helpers.ReturnName(null, null, _target.Name)}.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                player.Status = Player.PlayerStatus.Mounted;
                player.Mount = _target;

                room.mobs.Remove(_target);

            }
            else if (_target == null)
            {
                HubContext.SendToClient("You don't see that here?", player.HubGuid);
            }


        }


        public static void Dismount(Player player, Room room, string target = "")
        {


            if (player.Mount == null)
            {
                HubContext.SendToClient($"You are not mounted.",  player.HubGuid);
                return;
            }

            HubContext.SendToClient($"You dismount {Helpers.ReturnName(null, null, player.Mount.Name).ToLower()}", player.HubGuid);


            foreach (var character in room.players)
            {
                if (character != player)
                {

                    var roomMessage =
                        $"{Helpers.ReturnName(player, character, string.Empty)} dismounts {Helpers.ReturnName(null, null, player.Mount.Name).ToLower()}.";

                    HubContext.SendToClient(roomMessage, character.HubGuid);
                }
            }

            player.Status = Player.PlayerStatus.Standing;
            room.mobs.Add(player.Mount);
            player.Mount = null;

           
        }
    }
}
