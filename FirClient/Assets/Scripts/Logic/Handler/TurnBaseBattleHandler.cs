using FirClient.Data;
using FirClient.Extensions;
using FirClient.Logic.FSM.TurnBaseState;
using System.Collections.Generic;

namespace FirClient.Logic.Handler
{
    public class TurnBaseBattleHandler : BaseBattleHandler
    {
        public override void InitNpcTeams(List<string> teamDatas)
        {
            turnTeams.Clear();
            foreach (string teamItem in teamDatas)
            {
                uint teamid = teamItem.ToUint();
                TeamData item = configMgr.GetTeamData(teamid);
                if (item != null)
                {
                    turnTeams.Enqueue(item);
                }
            }
        }

        public override async void StartFight()
        {
            await DoStartFight<TSearchState>(NpcType.Hero);
            await DoStartFight<TSearchState>(NpcType.Enemy);

            timerMgr.AddTimer(1, 0, (obj) =>
            {
                battleFsm.GetGlobalVar<bool>("isTakeNewToken").value = true;
            });
        }

        public override async void MoveNextTurn()
        {
            await DoStartFight<TSearchState>(NpcType.Enemy);
            battleFsm.GetGlobalVar<bool>("isTakeNewToken").value = true;
        }

        public override int GetTeamCount()
        {
            return turnTeams.Count;
        }

        public override TeamData NextTeamData()
        {
            return turnTeams.Dequeue();
        }

        public override void OnDispose()
        {
            turnTeams.Clear();
        }
    }
}