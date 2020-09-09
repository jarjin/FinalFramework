using FirClient.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 移动NPC
    /// </summary>
    internal class EvMoveNpc : BaseSceneEvent
    {
        public override void OnExecute(Vector2 currPos, string param, Action moveNext)
        {
            float moveTime = 0f;
            Vector2? newPos = null;
            TakeMoveData(currPos, param, ref moveTime, ref newPos);

            if (newPos != null)
            {
                var dic = new Dictionary<long, Vector3>();
                embattlePosMgr.CaleHeroNpcPos(newPos.Value, ref dic);

                foreach (var de in dic)
                {
                    var npcid = de.Key;
                    var movePos = de.Value;

                    var npcData = npcDataMgr.GetNpcData(npcid);
                    if (npcData != null)
                    {
                        npcData.npcState = NpcState.Move;
                    }
                    battleLogicMgr.SetNpcFaceDir(npcid, FaceDir.Right);
                    battleLogicMgr.NpcMove(npcid, movePos, moveTime, OnEvMoveNpcOK);
                }
            }
            base.OnExecute(param, moveNext);
        }

        /// <summary>
        /// 移动NPC事件完成
        /// </summary>
        /// <param name="param"></param>
        private void OnEvMoveNpcOK(object param)
        {
            var npcid = (long)param;
            GLogger.Green("OnEvMoveNpcOK:>" + npcid);

            var npcData = npcDataMgr.GetNpcData(npcid);
            if (npcData != null)
            {
                npcData.npcState = NpcState.Ready;
            }
            if (!npcDataMgr.IsAllNpcStateOK(NpcType.Hero, NpcState.Move))
            {
                return;     //移动还没有全部完成
            }
            if (eventMgr.IsEndTurnEvent())
            {
                Debug.LogError("IsEndTurnEvent EXEC OK!!!");
            }
            else
            {
                eventMgr.NewTurnEvent(delegate (Vector2 pos)
                {
                    Debug.Log("OnNpcMoveOK event at " + pos + " execute ok!!!");
                });
            }
        }
    }
}