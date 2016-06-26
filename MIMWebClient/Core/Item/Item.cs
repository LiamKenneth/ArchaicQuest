using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.Item
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    public class Item :BaseItem
    {

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
        public AttackType attackType { get; set; }
        public EqSlot eqSlot { get; set; }
        public string weaponType { get; set; }
        public int weaponSpeed { get; set; }

    }
}
