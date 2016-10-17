using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace MIMWebClient.Core.Player
{
    public class Recall
    {
        [BsonElement("rre")]
        public string Region;

        [BsonElement("rar")]
        public string Area;

        [BsonElement("rari")]
        public int AreaId;
    }
}