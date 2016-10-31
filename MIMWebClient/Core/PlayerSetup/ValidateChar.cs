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
            var doesNotExist = false;

            if (player != null)
            {
                doesNotExist = true;
            }

            //send back to ui here

            return doesNotExist;
        }


        public bool CheckPassword(string hubguid, string characterName, string password)
        {
            var context = HubContext.getHubContext;
            if (password == null)
            {
                context.Clients.Client(hubguid).characterPasswordError("Yeah you need a password");
                return false;
            }

            var player = Save.GetPlayer(characterName);

            var match = player.Password.Equals(password);

            return match;
        }


        public bool CharacterisValid(string hubguid, string characterName, string password)
        {
            var context = HubContext.getHubContext;

            if (characterName == null)
            {
                context.Clients.Client(hubguid).characterNameLoginError("Your character name must be atleast 3 characters long");

                return false;
            }
            
           
            if (characterName.Length <= 0)
            {
                context.Clients.Client(hubguid).characterNameLoginError("Your character name must be atleast 3 characters long");

                return false;

            }
            

            var foundChar = CharacterExist(characterName);

            if (foundChar == false)
            {
                context.Clients.Client(hubguid).characterNameLoginError("No Character exists by the name of " + characterName);
                return false;
            }

            context.Clients.Client(hubguid).characterNameLoginError("");

            var passMatch = CheckPassword(hubguid, characterName, password);

            if (passMatch == false)
            {
                context.Clients.Client(hubguid).characterPasswordError("Password incorrect");

                return false;

            }
           
         
                context.Clients.Client(hubguid).characterPasswordError("");
             

            return true;

            //All good lets login and play
        }

    }
}
