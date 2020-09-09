using FirClient.Data;
using FirClient.Extensions;
using System;
using UnityEngine;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 进入副本事件
    /// </summary>
    internal class EvEnterDungeon : BaseSceneEvent
    {
        public override void OnExecute(string param, Action moveNext)
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
                    eventMgr.Initialize(dungeonData.events);
                    eventMgr.NewTurnEvent(delegate (Vector2 pos)
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
    }
}