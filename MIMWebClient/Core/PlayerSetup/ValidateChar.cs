using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core.PlayerSetup
{
    using MIMWebClient.Core.Events;

    class ValidateChar
    {

        public bool CharacterExist(string characterName)
        {
            var player = Save.GetPlayer(characterName);
            bool exists = player == null;

            return exists;
        }
    }
}
