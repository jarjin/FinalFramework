using System;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 进入场景
    /// </summary>
    internal class EvLoadScene : BaseSceneEvent
    {
        public override void OnExecute(string param, Action moveNext)
        {
            battleLogicMgr.EnterScene(uint.Parse(param), moveNext);
        }
    }
}