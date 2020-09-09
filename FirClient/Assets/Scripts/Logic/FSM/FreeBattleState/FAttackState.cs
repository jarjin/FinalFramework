using FirClient.Component.FSM;
using UnityEngine;
using FirClient.Extensions;
using FirClient.Data;

namespace FirClient.Logic.FSM.FreeBattleState
{
    public class FAttackState : FsmState
    {
        private NPCData myNpcData;
        private FsmVar<long> mynpcId;
        private FsmVar<long> targetId;
        private NpcFSM npcFsm;

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

            myNpcData = npcDataMgr.GetNpcData(mynpcId.value);
            myNpcData.npcState = NpcState.Attack;

            GLogger.Log(mynpcId.value + " enter Attack...");
            NpcSkillAttack();
        }

        private void NpcSkillAttack()
        {
            if (myNpcData != null && myNpcData.hp > 0)
            {
                var defender = npcDataMgr.GetNpcData(targetId.value);
                if (defender != null)
                {
                    defender.hp -= defender.npcType == NpcType.Hero ? 0 : 50;
                    if (defender.hp < 0)
                    {
                        defender.hp = 0;
                    }
                    bool bUseSkill = false;
                    if (myNpcData.skillConsume > 0 && myNpcData.mp >= myNpcData.skillConsume)
                    {
                        bUseSkill = true;
                        myNpcData.mp -= myNpcData.skillConsume;
                    }
                    battleLogicMgr.NpcSkillAttack(myNpcData, defender, bUseSkill, OnNpcSkillAttackOK);
                }
                else
                {
                    TryResetNextTurn();
                }
            }
        }

        async void OnNpcSkillAttackOK(object o)
        {
            var defenderid = (long)o;
            var defender = npcDataMgr.GetNpcData(defenderid);
            if (defender != null)
            {
                if (defender.hp <= 0)
                {
                    defender.fsm.ChangeState<FDeathState>();
                }
                else
                {
                    var time = LogicUtil.Random(0.5f, 2f);
                    await new WaitForSeconds(time);
                    NpcSkillAttack();
                    return;
                }
            }
            TryResetNextTurn();
        }

        void TryResetNextTurn()
        {
            TryChangeState<FSearchState>();
            var enemyDatas = npcDataMgr.GetNpcDatas(NpcType.Enemy);
            if (enemyDatas.Count == 0)
            {
                battleHandlerMgr.MoveNextTurn();
            }
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}