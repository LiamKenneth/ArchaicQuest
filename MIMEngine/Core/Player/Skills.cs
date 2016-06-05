using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.Player
{
   public class Skills
    {
       public string Name { get; set; }

       public int Skill { get; set; }
    
        //Says what status a skill can be used from
        // Fighting means in a fight only
        //Resting means everything
        //standing means all but resting
        //can't use skills while asleep
       public string UsableFromStatus { get; set; }

       public int LevelObtained { get; set; }

        public string HelpText { get; set; }

        public string Syntax { get; set; }
    }
}
