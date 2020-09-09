using System.Collections.Generic;
using FirClient.Define;
using FirClient.Extensions;
using FirClient.View;
using UnityEngine;

namespace FirClient.Manager
{
    public class EffectManager : BaseManager
    {
        private static long objId = 0;
        private Dictionary<long, EffectView> mEffects = new Dictionary<long, EffectView>();

        public override void Initialize()
        {
            var effectList = configMgr.GetEffectList();
            foreach (var de in effectList)
            {
                var abPath = "Prefabs/Effect/" + de.Value.resource;
                resMgr.LoadAssetAsync<GameObject>(abPath, new string[] { de.Value.resource }, delegate(Object[] prefabs)
                {
                    var poolName = de.Value.name;
                    var effectPrefab = prefabs[0] as GameObject;
                    objMgr.CreatePool(poolName, 5, 10, effectPrefab);
                });
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// 播放一个特效
        /// </summary>
        public EffectView Create(uint effectId, Vector3 spawnPos, bool bPlaySound = true)
        {
            var data = configMgr.GetEffectData(effectId.ToString());
            var effectObj = objMgr.Get(PoolNames.EFFECT);
            if (effectObj != null)
            {
                effectObj.transform.SetParent(battleScene.transform);
                effectObj.transform.SetLayer(Layers.Default);

                var effect = new EffectView();
                effect.Initialize(data, ++objId, spawnPos, bPlaySound);
                effectObj.AddComponent<ViewObject>().BindView(effect);
                effect.gameObject.SetActive(true);

                mEffects.Add(objId, effect);
                return effect;
            }
            return null;
        }

        /// <summary>
        /// 获取特效
        /// </summary>
        public EffectView GetView(long id)
        {
            EffectView view = null;
            mEffects.TryGetValue(id, out view);
            return view;
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}