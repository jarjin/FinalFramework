using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FirClient.Component
{
    public class AStarCtrl : BaseBehaviour, IAStar
    {
        private Vector3Int startPos, goalPos;
        private Tilemap npcTilemap;
        private Tilemap groundTilemap;
        private AStar mAStar = new AStar();
        private AStarDebugger aStarDebugger;
        private GameObject debugCanvas = null;

        public void Initialize()
        {
            CreateAStarDebugger();
            mAStar.Startup(this);
        }

        /// <summary>
        /// 创建AStar调试器
        /// </summary>
        public void CreateAStarDebugger()
        {
            debugCanvas = GameObject.Find("MainGame/DebugCanvas");
            if (debugCanvas != null)
            {
                var gameObj = new GameObject("AStarDebugger");
                gameObj.layer = LayerMask.NameToLayer("UI");
                gameObj.transform.SetParent(debugCanvas.transform);
                gameObj.transform.localScale = Vector3.one;
                gameObj.transform.localPosition = Vector3.zero;
                gameObj.AddComponent<RectTransform>().sizeDelta = Vector2.zero;

                aStarDebugger = gameObj.AddComponent<AStarDebugger>();
                aStarDebugger.Initialize(this);
            }
        }

        /// <summary>
        /// 运行算法
        /// </summary>
        public Stack<Vector3Int> RunAlgorithm(Vector3Int start, Vector3Int goal)
        {
            startPos = start; goalPos = goal;
            Debug.LogError("from:" + start + " to:" + goal);
            return mAStar.Algorithm(start, goal);
        }

        /// <summary>
        /// 检查是否存在节点
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsWalkable(Vector3Int pos)
        {
            return false;
            //if (npcTilemap == null)
            //{
            //    npcTilemap = tilemapMgr.GetTilemap("Tilemap");
            //}
            //if (groundTilemap == null)
            //{
            //    groundTilemap = tilemapMgr.GetTilemap("Ground");
            //}
            //var isExistTile = npcTilemap.GetTile(pos) != null;
            //var isExistGround = groundTilemap.GetTile(pos) != null;

            //var isExistNpc = false;
            //foreach(var de in npcMgr.Npcs)
            //{
            //    var npcView = de.Value as NPCView;
            //    if (npcView != null && npcView.NpcData.position == pos)
            //    {
            //        isExistNpc = true;
            //    }
            //}
            //return isExistGround && !isExistTile && !isExistNpc;
        }

        /// <summary>
        /// 找出可到达的路径
        /// </summary>
        public Stack<Vector3Int> ReachablePath(Vector3Int currPos, List<Vector3Int> avaliPos)
        {
            var paths = new List<Stack<Vector3Int>>();
            foreach(Vector3Int pos in avaliPos)
            {
                bool wasBreak = false;
                var currPath = RunAlgorithm(currPos, pos);
                if (currPath.Count > 0)
                {
                    foreach (var point in currPath)
                    {
                        if (!IsWalkable(point))
                        {
                            wasBreak = true;
                            break;
                        }
                    }
                    if (!wasBreak)
                    {
                        paths.Add(currPath);
                    }
                }
            }
            if (paths.Count > 0)
            {
                paths.Sort((a, b) => {
                    if (a.Count > b.Count) return 1;
                    else if (a.Count == b.Count) return 0;
                    else return -1;
                });
                return paths[0];
            }
            return null;
        }

        /// <summary>
        /// 画调试节点
        /// </summary>
        public void DrawDebuggerNode(Dictionary<Vector3Int, Node> allNode, Vector3Int[] path)
        {
            if (aStarDebugger != null)
            {
                aStarDebugger.CreateTiles(allNode, path, startPos, goalPos);
            }
        }

        /// <summary>
        /// 获取CELL位置
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 GetCellPos(Vector3Int pos)
        {
            if (npcTilemap == null)
            {
                return Vector3.zero;
            }
            return npcTilemap.CellToWorld(pos);
        }

        /// <summary>
        /// 获取可用目标点
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Vector3Int> AvailableGoalPos(Vector3Int pos)
        {
            var result = new List<Vector3Int>();
            var leftPos = new Vector3Int(pos.x - 1, pos.y, pos.z);
            if (IsWalkable(leftPos))
            {
                result.Add(leftPos);
            }
            var rightPos = new Vector3Int(pos.x + 1, pos.y, pos.z);
            if (IsWalkable(rightPos))
            {
                result.Add(rightPos);
            }
            var topPos = new Vector3Int(pos.x, pos.y + 1, pos.z);
            if (IsWalkable(topPos))
            {
                result.Add(topPos);
            }
            var bottomPos = new Vector3Int(pos.x, pos.y - 1, pos.z);
            if (IsWalkable(bottomPos))
            {
                result.Add(bottomPos);
            }
            return result;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            mAStar.Reset();
            aStarDebugger.Reset();
            npcTilemap = null;
            groundTilemap = null;
        }
    }
}

