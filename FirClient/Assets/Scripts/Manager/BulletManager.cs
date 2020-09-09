using System.Collections.Generic;
using FirClient.Define;
using FirClient.View;
using UnityEngine;
using FirClient.Extensions;

namespace FirClient.Manager
{
    public class BulletManager : BaseManager
    {
        private static long objId = 0;
        private Dictionary<long, BulletView> mBullets = new Dictionary<long, BulletView>();

        public override void Initialize()
        {
            var bulletList = configMgr.GetBulletList();
            foreach (var de in bulletList)
            {
                var abPath = "Prefabs/Bullet/" + de.Value.resource;
                resMgr.LoadAssetAsync<GameObject>(abPath, new string[] { de.Value.resource }, delegate (Object[] prefabs)
                {
                    var poolName = de.Value.name;
                    var bulletPrefab = prefabs[0] as GameObject;
                    objMgr.CreatePool(poolName, 5, 10, bulletPrefab);
                });
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public BulletView Create(uint bulletid, Vector3 startPos, Vector3 destPos, float duration)
        {
            Vector3 v = destPos - startPos;
            v.z = 0;
            var rotation = Quaternion.FromToRotation(Vector3.right, v);

            var data = configMgr.GetBulletData(bulletid.ToString());
            var bulletObj = objMgr.Get(PoolNames.BULLET);
            if (bulletObj != null)
            {
                var bullet = new BulletView();
                bullet.Initialize(data, ++objId, destPos, rotation, duration);

                bulletObj.transform.SetParent(battleScene.transform);
                bulletObj.transform.SetLayer(Layers.Default);
                bulletObj.transform.position = startPos;
                bulletObj.AddComponent<ViewObject>().BindView(bullet);
                bulletObj.SetActive(true);

                mBullets.Add(objId, bullet);
                return bullet;
            }
            return null;
        }

        public BulletView GetView(long objId)
        {
            BulletView view = null;
            mBullets.TryGetValue(objId, out view);
            return view;
        }

        public bool RemoveView(long objId)
        {
            return mBullets.Remove(objId);
        }

        public override void OnDispose()
        {
        }
    }
}
