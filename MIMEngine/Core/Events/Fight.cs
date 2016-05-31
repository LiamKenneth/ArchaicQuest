using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Runtime.CompilerServices;
    using System.Security.Permissions;
    using System.Threading;

    using MIMEngine.Core.Item;
    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    public class Fight
    {

        public static Player attacker { get; set; }
        public static Room room { get; set; }
        public static string attackOptions { get; set; }

        public Fight(Player attackerData, Room roomData, string attackOptionsData)
        {
            attacker = attackerData;
            room = roomData;
            attackOptions = attackOptionsData;

        }

        public static void StartFight(Player attackerData, Room roomData, string attackOptionsData)
        {
            Fight2 kill = new Fight2(attackerData, roomData, attackOptionsData);
            kill.StartFight();
        }

        public static Player FindTarget(Room room, string defender)
        {
            Player target = room.players.Find(x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase));

            return target;
        }


        public  void MeleeAttack(object state)
        {

            //find defender 
            Player defender = FindTarget(room, attackOptions);

            if (defender == null)
            {
                HubContext.SendToClient("No one here", attacker.HubGuid);
                return;

            }
            //Get Attacker Weapon
            string RightHand = attacker.Equipment.RightHand;
            string LeftHand = attacker.Equipment.LeftHand;
            Item RightHandWep = null;
            Item LeftHandWep = null;
            bool dualWield = false;
            bool TwoHanded = false;
            bool handToHand = false;


            if (RightHand == "Nothing" && LeftHand == "Nothing")
            {
                handToHand = true;
            }
            else
            {
                RightHandWep = attacker.Inventory.Find(x => x.name == RightHand && x.location == "Equipped");
                LeftHandWep = attacker.Inventory.Find(x => x.name == LeftHand && x.location == "Equipped");
            }

            double offense = Offense(attacker);
            double evasion = Evasion(defender);

            double toHit = (offense / evasion) * 100;
            int chance = D100();


            if (toHit > chance)
            {
                HubContext.SendToClient("You hit " + defender.Name, attacker.HubGuid);
            }
            else
            {
                HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
            }


        }

        public static int D100()
        {
            var die = new PlayerStats();
            int roll = die.dice(1, 100);

            return roll;
        }

        public static double Offense(Player player)
        {
            double handToHandSkill = 0.75;
            int wieldedWeaponSkill = 0;
            int dexterity = player.Dexterity;
            int strength = player.Strength;


            //(Weapon Skill + (Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue);

            double off = handToHandSkill + (dexterity / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

            //Based on skill and a random number, an Offensive Force / Factor(OF) is generated.
            //This number is bonused by the user's Agility as modified by the weapon's balance.
            //    The number is compared to the effective score of the defender's combined Evasion, Parry, and Shield. From this a chance of hitting is determined. At exactly equal OF and defense, the chance to hit is 66%.
            //    There is always a chance to both hit or miss, regardless of the numbers. 
            return off;
        }

        public static double Evasion(Player player)
        {
            double evasionSkill = 0.75;
            double blockSkill = 0.15;
            double parrySkill = 0;
            int dexterity = player.Dexterity;

            //((Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue)

            double evade = evasionSkill + blockSkill + parrySkill + (dexterity / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

            return evade;
        }



        public static void Punch(Player attacker, Player defender, Room room)
        {
            HubContext.SendToClient("You clench your fist and pull your arm back", attacker.HubGuid);
            HubContext.SendToClient(attacker.Name + " Pulls his arm back aiming a punch at you.", attacker.HubGuid, defender.HubGuid, false, true);
            HubContext.broadcastToRoom(attacker.Name + " clenches his fist and pulls his arm back aiming for " + defender.Name, room.players, attacker.HubGuid, true);

            //get attacker strength

            //get attacker attack points

            //get defender defece points

            //hit them!
        }
    }
}
