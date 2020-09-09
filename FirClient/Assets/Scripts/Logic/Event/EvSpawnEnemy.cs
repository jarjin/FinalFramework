using FirClient.Data;
using FirClient.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 出生敌人，
    /// </summary>
    internal class EvSpawnEnemy : BaseSceneEvent
    {
        public override void OnExecute(string param, Action moveNext)
        {
            Debug.Log("EvSpawnEnemy...");
            List<string> teamids = param.ToList<string>(',');
            battleHandlerMgr.InitNpcTeams(teamids, moveNext);
        }
    }
}