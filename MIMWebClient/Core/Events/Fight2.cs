using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Events
{
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    using MIMWebClient.Core.Item;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;
    using Player;
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
        public static void StartFight(Player attacker, Room room, string attackOptions)
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

            if (attacker.Name.Equals(defender.Name))
            {
                HubContext.SendToClient("You can't kill yourself", attacker.HubGuid);
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

            defender.Status = Player.PlayerStatus.Fighting;
            attacker.Status = Player.PlayerStatus.Fighting;

            if (room.fighting == null)
            {
                room.fighting = new List<string>();
            }

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


            Task.Run(() => HitTarget(attacker, defender, room, 1000));
            Task.Run(() => HitTarget(defender, attacker, room, 1000));

            
 
     
            

        }

        private static async Task HitTarget(Player attacker, Player defender, Room room, int delay)
        {


            while (attacker.Status == PlayerSetup.Player.PlayerStatus.Fighting && defender.Status == PlayerSetup.Player.PlayerStatus.Fighting)
            {
                bool canAttack = CanAttack(attacker, defender);

                if (canAttack)
                {
                    bool alive = IsAlive(attacker, defender);

                    if (alive)
                    {

                           await Task.Delay(delay);

                        double offense = Offense(attacker);
                        double evasion = Evasion(defender);

                        double toHit = (offense / evasion) * 100;
                        int chance = D100();


                        ShowAttack(attacker, defender, room, toHit, chance);

                        //Prompt.ShowPrompt(attacker);
                        //Prompt.ShowPrompt(defender);
                        Score.UpdateUiPrompt(attacker);
                        Score.UpdateUiPrompt(defender);
                    }

                }

            }

        }

        public static Player FindTarget(Room room, string defender)
        {
            Player target = room.players.Find(x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                            ?? room.mobs.Find(x => x.Name.ToLower().Contains(defender.ToLower()));

            return target;
        }

        public static bool CanAttack(Player attacker, Player defender)
        {

            bool canAttack = !(attacker.Status == PlayerSetup.Player.PlayerStatus.Standing || defender.Status == PlayerSetup.Player.PlayerStatus.Standing);

            if (attacker.Target.Name != defender.Name)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                canAttack = false;
            }

            //using skill, casting. 
            // probably should set busy when picking up items(disarmed weapon)
            // wielding an item 
            // getting potion
            // quaffing
            // disallow changing armour while in combat
            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Busy)
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
            int damage = 0;
            var wielded = attacker.Equipment.RightHand;
            Item weapon = null;
            if (wielded == "Nothing")
            {
                //weapon skill * 0.075 * critical hit modifier
                damage = (int)(50 * 0.025 * 1);
            }
            else
            {
                //find weapon
                weapon = attacker.Inventory.Find(x => x.name.Equals(wielded) && x.location.Equals("worn"));
            }

            if (weapon != null)
            {
                Helpers helper = new Helpers();
                damage = helper.dice(1, weapon.stats.damMin, weapon.stats.damMax);
            }

            int ArRating = ArmourRating(defender);


            int armourReduction = ArRating / damage * strength;

            if (armourReduction > 4)
            {
                armourReduction = 4;
            }
            else if (armourReduction <= 0)
            {
                armourReduction = 1;
            }

            int totalDamage = damage + strength / armourReduction;

            return totalDamage;
        }

        public static int ArmourRating(Player defender)
        {
            return 1 + defender.ArmorRating;
        }

        public static KeyValuePair<string, string> WeaponAttackName(Player attacker)
        {
            var wielded = attacker.Equipment.RightHand;
            Item weapon = null;
            if (wielded == "Nothing")
            {
               return new KeyValuePair<string, string>("punch", "punches");
            }
          
            //find weapon
            weapon = attacker.Inventory.Find(x => x.name.Equals(wielded) && x.location.Equals("worn"));

           /// weapon.attackType = Item.AttackTypes.Slash;
            //add attack string to weapons
            return new KeyValuePair<string, string>("","");
        }

        public static void ShowAttack(Player attacker, Player defender, Room room, double toHit, int chance)
        {
            bool alive = IsAlive(attacker, defender);

            if (alive)
            {

                if (toHit > chance)
                {
                    int dam = Damage(attacker, defender);
                    var damageText = DamageText(dam);

                    if (attacker.Type == "Player")
                    {
                        HubContext.SendToClient("Your hit " + damageText.Value + " " + defender.Name + "[" + dam + "]", attacker.HubGuid);
                    }
                    if (defender.Type == "Player")
                    {
                        HubContext.SendToClient(attacker.Name + "'s hit " + damageText.Value + " you [" + dam + "]", defender.HubGuid);
                    }

                    defender.HitPoints -= dam;

                    if (defender.HitPoints < 0)
                    {
                        defender.HitPoints = 0;
                    }
                    HubContext.SendToAllExcept(attacker.Name + "'s hit " + damageText.Value + " " + defender.Name, room.fighting, room.players);

                    if (!IsAlive(attacker, defender))
                    {
                        IsDead(attacker, defender, room);
                    }

                }
                else
                {
                    if (attacker.Type == "Player")
                    {
                        HubContext.SendToClient("You miss " + defender.Name, attacker.HubGuid);
                    }
                    if (defender.Type == "Player")
                    {
                        HubContext.SendToClient(attacker.Name + " misses you ", defender.HubGuid);
                    }
                    HubContext.SendToAllExcept(attacker.Name + " misses " + defender.Name, room.fighting, room.players);
                }
            }


        }

        public string MissMessage(Player attacker, Player defender)
        {
            string[] missText = new[]
                                    {
                                        "You side step out of the way of " + defender.Name + "'s attack",
                                        "You lean back out of the way of " + defender.Name + "'s attack",
                                        "You duck out of the way of " + defender.Name + "'s attack",
                                        "You weave out of the way of " + defender.Name + "'s attack"
                                    };

            return missText[0];
        }

        public static KeyValuePair<string, string> DamageText(int damage)
        {
            switch (damage)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    return new KeyValuePair<string, string>("scratch", "scratches");
                case 5:
                case 6:
                case 7:
                case 8:
                    return new KeyValuePair<string, string>("graze", "grazes");
                case 9:
                case 10:
                case 11:
                case 12:
                    return new KeyValuePair<string, string>("hit", "hits");
                case 13:
                case 14:
                case 15:
                case 16:
                    return new KeyValuePair<string, string>("injure", "injures");
                case 17:
                case 18:
                case 19:
                case 20:
                    return new KeyValuePair<string, string>("wound", "wounds");
                case 21:
                case 22:
                case 23:
                case 24:
                    return new KeyValuePair<string, string>("maul", "mauls");
                case 25:
                case 26:
                case 27:
                case 28:
                    return new KeyValuePair<string, string>("decimate", "decimates");
                case 29:
                case 30:
                case 31:
                case 32:
                    return new KeyValuePair<string, string>("devastate", "devastates");
                case 33:
                case 34:
                case 35:
                case 36:
                    return new KeyValuePair<string, string>("maim", "maims");
                case 37:
                case 38:
                case 39:
                case 40:
                    return new KeyValuePair<string, string>("MUTILATE", "MUTILATES");
                case 41:
                case 42:
                case 43:
                case 44:
                    return new KeyValuePair<string, string>("DISEMBOWEL", "DISEMBOWELS");
                case 45:
                case 46:
                case 47:
                case 48:
                    return new KeyValuePair<string, string>("MASSACRE", "MASSACRES");
                case 49:
                case 50:
                case 51:
                case 52:
                    return new KeyValuePair<string, string>("*** DEMOLISH ***", "*** DEMOLISHS ***");
                default:
                    return new KeyValuePair<string, string>("*** ANNIHILATES ***", "*** ANNIHILATES ***"); ;
            }


        }

        public static void IsDead(Player attacker, Player defender, Room room)
        {
            if (defender.HitPoints <= 0)
            {

                HubContext.SendToAllExcept(defender.Name + " dies ", room.fighting, room.players);

                if (defender.Type == "Player")
                {
                    HubContext.SendToClient("You die", defender.HubGuid);
                }
                if (attacker.Type == "Player")
                {
                    HubContext.SendToClient(defender.Name + " dies", attacker.HubGuid);
                }

                var defenderCorpse = defender;

                //unequip
                foreach (var item in defenderCorpse.Inventory)
                {
                    item.location = Item.ItemLocation.Inventory;
                }
                var oldRoom = room;
                room.corpses.Add(defender);

                attacker.Status = PlayerSetup.Player.PlayerStatus.Standing;

                defender.Status = defender.Type == "Player" ? PlayerSetup.Player.PlayerStatus.Ghost : PlayerSetup.Player.PlayerStatus.Dead;

                Cache.updateRoom(room, oldRoom);

                var xp = new Experience();

                int xpGain = xp.MobXP(attacker, defender);
                attacker.Experience += xpGain;
                HubContext.SendToClient("You gain " + xpGain + "XP", attacker.HubGuid);
                //calc xp
                //create corpse
            }

            if (attacker.HitPoints <= 0)
            {
                HubContext.SendToAllExcept(attacker.Name + " dies ", room.fighting, room.players);

                if (attacker.Type == "Player")
                {
                    HubContext.SendToClient("You die", attacker.HubGuid);
                }

                if (defender.Type == "Player")
                {
                    HubContext.SendToClient(attacker.Name + " dies", defender.HubGuid);
                }

                var attackerCorpse = attacker;

                //unequip
                foreach (var item in attackerCorpse.Inventory)
                {
                    item.location = Item.ItemLocation.Inventory;
                }
                var oldRoom = room;
                room.corpses.Add(attacker);

                defender.Status = PlayerSetup.Player.PlayerStatus.Standing;

                attacker.Status = attacker.Type == "Player" ? PlayerSetup.Player.PlayerStatus.Ghost : PlayerSetup.Player.PlayerStatus.Dead;

                Cache.updateRoom(room, oldRoom);

                var xp = new Experience();

               int xpGain = xp.MobXP(defender, attacker);
                defender.Experience += xpGain;
                HubContext.SendToClient("You gain " + xpGain + "XP", defender.HubGuid);


                //calc xp

                //create corpse
            }
        }

        public static bool IsAlive(Player attacker, Player defender)
        {
            if (defender.HitPoints <= 0)
            {
                return false;
            }

            if (attacker.HitPoints <= 0)
            {
                return false;
            }

            return true;
        }

        public static int D100()
        {
            var die = new PlayerStats();
            int roll = die.dice(1, 100);

            return roll;
        }

        public static double Offense(Player player)
        {
            //need error checking
            //workout wepean type check weapon skill
            //var weapon = player.Inventory.Find(x => x.location.Equals(Item.ItemLocation.Worn.ToString() && x.eqSlot.Equals(Item.EqSlot.Hand)));

            var weapon = player.Inventory.Find(x => x.location == Item.ItemLocation.Wield);
            Item.WeaponType weaponType;
            double weaponSkill = 0;

            if (weapon != null)
            {
                 weaponType = weapon.weaponType;
                 weaponSkill = player.Skills.Find(x => x.Name == weaponType.ToString())?.Proficiency ?? 0;

            }
            else
            {
                //Hand To Hand

                weaponType = Item.WeaponType.HandToHand;
                weaponSkill = player.Skills.Find(x => x.Name == "Hand to Hand")?.Proficiency ?? 0;
 
            }


            int dexterity = player.Dexterity;
            int strength = player.Strength;


            //(Weapon Skill + (Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue);

            double off = weaponSkill + (dexterity / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

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
