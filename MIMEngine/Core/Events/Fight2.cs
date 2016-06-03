using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Security.Permissions;
    using System.Threading;

    using MIMEngine.Core.Item;
    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;
    using System.Timers;
    public class Fight2
    {

        public static async Task StartFight(Player attacker, Room room, string attackOptions, bool isAttacker)
        {
            if (attacker == null)
            {
                return;
            }

            /* player can only attack one target
         * if player gets attacked by something else they cannot fight back until
         * they have ended the fight they are already in.
         * player defence should be divided by the number of people they are being attacked by.
         * 
         */

            //automated Combat rounds for melee attacks
            //kinda boring but this is mvp

            //find defender 
            Player defender = FindTarget(room, attackOptions);

            if (defender == null)
            {
                HubContext.SendToClient("No one here", attacker.HubGuid);
                return;

            }

            //get speed of attackr and defender?
            // Defender(defender, attacker, room);


            //Check if players are alive

        
                  HitTarget(attacker, defender, room);

            if (attacker.HitPoints <= 0)
            {
                return;
            }

            if (defender.HitPoints <= 0)
            {
                return;
            }

            int attackerSpeed = 3000;
            int defenderSpeed = 250;

            while (defender.HitPoints > 0 && attacker.HitPoints > 0)
            {
                Task.Delay(3000);

               
                double offense = Offense(attacker);
                double evasion = Evasion(defender);

                double toHit = (offense / evasion) * 100;
                int chance = D100();


                if (toHit > chance)
                {
                    HubContext.SendToClient("You hit " + defender.Name, attacker.HubGuid);
                    HubContext.SendToClient(
                        attacker.Name + " hits you ",
                        attacker.HubGuid,
                        defender.HubGuid,
                        false,
                        true);

                    attacker.HitPoints -= 1;
                }
                else
                {
                    HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
                    HubContext.SendToClient(
                        attacker.Name + " misses you ",
                        attacker.HubGuid,
                        defender.HubGuid,
                        false,
                        true);
                }
                



            }


           

            //HitTarget(attacker, defender, room);

        }

        private static  Action<Task> HitTarget(Player attacker, Player defender, Room room)
        {
           
            double offense = Offense(attacker);
            double evasion = Evasion(defender);

            double toHit = (offense / evasion) * 100;
            int chance = D100();


            if (toHit > chance)
            {
                HubContext.SendToClient("You hit " + defender.Name, attacker.HubGuid);
                HubContext.SendToClient(
                    attacker.Name + " hits you ",
                    attacker.HubGuid,
                    defender.HubGuid,
                    false,
                    true);

                attacker.HitPoints -= 1;
            }
            else
            {
                HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
                HubContext.SendToClient(
                    attacker.Name + " misses you ",
                    attacker.HubGuid,
                    defender.HubGuid,
                    false,
                    true);
            }

            return null;

        }

        public static async Task Attacker(Player attacker, Player defender, Room room)
        {
            await Task.Delay(3000);
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
                HubContext.SendToClient(
                    attacker.Name + " hits you ",
                    attacker.HubGuid,
                    defender.HubGuid,
                    false,
                    true);

                defender.HitPoints -= 1;
            }
            else
            {
                HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
            }


            if (defender.HitPoints > 0)
            {
                //fight again
                  StartFight(attacker, room, defender.Name, true);

            }
            else
            {
                HubContext.SendToClient(defender.Name + " died", attacker.HubGuid);

                return;
            }
        }

        public static async Task Defender(Player defender, Player attacker, Room room)
        {
           await Task.Delay(3000);
            //Get Attacker Weapon
            string RightHand = defender.Equipment.RightHand;
            string LeftHand = defender.Equipment.LeftHand;
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
                RightHandWep = defender.Inventory.Find(x => x.name == RightHand && x.location == "Equipped");
                LeftHandWep = defender.Inventory.Find(x => x.name == LeftHand && x.location == "Equipped");
            }

            double offense = Offense(defender);
            double evasion = Evasion(attacker);

            double toHit = (offense / evasion) * 100;
            int chance = D100();


            if (toHit > chance)
            {
                HubContext.SendToClient("You hit " + attacker.Name, defender.HubGuid);
                HubContext.SendToClient(
                    defender.Name + " hits you ",
                    defender.HubGuid,
                    attacker.HubGuid,
                    false,
                    true);

                attacker.HitPoints -= 1;
            }
            else
            {
                HubContext.SendToClient("You miss " + attacker.Name, defender.HubGuid);
            }

            if (attacker.HitPoints > 0)
            {
                //fight again
                   StartFight(defender, room, attacker.Name, false);
            }
            else
            {
               
                HubContext.SendToClient(attacker.Name + " died", defender.HubGuid);

                return;
            }
        }

        public static Player FindTarget(Room room, string defender)
        {
            Player target = room.players.Find(x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase));

            return target;
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
