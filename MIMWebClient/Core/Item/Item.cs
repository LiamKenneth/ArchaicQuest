using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Item
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public class Item : BaseItem
    {

        public enum ItemFlags
        {
            glow, //affect n/a
            hum,  //affect n/a
            dark, //affect n/a
            evil, //glow on det.evil
            bless, // +20% resist dam. 2 x duration poison
            invis, //invisible
            hidden, //hidden
            nodrop, // cannot drop
            noremove, //cannot remove w/o remove curse
            antievil, //zap if align -350 & lower
            antigood, //zap if align +350 & lower
            antineutral, //zap if align -350 to +350
            nolocate,   //locate spell fails
            equipable, // can be equipped
            container, // isContainer
            locked // container only
        }


        public enum ItemLocation
        {
            Room,
            Inventory,
            Worn
        }


        public enum EqSlot
        {
            Floating,
            Head,
            Face,
            Eyes,
            Ear,
            Neck,
            Cloak,
            AboutBody,
            Body,
            Waist,
            Sheath,
            Back,
            Wrist,
            Hand,
            Ring,
            Legs,
            Feet
        }

        public enum AttackType
        {
            Slash,
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
            Axe,
            Dagger,
            Exotic,
            Flail,
            Blunt,
            Polearm,
            Spear,
            Staff,
            Sword,
            Whip,
            Bows,
            Arrows
        }

        public enum ArmourType
        {
            Cloth,
            Leather,
            [Description("Studded Leather")]
            StuddedLeather,
            [Description("Chain Mail")]
            ChainMail,
            [Description("Plate Mail")]
            PlateMail,


        }

        public List<Item> containerItems;
        public DamageType damageType { get; set; }
        public ArmourType armourType { get; set; }
        public ItemFlags itemFlags { get; set; }
        public AttackType attackType { get; set; }
        public EqSlot eqSlot { get; set; }
        public WeaponType weaponType { get; set; }
        public int weaponSpeed { get; set; }

    }
}
