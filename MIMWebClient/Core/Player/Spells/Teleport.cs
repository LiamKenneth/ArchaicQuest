using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.World;

namespace MIMWebClient.Core.Player.Skills
{

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using System.Threading.Tasks;

    public class Teleport : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill TeleportSkill { get; set; }

        public static void StartTeleport(Player player, Room room)
        {
            //Check if player has spell
            var hasSpell = Skill.CheckPlayerHasSkill(player, TeleporAb().Name);

            if (hasSpell == false)
            {
                HubContext.SendToClient("You don't know that spell.", player.HubGuid);
                return;
            }

        

            if (!_taskRunnning)
            {

                if (player.ManaPoints < TeleporAb().ManaCost)
                {
                    HubContext.SendToClient("You attempt to draw energy but fail", player.HubGuid);

                    return;
                }


                player.ManaPoints -= TeleporAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("You utter iter multo ieiunium.", player.HubGuid);

            
                foreach (var character in room.players)
                {
                    if (character != player)
                    {
                      
                        var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} utters iter multo ieiunium.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                Task.Run(() => DoTeleport(player, room));

            }



        }

        private static async Task DoTeleport(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);
 
            if (_target == null)
            {
                var castingTextAttacker = "You close your eyes and open them to find yourself somewhere else.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
             
               var goToRoom = Areas.ListOfRooms()[Helpers.diceRoll.Next(Areas.ListOfRooms().Count)];

               
                foreach (var character in room.players)
                {

                    if (character == attacker)
                    {
                        continue;
                    }

                    if (character != attacker)
                    {
                        var hisHer = Helpers.ReturnHisOrHers(attacker, character, false);
                        var roomMessage =
                            $"{Helpers.ReturnName(attacker, character, string.Empty)} closes {hisHer} eyes and then vanishes, only wisps of smoke are left behind.";

                        HubContext.SendToClient(roomMessage, character.HubGuid);
                    }
                }

                var randomExitFroMRandomRoom = goToRoom.exits[Helpers.diceRoll.Next(goToRoom.exits.Count)];

                Movement.Teleport(attacker, room, randomExitFroMRandomRoom);


            }

            //incase player status has changed from busy
            if (attacker.Status == Player.PlayerStatus.Busy)
            {
                attacker.Status = Player.PlayerStatus.Standing;
            }

            attacker.Status = Player.PlayerStatus.Busy;
            _target = null;
            _taskRunnning = false;


        }

        public static Skill TeleporAb()
        {


            var skill = new Skill
            {
                Name = "Teleport",
                SpellGroup = SpellGroupType.Universal,
                SkillType = Type.Spell,
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 1,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast teleport"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Makes you teleport to an area at random. Use with caution, you wouldn't want to end up in a snake pit.",
                DateUpdated = "17/04/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}