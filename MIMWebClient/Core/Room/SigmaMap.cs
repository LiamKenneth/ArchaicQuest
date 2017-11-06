using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MIMWebClient.Core.AI;
using MIMWebClient.Core.Events;
using MIMWebClient.Core.World;
using Newtonsoft.Json;

namespace MIMWebClient.Core.Room
{

    public class SigmaMapNode
    {
        public string id { get; set; }
        public string label { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public int size { get; set; } = 2;
        public int defaultLabelSize { get; set; } = 12;
        public string color { get; set; } = "#ccc";
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



    public static class SigmaMap
    {

        public static  void DrawMap(string playerId)
        {

            var player = Cache.getPlayer(playerId);

            var nodes = new List<SigmaMapNode>();
            var edges = new List<SigmaMapEdge>();


            var roomSetUp = new BreadthFirstSearch();

        var list = roomSetUp.AssignCoords("Anker", "Anker");

            foreach (var node in list)
            {

                var y = "";


                if (node.coords.Y == 0)
                {
                    y = "0";
                }
                else if (node.coords.Y > 0)
                {
                    y = (node.coords.Y * -1).ToString();
                }
                else if (node.coords.Y < 0)
                {
                    y = Math.Abs(node.coords.Y).ToString();
                }

                var mapNode = new SigmaMapNode()
                {
                    id = "node" + node.areaId,
                    label = node.title,
                    x = node.coords.X.ToString(),
                    y = y,
                     
                };

                var room = list.Distinct().FirstOrDefault(z => z.areaId == node.areaId);

                if (room.needsBoat)
                {
                    mapNode.color = "#446CB3";
                }
                else if (room.type == Room.RoomType.Shop)
                {
                    mapNode.color = "#fed967";
                }

                nodes.Add(mapNode);

                foreach (var exit in node.exits)
                {
 
                
                        var mapEdge = new SigmaMapEdge()
                    {
                        id = "edge" + node.areaId + exit.areaId,
                        source = "node" + node.areaId,
                        target = "node" + exit.areaId,
                      
                    };

                    if (!edges.Contains(mapEdge))
                    {
                        edges.Add(mapEdge);
                    }

                 
                }
            }


            var json = new SigmaMapJSON()
            {
                edges = edges,
                nodes = nodes
            };

            var brokenEdges = new List<string>();

            foreach (var e in edges.ToList())
            {
                if (nodes.FirstOrDefault(x => x.id == e.target) == null)
                {
                    brokenEdges.Add(e.id);
                    edges.Remove(e);
                }
              
            }

            using (StreamWriter file = File.CreateText(@"C:\path.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, json);
            }

            HubContext.Instance.GetMap(playerId, json);
        }

    }
}