using UnityEngine;
using System.Collections.Generic;
using FirClient.View;
using FirClient.Define;
using FirClient.Data;
using LuaInterface;

namespace FirClient.Manager
{
    public class NPCManager : BaseManager
    {
        private readonly object npcLock = new object();
        private Dictionary<long, INPCView> mNpcs = new Dictionary<long, INPCView>();

        public long Current { get; set; }

        public Dictionary<long, INPCView> Npcs
        {
            get { return mNpcs; }
        }

        [NoToLua]
        public override void Initialize()
        {
        }

        [NoToLua]
        public override void OnUpdate(float deltaTime)
        {
        }

        public void AddNpc(long npcid, INPCView view)
        {
            lock (npcLock)
            {
                if (!Npcs.ContainsKey(npcid))
                {
                    Npcs.Add(npcid, view);
                }
            }
        }

        public INPCView GetNpc(long npcid)
        {
            lock (npcLock)
            {
                if (Npcs.ContainsKey(npcid))
                {
                    return Npcs[npcid];
                }
                return null;
            }
        }

        public void RemoveNpc(long npcid)
        {
            lock (npcLock)
            {
                Npcs.Remove(npcid);
            }
        }

        public INPCView GetCurrentNpc()
        {
            return GetNpc(Current);
        }

        /// <summary>
        /// 创建NPC
        /// </summary>
        public T CreateNpc<T>(NPCData npcData, Transform parent) where T : NPCView, new()
        {
            var gameObj = objMgr.Get(PoolNames.NPC);     //一个客户端对象
            gameObj.transform.SetParent(parent);
            gameObj.transform.localScale = Vector3.one;
            gameObj.transform.localPosition = Vector3.zero;
            gameObj.gameObject.SetActive(true);

            var npcView = new T();
            npcView.NpcData = npcData;
            gameObj.GetComponent<ViewObject>().BindView(npcView);
            return npcView;
        }                                                         

        /// <summary>
        /// 移除角色
        /// </summary>
        public void RemoveNpc<T>(long npcid) where T : NPCView
        {
            var view = GetNpc(npcid);
            if (view != null)
            {
                var npcView = view as NPCView;
                if (npcView != null)
                {
                    npcView.OnDispose();
                    objMgr.Release(npcView.gameObject);
                }
            }
            RemoveNpc(npcid);
        }

        [NoToLua]
        public override void OnDispose()
        {
        }
    }
}