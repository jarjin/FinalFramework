using FirClient.Data;
using System.Collections.Generic;

namespace FirClient.Logic.Manager
{
    public class EventMappingManager : LogicBehaviour
    {
        private Dictionary<long, GameEventData> evMappings = new Dictionary<long, GameEventData>();

        /// <summary>
        /// 添加事件映射
        /// </summary>
        public void Add(GameEventData gameEvent)
        {
            long id = gameEvent.eventId;
            if (!evMappings.ContainsKey(id))
            {
                evMappings.Add(id, gameEvent);
            }
        }

        /// <summary>
        /// 移除并获取映射事件
        /// </summary>
        public GameEventData Remove(long id)
        {
            GameEventData result = null;
            evMappings.TryGetValue(id, out result);
            evMappings.Remove(id);
            return result;
        }

        /// <summary>
        /// 清理事件映射
        /// </summary>
        public void Clear()
        {
            evMappings.Clear();
        }
    }
}