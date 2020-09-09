using FirClient.Data;
using System;
using UnityEngine;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 出生英雄NPC
    /// </summary>
    public class EvSpawnHero : BaseSceneEvent
    {
        public override void OnExecute(string param, Action moveNext)
        {
            Debug.Log("EvSpawnHero..." + param);
            battleHandlerMgr.SpawnHeros(moveNext);
        }
    }
}

