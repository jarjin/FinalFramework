using FirClient.Component.FSM;
using FirClient.Data;
using UnityEngine;
using FirClient.Extensions;
using System;

namespace FirClient.Logic.FSM.FreeBattleState
{
    public class FSearchState : FsmState
    {
        private NpcFSM npcFsm;
        private NPCData myNpcData;
        private FsmVar<long> mynpcId;
        private FsmVar<long> targetId;

        public override void Initialize()
        {
            base.Initialize();
            npcFsm = (NpcFSM)Machine;
        }

        public override void Enter()
        {
            base.Enter();

            mynpcId = npcFsm.GetVar<long>("mynpcId");
            targetId = npcFsm.GetVar<long>("targetId");
            targetId.value = 0;

            myNpcData = npcDataMgr.GetNpcData(mynpcId.value);
            myNpcData.npcState = NpcState.Search;

            GLogger.Log(mynpcId.value + " enter Search...");
        }

        Vector3 FindBattlePos()
        {
            Vector3 currPos = embattlePosMgr.GetBattlePos();
            GLogger.Gray("Find Battle Pos::>" + currPos + " npcid:>" + mynpcId.value);

            var viewPortMargin = LogicConst.Viewport_Margin;
            if (myNpcData.npcType == NpcType.Hero)
            {
                var targetData = npcDataMgr.GetNpcData(targetId.value);
                if (targetData.position.x < myNpcData.position.x)
                {
                    currPos.x -= viewPortMargin / 1.5f - 0.6f;
                }
                else
                {
                    currPos.x += viewPortMargin / 1.5f - 0.6f;
                }
            }
            else
            {
                var v = myNpcData.faceDir == FaceDir.Left ? 1 : -1;
                currPos.x += v * viewPortMargin / 1.5f;
            }
            currPos.y = myNpcData.position.y;
            currPos.z = myNpcData.position.z;
            return currPos;
        }

        void MoveToBattlePos(float moveTime, Action execOK)
        {
            var moveOK = execOK;
            var pos = FindBattlePos();
            if (Vector2.Distance(myNpcData.position, pos) <= 0.001f)
            {
                if (moveOK != null) moveOK();
                return;
            }
            SetNpcFaceDir(targetId.value);
            battleLogicMgr.NpcMove(mynpcId.value, pos, moveTime, delegate (object o) 
            {
                if (moveOK != null) moveOK();
            });
        }

        async void SwitchAttackState(float sec = 1f)
        {
            await new WaitForSeconds(sec);
            if (targetId != null && targetId.value > 0)
            {
                SetNpcFaceDir(targetId.value);
                TryChangeState<FAttackState>();
            }
        }

        /// <summary>
        /// 设置NPC脸朝向
        /// </summary>
        void SetNpcFaceDir(long targetNpcId)
        {
            var myData = npcDataMgr.GetNpcData(mynpcId.value);
            var targetData = npcDataMgr.GetNpcData(targetNpcId);
            if (myData != null && targetData != null)
            {
                var dir = myData.position.x >= targetData.position.x ? FaceDir.Left : FaceDir.Right;
                battleLogicMgr.SetNpcFaceDir(myData.npcid, dir);
            }
        }

        void FindAttackTarget()
        {
            var distance = 0f;
            var npcType = myNpcData.npcType == NpcType.Enemy ? NpcType.Hero : NpcType.Enemy;
            var npcDatas = npcDataMgr.GetNpcDatas(npcType);

            foreach (var data in npcDatas)
            {
                var tarDistance = Vector2.Distance(myNpcData.position, data.position);
                if (distance == 0f || tarDistance < distance)
                {
                    distance = tarDistance;
                    targetId.value = data.npcid;
                }
            }
        }

        void SearchTarget()
        {
            if (myNpcData.npcState != NpcState.Search)
            {
                return;
            }
            this.FindAttackTarget();
            if (targetId == null || targetId.value == 0)
            {
                return;
            }
            if (myNpcData.npcType == NpcType.Enemy)
            {
                myNpcData.npcState = NpcState.LockTarget;
                MoveToBattlePos(2, delegate ()
                {
                    SwitchAttackState(0.5f);
                });
            }
            else
            {
                var targets = npcDataMgr.GetNpcDatas(NpcType.Enemy);
                if (targets.Count > 0)
                {
                    myNpcData.npcState = NpcState.LockTarget;
                    var roleData = configMgr.GetRoleData(myNpcData.roleid);
                    if (roleData != null)
                    {
                        switch (roleData.job)
                        {
                            case JobType.Warrior:
                                MoveToBattlePos(1.5f, delegate ()
                                {
                                    SwitchAttackState(0.5f);
                                });
                            break;
                            case JobType.Mage:
                            case JobType.Archer:
                                SwitchAttackState(1f);
                            break;
                        }
                    }
                }
            }
        }

        public override void Execute()
        {
            SearchTarget();
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}