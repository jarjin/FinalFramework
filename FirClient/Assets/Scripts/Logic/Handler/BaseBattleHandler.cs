using FirClient.Component.FSM;
using FirClient.Data;
using FirClient.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FirClient.Logic.Handler
{
    public class BaseBattleHandler : LogicBehaviour
    {
        protected Queue<TeamData> turnTeams = new Queue<TeamData>();

        public virtual void StartFight() { }
        public virtual void MoveNextTurn() { }
        public virtual void InitNpcTeams(List<string> teamDatas) { }
        public virtual TeamData NextTeamData() { return null; }
        public virtual int GetTeamCount() { return 0; }


        protected async Task DoStartFight<T>(NpcType npcType) where T : FsmState
        {
            await new WaitForEndOfFrame();
            var npcDatas = npcDataMgr.GetNpcDatas(npcType);
            foreach (var npcData in npcDatas)
            {
                if (npcData != null)
                {
                    while (npcData.fsm == null || npcData.npcState != NpcState.Ready)
                    {
                        await new WaitForEndOfFrame();
                    }
                    npcData.fsm.ChangeState<T>();
                }
            }
        }
    }
}