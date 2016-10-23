using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Mob.Events
{
    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    public class Trainer
    {
        public static void Practice(Player player, Player trainer, string skillToPractice)
        {
          //  var foundTrainer = FindTrainer(trainer);
        }

        public static Player FindTrainer(string trainerToFind, Room room)
        {
            var nth = FindNth.Findnth(trainerToFind);
         return FindItem.Player(room.players, nth.Key, trainerToFind);
        }
    }
}