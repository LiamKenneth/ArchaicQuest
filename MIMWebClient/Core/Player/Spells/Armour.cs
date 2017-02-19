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

            var foundTarget = Skill.FindTarget(target, room);

            if (foundTarget != null)
            {

                // cast armour on target

            }


            if (!_taskRunnning && player.Target != null)
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

                HubContext.SendToClient("Awhite sphere begins swirling between " + Helpers.ReturnName(player, null) + " hands " + Helpers.ReturnHisOrHers(player.Gender) + " as they begin chanting the Armour spell", player.HubGuid,
                    player.Target.HubGuid, false, true);

                HubContext.broadcastToRoom("A white sphere begins swirling between " +
                    Helpers.ReturnName(player, null) + " hands " + Helpers.ReturnHisOrHers(player.Gender) + " as they begin chanting the armour spell " + Helpers.ReturnName(player.Target, null), room.players, player.HubGuid, true);

                Task.Run(() => DoArmour(player, room));

            }
            else
            {
                if (player.Target == null)
                {

                    //TODO REfactor
                    player.ManaPoints -= ArmourAb().ManaCost;

                    Score.UpdateUiPrompt(player);

                    HubContext.SendToClient("A white sphere begins swirling between your hands as you begin chanting the armour spell", player.HubGuid);

                    HubContext.SendToClient("Awhite sphere begins swirling between " + Helpers.ReturnName(player, null) + " hands " + Helpers.ReturnHisOrHers(player.Gender) + " as they begin chanting the Armour spell", player.HubGuid,
                        player.Target.HubGuid, false, true);

                    HubContext.broadcastToRoom("A white sphere begins swirling between " +
                        Helpers.ReturnName(player, null) + " hands " + Helpers.ReturnHisOrHers(player.Gender) + " as they begin chanting the armour spell " + Helpers.ReturnName(player.Target, null), room.players, player.HubGuid, true);

                    Task.Run(() => DoArmour(player, room));
                     
                }

                HubContext.SendToClient("You are trying to cast magic missle", player.HubGuid);

            }

        }

        private static async Task DoArmour(Player attacker, Room room)
        {
            _taskRunnning = true;
            attacker.Status = Player.PlayerStatus.Busy;


            await Task.Delay(500);

            if (attacker.Target == null)
            {
                var castingTextAttacker =
                    "You release the white sphere from your hands and it surrounds your whole body providing extra protection.";
                 
                var castingTextRoom = Helpers.ReturnName(attacker, null) +  " releases a white glowing sphere which surrounds " + Helpers.ReturnHisOrHers(attacker.Gender) + " body.";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                
                HubContext.SendToAllExcept(castingTextRoom, room.fighting, room.players);

                attacker.ArmorRating += 20;

            }
            else
            {
                var castingTextAttacker =
                   "Your white sphere from your hands surrounds your whole body providing extra protection.";
                var castingTextDefender = Helpers.ReturnName(attacker, null) + " sends a white glowing ball straight towards you.";
                var castingTextRoom = Helpers.ReturnName(attacker, null) +
                                      " sends a white glowing ball  straight towards " +
                                      Helpers.ReturnName(attacker.Target, null) + ".";

                HubContext.SendToClient(castingTextAttacker, attacker.HubGuid);
                HubContext.SendToClient(castingTextDefender, attacker.Target.HubGuid);
                HubContext.SendToAllExcept(castingTextRoom, room.fighting, room.players);

                attacker.Target.ArmorRating += 20;

            }

 
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
