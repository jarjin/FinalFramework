using UnityEngine;
using System;
using System.Collections.Generic;
using UObject = UnityEngine.Object;
using FirClient.Data;

namespace FirClient.View
{
    public class NPCView : BaseBehaviour, INPCView
    {
        public GameObject gameObject;
        public ViewObject viewObject;

        protected GameObject roleObject;
        protected Dictionary<string, Action> actActions = new Dictionary<string, Action>();

        private NPCData npcData;
        public NPCData NpcData { get => npcData; set => npcData = value; }

        public virtual void OnAwake()
        { 
        }

        public virtual void Initialize(NPCData npcData, Vector3 spawnPos, Action initOK = null) 
        {
        }

        /// <summary>
        /// 加载NPC预制体
        /// </summary>
        protected void CreateNpcObject(string roleid, Vector3 pos, Vector2 scale, Action<GameObject> loadOK)
        {
            var path = "Prefabs/Character/" + roleid;
            resMgr.LoadAssetAsync<GameObject>(path, new [] { roleid }, delegate(UObject[] prefabs) 
            {
                if (prefabs[0] == null) return;
                var prefab = prefabs[0] as GameObject;
                if (prefab != null) 
                {
                    CreateNpcObject(prefab, pos, scale, loadOK);
                }
            });
        }

        /// <summary>
        /// 创建NPC对象
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="scale"></param>
        void CreateNpcObject(GameObject prefab, Vector3 pos, Vector2 scale, Action<GameObject> loadOK) 
        {
            gameObject.name = "user_" + NpcData.npcid;
            gameObject.transform.position = pos;

            roleObject = Instantiate<GameObject>(prefab);
            roleObject.name = "roleObject";
            roleObject.transform.SetParent(gameObject.transform);
            roleObject.transform.localPosition = Vector2.zero;
            roleObject.transform.localScale = scale;
            roleObject.transform.localEulerAngles = Vector3.zero;

            if (loadOK != null) loadOK(gameObject);
        }

        /// <summary>
        /// 播放NPC出生特效
        /// </summary>
        /// <param name="audioName"></param>
        protected void PlayNpcSpawnSound(string audioName) 
        {
            soundMgr.Play("Audios/" + audioName);
        }

        public virtual void OnUpdate()
        {
        }

        protected void OnNpcDeath()
        {
            if (roleObject != null)
            {
                Destroy(roleObject);
            }
        }

        public virtual void OnDispose()
        {
        }
    }
}