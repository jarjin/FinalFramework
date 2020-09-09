using FirClient.Data;
using FirClient.Define;
using FirClient.Logic.Manager;
using FirClient.Utility;
using FirClient.View;
using System;
using UnityEngine;

namespace FirClient.Manager
{
    public class BattleViewManager : BaseManager
    {
        private LogicManager logicMgr = null;

        public override void Initialize()
        {
            logicMgr = new LogicManager
            (
                Util.CameraHalfWidth()          //视口边距      
            );
            logicMgr.Initialize();              //启动逻辑层管理器

            isOnUpdate = true;
            Messenger.AddListener<GameEventData>(EventNames.EvNpcSpawn, OnNpcSpawn);
            Messenger.AddListener<GameEventData>(EventNames.EvNpcSkillAttack, OnNpcSkillAttack);
            Messenger.AddListener<long>(EventNames.EvNpcDeath, OnNpcDeath);
            Messenger.AddListener<Vector2, float>(EventNames.EvMoveCamera, OnMoveCamera);
            Messenger.AddListener<uint, Action>(EventNames.EvEnterScene, OnEnterScene);
            Messenger.AddListener<GameEventData>(EventNames.EvEnterDungeon, OnEnterDungeon);
            Messenger.AddListener<GameEventData>(EventNames.EvBattleStart, OnBattleStart);
            Messenger.AddListener<GameEventData>(EventNames.EvBattleEnd, OnBattleEnd);
            Messenger.AddListener<long>(EventNames.EvChooseNpc, OnChooseNpc);
            Messenger.AddListener<GameEventData>(EventNames.EvNpcMove, OnNpcMove);
            Messenger.AddListener<long, FaceDir>(EventNames.EvNpcFaceDir, OnNpcFaceDir);
            Messenger.AddListener<GameEventData>(EventNames.EvNpcShow, OnNpcShow);
            Messenger.AddListener<uint, uint, uint>(EventNames.EvShowDialog, OnShowDialog);
        }

        private void OnShowDialog(uint storyid, uint pageid, uint dlgid)
        {
            Util.CallLuaMethod("AddMsgItem", storyid, pageid, dlgid);
        }

        private void OnNpcFaceDir(long npcid, FaceDir dir)
        {
            var npcView = npcMgr.GetNpc(npcid) as RoleView;
            if (npcView != null)
            {
                npcView.SetFaceDir(dir);
            }
        }

        private void OnNpcMove(GameEventData eventData)
        {
            var evNpcMove = eventData.evParam as NpcMoveEvent;
            if (evNpcMove != null)
            {
                long npcid = evNpcMove.npcid;
                var npcView = npcMgr.GetNpc(npcid) as RoleView;
                if (npcView != null)
                {
                    long eventid = eventData.eventId;
                    Vector3 nextPos = evNpcMove.movePos;
                    float moveTime = evNpcMove.moveTime;
                    npcView.MoveTo(npcid, nextPos, moveTime, delegate (long moveNpcId)
                    {
                        Messenger.Broadcast<long>(EventNames.EvNpcMoveOK, eventid);
                    });
                    Debug.Log("OnNpcMove npcid:" + npcid + " nextPos:" + nextPos);
                }
            }
        }

