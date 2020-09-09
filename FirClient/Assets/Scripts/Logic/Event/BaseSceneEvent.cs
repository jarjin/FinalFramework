using FirClient.Data;
using System;
using UnityEngine;

namespace FirClient.Logic.Event
{
    public class BaseSceneEvent : LogicBehaviour, ISceneEvent
    {
        public override void Initialize() { }
        public override void OnDispose() { }
        public virtual void OnExecute(string param, Action moveNext) { moveNext?.Invoke(); }
        public virtual void OnExecute(Vector2 currPos, string param, Action moveNext) { moveNext?.Invoke(); }

        /// <summary>
        /// 获取移动数据
        /// </summary>
        protected void TakeMoveData(Vector2 currPos, string param, ref float moveTime, ref Vector2? newPos)
        {
            if (string.IsNullOrEmpty(param))
            {
                newPos = new Vector2(currPos.x, currPos.y);
            }
            else
            {
                MoveObjectType moveType = MoveObjectType.CurrentPos;
                if (param.IndexOf(',') > -1)
                {
                    var paramStrs = param.Split(',');
                    moveType = (MoveObjectType)uint.Parse(paramStrs[0]);
                    moveTime = float.Parse(paramStrs[1]);
                }
                else
                {
                    moveType = (MoveObjectType)uint.Parse(param);
                }
                switch (moveType)
                {
                    case MoveObjectType.CurrentPos:
                        newPos = new Vector2(currPos.x, currPos.y);
                        break;
                    case MoveObjectType.NextPos:
                        var nextPos = eventMgr.GetNextPos();
                        if (nextPos != null)
                        {
                            newPos = new Vector2(nextPos.Value.x, nextPos.Value.y);
                        }
                        break;
                    case MoveObjectType.SpecifiedPos:
                        break;
                    default:
                        newPos = new Vector2(currPos.x, currPos.y);
                        break;
                }
            }
        }
    }
}
