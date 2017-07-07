using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.World.Anker.Scripts
{
    public class VilliageIdiot
    {

        public static void Annoy(PlayerSetup.Player player, PlayerSetup.Player mob, Room.Room room)
        {
            var phrase = RandomResponses();
            HubContext.SendToClient($"<span class='sayColor'>{mob.Name} says to you \"{phrase}\"</span>", player.HubGuid);

            foreach (var character in room.players)
            {
                if (character != player)
                {
                    HubContext.SendToClient($"<span class='sayColor'>{mob.Name} says to {Helpers.ReturnName(player, character, String.Empty)} \"{phrase}\"</span>", character.HubGuid);
                }
            }

            Follow.MobStalk(mob, player, room);

        }

        public static string RandomResponses()
        {
            var response = new List<string>
            {
                "Hi, I'm Newber. Nice place, huh?",
                "So, killed any monsters yet?",
                "Ever been to Stellum? I've been to Stellum.",
                "Ugh, I think i stepped in something.",
                "Everyone in Anker used to throw rocks at me and tell me I was annoying.",
                "What time is it ?",
                "I haven't had a conversation this long, well...ever!",
                "What's that big weapon for?",
                "Those colours look pretty stoopid on you...",
                "I once knew this guy called Dilby. He threw rocks at me too. Are you gonna throw rocks at me?",
                "I heard there are giant spiders in the woods, I don't like Spiders.",
                "I used to be an adenturer once, Then I took an arrow to the knee",
                "Am I annoying you?",
                "Am I annoying you yet? You sure look annoyed.",
                "Hey there, I just want to be friends.",
                "I can tell you're annoyed, if you don't want me following you just say so",
            };

 
            return response[Helpers.Rand(0, response.Count)];
        }
    }
}