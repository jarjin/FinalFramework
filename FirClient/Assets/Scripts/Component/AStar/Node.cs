using UnityEngine;

namespace FirClient.Component
{
    public class Node
    {
        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }

        public Vector3Int Position { get; set ; }

        public Node Parent { get; set; }

        public Node(Vector3Int pos)
        {
            this.Position = pos;
        }
    }
}

