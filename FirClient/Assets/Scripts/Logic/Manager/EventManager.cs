using FirClient.Data;
using FirClient.Define;
using FirClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FirClient.Logic.Manager
{
    public class EventManager : LogicBehaviour
    {
        private int evPosIndex = 0;
        private List<SceneEvent> sceneEvent;
        private Action<Vector2> execOK;
        private Queue<EventData> events = new Queue<EventData>();

        public void Initialize(List<SceneEvent> events)
        {
            this.evPosIndex = 0;
            this.sceneEvent = events;
        }

        public void NewTurnEvent(Action<Vector2> execOK) 
        {
            this.events.Clear();
            this.execOK = execOK;

            var currEvent = sceneEvent[evPosIndex++];
            var currPos = currEvent.pos.Value;

            battleTurnMgr.Initialize(currPos);
            var eventObjs = GetEventObject(currPos);
            if (eventObjs != null)
            {
                foreach (EventData ev in eventObjs)
                {
                    events.Enqueue(ev);
                }
            }
            MoveNextEvent(currPos);
        }

        public bool IsEndTurnEvent()
        {
            if (sceneEvent != null)
            {
                return evPosIndex >= sceneEvent.Count;
            }
            return true;
        }

        public Vector2? GetNextPos()
        {
            if (IsEndTurnEvent())
            {
                return null;
            }
            var currEvent = sceneEvent[evPosIndex];
            if (currEvent != null)
            {
                return currEvent.pos;
            }
            return null;
        }

        List<EventData> GetEventObject(Vector2 currPos)
        {
            if (sceneEvent != null)
            {
                foreach(var ev in sceneEvent)
                {
                    if (ev.pos == currPos)
                    {
                        return ev.eventObjs;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 下一个事件
        /// </summary>
        void MoveNextEvent(Vector2 currPos)
        {
            if (events.Count == 0)
            {
                if (execOK != null) execOK(currPos);
                return;
            }
            var evData = events.Dequeue();
            if (evData == null)
            {
                MoveNextEvent(currPos);
                return;
            }
            GLogger.Yellow("MoveNext evType:" + evData.type);

            switch (evData.type)
            {
                case EventsType.SpawnHeroNpc:
                    OnEvSpawnHeroNpc(() => MoveNextEvent(currPos));
                break;
                case EventsType.SpawnTeamNpc:
                    OnEvSpawnTeamNpc(evData.value, () => MoveNextEvent(currPos));
                break;
                case EventsType.Dungeon:
                    OnEvDungeon(evData.value, () => MoveNextEvent(currPos));
                break;
                case EventsType.Dialog:
                    OnEvDialog(evData.value, () => MoveNextEvent(currPos));
                break;
                case EventsType.MoveCamera:
                    OnEvMoveCamera(currPos, evData.value, () => MoveNextEvent(currPos));
                break;
                case EventsType.MoveNpc:
                    OnEvMoveNpc(currPos, evData.value, () => MoveNextEvent(currPos));
                break;
                case EventsType.LoadScene:
                    OnEvLoadScene(evData.value, () => MoveNextEvent(currPos));
                break;
                case EventsType.StartFight:
                    OnEvStartFight(evData.value, () => MoveNextEvent(currPos));
                break;
            }
        }

        /// <summary>
        /// 开始战斗
        /// </summary>
        private void OnEvStartFight(string value, Action moveNext)
        {
            battleTurnMgr.StartFight(moveNext);
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        private void OnEvLoadScene(string value, Action moveNext)
        {
            battleLogicMgr.EnterScene(uint.Parse(value), moveNext);
        }

        /// <summary>
        /// 进入场景完成
        /// </summary>
        public void BeginPlayEvent(uint mapid)
        {
            var mapData = configMgr.GetMapData(mapid);
            if (mapData != null)
            {
                LogicConst.SceneType = SceneType.BigScene;
                logicMgr.InitBattleFsm();

                this.Initialize(mapData.events);
                this.NewTurnEvent(delegate (Vector2 pos)
                {
                    Debug.Log("OnEnterSceneOK event at " + pos + " execute ok!!!");
                });
            }
        }

        /// <summary>
        /// 移动NPC
        /// </summary>
        private void OnEvMoveNpc(Vector2 currPos, string param, Action moveNext)
        {
            float moveTime = 0f;
            Vector2? newPos = null;
            TakeMoveData(currPos, param, ref moveTime, ref newPos);

            if (newPos != null)
            {
                var dic = new Dictionary<long, Vector3>();
                embattlePosMgr.CaleHeroNpcPos(newPos.Value, ref dic);

                foreach(var de in dic)
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
            if (moveNext != null) moveNext();
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
            if (IsEndTurnEvent())
            {
                Debug.LogError("IsEndTurnEvent EXEC OK!!!");
            }
            else
            {
                NewTurnEvent(delegate (Vector2 pos)
                {
                    Debug.Log("OnNpcMoveOK event at " + pos + " execute ok!!!");
                });
            }
        }

        /// <summary>
        /// 出生英雄NPC，通过服务器数据产生
        /// </summary>
        void OnEvSpawnHeroNpc(Action moveNext)
        {
            Debug.Log("OnEvSpawnHeroNpc...");
            battleTurnMgr.SpawnHeros(moveNext);
        }

        /// <summary>
        /// 出生敌人NPC，通过场景事件产生的都是敌人怪物
        /// </summary>
        private void OnEvSpawnTeamNpc(string param, Action moveNext)
        {
            Debug.Log("OnEvSpawnTeamNpc...");
            List<string> teamids = param.ToList<string>(',');
            battleTurnMgr.InitNpcTeams(teamids, moveNext);
        }

        /// <summary>
        /// 对话事件
        /// </summary>
        void OnEvDialog(string param, Action moveNext)
        {
            Debug.Log("evDialog:>" + param);
            var dlgData = param.ToList<uint>(',');
            if (dlgData.Count == 3)
            {
                battleLogicMgr.ShowDialog(dlgData[0], dlgData[1], dlgData[2]);
                if (moveNext != null) moveNext();
            }
            else if (dlgData.Count == 4)
            {
                var delayTime = dlgData[3];
                var nextAction = moveNext;
                battleLogicMgr.ShowDialog(dlgData[0], dlgData[1], dlgData[2]);
                timerMgr.AddTimer(delayTime, 0, (obj) =>
                {
                    if (nextAction != null) nextAction();
                });
            }
        }

        /// <summary>
        /// 进入副本事件
        /// </summary>
        private void OnEvDungeon(string param, Action moveNext)
        {
            battleLogicMgr.BattleEnd();
            var dungeonData = param.ToList<uint>(',');
            if (dungeonData != null)
            {
                var chapterid = dungeonData[0];
                var dungeonid = dungeonData[1];
                battleLogicMgr.EnterDungeon(chapterid, dungeonid, null, OnEvDungeonEventOK);
            }
        }

        /// <summary>
        /// 副本事件完成
        /// </summary>
        private void OnEvDungeonEventOK(object param)
        {
            var enterEvent = param as EnterDungeonEvent;
            if (enterEvent != null)
            {
                Debug.Log("OnEnterDungeonOK---->>>chapterid:" + enterEvent.chapterid + " dungeonid:" + enterEvent.dungeonid);

                LogicConst.SceneType = SceneType.Dungeon;
                logicMgr.InitBattleFsm();

                var dungeonData = configMgr.GetDungeonData(enterEvent.chapterid, enterEvent.dungeonid);
                if (dungeonData != null)
                {
                    this.Initialize(dungeonData.events);
                    this.NewTurnEvent(delegate (Vector2 pos)
                    {
                        Debug.Log("OnEnterDungeonOK event at " + pos + " execute ok!!!");
                    });
                }
                //if (enterEvent.action != null) 
                //{
                //    enterEvent.action();   //进入副本后交给新事件组，不需要回调，否则会多执行一个新的moveNext
                //}
            }
        }

        /// <summary>
        /// 移动相机事件
        /// </summary>
        private void OnEvMoveCamera(Vector2 currPos, string param, Action moveNext)
        {
            float moveTime = 0f;
            Vector2? movePos = null;
            TakeMoveData(currPos, param, ref moveTime, ref movePos);
            var newPos = new Vector2(movePos.Value.x, 0);

            Messenger.Broadcast<Vector2, float>(EventNames.EvMoveCamera, newPos, moveTime);
            if (moveNext != null) moveNext();
        }

        /// <summary>
        /// 获取移动数据
        /// </summary>
        void TakeMoveData(Vector2 currPos, string param, ref float moveTime, ref Vector2? newPos)
        {
            if (string.IsNullOrEmpty(param))
            {
                newPos = new Vector2(currPos.x, currPos.y);
            }
            else
            {
                MoveObjectType moveType = MoveObjectType.CurrentPos;
                if (param.IndexOf(',') > -1)
                {
                    var paramStrs = param.Split(',');
                    moveType = (MoveObjectType)uint.Parse(paramStrs[0]);
                    moveTime = float.Parse(paramStrs[1]);
                }
                else
                {
                    moveType = (MoveObjectType)uint.Parse(param);
                }
                switch (moveType)
                {
                    case MoveObjectType.CurrentPos:
                        newPos = new Vector2(currPos.x, currPos.y);
                    break;
                    case MoveObjectType.NextPos:
                        var nextPos = GetNextPos();
                        if (nextPos != null)
                        {
                            newPos = new Vector2(nextPos.Value.x, nextPos.Value.y);
                        }
                    break;
                    case MoveObjectType.SpecifiedPos:
                    break;
                    default:
                        newPos = new Vector2(currPos.x, currPos.y);
                    break;
                }
            }
        }

        public override void OnDispose()
        {
        }
    }
}

