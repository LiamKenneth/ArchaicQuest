using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Core
{
    using System.Collections.Concurrent;

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player;
    using MIMWebClient.Core.Room;

    using Newtonsoft.Json.Linq;
    using System.Threading;
    using Mob.Events;
    using World.Tutorial;

    public class Event
    {


 

        //public static Dictionary<string, Action> commandList { get; set; }
        public static Dictionary<string, Action> Events(string eventName, PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room, string option = "", string calledBy = "")
        {

            var eventList = new Dictionary<String, Action>();
            eventList.Add("greet", () => Greeting.greet(player, mob, room));
            eventList.Add("tutorial", () => Tutorial.setUpTut(player, room, option, calledBy));
            eventList.Add("rescue", () => Tutorial.setUpRescue(player, room, option, calledBy));
            eventList.Add("awakening", () => Tutorial.setUpAwakening(player, room, option, calledBy));

            return eventList;
        }

 

        public static  void ParseCommand(string eventName, PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room = null, string option = "", string calledBy = "")
        {

             //TODO: do this only once
            var command = Event.Events(eventName, player, mob, room, option, calledBy);
 
            var fire = command.FirstOrDefault(x => x.Key.StartsWith(eventName, StringComparison.InvariantCultureIgnoreCase));

            fire.Value?.Invoke();


        }
    }
}