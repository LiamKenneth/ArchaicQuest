using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Item.Actions
{
    public class use
    {
        public void useObject(string obj, Room.Room room, PlayerSetup.Player player)
        {
            var item = room.items.Find(x => x.name.Equals(obj));

            HubContext.Instance.SendToClient("You rotate the lever and the bucket plunges in to the pool", player.HubGuid);
        }
    }
}