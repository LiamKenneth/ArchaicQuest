using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{

    using MIMEngine.Core.PlayerSetup;
    using MIMEngine.Core.Room;

    public class Fight2
    {

        public static async Task StartFight(Player attacker, Room room, string attackOptions, bool isAttacker, bool initFight)
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

            

   

            IsDead(attacker, defender);

           

        }

        private static async Task HitTarget(Player attacker, Player defender, Room room, int delay)
        {


            while (attacker.HitPoints > 0 && defender.HitPoints > 0 && attacker.Status == "Fighting" && defender.Status == "Fighting")
            {
                await Task.Delay(delay);

                if (attacker.Status == "Standing" || defender.Status == "Standing")
                {
                    return;
                }

                if (attacker.Target.Name != defender.Name)
                {
                    return;
                }

                double offense = Offense(attacker);
                double evasion = Evasion(defender);

                double toHit = (offense / evasion) * 100;
                int chance = D100();


                ShowAttack(attacker, defender, room, toHit, chance);

                Core.Player.Prompt.ShowPrompt(attacker);
                Core.Player.Prompt.ShowPrompt(defender);

            }



        }

        public static Player FindTarget(Room room, string defender)
        {
            Player target = room.players.Find(x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase));

            return target;
        }

        public static void ShowAttack(Player attacker, Player defender, Room room, double toHit, int chance)
        {
            if (toHit > chance)
            {
                HubContext.SendToClient("You hit " + defender.Name, attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " hits you ", defender.HubGuid);
                HubContext.broadcastToRoom(attacker.Name + " hits " + defender.Name, room.players, attacker.HubGuid, true);

                defender.HitPoints -= 1;
            }
            else
            {
                HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
                HubContext.SendToClient(attacker.Name + " misses you ", defender.HubGuid);
                HubContext.broadcastToRoom(attacker.Name + " misses " + defender.Name, room.players, attacker.HubGuid, true);
            }
        }

        public static void IsDead(Player attacker, Player defender)
        {
            if (defender.HitPoints <= 0)
            {
                HubContext.SendToClient(defender.Name + " dies", attacker.HubGuid);
                HubContext.SendToClient("You die", defender.HubGuid);

                defender.Status = "Standing";
                attacker.Status = "Standing";
                //calc xp
                //create corpse
            }

            if (attacker.HitPoints <= 0)
            {
                HubContext.SendToClient(attacker.Name + " dies", defender.HubGuid);
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
