using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public enum TweenType {
    None,
    SizeDelta,
    AnchPos,
}

namespace FirClient.Component
{
    public class TweenAnimation : GameBehaviour
    {
        public TweenType tweenType = TweenType.None;
        public int loops = -1;
        public LoopType type = LoopType.Yoyo;
        public float duration;
        public float delay;
        public float endSizeMultiple = 1f;
        public Vector2 endValue;
        public Ease ease;
        public bool Children = false;

        private List<Tweener> tweeners = new List<Tweener>();

        // Use this for initialization
        void Start()
        {
        }

        void Play(RectTransform rect)
        {
            Tweener tweener = null;
            switch (tweenType)
            {
                case TweenType.None: return;
                case TweenType.AnchPos:
                tweener = rect.DOAnchorPos(endValue, duration).SetEase(ease).SetDelay(delay).SetLoops(loops, type);
                break;
                case TweenType.SizeDelta:
                if (endSizeMultiple != 1f)
                {
                    endValue = rect.sizeDelta * endSizeMultiple;
                }
                tweener = rect.DOSizeDelta(endValue, duration).SetEase(ease).SetDelay(delay).SetLoops(loops, type);
                break;
            }
            tweener.Play();
            tweeners.Add(tweener);
        }

        public void SetPlay(bool isPlay)
        {
            if (isPlay)
            {
                tweeners.Clear();
                Play(transform as RectTransform);
                if (Children)
                {
                    var rects = transform.GetComponentsInChildren<RectTransform>();
                    for (int i = 0; i < rects.Length; i++)
                    {
                        Play(rects[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < tweeners.Count; i++)
                {
                    tweeners[i].Kill();
                }
                tweeners.Clear();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}