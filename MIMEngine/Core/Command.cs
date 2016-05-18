using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMEngine.Core
{
    using MIMEngine.Core.Events;
    using MIMEngine.Core.Room;

    using Newtonsoft.Json.Linq;

    public class Command
    {
    //public static Dictionary<string, Action> commandList { get; set; }
        public static Dictionary<string, Action> Commands(string commandOptions, PlayerSetup.Player playerData, Room.Room room)
        {
            var commandList = new Dictionary<String, Action>(); 
            commandList.Add("north", () => Movement.Move(playerData, room, "North"));
            commandList.Add("look", () => LoadRoom.ReturnRoom(room, commandOptions, "look"));
            commandList.Add("examine", () => LoadRoom.ReturnRoom(room, commandOptions, "examine"));
            commandList.Add("touch", () => LoadRoom.ReturnRoom(room, commandOptions, "touch"));
            commandList.Add("smell", () => LoadRoom.ReturnRoom(room, commandOptions, "smell"));
            commandList.Add("taste", () => LoadRoom.ReturnRoom(room, commandOptions, "taste"));
            commandList.Add("score", () => Score.ReturnScore(playerData));
//            commandList.Add("hello", sayHello);
//            commandList.Add("Yo", () => sayHello(commandOptions));
//            commandList.Add("North", () => Move("North"));
//            commandList.Add("East", () => Move("East"));
//            commandList.Add("West", () => Move("West")) 
            return commandList;
        }

 

        public static void ParseCommand(string input, PlayerSetup.Player playerData, Room.Room room = null)
        {

            //testing
            string enteredCommand = input;
            string[] commands = enteredCommand.Split(' ');
            string commandKey = commands[0];
            string commandOptions = string.Empty;
            // testing
 
            if (commands.Length >= 2)
            {
                commandOptions = enteredCommand.Substring(enteredCommand.IndexOf(' ', 1)).Trim();
            }
 
             //TODO: do this only once
            var command = Commands(commandOptions, playerData, room);
 
            var fire = command.FirstOrDefault(x => x.Key.StartsWith(commandKey));

            if (fire.Value != null)
            {
                fire.Value();
            }
            else
            {
                HubProxy.MimHubServer.Invoke("SendToClient", "Sorry you can't do that.");
            }
           
        }
    }
}