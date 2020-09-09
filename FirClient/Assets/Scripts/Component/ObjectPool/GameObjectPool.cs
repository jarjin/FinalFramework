using UnityEngine;
using System.Collections.Generic;

namespace FirClient.ObjectPool
{
    public class GameObjectPool
    {
        private int maxSize;
        private int poolSize;
        private string poolName;
        private Transform poolRoot;
        private GameObject poolObjectPrefab;
        private bool selfGrowing;
        private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

        public GameObjectPool(string poolName, GameObject poolObjectPrefab, int initCount, int maxSize, Transform pool, bool selfGrowing = false)
        {
            this.poolName = poolName;
            this.poolSize = initCount;
            this.maxSize = maxSize;
            this.poolRoot = pool;
            this.poolObjectPrefab = poolObjectPrefab;
            this.selfGrowing = selfGrowing;

            for (int index = 0; index < initCount; index++)
            {
                AddObjectToPool(NewObjectInstance());
            }
        }

        //add to pool
        private void AddObjectToPool(PoolObject po)
        {
            po.name = poolName;
            po.gameObject.SetActive(false);
            availableObjStack.Push(po);
            po.isPooled = true;
            po.transform.SetParent(poolRoot, false);
        }

        private PoolObject NewObjectInstance()
        {
            var go = GameObject.Instantiate<GameObject>(poolObjectPrefab);
            go.name = poolName;
            var po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                po = go.AddComponent<PoolObject>();
            }
            po.poolName = poolName;
            return po;
        }

        public PoolObject NextAvailableObject()
        {
            PoolObject po = null;
            if (availableObjStack.Count > 0)
            {
                po = availableObjStack.Pop();
            }
            else if (poolSize < maxSize)
            {
                poolSize++;
                po = NewObjectInstance();
                Debug.Log(string.Format("Growing pool {0}. New size: {1}", poolName, poolSize));
            }
            else if (selfGrowing)
            {
                poolSize++;
                maxSize++;
                po = NewObjectInstance();
                Debug.LogWarning(string.Format("Growing pool {0}. New size: {1}", poolName, poolSize));
            }
            else
            {
                Debug.LogError("No object available & cannot grow pool: " + poolName);
            }
            return po;
        }

        public void ReturnObjectToPool(PoolObject obj)
        {
            if (poolName.Equals(obj.poolName))
            {
                AddObjectToPool(obj);
            }
            else
            {
                Debug.LogError(string.Format("Trying to add object to incorrect pool {0} ", poolName));
            }
        }
    }
}