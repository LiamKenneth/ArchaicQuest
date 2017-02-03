using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.AI
{
    public class Node
    {
        public string ID { get; private set; }
        public Node Parent { get; set; } 
        public List<Node> NeighborNodes { get; private set; }

        public Node(string id, List<Node> adjacentNodes)
        {
            ID = id;
            NeighborNodes = adjacentNodes;
        }
    }

     
}