using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Security.Cryptography.X509Certificates;

    using MIMEngine.Core.Item;
    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    public class Fight2
    {
        /// <summary>
        /// Starts a fight between two players or mobs
        /// The defender can only fight it's original target 
        /// so cant fight back at multiple targets
        /// </summary>
        /// <param name="attacker">The attacker</param>
        /// <param name="room">The room</param>
        /// <param name="attackOptions">The attackers Name for now</param>
        /// <returns></returns>
        public static async Task StartFight(Player attacker, Room room, string attackOptions)
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

            if (attacker.HitPoints <= 0)
            {
                return;
            }

            if (defender.HitPoints <= 0)
            {
                return;
            }

            defender.Status = "Fighting";
            attacker.Status = "Fighting";
            room.fighting.Add(attacker.HubGuid);
            room.fighting.Add(defender.HubGuid);

            if (attacker.Target == null)
            {
                attacker.Target = defender;
            }

            if (defender.Target == null)
            {
                defender.Target = attacker;
            }


            double offense = Offense(attacker);
            double evasion = Evasion(defender);

            double toHit = (offense / evasion) * 100;
            int chance = D100();


            ShowAttack(attacker, defender, room, toHit, chance);

            HitTarget(attacker, defender, room, 5000);

            HitTarget(defender, attacker, room, 5000);


            IsDead(attacker, defender, room);



        }

        private static async Task HitTarget(Player attacker, Player defender, Room room, int delay)
        {


            while (attacker.HitPoints > 0 && defender.HitPoints > 0 && attacker.Status == "Fighting" && defender.Status == "Fighting")
            {
                bool canAttack = CanAttack(attacker, defender);

                if (canAttack)
                {
                    await Task.Delay(delay);

                    double offense = Offense(attacker);
                    double evasion = Evasion(defender);

                    double toHit = (offense / evasion) * 100;
                    int chance = D100();


                    ShowAttack(attacker, defender, room, toHit, chance);

                    Core.Player.Prompt.ShowPrompt(attacker);
                    Core.Player.Prompt.ShowPrompt(defender);
                }

            }

        }

        public static Player FindTarget(Room room, string defender)
        {
            Player target = room.players.Find(x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase));

            return target;
        }

        public static bool CanAttack(Player attacker, Player defender)
        {

            bool canAttack = true;

            if (attacker.Status == "Standing" || defender.Status == "Standing")
            {
                canAttack = false;
            }

            if (attacker.Target.Name != defender.Name)
            {
                canAttack = false;
            }

            if (attacker.Status == "Sleeping")
            {
                canAttack = false;
            }

            return canAttack;
        }

        /// <summary>
        ///  (Weapon Damage * Strength Modifier * Condition Modifier * Critical Hit Modifier) / Armor Reduction.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public static int Damage(Player attacker, Player defender)
        {
            // (Weapon Damage * Strength Modifier * Condition Modifier * Critical Hit Modifier) / Armor Reduction.
            int strength = attacker.Strength;
            int MaxDamage = 0;
            int MinDamage = 0;
            int damage = 0;
            var wielded = attacker.Equipment.RightHand;
            Item weapon = null;
            if (wielded == "Nothing")
            {
                damage = 5;
            }
            else
            {
                //find weapon
                weapon = attacker.Inventory.Find(x => x.name.Equals(weapon) && x.location.Equals("worn"));
            }

            if (weapon != null)
            {
                damage = weapon.stats.damMax;
            }

          int ArmourRedux = ArmourReduction(defender);

             
          return  ArmourRedux / damage * strength;
        }

        public static int ArmourReduction(Player defender)
        {
            return defender.ArmorRating;
        }

        public static void ShowAttack(Player attacker, Player defender, Room room, double toHit, int chance)
        {
            if (toHit > chance)
            {
                int dam = Damage(attacker, defender);

                HubContext.SendToClient("You hit " + defender.Name + "[" + dam +"]", attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " hits you [" + dam +"]", defender.HubGuid);
                //BUG: this also broadcast to the players fighting
                defender.HitPoints -= dam;
                HubContext.SendToAllExcept(attacker.Name + " hits " + defender.Name, room.fighting, room.players);


                
            }
            else
            {
                HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " misses you ", defender.HubGuid);
                HubContext.SendToAllExcept(attacker.Name + " misses " + defender.Name, room.fighting, room.players);
            }
        }

        public static void IsDead(Player attacker, Player defender, Room room)
        {
            if (defender.HitPoints <= 0)
            {
                HubContext.SendToAllExcept(defender.Name + " dies ", room.fighting, room.players);
                HubContext.SendToClient("You die", defender.HubGuid);

                defender.Status = "Standing";
                attacker.Status = "Standing";
                //calc xp
                //create corpse
            }

            if (attacker.HitPoints <= 0)
            {
                HubContext.SendToAllExcept(attacker.Name + " dies ", room.fighting, room.players);
                HubContext.SendToClient("You die", attacker.HubGuid);

                defender.Status = "Standing";
                attacker.Status = "Standing";
                //calc xp

                //create corpse
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
