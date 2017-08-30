using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MIMWebClient.Core.AI;
using MIMWebClient.Core.Events;
using Newtonsoft.Json;

namespace MIMWebClient.Core.Room
{

    public class SigmaMapNode
    {
        public string id { get; set; }
        public string label { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int size { get; set; } = 2;
        public int defaultLabelSize { get; set; } = 12;
    }
 
    public class SigmaMapEdge
    {
        public string id { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string color { get; set; } = "#ccc";
    }


    public class SigmaMapJSON
    {
        public List<SigmaMapNode> nodes { get; set; }
        public List<SigmaMapEdge> edges { get; set; }
        
    }



    public class SigmaMap
    {

        public void DrawMap(string playerId)
        {

            var player = Cache.getPlayer(playerId);

            var nodes = new List<SigmaMapNode>();
            var edges = new List<SigmaMapEdge>();


            var roomSetUp = new BreadthFirstSearch();

        var list = roomSetUp.AssignCoords("Anker", "Anker");

            foreach (var node in list)
            {
                var mapNode = new SigmaMapNode()
                {
                    id = "node" + node.areaId,
                    label = node.title,
                    x = node.coords.X,
                    y = node.coords.Y,
                };

                nodes.Add(mapNode);

                foreach (var exit in node.exits)
                { 

                    var mapEdge = new SigmaMapEdge()
                    {
                        id = "edge" + node.areaId + exit.areaId,
                        source = "node" + node.areaId,
                        target = "node" + exit.areaId
                    };

                   edges.Add(mapEdge);
                }
            }


            var json = new SigmaMapJSON()
            {
                edges = edges,
                nodes = nodes
            };

            using (StreamWriter file = File.CreateText(@"C:\path.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, json);
            }


        }

}
}