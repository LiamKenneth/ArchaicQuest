using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.World
{
    using MIMWebClient.Core.Room;

    public class UpdateDb
    {
        public void SaveRooms()
        {
            var rooms = new List<Room>();
            var anker = Anker.Anker.AnkerArea();
            rooms.Add(anker);

           //Add or edit
        }
    }
}