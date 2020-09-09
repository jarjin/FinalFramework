using FirClient.Data;
using FirClient.Logic.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.Manager
{
    public class EventManager : LogicBehaviour
    {
        private int evPosIndex = 0;
        private List<SceneEvent> sceneEvent;
        private Action<Vector2> execOK;
        private Queue<EventData> events = new Queue<EventData>();
        private Dictionary<EventsType, BaseSceneEvent> sceneEvents = new Dictionary<EventsType, BaseSceneEvent>();

        public override void Initialize()
        {
            sceneEvents.Add(EventsType.SpawnHero, new EvSpawnHero());
            sceneEvents.Add(EventsType.SpawnEnemy, new EvSpawnEnemy());
            sceneEvents.Add(EventsType.EnterDungeon, new EvEnterDungeon());
            sceneEvents.Add(EventsType.ShowDialog, new EvShowDialog());
            sceneEvents.Add(EventsType.MoveCamera, new EvMoveCamera());
            sceneEvents.Add(EventsType.MoveNpc, new EvMoveNpc());
            sceneEvents.Add(EventsType.LoadScene, new EvLoadScene());
            sceneEvents.Add(EventsType.StartFight, new EvStartFight());
        }


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

            battleHandlerMgr.Initialize(currPos);
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

            var currEvent = sceneEvents[evData.type];
            if (currEvent != null)
            {
                if (evData.type == EventsType.MoveNpc || evData.type == EventsType.MoveCamera)
                {
                    currEvent.OnExecute(currPos, evData.value, () => MoveNextEvent(currPos));
                }
                else
                {
                    currEvent.OnExecute(evData.value, () => MoveNextEvent(currPos));
                }
            }
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

        public override void OnDispose()
        {
        }
    }
}

