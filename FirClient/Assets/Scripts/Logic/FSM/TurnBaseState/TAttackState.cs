using FirClient.Component.FSM;
using FirClient.Data;
using UnityEngine;

namespace FirClient.Logic.FSM.TurnBaseState
{
    public class TAttackState : FsmState
    {
        private NpcFSM npcFsm;
        private NPCData myNpcData;
        private FsmVar<long> mynpcId;
        private FsmVar<long> targetId;
        private FsmVar<long> tokenNpcId;
        private FsmVar<bool> isTakeNewToken;

        public override void Initialize()
        {
            base.Initialize();
            npcFsm = (NpcFSM)Machine;
        }

        public override void Enter()
        {
            base.Enter();
            mynpcId = npcFsm.GetVar<long>("mynpcId");
            myNpcData = npcDataMgr.GetNpcData(mynpcId.value);

            targetId = npcFsm.GetVar<long>("targetId");
            tokenNpcId = npcFsm.GetGlobalVar<long>("tokenNpcId");
            isTakeNewToken = npcFsm.GetGlobalVar<bool>("isTakeNewToken");

            NpcAttack();
        }

        private void NpcAttack()
        {
            if (mynpcId != null && targetId != null)
            {
                var attacker = npcDataMgr.GetNpcData(mynpcId.value);
                var defender = npcDataMgr.GetNpcData(targetId.value);
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
                battleLogicMgr.NpcSkillAttack(attacker, defender, bUseSkill, OnNpcAttackOK);
                Debug.Log("attackid:>" + attacker + " targetid:>" + defender);
            }
        }

        /// <summary>
        /// 攻击完成，将TOKEN交还给BattleFSM
        /// </summary>
        private void OnNpcAttackOK(object o)
        {
            tokenNpcId.value = 0;
            isTakeNewToken.value = true;

            var defenderid = (long)o;
            var defender = npcDataMgr.GetNpcData(defenderid);
            if (defender != null && defender.hp == 0)
            {
                defender.fsm.ChangeState<TDeathState>();
            }
            Machine.ChangeState<TSearchState>();
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