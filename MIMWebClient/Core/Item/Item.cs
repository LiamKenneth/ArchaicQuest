using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Item
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public class Item : BaseItem
    {


  //      None Light     Scroll Wand        Staff Weapon
  //Fireweapon Missile   Treasure Armor       Potion Worn
  //Furniture Trash     Oldtrap Container   Note Drink_con
  //Key Food      Money Pen         Boat Corpse_npc
  //Corpse_pc Fountain  Pill Blood       Bloodstain Scraps
  //Pipe Herb_con  Herb Incense     Fire Book
  //Switch Lever     Pullchain Button      Dial Rune
  //Runepouch Match     Trap Map         Portal Paper
  //Tinder Lockpick  Spike Disease     Oil Fuel
  //Short_bow Long_bow  Crossbow Projectile  Quiver Shovel
  //Salve

        public enum ItemType
        {
            Armour,
            Book,
            Container,
            Food,
            Drink,
            Key,
            light,
            Misc,
            note,
            Potion,
            Weapon,
            Gold,
            Silver,
            Copper       
        }

        public enum ItemFlags
        {
            antievil, //zap if align -350 & lower
            antigood, //zap if align +350 & lower
            antineutral, //zap if align -350 to +350
            bless, // +20% resist dam. 2 x duration poison
            container, // isContainer
            dark, //affect n/a
            equipable, // can be equipped
            evil, //glow on det.evil
            glow, //affect n/a
            hidden, //hidden
            hum,  //affect n/a
            invis, //invisible
            locked, // container only
            nodrop, // cannot drop
            nolocate,   //locate spell fails
            noremove, //cannot remove w/o remove curse


        }


        public enum ItemLocation
        {
            Room,
            Inventory,
            Worn,
            Wield
        }


        public enum EqSlot
        {
            Light,
            Finger,
            Neck,
            Face,
            Head,
            Torso,
            Legs,
            Feet,
            Hands,
            Arms,
            Body,
            Waist,
            Wrist,
            Wielded,
            Held,
            Shield,
            Floating
           
        }

        public enum AttackType
        {
            Slash = 1,
            Slice,
            Stab,
            Thrust,
            Chop,
            Claw,
            Cleave,
            Charge,
            Crush,
            Smash,
            Pierce,
            Punch,
            Pound,
            Scratch,
            Slap,
            Whip,
        }

        public enum DamageType
        {
            Acidic,
            Blast,
            Chill,
            Flame,
            Flaming,
            Freezing,
            Shocking,
            Holy,
            Cursed,
            Vampric,
            Poisoned,
            Stun
        }

        public enum WeaponType
        {
            Axe = 1,
            Exotic = 2,
            Flail = 3,
            Blunt = 4,
            Polearm = 5,
            Spear,
            Staff,
            LongBlades,
            ShortBlades,
            Whip,
            Bows,
            Arrows,
            Crossbow,
            Bolt,
            HandToHand
        }

        public enum ArmourType
        {
            Cloth = 1,
            Leather,
            [Display(Name = "Studded Leather")]
            StuddedLeather,
            [Display(Name = "Chain Mail")]
            ChainMail,
            [Display(Name = "Plate Mail")]
            PlateMail,


        }

        public ItemContainer containerItems;
        public List<DamageType> damageType { get; set; }
        public ArmourType armourType { get; set; }
        public List<ItemFlags> itemFlags { get; set; }
        public ItemType type { get; set; }
        public AttackType attackType { get; set; }
        public EqSlot eqSlot { get; set; }
        public WeaponType weaponType { get; set; }
        public int weaponSpeed { get; set; }
        public bool isHiddenInRoom { get; set; }
        public int count { get; set; }
        public int Gold { get; set; }
        public bool QuestItem { get; set; }
        public ArmourRating ArmorRating { get; set; }   
        public double Weight { get; set; }
 
        /// <summary>
        /// how many ticks till item decays
        /// </summary>
        public int decayTimer { get; set; } = 0;
    }
}
