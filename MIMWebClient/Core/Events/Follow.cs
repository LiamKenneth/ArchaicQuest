using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class Follow
    {
        public static void FollowThing(PlayerSetup.Player follower, Room.Room room, string thingToFollow)
        {

            string[] options = thingToFollow.Split(' ');
            int nth = -1;
            string getNth = string.Empty;
            string objectToFind = String.Empty;


            if (options.Length == 3)
            {
                objectToFind = options[2];

                if (objectToFind.IndexOf('.') != -1)
                {
                    getNth = objectToFind.Substring(0, objectToFind.IndexOf('.'));
                    int.TryParse(getNth, out nth);
                }


            }

            var findPerson = FindItem.Player(room.players, nth, objectToFind);

            if (findPerson == null) { return; }

            if (findPerson.Followers == null)
            {
                findPerson.Followers = new List<PlayerSetup.Player>();
            }

            //check if player already following player

            if (findPerson.Followers.Contains(follower))
            {
                if (follower.HubGuid != null)
                {
                    HubContext.SendToClient("You are already following them.", follower.HubGuid);
                }
                 
            }
            else
            {
                HubContext.SendToClient($"{Helpers.ReturnName(follower, findPerson, String.Empty)} begins following you", findPerson.HubGuid);

                foreach (var character in room.players)
                {
                    if (character != follower || character != findPerson)
                    {
                        HubContext.SendToClient($"{Helpers.ReturnName(follower, findPerson, String.Empty)} begins following {Helpers.ReturnName(findPerson, follower, String.Empty)}", character.HubGuid);
                    }
                      
                }

                findPerson.Followers.Add(follower);
            }
        }

        public static void MobStalk(PlayerSetup.Player follower, PlayerSetup.Player player, Room.Room room)
        {

            if (player.Followers == null)
            {
                player.Followers = new List<PlayerSetup.Player>();
            }

            if (player.Followers.Contains(follower))
            {
                return;
            }

            HubContext.SendToClient($"{follower.Name} begins following you", player.HubGuid);

            foreach (var character in room.players)
            {
                if (character != follower || character != player)
                {
                    HubContext.SendToClient($"{follower.Name} begins following {Helpers.ReturnName(player, follower, String.Empty)}", character.HubGuid);
                }
            }

            player.Followers.Add(follower);
        }
    }
}