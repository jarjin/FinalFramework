using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FirClient.Component
{
    public class AStar
    {
        private const int LinearScore = 10;
        private const int DiagonalScore = 14;

        private const bool useDiagonal = false;     //是否使用对角线
        private IAStar mInvoker = null;
        private Node currNode = null;
        private Stack<Vector3Int> paths;
        private Vector3Int startPos, goalPos;
        private HashSet<Node> openList = new HashSet<Node>();
        private HashSet<Node> closedList = new HashSet<Node>();
        private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

        public void Startup(IAStar astar)
        {
            if (astar == null)
            {
                Debug.LogError("A* Startup Failed!!! astar was null~");
                return;
            }
            this.mInvoker = astar;
        }

        private void Initialize()
        {
            currNode = GetNode(startPos);
            openList.Clear();
            closedList.Clear();
            openList.Add(currNode);
        }

        private Node GetNode(Vector3Int pos)
        {
            if (allNodes.ContainsKey(pos))
            {
                return allNodes[pos];
            }
            else
            {
                var node = new Node(pos);
                allNodes.Add(pos, node);
                return node;
            }
        }

        public Stack<Vector3Int> Algorithm(Vector3Int start, Vector3Int goal)
        {
            startPos = start;
            goalPos = goal;
            Initialize();

            paths = new Stack<Vector3Int>();
            while (openList.Count > 0 && paths.Count == 0)
            {
                var neightbors = FindNeighbors(currNode.Position);
                ExamineNeighbors(neightbors, currNode);

                UpdateNode(ref currNode);   //更新

                GeneratePath(currNode);
            }
            mInvoker.DrawDebuggerNode(allNodes, paths.ToArray());
            return paths;
        }

        private List<Node> FindNeighbors(Vector3Int parentPos)
        {
            var neighbors = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var neighborPos = new Vector3Int(parentPos.x - x, parentPos.y - y, parentPos.z);
                    if (x != 0 || y != 0)
                    {
                        if (neighborPos != startPos && mInvoker.IsWalkable(neighborPos))
                        {
                            var neighbor = GetNode(neighborPos);
                            neighbors.Add(neighbor);
                        }
                    }
                }
            }
            return neighbors;
        }

        private void ExamineNeighbors(List<Node> neighbors, Node current)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                Node neighbor = neighbors[i];
                //if (!ConnectedDiagonally(current, neighbor))
                //{
                //    continue;   //如果不连续，跳过
                //}
                var g = DetermineGScore(current.Position, neighbor.Position);

                if (!useDiagonal && g == DiagonalScore)
                {
                    continue;   //如果不使用对角线
                }
                if (openList.Contains(neighbor))
                {
                    if (current.G + g < neighbor.G)
                    {
                        CalcValues(current, neighbor, g);
                    }
                }
                else if (!closedList.Contains(neighbor))
                {
                    CalcValues(current, neighbor, g);
                    openList.Add(neighbor);
                }
            }
        }

        private void CalcValues(Node parent, Node neighbor, int cost)
        {
            neighbor.Parent = parent;
            neighbor.G = parent.G + cost;
            neighbor.H = (Math.Abs(neighbor.Position.x - goalPos.x) + Math.Abs(neighbor.Position.y - goalPos.y)) * 10;
            neighbor.F = neighbor.G + neighbor.H;
        }

        private void UpdateNode(ref Node current)
        {
            openList.Remove(current);
            closedList.Add(currNode);

            if (openList.Count > 0)
            {
                current = openList.OrderBy(x => x.F).First();
            }
        }

        private int DetermineGScore(Vector3Int current, Vector3Int neighbor)
        {
            int gScore = 0;
            int x = current.x - neighbor.x;
            int y = current.y - neighbor.y;
            if (Math.Abs(x - y) % 2 == 1)
            {
                gScore = LinearScore;
            }
            else
            {
                gScore = DiagonalScore;
            }
            return gScore;
        }

        private bool ConnectedDiagonally(Node current, Node neighbor)
        {
            var direct = current.Position - neighbor.Position;
            var first = new Vector3Int(current.Position.x + direct.x * -1, current.Position.y, current.Position.z);
            var second = new Vector3Int(current.Position.x, current.Position.y + direct.y * -1, current.Position.z);

            bool isExistFirst = mInvoker.IsWalkable(first);
            bool isExistSecond = mInvoker.IsWalkable(second);
            if (isExistFirst && isExistSecond)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 产生路径
        /// </summary>
        /// <param name="current"></param>
        private void GeneratePath(Node current)
        {
            if (current.Position == goalPos)
            {
                while(current.Position != startPos)
                {
                    paths.Push(current.Position);
                    current = current.Parent;
                }
            }
        }

        public void Reset()
        {
            currNode = null;
            paths.Clear();
            openList.Clear();
            closedList.Clear();
        }
    }
}
