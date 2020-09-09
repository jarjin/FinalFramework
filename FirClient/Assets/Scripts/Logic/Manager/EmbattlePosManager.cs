using FirClient.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.Manager
{
    public class EmbattlePosManager : LogicBehaviour
    {
        private Vector2 battlePos;
        private Dictionary<EmbattleType, List<EmbattlePos>> embattles = new Dictionary<EmbattleType, List<EmbattlePos>>();

        public override void Initialize()
        {
            base.Initialize();
        }

        internal void Initialize(Vector2 currPos)
        {
            battlePos = currPos;
            embattles.Clear();
            var types = new EmbattleType[] 
            { 
                EmbattleType.Left, 
                EmbattleType.Right, 
                EmbattleType.Center, 
                EmbattleType.BothSides
            };
            var NumPos = 5;

            foreach (EmbattleType emType in types)
            {
                for (uint i = 1; i <= NumPos; i++)
                {
                    if (emType == EmbattleType.BothSides && i >= NumPos)
                    {
                        continue;
                    }
                    List<EmbattlePos> items = null;
                    embattles.TryGetValue(emType, out items);
                    if (items == null)
                    {
                        items = new List<EmbattlePos>();
                        embattles.Add(emType, items);
                    }
                    FaceDir dir = FaceDir.Left;
                    var newPos = FindEmptyPos(currPos, emType, i, ref dir);

                    var item = new EmbattlePos();
                    item.id = i;
                    item.pos = newPos;
                    item.isUsing = false;
                    item.faceDir = dir;
                    items.Add(item);
                }
            }
        }

        public Vector2 GetBattlePos()
        {
            return battlePos;
        }

        internal void CaleHeroNpcPos(Vector2 newPos, ref Dictionary<long, Vector3> dic)
        {
            var npcs = npcDataMgr.GetNpcDatas(NpcType.Hero);
            foreach(var npc in npcs)
            {
                npc.position = FindEmptyPos(newPos, EmbattleType.Center, npc.index, ref npc.faceDir);
                dic.Add(npc.npcid, npc.position);
            }
        }

        public EmbattlePos GetItem(EmbattleType type, uint index)
        {
            List<EmbattlePos> items = null;
            embattles.TryGetValue(type, out items);
            if (items != null)
            {
                foreach(var item in items)
                {
                    if (item.id == index)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public void PushbackItem(EmbattleType type, Vector3 pos)
        {
            List<EmbattlePos> items = null;
            embattles.TryGetValue(type, out items);
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.pos == pos)
                    {
                        item.isUsing = false;
                        break;
                    }
                }
            }
        }

        public void Reset()
        {
            embattles.Clear();
        }

        /// <summary>
        /// 3          3
        ///   1     1
        /// 4    *     4
        ///   2     2
        /// 5          5
        /// </summary>
        Vector3 FindEmptyPos(Vector3 pos, EmbattleType type, uint index, ref FaceDir dir)
        {
            var newPos = new Vector3(pos.x, pos.y, 0);
            var embattleData = configMgr.GetEmbattlePosData(type);
            if (!embattleData.ContainsKey(index))
            {
                return Vector3.zero;
            }
            var offsetPos = embattleData[index];
            if (type == EmbattleType.BothSides)
            {
                var viewPortMargin = LogicConst.Viewport_Margin;
                var v = offsetPos.x > 0 ? 1 : -1;
                newPos.x += v * viewPortMargin + offsetPos.x;
                dir = offsetPos.x > 0 ? FaceDir.Left : FaceDir.Right;
            }
            else
            {
                newPos.x += offsetPos.x;
                if (type == EmbattleType.Center)
                {
                    dir = FaceDir.Right;
                }
                else
                {
                    dir = type == EmbattleType.Left ? FaceDir.Right : FaceDir.Left;
                }
            }
            newPos.y += offsetPos.y;
            newPos.z += offsetPos.z;
            return newPos;
        }

        public override void OnDispose()
        {
            base.OnDispose();
        }
    }
}