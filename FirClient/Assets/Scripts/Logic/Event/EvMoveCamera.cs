using FirClient.Define;
using System;
using UnityEngine;

namespace FirClient.Logic.Event
{
    /// <summary>
    /// 移动相机事件
    /// </summary>
    internal class EvMoveCamera : BaseSceneEvent
    {
        public override void OnExecute(Vector2 currPos, string param, Action moveNext)
        {
            float moveTime = 0f;
            Vector2? movePos = null;
            TakeMoveData(currPos, param, ref moveTime, ref movePos);
            var newPos = new Vector2(movePos.Value.x, 0);

            Messenger.Broadcast<Vector2, float>(EventNames.EvMoveCamera, newPos, moveTime);
            if (moveNext != null) moveNext();
        }
    }
}