using FirClient.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Component
{
    public class CEventObject : MonoBehaviour
    {
        [SerializeField][HideInInspector]
        private List<EventData> eventIds;

        public List<EventData> EventIds
        {
            get { return eventIds; }
        }

        [SerializeField][HideInInspector]
        private int m_selectedIndex = -1;

        /// <summary>
        /// 序列化事件
        /// </summary>
        public string SerializeEvents()
        {
            var result = new List<string>();
            if (EventIds != null)
            {
                foreach(EventData ev in EventIds)
                {
                    if (!string.IsNullOrEmpty(ev.value))
                    {
                        result.Add((uint)ev.type + ":" + ev.value);
                    }
                    else
                    {
                        result.Add(((uint)ev.type).ToString());
                    }
                }
            }
            return string.Join<string>("_", result);
        }
    }
}
