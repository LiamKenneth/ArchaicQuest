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

            if (thingToFollow == "noFollow" && follower.CanFollow)
            {
                follower.CanFollow = false;
                HubContext.SendToClient("You can no longer be followed.", follower.HubGuid);

                if (follower.Following != null)
                {
                    HubContext.SendToClient($"You stop following {follower.Following.Name}.", follower.HubGuid);
                    follower.Following?.Followers.Remove(follower);
                    follower.Following = null;
                }

                if (follower.Followers != null)
                {
                    foreach (var follow in follower.Followers)
                    {
                        HubContext.SendToClient($"You stop following {follower.Name}.", follow.HubGuid);
                        HubContext.SendToClient($"{follow.Name} stops following you.", follower.HubGuid);
                        follow.Following = null;
                    }
                }

                follower.Followers = null;

                return;
            }

            if (thingToFollow == "noFollow" && !follower.CanFollow)
            {
                follower.CanFollow = true;
                HubContext.SendToClient("You can now be followed.", follower.HubGuid);

                return;
            }

            follower.CanFollow = true;

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


            }else if (options.Length >= 1)
            {
                objectToFind = options[0];
            }

            var findPerson = FindItem.Player(room.players, nth, objectToFind);

            if (findPerson == null) { HubContext.SendToClient("No one here by that name to follow.", follower.HubGuid);  return; }

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

                if (findPerson.Name == follower.Name)
                {
                    if (follower.Following != null)
                    {
                        
                        follower.Following.Followers.Remove(follower);
                        HubContext.SendToClient($"You stop following {follower.Following.Name}.", findPerson.HubGuid);
                        follower.Following = null;

                    }
                    else
                    {
                        HubContext.SendToClient($"You can't follow yourself.", findPerson.HubGuid);
                    }

                   
                    
                    return;
                }

                if (!findPerson.CanFollow)
                {
                    HubContext.SendToClient($"{Helpers.ReturnName(findPerson, follower, String.Empty)} does not want to be followed", follower.HubGuid);
                    return;
                }

                if (findPerson.Following?.Name == follower.Name)
                {
                    HubContext.SendToClient($"You can't follow {Helpers.ReturnName(findPerson, follower, String.Empty)} as they are following you.", follower.HubGuid);
                    return;
                }

                HubContext.SendToClient($"{Helpers.ReturnName(follower, findPerson, String.Empty)} begins following you", findPerson.HubGuid);
                HubContext.SendToClient($"You follow {Helpers.ReturnName(findPerson, follower, String.Empty)}", follower.HubGuid);

                foreach (var character in room.players)
                {
                    if (character.Name != follower.Name && character != findPerson)
                    {
                        HubContext.SendToClient($"{Helpers.ReturnName(follower, findPerson, String.Empty)} begins following {Helpers.ReturnName(findPerson, follower, String.Empty)}", character.HubGuid);
                    }
                      
                }

                findPerson.Followers.Add(follower);
                follower.Following = findPerson;
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