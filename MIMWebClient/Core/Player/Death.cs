using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Player
{
    public class Death
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RoomName { get; set; }
        public string RoomId { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string KilledBy { get; set; }

    }
}