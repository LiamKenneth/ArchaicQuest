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

    public class Armour : Skill
    {
        private static bool _taskRunnning = false;
        private static Player _target = new Player();
        public static Skill ArmourSkill { get; set; }

        public static void StartArmour(Player player, Room room, string target = "")
        {
            //Check if player has spell
            Skill.CheckPlayerHasSkill(player, ArmourAb().Name);

            _target = Skill.FindTarget(target, room);

            //Fix issue if target has similar name to user and they use abbrivations to target them
            if (_target == player)
            {
                _target = null;
            }


            if (!_taskRunnning && _target != null)
            {

                if (player.ManaPoints < ArmourAb().ManaCost)
                {
                    HubContext.SendToClient("You attempt to draw energy to your hands but fail", player.HubGuid);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= ArmourAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("Your hands start to glow as you begin chanting the armour spell", player.HubGuid);

                var playersInRoom = new List<Player>(room.players);

                //todo Stop double echo to target
                //To target: Vall sends a white glowing ball straight towards you surrounding you in magical armour.
                //To room : Vall sends a white glowing ball straight towards Val which surrounds them in magical armour..
                HubContext.broadcastToRoom(Helpers.ReturnName(player, null) + " hands start to glow as they begin chanting the Armour spell", playersInRoom, player.HubGuid, true);

                Task.Run(() => DoArmour(player, room));

            }
            else
            {
                if (_target == null)
                {

                    //TODO REfactor
                    player.ManaPoints -= ArmourAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    HubContext.SendToClient("Your hands start to glow as you begin chanting the armour spell", player.HubGuid);

                    HubContext.broadcastToRoom(Helpers.ReturnName(player, null) + " hands start to glow as they begin chanting the Armour spell", room.players, player.HubGuid, true);

                    Task.Run(() => DoArmour(player, room));
                     
                }

                 

            }

        }

        private static async Task DoArmour(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            if (_target == null)
            {
                var castingTextAttacker =
                    "You place your hands upon your chest engulfing yourself in a white protective glow.";

                var castingTextRoom =  Helpers.ReturnName(attacker, null) +  " places " + Helpers.ReturnHisOrHers(attacker.Gender, false) + " hands upon " + Helpers.ReturnHisOrHers(attacker.Gender, false) + " chest engulfing himself in a white protective glow.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                
                HubContext.SendToAllExcept(castingTextRoom, room.fighting, room.players);

                attacker.ArmorRating += 20;

            }
            else
            {
                var castingTextAttacker =
                      "You place your hands upon " + Helpers.ReturnName(_target, null) + " engulfing them in a white protective glow.";

                var castingTextDefender = Helpers.ReturnName(attacker, null) + " touches your chest engulfing you in a white protective glow.";

                var castingTextRoom = Helpers.ReturnName(attacker, null) +
                                      " touches " + Helpers.ReturnName(_target, null) + " engulfing them in a white protective glow.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.SendToClient(castingTextDefender, _target.HubGuid);
                HubContext.broadcastToRoom(castingTextRoom, room.players, attacker.HubGuid, true);

                _target.ArmorRating += 20;

            }

            _target = null;
            _taskRunnning = false;
     

        }

        public static Skill ArmourAb()
        {


            var skill = new Skill
            {
                Name = "Armour",
                CoolDown = 0,
                Delay = 0,
                LevelObtained = 2,
                ManaCost = 10,
                Passive = false,
                Proficiency = 1,
                MaxProficiency = 95,
                UsableFromStatus = "Standing",
                Syntax = "cast armour / cast armour <Target>"
            };


            var help = new Help
            {
                Syntax = skill.Syntax,
                HelpText = "Increases Armour rating by 20",
                DateUpdated = "28/03/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
