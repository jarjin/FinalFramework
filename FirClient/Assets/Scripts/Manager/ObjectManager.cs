using UnityEngine;
using System.Collections.Generic;
using FirClient.ObjectPool;
using UnityEngine.Events;
using FirClient.Network;
using FirClient.Define;

namespace FirClient.Manager
{
    public class ObjectManager : BaseManager
    {
        private Transform m_PoolRootObject = null;
        private Dictionary<string, object> m_ObjectPools = new Dictionary<string, object>();
        private Dictionary<string, GameObjectPool> m_GameObjectPools = new Dictionary<string, GameObjectPool>();

        Transform PoolRootObject
        {
            get
            {
                if (m_PoolRootObject == null)
                {
                    var objectPool = new GameObject("ObjectPool");
                    objectPool.transform.SetParent(ManagementCenter.managerObject.transform);
                    objectPool.transform.localScale = Vector3.one;
                    objectPool.transform.localPosition = Vector3.zero;
                    m_PoolRootObject = objectPool.transform;
                }
                return m_PoolRootObject;
            }
        }

        public override void Initialize()
        {
            var abName1 = "Prefabs/Object/NPCObject";
            var assetNames1 = new string[] { "NPCObject" };
            resMgr.LoadAssetAsync<GameObject>(abName1, assetNames1, delegate (Object[] prefabs)
            {
                var npcPrefab = prefabs[0] as GameObject;
                this.CreatePool(PoolNames.NPC, 5, 10, npcPrefab, true);
            });

            var abName2 = "Prefabs/Object/BulletObject";
            var assetNames2 = new string[] { "BulletObject" };
            resMgr.LoadAssetAsync<GameObject>(abName2, assetNames2, delegate (Object[] prefabs)
            {
                var bulletPrefab = prefabs[0] as GameObject;
                this.CreatePool(PoolNames.BULLET, 5, 10, bulletPrefab);
            });

            var abName3 = "Prefabs/Object/EffectObject";
            var assetNames3 = new string[] { "EffectObject" };
            resMgr.LoadAssetAsync<GameObject>(abName3, assetNames3, delegate (Object[] prefabs)
            {
                var effectPrefab = prefabs[0] as GameObject;
                this.CreatePool(PoolNames.EFFECT, 5, 10, effectPrefab);
            });

            ///创建包对象池
            var packetPool = this.CreatePool<PacketData>(delegate (PacketData packet) { }, delegate (PacketData packet)
            {
                if (packet != null) packet.Reset(); //重置
            });
            for (int i = 0; i < AppConst.NetMessagePoolMax; i++)
            {
                packetPool.Release(new PacketData());
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public GameObjectPool CreatePool(string poolName, int initSize, int maxSize, GameObject prefab, bool selfGrowing = false)
        {
            var pool = new GameObjectPool(poolName, prefab, initSize, maxSize, PoolRootObject, selfGrowing);
            m_GameObjectPools[poolName] = pool;
            return pool;
        }

        public GameObjectPool GetPool(string poolName)
        {
            if (m_GameObjectPools.ContainsKey(poolName))
            {
                return m_GameObjectPools[poolName];
            }
            return null;
        }

        public GameObject Get(string poolName)
        {
            GameObject result = null;
            if (m_GameObjectPools.ContainsKey(poolName))
            {
                GameObjectPool pool = m_GameObjectPools[poolName];
                var poolObj = pool.NextAvailableObject();
                if (poolObj == null)
                {
                    Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
                }
                else
                {
                    result = poolObj.gameObject;
                }
            }
            else
            {
                Debug.LogError("Invalid pool name specified: " + poolName);
            }
            return result;
        }

        public void Release(GameObject gameObj)
        {
            if (gameObj == null || AppConst.AppState == AppState.Exiting)
            {
                return;
            }
            var poolObject = gameObj.GetComponent<PoolObject>();
            if (poolObject != null)
            {
                var poolName = poolObject.poolName;
                if (m_GameObjectPools.ContainsKey(poolName))
                {
                    var pool = m_GameObjectPools[poolName];
                    pool.ReturnObjectToPool(poolObject);
                }
                else
                {
                    Debug.LogWarning("No pool available with name: " + poolName);
                }
            }
        }

        ///-----------------------------------------------------------------------------------------------
        public ObjectPool<T> CreatePool<T>(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease) where T : class
        {
            var type = typeof(T);
            var pool = new ObjectPool<T>(actionOnGet, actionOnRelease);
            m_ObjectPools[type.Name] = pool;
            return pool;
        }

        public ObjectPool<T> GetPool<T>() where T : class
        {
            var type = typeof(T);
            ObjectPool<T> pool = null;
            if (m_ObjectPools.ContainsKey(type.Name))
            {
                pool = m_ObjectPools[type.Name] as ObjectPool<T>;
            }
            return pool;
        }

        public bool Exist<T>()
        {
            var type = typeof(T);
            return m_ObjectPools.ContainsKey(type.Name);
        }

        public T Get<T>() where T : class
        {
            var pool = GetPool<T>();
            if (pool != null)
            {
                return pool.Get();
            }
            return default(T);
        }

        public void Release<T>(T obj) where T : class
        {
            var pool = GetPool<T>();
            if (pool != null)
            {
                pool.Release(obj);
            }
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}