using FirClient.Data;
using FirClient.Define;
using System;
using UnityEngine;

namespace FirClient.Logic.Manager
{
    public class BattleLogicManager : LogicBehaviour
    {
        public override void Initialize()
        {
            Messenger.AddListener<uint>(EventNames.EvBeginPlay, OnBeginPlay);
            Messenger.AddListener<long>(EventNames.EvEnterDungeonOK, OnEnterDungeonOK);
            Messenger.AddListener<long>(EventNames.EvNpcSkillAttackOK, OnNpcSkillAttackOK);
            Messenger.AddListener<long>(EventNames.EvNpcMoveOK, OnNpcMoveOK);
            Messenger.AddListener<long>(EventNames.EvNpcShowOK, OnNpcShowOK);
            Messenger.AddListener<long>(EventNames.EvNpcSpawnOK, OnNpcSpawnOK); 
        }

        /// <summary>
        /// 启动播放事件
        /// </summary>
        private void OnBeginPlay(uint mapid)
        {
            eventMgr.BeginPlayEvent(mapid);
        }

        /// <summary>
        /// 载入场景
        /// </summary>
        public void EnterScene(uint mapid, Action callback)
        {
            Messenger.Broadcast<uint, Action>(EventNames.EvEnterScene, mapid, callback);
        }

        /// <summary>
        /// NPC出生
        /// </summary>
        public void NpcSpawn(NPCData npcData, Action<object> callback)
        {
            var spawnEvent = new NpcSpawnEvent();
            spawnEvent.isShowHUD = LogicConst.BattleType == BattleType.TurnBase;
            spawnEvent.npcData = npcData;
            spawnEvent.callback = callback;

            var evData = new GameEventData(GameEventType.NpcSpawn, spawnEvent);
            evMappingMgr.Add(evData);
            Messenger.Broadcast<GameEventData>(EventNames.EvNpcSpawn, evData);
        }

        private void OnNpcSpawnOK(long eventId)
        {
            var evData = evMappingMgr.Remove(eventId);
            if (evData != null)
            {
                var npSpawnEvent = evData.evParam as NpcSpawnEvent;
                if (npSpawnEvent != null)
                {
                    npSpawnEvent.callback(npSpawnEvent.npcData.npcid);
                }
            }
        }

        /// <summary>
        /// NPC移动
        /// </summary>
        public void NpcMove(long npcid, Vector3 movePos, float moveTime, Action<object> callback)
        {
            var moveEvent = new NpcMoveEvent();
            moveEvent.npcid = npcid;
            moveEvent.movePos = movePos;
            moveEvent.callback = callback;
            moveEvent.moveTime = moveTime;

            var evData = new GameEventData(GameEventType.NpcMove, moveEvent);
            evMappingMgr.Add(evData);
            Messenger.Broadcast<GameEventData>(EventNames.EvNpcMove, evData);
        }

        /// <summary>
        /// NPC移动完成
        /// </summary>
        private void OnNpcMoveOK(long eventId)
        {
            var evData = evMappingMgr.Remove(eventId);
            if (evData != null)
            {
                var npcMoveEvent = evData.evParam as NpcMoveEvent;
                if (npcMoveEvent != null)
                {
                    var npcData = npcDataMgr.GetNpcData(npcMoveEvent.npcid);
                    if (npcData != null)
                    {
                        npcData.position = npcMoveEvent.movePos;
                    }
                    npcMoveEvent.callback(npcMoveEvent.npcid);
                }
            }
        }

        /// <summary>
        /// NPC展示
        /// </summary>
        public void NpcShow(long npcid, float showTime, Action<object> callback)
        {
            var showEvent = new NpcShowEvent();
            showEvent.npcid = npcid;
            showEvent.showTime = showTime;
            showEvent.callback = callback;

            var evData = new GameEventData(GameEventType.NpcShow, showEvent);
            evMappingMgr.Add(evData);
            Messenger.Broadcast<GameEventData>(EventNames.EvNpcShow, evData);
        }

        /// <summary>
        /// NPC展示完成
        /// </summary>
        private void OnNpcShowOK(long eventId)
        {
            var evData = evMappingMgr.Remove(eventId);
            if (evData != null)
            {
                var showEvent = evData.evParam as NpcShowEvent;
                if (showEvent != null)
                {
                    showEvent.callback(showEvent.npcid);
                }
            }
        }

        /// <summary>
        /// 进入副本战斗
        /// </summary>
        public void EnterDungeon(uint chapterid, uint dungeonid, Action moveNext, Action<object> callback)
        {
            var enterEvent = new EnterDungeonEvent();
            enterEvent.chapterid = chapterid;
            enterEvent.dungeonid = dungeonid;
            enterEvent.callback = callback;
            enterEvent.action = moveNext;

            var evData = new GameEventData(GameEventType.EnterDungeon, enterEvent);
            evMappingMgr.Add(evData);
            Messenger.Broadcast<GameEventData>(EventNames.EvEnterDungeon, evData);
        }

        /// <summary>
        /// 进入副本OK
        /// </summary>
        private void OnEnterDungeonOK(long eventId)
        {
            var evData = evMappingMgr.Remove(eventId);
            if (evData != null)
            {
                var enterEvent = evData.evParam as EnterDungeonEvent;
                if (enterEvent != null)
                {
                    enterEvent.callback(enterEvent);
                }
            }
        }

