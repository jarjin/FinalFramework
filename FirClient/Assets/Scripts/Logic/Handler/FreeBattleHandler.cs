using FirClient.Data;
using FirClient.Extensions;
using FirClient.Logic.FSM.FreeBattleState;
using System.Collections.Generic;

namespace FirClient.Logic.Handler
{
    public class FreeBattleHandler : BaseBattleHandler
    {
        public override void InitNpcTeams(List<string> teamDatas)
        {
            turnTeams.Clear();
            foreach (string teamItem in teamDatas)
            {
                var items = teamItem.ToList<uint>('-');
                var teamid = items[0];
                var turnNum = items[1];
                var maxNum = items[2];
                TeamData itemData = configMgr.GetTeamData(teamid);
                if (itemData != null)
                {
                    for (int i = 0; i < turnNum; i++)
                    {
                        TeamData newItem = new TeamData();
                        newItem.id = itemData.id;
                        newItem.teamNpcs = new List<TeamNpcData>();
                        for (int j = 0; j < maxNum; j++)
                        {
                            int index = LogicUtil.Random(0, itemData.teamNpcs.Count);
                            var teamNpc = itemData.teamNpcs[index];
                            newItem.teamNpcs.Add(teamNpc);
                        }
                        turnTeams.Enqueue(newItem);
                    }
                }
            }
        }

        public override async void StartFight()
        {
            await DoStartFight<FSearchState>(NpcType.Hero);
            await DoStartFight<FSearchState>(NpcType.Enemy);
        }

        public override async void MoveNextTurn()
        {
            await DoStartFight<FSearchState>(NpcType.Enemy);
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