        /// <summary>
        /// 选择NPC
        /// </summary>
        private void OnChooseNpc(long npcid)
        {
            npcMgr.Current = npcid;
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        private void OnEnterScene(uint mapid, Action moveNext)
        {
            Util.CallLuaMethod("EnterScene", mapid, moveNext);
        }

        /// <summary>
        /// 进入副本
        /// </summary>
        private void OnEnterDungeon(GameEventData eventData)
        {
            var enterEvent = eventData.evParam as EnterDungeonEvent;
            if (enterEvent != null)
            {
                long eventid = eventData.eventId;
                uint chapterid = enterEvent.chapterid;
                uint dungeonid = enterEvent.dungeonid;
                Util.CallLuaMethod("EnterDungeon", chapterid, dungeonid, (Action)delegate ()
                {
                    Messenger.Broadcast<long>(EventNames.EvEnterDungeonOK, eventid);
                });
            }
        }

        /// <summary>
        /// 战斗开始
        /// </summary>
        private void OnBattleStart(GameEventData eventData)
        {
            var battleStartEvent = eventData.evParam as BattleStartEvent;
            GLogger.Magenta("BattleStart----->>>" + battleStartEvent.type);
        }

        /// <summary>
        /// 战斗结束
        /// </summary>
        private void OnBattleEnd(GameEventData eventData)
        {
            var battleEndEvent = eventData.evParam as BattleEndEvent;
            var battleType = battleEndEvent.type;
            if (battleType == BattleType.TurnBase)
            {
                Util.CallLuaMethod("LeaveDungeon", (Action)delegate ()
                {
                    GLogger.Red("BattleEnd---->>>" + battleType);
                });
            }
            else
            {
                GLogger.Red("BattleEnd---->>>" + battleType);
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            Messenger.Broadcast<float>(EventNames.EvLogicUpdate, deltaTime);
        }

        /// <summary>
        /// 出生事件角色
        /// </summary>
        private void OnNpcSpawn(GameEventData evData)
        {
            var spawnEvent = evData.evParam as NpcSpawnEvent;
            if (spawnEvent != null)
            {
                var eventid = evData.eventId;
                var npcData = spawnEvent.npcData;
                var isShowHUD = spawnEvent.isShowHUD;

                var roleView = npcMgr.CreateNpc<RoleView>(npcData, battleScene.transform);
                npcMgr.AddNpc(npcData.npcid, roleView);

                roleView.Initialize(npcData, npcData.position, delegate ()
                {   
                    roleView.SpawnNpcObject(isShowHUD, Layers.Default);
                    Messenger.Broadcast<long>(EventNames.EvNpcSpawnOK, eventid);
                });
            }
        }

        /// <summary>
        /// NPC显示
        /// </summary>
        /// <param name="evData"></param>
        private void OnNpcShow(GameEventData evData)
        {
            var npcShowEvent = evData.evParam as NpcShowEvent;
            if (npcShowEvent != null)
            {
                var roleView = npcMgr.GetNpc(npcShowEvent.npcid) as RoleView;
                if (roleView != null)
                {
                    var eventid = evData.eventId;
                    var showTime = npcShowEvent.showTime;
                    roleView.ShowNpc(showTime, delegate ()
                    {
                        Messenger.Broadcast<long>(EventNames.EvNpcShowOK, eventid);
                    });
                }
            }
        }

        /// <summary>
        /// 移动相机
        /// </summary>
        private void OnMoveCamera(Vector2 pos, float time)
        {
            Util.CallLuaMethod("MoveCamera", pos, time);
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        private void OnNpcSkillAttack(GameEventData evData)
        {
            var evBattle = evData.evParam as NpcSkillAttackEvent;
            if (evBattle != null)
            {
                var eventid = evData.eventId;
                var attackerid = evBattle.attackerid;
                var defenderid = evBattle.defenderid;

                var attacker = npcMgr.GetNpc(attackerid) as RoleView;
                var defender = npcMgr.GetNpc(defenderid) as RoleView;
                if (attacker != null && defender != null)
                {
                    GLogger.White("OnAttackNpc:>>eventid:" + eventid + " attacker:" + attackerid + " defender:" + defenderid + " currhp:" + evBattle.currHp + " maxhp:" + evBattle.maxHp);
                    attacker.NpcSkillAttack(defender, evBattle, delegate ()
                    {
                        Messenger.Broadcast<long>(EventNames.EvNpcSkillAttackOK, eventid);
                    });
                }
            }
        }

        /// <summary>
        /// NPC死亡
        /// </summary>
        private void OnNpcDeath(long npcid)
        {
            var deathNpcId = npcid;
            var npcView = npcMgr.GetNpc(deathNpcId) as RoleView;
            if (npcView != null)
            {
                npcView.OnNpcDeath(delegate ()
                {
                    npcMgr.RemoveNpc<RoleView>(deathNpcId);
                });
            }
            Debug.LogWarning("OnNpcDeath:>>" + npcid);
        }

        public override void OnDispose()
        {
            isOnUpdate = false;
            Messenger.RemoveListener<GameEventData>(EventNames.EvNpcSpawn, OnNpcSpawn);
            Messenger.RemoveListener<GameEventData>(EventNames.EvNpcSkillAttack, OnNpcSkillAttack);
            Messenger.RemoveListener<long>(EventNames.EvNpcDeath, OnNpcDeath);
            Messenger.RemoveListener<Vector2, float>(EventNames.EvMoveCamera, OnMoveCamera);
            Messenger.RemoveListener<uint, Action>(EventNames.EvEnterScene, OnEnterScene);
            Messenger.RemoveListener<GameEventData>(EventNames.EvEnterDungeon, OnEnterDungeon);
            Messenger.RemoveListener<GameEventData>(EventNames.EvBattleStart, OnBattleStart);
            Messenger.RemoveListener<GameEventData>(EventNames.EvBattleEnd, OnBattleEnd);
            Messenger.RemoveListener<long>(EventNames.EvChooseNpc, OnChooseNpc);
            Messenger.RemoveListener<GameEventData>(EventNames.EvNpcMove, OnNpcMove);
            Messenger.RemoveListener<long, FaceDir>(EventNames.EvNpcFaceDir, OnNpcFaceDir);
            Messenger.RemoveListener<GameEventData>(EventNames.EvNpcShow, OnNpcShow);
            Messenger.RemoveListener<uint, uint, uint>(EventNames.EvShowDialog, OnShowDialog);
        }
    }
}

