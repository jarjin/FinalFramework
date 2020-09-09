using UnityEngine;
using FirClient.Data;
using DG.Tweening;

namespace FirClient.View
{
    public class BulletView : ObjectView
    {
        private long objid;
        private Vector3 destPos;
        private Quaternion rotation;
        private float duration;
        private BulletData data;
        private GameObject gameObj;

        public void Initialize(BulletData data, long id, Vector3 pos, Quaternion angle, float duration)
        {
            this.data = data;
            this.objid = id;
            this.destPos = pos;
            this.rotation = angle;
            this.duration = duration;
        }

        public override void OnAwake()
        {
            var objName = "Bullet_" + objid;
            gameObject.name = objName;
            gameObject.SetActive(true);

            gameObj = objMgr.Get(data.name);
            gameObj.transform.SetParent(gameObject.transform);
            gameObj.transform.localScale = Vector3.one * 0.5f;
            gameObj.transform.localPosition = Vector3.zero;
            gameObj.transform.rotation = rotation;
            gameObj.SetActive(true);

            var spriteRender = gameObj.GetComponent<SpriteRenderer>();
            spriteRender.sortingOrder = LayerMask.NameToLayer("Effect");
            spriteRender.sortingLayerName = "Effect";

            //soundMgr.Play("Sounds/" + data.sound);
            gameObject.transform.DOMove(destPos, duration).SetEase(Ease.Linear).OnComplete(OnDispose);
        }

        public override void OnDispose()
        {
            Destroy(viewObject);
            objMgr.Release(gameObj);
            objMgr.Release(gameObject);
            this.data = null;
        }
    }
}