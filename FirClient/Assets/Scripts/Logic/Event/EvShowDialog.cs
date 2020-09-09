using FirClient.Extensions;
using System;
using UnityEngine;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 对话事件
    /// </summary>
    internal class EvShowDialog : BaseSceneEvent
    {
        public override void OnExecute(string param, Action moveNext)
        {
            Debug.Log("EvShowDialog:>" + param);
            var dlgData = param.ToList<uint>(',');
            if (dlgData.Count == 3)
            {
                battleLogicMgr.ShowDialog(dlgData[0], dlgData[1], dlgData[2]);
                if (moveNext != null) moveNext();
            }
            else if (dlgData.Count == 4)
            {
                var delayTime = dlgData[3];
                var nextAction = moveNext;
                battleLogicMgr.ShowDialog(dlgData[0], dlgData[1], dlgData[2]);
                timerMgr.AddTimer(delayTime, 0, (obj) =>
                {
                    if (nextAction != null) nextAction();
                });
            }
        }
    }
}