        /// <summary>
        /// 战斗开始
        /// </summary>
        public void BattleStart()
        {
            battleFsm.SetEnable(true);

            var battleStartEvent = new BattleStartEvent();
            battleStartEvent.type = LogicConst.BattleType;

            var evData = new GameEventData(GameEventType.BattleEvent, battleStartEvent);
            Messenger.Broadcast<GameEventData>(EventNames.EvBattleStart, evData);
        }

        /// <summary>
        /// 挑战结束
        /// </summary>
        /// <param name="isWin"></param>
        public void BattleEnd()
        {
            battleFsm.SetEnable(false);

            if (LogicConst.BattleType == BattleType.TurnBase)
            {
                battleFsm.GetGlobalVar<uint>("tokenNpcId").value = 0;
                battleFsm.GetGlobalVar<bool>("isTakeNewToken").value = false;
            }
            ClearBattleNpcs();
            embattlePosMgr.Reset();
            evMappingMgr.Clear();

            var battleEndData = new BattleEndEvent();
            battleEndData.result = true;
            battleEndData.type = LogicConst.BattleType;

            var evData = new GameEventData(GameEventType.BattleEvent, battleEndData);
            Messenger.Broadcast<GameEventData>(EventNames.EvBattleEnd, evData);
        }

        /// <summary>
        /// 清理战斗NPC
        /// </summary>
        private void ClearBattleNpcs()
        {
            NpcType[] npcTypes = { NpcType.Hero, NpcType.Enemy };
            for (int i = 0; i < npcTypes.Length; i++)
            {
                var npcType = npcTypes[i];
                var npcDatas = npcDataMgr.GetNpcDatas(npcType);
                if (npcDatas.Count == 0)
                {
                    continue;
                }
                for (int j = 0; j < npcDatas.Count; j++)
                {
                    var target = npcDatas[j];
                    target.fsm.RemoveAllStates();
                    target.fsm = null;
                    Messenger.Broadcast<long>(EventNames.EvNpcDeath, target.npcid);
                }
                npcDataMgr.ClearNpcData(npcType);
            }
        }

        /// <summary>
        /// 技能攻击
        /// </summary>
        public void NpcSkillAttack(NPCData attacker, NPCData defender, bool useSkill, Action<object> callback)
        {
            bool isMoveToTarget = false;
            if (LogicConst.BattleType == BattleType.TurnBase)
            {
                isMoveToTarget = attacker.jobType == JobType.Warrior;
            }
            var attackEvent = new NpcSkillAttackEvent();
            attackEvent.attackerid = attacker.npcid;
            attackEvent.defenderid = defender.npcid;
            attackEvent.bMoveToTarget = isMoveToTarget;
            attackEvent.bPlaySound = LogicConst.BattleType == BattleType.TurnBase;
            attackEvent.bUseSkill = useSkill;

            attackEvent.currHp = defender.hp;
            attackEvent.maxHp = defender.hpMax;
            attackEvent.amount = LogicUtil.Random(-100, 0);
            attackEvent.callback = callback;

            var evData = new GameEventData(GameEventType.Battle, attackEvent);
            evMappingMgr.Add(evData);
            Messenger.Broadcast<GameEventData>(EventNames.EvNpcSkillAttack, evData);
        }

        /// <summary>
        /// 攻击完成
        /// </summary>
        /// <param name="eventId"></param>
        private void OnNpcSkillAttackOK(long eventId)
        {
            var gameEvent = evMappingMgr.Remove(eventId);
            if (gameEvent != null)
            {
                var attackEvent = gameEvent.evParam as NpcSkillAttackEvent;
                if (attackEvent != null)
                {
                    attackEvent.callback(attackEvent.defenderid);
                    GLogger.Log("OnNpcSkillAttackOK eventid:"+ eventId + " attacker:" + attackEvent.attackerid + " defender:" + attackEvent.defenderid);
                }
            }
        }

        /// <summary>
        /// NPC死亡
        /// </summary>
        public void NpcDeath(long npcid)
        {
            Messenger.Broadcast<long>(EventNames.EvNpcDeath, npcid);
        }

        /// <summary>
        /// 设置NPC朝向
        /// </summary>
        public void SetNpcFaceDir(long npcid, FaceDir faceDir)
        {
            Messenger.Broadcast<long, FaceDir>(EventNames.EvNpcFaceDir, npcid, faceDir);
        }

        /// <summary>
        /// 显示剧情对话
        /// </summary>
        public void ShowDialog(uint storyid, uint pageid, uint dlgid)
        {
            Messenger.Broadcast<uint, uint, uint>(EventNames.EvShowDialog, storyid, pageid, dlgid);
        }

        public override void OnDispose()
        {
            Messenger.RemoveListener<uint>(EventNames.EvBeginPlay, OnBeginPlay);
            Messenger.RemoveListener<long>(EventNames.EvEnterDungeonOK, OnEnterDungeonOK);
            Messenger.RemoveListener<long>(EventNames.EvNpcSkillAttackOK, OnNpcSkillAttackOK);
            Messenger.RemoveListener<long>(EventNames.EvNpcMoveOK, OnNpcMoveOK);
            Messenger.RemoveListener<long>(EventNames.EvNpcShowOK, OnNpcShowOK);
            Messenger.RemoveListener<long>(EventNames.EvNpcSpawnOK, OnNpcSpawnOK);
        }
    }
}
