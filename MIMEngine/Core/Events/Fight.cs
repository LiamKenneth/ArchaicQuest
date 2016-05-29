using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Events
{
    using System.Threading;

    using MIMEngine.Core.PlayerSetup;

    public class Fight
    {
       public static void StartFight(Player attacker, Player defender)
       {
           
       }

        public static void CombatRound(Player attacker, Player defender)
        {
            var round = new System.Timers.Timer { Interval = (2000) };

             
        }
    }
}
