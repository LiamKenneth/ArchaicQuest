using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Loging
{
    public class QuitLocation
    {
        public Guid Id { get; set; }
        public string RoomName { get; set; }
        public int RoomId { get; set; }
        public string PlayerName { get; set; }
    }

    public class UnfinishedQuests
    {
        public Guid Id { get; set; }
        public string QuestName { get; set; }

    }

    public class Deaths
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string RoomName { get; set; }
        public string Area { get; set; }
        public int AreaId { get; set; }
        public string KilledBy { get; set; }
        public DateTime Date { get; set; }

    }

 
}