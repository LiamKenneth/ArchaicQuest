using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core
{
    using MIMEngine.Core.Events;

    using Newtonsoft.Json.Linq;

    public class Command
    {

        public static Dictionary<string, Action> Commands(string commandOptions, PlayerSetup.PlayerSetup playerData, JObject room)
        {
            var commandList = new Dictionary<String, Action>(); 
            commandList.Add("north", () => Move.MoveCharacter(playerData, room, "North"));
            commandList.Add("look", () => LoadRoom.ReturnRoom(room));
            commandList.Add("score", () => Score.ReturnScore(playerData));
//            commandList.Add("hello", sayHello);
//            commandList.Add("Yo", () => sayHello(commandOptions));
//            commandList.Add("North", () => Move("North"));
//            commandList.Add("East", () => Move("East"));
//            commandList.Add("West", () => Move("West")) 
            return commandList;
        }

 

        public static void ParseCommand(string input, PlayerSetup.PlayerSetup playerData, JObject room = null)
        {

            //testing
            string enteredCommand = input;
            string[] commands = enteredCommand.Split(' ');
            string commandKey = commands[0];
            string commandOptions = string.Empty;
            // testing
 
            if (commands.Length >= 2)
            {
                commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(' ', 1));
            }
 
 
            var command = Commands(commandOptions, playerData, room);
 
            var fire = command.FirstOrDefault(x => x.Key.StartsWith(commandKey));

            fire.Value();
        }
    }
}