using FirClient.Component;
using FirClient.Data;
using UnityEngine;
using FirClient.Extensions;

namespace FirClient.View 
{
    public class EffectView : ObjectView 
    {
        private long objid;
        private CAnimActor antActor;
        private GameObject gameObj;
        private CSwf swf;

        private bool isPlaySound;
        private EffectData data;
        private Vector3 spawnPos;

        public void Initialize(EffectData data, long id, Vector3 spawnPos, bool bPlaySound) 
        {
            this.objid = id;
            this.data = data;
            this.spawnPos = spawnPos;
            this.isPlaySound = bPlaySound;
        }

        public override void OnAwake() 
        {
            gameObj = objMgr.Get(data.name);
            gameObj.transform.SetParent(gameObject.transform);
            gameObj.transform.localScale = data.scale;
            gameObj.transform.localPosition = Vector3.zero;
            gameObj.SetLayer(Layers.Default);
            gameObj.SetActive(true);

            gameObject.name = "Effect_" + objid;
            gameObject.transform.position = spawnPos;

            if (data.type == EffectType.Sprite) //序列帧特效
            {
                var spriteRender = gameObj.GetComponent<SpriteRenderer>();
                spriteRender.sortingOrder = LayerMask.NameToLayer("Effect");
                spriteRender.sortingLayerName = "Effect";

                antActor = gameObj.GetComponent<CAnimActor>();
                antActor.timeScale = 0.8f;
                antActor.EventAnimationComplete += OnActorCompleted;
            } 
            else if(data.type == EffectType.Flash) //FLASH特效
            {
                swf = gameObj.GetComponent<CSwf>();
                swf.onStopPlayingEvent += OnSwfEffectOK;
                swf.PlayDefault();
            }
            if (isPlaySound)
            {
                soundMgr.Play("Audios/" + data.sound);
            }
        }

        private void OnSwfEffectOK(string obj)
        {
            Destroy(viewObject);
        }

        void OnActorCompleted(CAnimActor aActor, string aAnimationName) 
        {
            Destroy(viewObject);
        }

        public override void OnDispose() 
        {
            if(data.type == EffectType.Sprite) 
            {
                antActor.EventAnimationComplete -= OnActorCompleted;
            }
            objMgr.Release(gameObj);
            objMgr.Release(gameObject);
            this.data = null;
        }
    }

}