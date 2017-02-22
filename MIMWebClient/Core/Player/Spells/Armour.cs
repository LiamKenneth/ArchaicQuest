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
            var hasSpell = Skill.CheckPlayerHasSkill(player, ArmourAb().Name);

            if (hasSpell == false)
            {
                HubContext.SendToClient("You don't know that spell.", player.HubGuid);
                return;
            }

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
                    HubContext.SendToClient("You clasp your hands together but fail to form any energy", player.HubGuid);

                    var excludePlayerInBroadcast = new List<string> {player.HubGuid};

                    HubContext.SendToAllExcept(Helpers.ReturnName(player, null) + " clasps " + Helpers.ReturnHisOrHers(player.Gender) + " hands together but fails to form any energy", excludePlayerInBroadcast, room.players);

                    return;
                }

                //TODO REfactor

                player.ManaPoints -= ArmourAb().ManaCost;

                Score.UpdateUiPrompt(player);

                HubContext.SendToClient("A white sphere begins swirling between your hands as you begin chanting the armour spell", player.HubGuid);

                HubContext.SendToClient("A white sphere begins swirling between the hands of " + Helpers.ReturnName(player, null) + " as they begin chanting the Armour spell", player.HubGuid,
                    _target.HubGuid, false, true);

                var playersInRoom = new List<Player>(room.players);
                //remove target
                 playersInRoom.Remove(_target);

                //todo Stop double echo to target
                //To target: Vall sends a white glowing ball straight towards you surrounding you in magical armour.
                //To room : Vall sends a white glowing ball straight towards Val which surrounds them in magical armour..
                HubContext.broadcastToRoom("A white sphere begins swirling between the hands of " +
                    Helpers.ReturnName(player, null) + " as they begin chanting the armour spell.", playersInRoom, player.HubGuid, true);

                Task.Run(() => DoArmour(player, room));

            }
            else
            {
                if (_target == null)
                {

                    //TODO REfactor
                    player.ManaPoints -= ArmourAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    HubContext.SendToClient("A white sphere begins swirling between your hands as you begin chanting the armour spell", player.HubGuid);

                    HubContext.broadcastToRoom("A white sphere begins swirling between the hands of " +
                        Helpers.ReturnName(player, null) + " as they begin chanting the armour spell ", room.players, player.HubGuid, true);

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
                    "You release the white sphere from your hands and it surrounds your whole body providing extra protection.";
                 
                var castingTextRoom =  Helpers.ReturnName(attacker, null) +  " releases a white glowing sphere which surrounds " + Helpers.ReturnHisOrHers(attacker.Gender, false) + " body.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                
                HubContext.SendToAllExcept(castingTextRoom, room.fighting, room.players);

                attacker.ArmorRating += 20;

            }
            else
            {
                var castingTextAttacker =
                   "You launch a white sphere from your hands towards " + Helpers.ReturnName(_target, null) +" surrounding them in magical armour.";

                var castingTextDefender = Helpers.ReturnName(attacker, null) + " sends a white glowing ball straight towards you surrounding you in magical armour.";

                var castingTextRoom = Helpers.ReturnName(attacker, null) +
                                      " sends a white glowing ball straight towards " + Helpers.ReturnName(_target, null) + " which surrounds them in magical armour..";

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
                DateUpdated = "19/02/2017"

            };

            skill.HelpText = help;


            return skill;


        }
    }
}
