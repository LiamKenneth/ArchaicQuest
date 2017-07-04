using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    using System.Data.Entity.Infrastructure.Pluralization;

    public class Fight2
    {
        /// <summary>
        /// Starts a fight between two players or mobs
        /// The defender can only fight it's original target 
        /// so cant fight back at multiple targets
        /// </summary>
        /// <param name="attacker">The attacker</param>
        /// <param name="room">The room</param>
        /// <param name="defenderName">The defenders Name for now</param>
        /// <returns></returns>
        public static void PerpareToFight(Player attacker, Room room, string defenderName, bool busy = false)
        {
            

            if (attacker == null)
            {
                return;
            }

            if (attacker.Target != null)
            {
                HubContext.SendToClient("You can only fight one target.", attacker.HubGuid);
                return;
            }

            /* player can only attack one target
           * if player gets attacked by something else they cannot fight back until
           * they have ended the fight they are already in.
           * player defence should be divided by the number of people they are being attacked by.
           * 
           */

            //automated Combat rounds for melee attacks

            //find defender 
            Player defender = FindValidTarget(room, defenderName, attacker);


            if (defender == null)
            {
                HubContext.SendToClient("No one here by that name.", attacker.HubGuid);
                return;
            }



            defender.Status = Player.PlayerStatus.Fighting;

            attacker.Status = !busy ? Player.PlayerStatus.Fighting : Player.PlayerStatus.Busy;
          

            AddFightersIdtoRoom(attacker, defender, room);

            StartFight(attacker, defender, room);

        }

        /// <summary>
        /// Starts a fight between two players or mobs
        /// The defender can only fight it's original target 
        /// so cant fight back at multiple targets
        /// </summary>
        /// <param name="attacker">The attacker</param>
        /// <param name="room">The room</param>
        /// <param name="defenderName">The defenders Name for now</param>
        /// <returns></returns>
        public static void PerpareToFightBack(Player attacker, Room room, string defenderName, bool busy = false)
        {


            Player defender = FindValidTarget(room, defenderName, attacker);


            if (defender == null)
            {
                HubContext.SendToClient("No one here by that name.", attacker.HubGuid);
                return;
            }

            defender.Status = Player.PlayerStatus.Fighting;
 
            AddFightersIdtoRoom(attacker, defender, room, false);

            if (attacker.Target == null)
            {
                attacker.Target = defender;
                attacker.Status = Player.PlayerStatus.Fighting;
            }

            StartFight(defender, attacker, room);

        }

        public static void AddFightersIdtoRoom(Player attacker, Player defender, Room room, bool includeAttacker = true)
        {

            if (room.fighting == null)
            {
                room.fighting = new List<string>();
            }

            if (attacker.HubGuid != null)
            {
                room.fighting.Add(attacker.HubGuid);
            }

            if (defender.HubGuid != null)
            {
                room.fighting.Add(defender.HubGuid);
            }

            

        }


        public static void StartFight(Player attacker, Player defender, Room room)
        {

        

            if (defender.Target == null || defender.Target == attacker)
            {
                defender.Target = attacker;

                if (!defender.ActiveFighting)
                {
                    Task.Run(() => HitTarget(defender, attacker, room, 1500));

                }

            }

            if (attacker.Target == null || attacker.Target == defender)
            {
                attacker.Target = defender;


                if (attacker.Status == Player.PlayerStatus.Fighting)
                {


                    double offense = Offense(attacker);
                    double evasion = Evasion(defender);

                    double toHit = (offense / evasion) * 100;
                    int chance = D100();


                    ShowAttack(attacker, defender, room, toHit, chance, null);

                }

                if (!attacker.ActiveFighting)
                {
                    Task.Run(() => HitTarget(attacker, defender, room, 1500));
                }
           
            }
            //3000, 3 second timer needs to be a method taking in players dexterity, condition and spells to determine speed.

       
              
        }

        public static Player FindValidTarget(Room room, string defender, Player attacker)
        {


            var findObject = Events.FindNth.Findnth(defender);
            int nth = findObject.Key;
            string itemToFind = findObject.Value;

            Player defendingPlayer = null;
            if (nth == -1)
            {
                  defendingPlayer =
                    room.players.FirstOrDefault(
                        x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                    ?? room.mobs.FirstOrDefault(x => x.Name.ToLower().Contains(defender.ToLower()));
            }
            else
            {
                  defendingPlayer =
                   room.players.Skip(nth - 1).FirstOrDefault(x => x.Name.StartsWith(itemToFind, StringComparison.CurrentCultureIgnoreCase)) ?? room.mobs.Skip(nth - 1).FirstOrDefault(x => x.Name.ToLower().Contains(itemToFind.ToLower()));
            }
         

            if (defendingPlayer == null)
            {
                HubContext.SendToClient("No one here", attacker.HubGuid);
                attacker.Status = Player.PlayerStatus.Standing;

                return null;
            }

            if (attacker.Name.Equals(defendingPlayer.Name))
            {
                HubContext.SendToClient("You can't kill yourself", attacker.HubGuid);
                return null;
            }


            if (attacker.HitPoints <= 0)
            {
                HubContext.SendToClient("You cannot attack anything while dead", attacker.HubGuid);
                attacker.Status = Player.PlayerStatus.Standing;
                return null;
            }

            if (defendingPlayer.HitPoints <= 0)
            {
                HubContext.SendToClient("They are already dead.", attacker.HubGuid);
                attacker.Status = Player.PlayerStatus.Standing;
                return null;
            }

            return defendingPlayer;
        }

        private static async Task HitTarget(Player attacker, Player defender, Room room, int delay)
        {

            try
            {

                attacker.ActiveFighting = true;

                while (attacker.HitPoints > 0 && defender.HitPoints > 0)
                {
                    //check still here
                    Player target = FindValidTarget(room, defender.Name, attacker);


                    if (target == null)
                    {
                        HubContext.SendToClient("No one here by that name", attacker.HubGuid);
                        return;
                    }

                    bool canAttack = CanAttack(attacker, defender);

                    if (canAttack)
                    {
                        bool alive = IsAlive(attacker, defender);



                        if (alive && attacker.Status != Player.PlayerStatus.Busy)
                        {

                            await Task.Delay(delay);

                            double offense = Offense(attacker);
                            double evasion = Evasion(defender);

                            double toHit = (offense/evasion)*100;
                            int chance = D100();

                            Player checkAttackerAgainAfterDelay = FindValidTarget(room, defender.Name, attacker);
                            Player checkDefenderAgainAfterDelay = FindValidTarget(room, attacker.Name, defender);

                            if (checkAttackerAgainAfterDelay == null || checkDefenderAgainAfterDelay == null)
                            {
                                //Fixes bug where fleeing you would still get hit and hit back while in another room.
                                return;
                            }

                            ShowAttack(attacker, defender, room, toHit, chance, null);


                            if (attacker.Type == Player.PlayerTypes.Player)
                            {
                                Score.UpdateUiPrompt(attacker);
                            }
                            if (defender.Type == Player.PlayerTypes.Player)
                            {
                                Score.UpdateUiPrompt(defender);
                            }
                        }



                    }
                }


            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "fight"
                };

                Save.LogError(log);

            }

        }

       
        /// <summary>
        /// Finds the defending player or NPC
        /// </summary>
        /// <param name="room">Current room</param>
        /// <param name="defender">Player or NPC name</param>
        /// <returns></returns>
        public static Player FindTarget(Room room, string defender)
        {
            Player target = room.players.FirstOrDefault(x => x.Name.StartsWith(defender, StringComparison.CurrentCultureIgnoreCase))
                            ?? room.mobs.FirstOrDefault(x => x.Name.ToLower().Contains(defender.ToLower()));

            return target;
        }

        public static bool CanAttack(Player attacker, Player defender)
        {

            bool canAttack = attacker.Target.Name == defender.Name;

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Resting)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Incapitated)
            {
                canAttack = false;
            }

            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Dead)
            {
                canAttack = false;
            }


            if (attacker.Status == PlayerSetup.Player.PlayerStatus.Ghost)
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

        public static int CalculateDamageReduction(Player attacker, Player defender, int damage)
        {
            int ArRating = ArmourRating(defender);
            int armourReduction = ArRating / damage;

            if (armourReduction > 4)
            {
                armourReduction = 4;
            }
            else if (armourReduction <= 0)
            {
                armourReduction = 1;
            }

            return armourReduction;
        }

        public static Item GetAttackerWepon(Player attacker)
        {
            //currently everyone is right handed :-O
            var wielded = attacker.Equipment.Wielded;

            if (wielded == "Nothing")
            {
                return null;
            }

            var weapon = attacker.Inventory.FirstOrDefault(x => x.name.Equals(wielded) && x.location.Equals(Item.ItemLocation.Worn));

            return weapon;
        }

        public static int HandToHandDamage(Player attacker)
        {
            var die = new PlayerStats();
            var handToHand = die.dice(1, 6);

            double handToHandSkill = attacker.Skills.Find(x => x.Name.Equals("HandToHand"))?.Proficiency ?? 0;

            var handToHandSkillModifier = (handToHandSkill / handToHand) * 100;
            var damage = (int)((handToHandSkillModifier + handToHand) * 1);

            return damage;
        }

        /// <summary>
        ///  (Weapon Damage * Strength Modifier * Condition Modifier * Critical Hit Modifier) / Armor Reduction.
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        public static int Damage(Player attacker, Player defender, int criticalHit)
        {
            // (Weapon Damage * Strength Modifier * Condition Modifier * Critical Hit Modifier) / Armor Reduction.
           
            int damage;

            var weapon = GetAttackerWepon(attacker);

            if (weapon != null)
            {
                Helpers helper = new Helpers();
                damage = helper.dice(1, weapon.stats.damMin, weapon.stats.damMax);
            }
            else
            {
                //Unarmed so use hand to hand
                damage = HandToHandDamage(attacker);
            }

            var armourReduction = CalculateDamageReduction(attacker, defender, damage);


            int totalDamage = (damage / armourReduction) * criticalHit;

            return totalDamage;
        }

        public static int ArmourRating(Player defender)
        {
            return 1 + defender.ArmorRating;
        }

        public static KeyValuePair<string, string> WeaponAttackName(Player attacker, Skill skillUsed)
        {
            if (skillUsed == null)
            {

                var wielded = attacker.Equipment.Wielded;
                Item weapon = null;
                if (wielded == "Nothing")
                {
                    return new KeyValuePair<string, string>("punch", "punches");
                }

                //find weapon
                weapon = attacker.Inventory.Find(x => x.name.Equals(wielded) && x.eqSlot.Equals(Item.EqSlot.Wielded));

                if (weapon != null)
                {

                    return new KeyValuePair<string, string>(weapon.name, weapon.name);
                }
                else
                {
                    return new KeyValuePair<string, string>("hit", "hit");
                }
                /// weapon.attackType = Item.AttackTypes.Slash;
                //add attack string to weapons

            }

          //  var checkPlural = new EnglishPluralizationService();
            

            //Skill / spell name here
            return new KeyValuePair<string, string>(skillUsed.Name, skillUsed.Name);

        }

        public static int CriticalHit(double toHit, int chance)
        {
            var die = new PlayerStats();
            var chanceMod = die.dice(1, 4);
            var critical = chance * chanceMod;

            if (toHit >= chance)
            {
                return 2;
            }
            return 1;
        }

        /// <summary>
        /// Shows attack and damage to player
        /// </summary>
        /// <param name="attacker">the attacker</param>
        /// <param name="defender">the defender</param>
        /// <param name="room">The rom</param>
        /// <param name="toHit">Target tohit value</param>
        /// <param name="chance">Chance to hit value</param>
        /// <param name="skillUsed">This is used for skills and spells only</param>
        /// <param name="damage">This is used for skills and spells only</param>
        public static void ShowAttack(Player attacker, Player defender, Room room, double toHit, int chance, Skill skillUsed, int damage = 0)
        {
            bool alive = IsAlive(attacker, defender);
            int IsCritical = CriticalHit(toHit, chance);

            if (alive)
            {

                if (toHit > chance)
                {
                    var dam = damage > 0 ? damage : Damage(attacker, defender, IsCritical);
                   
                    var damageText = DamageText(dam);

                    if (IsAlive(attacker, defender))
                    {

                        HubContext.SendToClient("Your " + WeaponAttackName(attacker, skillUsed).Key + " " + damageText.Value.ToLower() + " " + Helpers.ReturnName(defender, attacker, null) + " [" + dam + "]", attacker.HubGuid);

                        HubContext.SendToClient(Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Value + " " + damageText.Value.ToLower() + " you [" + dam + "]", defender.HubGuid);

                        

                        foreach (var player in room.players)
                        {
                            if (player != attacker && player != defender)
                            {
                                HubContext.SendToClient(Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Value + " " + damageText.Value.ToLower() + " " + Helpers.ReturnName(defender, attacker, null), player.HubGuid);
                            }
                        }


                        defender.HitPoints -= dam;

                        if (defender.HitPoints < 0)
                        {
                            defender.HitPoints = 0;
                        }


                        if (!IsAlive(attacker, defender))
                        {
                            IsDead(attacker, defender, room);
                        }
                    }

                }
                else
                {
                    if (IsAlive(attacker, defender))
                    {

                        HubContext.SendToClient("Your " + WeaponAttackName(attacker, skillUsed).Key + " misses " + Helpers.ReturnName(defender, attacker, null), attacker.HubGuid);

                        HubContext.SendToClient(Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Key + " misses you ", defender.HubGuid);

                        foreach (var player in room.players)
                        {
                            if (player != attacker && player != defender)
                            {
                                HubContext.SendToClient(Helpers.ReturnName(attacker, defender, null) + "'s " + WeaponAttackName(attacker, skillUsed).Key + " misses " + Helpers.ReturnName(defender, attacker, null), player.HubGuid);
                            }
                        }

                      

                    

                     

                    }
                }
            }


        }

        public string MissMessage(Player attacker, Player defender)
        {
            string[] missText = new[]
                                    {
                                        "You side step away from " + defender.Name + "'s attack",
                                        "You lean back out of the way of " + defender.Name + "'s attack",
                                        "You duck out of the way of " + defender.Name + "'s attack",
                                        "You weave out of the way of " + defender.Name + "'s attack"
                                    };

            return missText[Helpers.diceRoll.Next(missText.Length)];
        }


        public string HitMessage(Player attacker, Player defender)
        {
            string[] hitText = new[]
                                    {
                                        "You side step away from " + defender.Name + "'s attack",
                                        "You lean back out of the way of " + defender.Name + "'s attack",
                                        "You duck out of the way of " + defender.Name + "'s attack",
                                        "You weave out of the way of " + defender.Name + "'s attack"
                                    };

            return hitText[0];
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

                foreach (var player in room.players)
                {
                    if (player != defender)
                    {
                        HubContext.SendToClient(defender.Name + " dies ", player.HubGuid);
                    }
                    else
                    {
                        HubContext.SendToClient("You die", defender.HubGuid);
                    }
                }

              

                defender.Target = null;


                //Turn corpse into room item
                var defenderCorpse = new Item
                {
                    name = "The corpse of " + defender.Name,
                    container = true,
                    containerItems = new List<Item>(),
                    description = new Description { look = "The slain corpse of " + defender.Name + " is here.", room = "The slain corpse of " + defender.Name }
                };

                foreach (var invItem in defender.Inventory)
                {
                    invItem.location = Item.ItemLocation.Room;
                    defenderCorpse.containerItems.Add(invItem);

                }

                

                var oldRoom = room;
                room.items.Add(defenderCorpse);
               room.corpses.Add(defender);

                if (defender.Type == Player.PlayerTypes.Mob || string.IsNullOrEmpty(defender.HubGuid))
                {
                    room.mobs.Remove(defender);
                }
                else
                {
                    //room.players.Remove(defender);
                    //Add slain player to recall
                }

                defender.Target = null;

                if (attacker.Target.Name == defender.Name)
                {
                    attacker.Target = null;
                    attacker.Status = PlayerSetup.Player.PlayerStatus.Standing;
                }
 
                defender.Status = defender.Type == Player.PlayerTypes.Player ? PlayerSetup.Player.PlayerStatus.Ghost : PlayerSetup.Player.PlayerStatus.Dead;

                Cache.updateRoom(room, oldRoom);

                var xp = new Experience();

                int xpGain = xp.GainXp(attacker, defender);
                attacker.Experience += xpGain;
                attacker.TotalExperience += xpGain;
                HubContext.SendToClient(xpGain + "XP", attacker.HubGuid);

                xp.GainLevel(attacker);
                //calc xp
                //create corpse

                foreach (var player in room.players)
                {

                    var roomdata = LoadRoom.DisplayRoom(room, player.Name);
                    Score.UpdateUiRoom(player, roomdata);
                }

                if (attacker.HubGuid != null)
                {
                    room.fighting.Remove(attacker.HubGuid);
                }

                if (defender.HubGuid != null)
                {
                    room.fighting.Remove(defender.HubGuid);
                }

                //remove followers

                if (defender.Following != null)
                {

                    if (defender.Followers.Count > 0)
                    {
                        foreach (var follower in defender.Followers)
                        {
                            if (follower.HubGuid != null)
                            {
                                HubContext.SendToClient("You stop following " + defender.Name, follower.HubGuid);
                            }
                           
                        }
                    }
                   
                    defender.Followers = null;
                    defender.Following = null;

                }

                // check if defender is following?
                if (attacker.Followers?.FirstOrDefault(x => x.Equals(defender)) != null)
                {
                    attacker.Followers.Remove(defender);

                    if (attacker.HubGuid != null)
                    {
                        HubContext.SendToClient(defender.Name + " stops following you", attacker.HubGuid);
                    }
                   
                }
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

            double off = weaponSkill + (strength / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

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


            var hasHaste = player.Affects?.FirstOrDefault(
                             x => x.Name.Equals("Haste", StringComparison.CurrentCultureIgnoreCase)) != null;

            if (hasHaste)
            {
                dexterity += 5;
            }

            //((Agility / 5) + (Luck / 10)) * (0.75 + 0.5 * Current Fatigue / Maximum Fatigue)

            double evade = evasionSkill + blockSkill + parrySkill + (dexterity / 5) * (0.75 + 0.5 * player.MovePoints / player.MaxMovePoints);

            return evade;
        }

     
    }
}
