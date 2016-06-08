using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Item
{
   
    public class Item :BaseItem
    {

        public enum AttackTypes
        {
            Slash,
            Stab,
            Chop,
            Pierce,
            Punch
        }

        public List<Item> containerItems;
        public string attackType { get; set; }
        public string weaponType { get; set; }
        public int weaponSpeed { get; set; }

    }



}
