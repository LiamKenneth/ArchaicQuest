using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core.PlayerSetup
{
    public class PlayerAccount
    {
        public static string Login(string playerName)
        {
            if (!string.IsNullOrEmpty(playerName) && playerName.Length >= 3)
            {
                PlayerSetup.CharacterSetup();
                return null;
            }
            else
            {
                return "You must enter a name with atleast 3 characters";
            }
        }
    }
}
