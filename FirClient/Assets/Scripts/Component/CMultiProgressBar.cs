using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FirClient.Extensions;

namespace FirClient.Component
{
    public class CMultiProgressBar : MonoBehaviour
    {
        [SerializeField] Image fillMiddle;
        [SerializeField] Image fillTop;
        [SerializeField] Image background;

        private void Start()
        {
            if (fillMiddle == null)
            {
                fillMiddle = gameObject.GetChild<Image>("FillMiddle");
            }
            if (fillTop == null)
            {
                fillTop = gameObject.GetChild<Image>("FillTop");
            }
            if (background == null)
            {
                background = gameObject.GetChild<Image>("Background");
            }
        }

        public void SetColors()
        {
        }

        public void SetValue(float v)
        {
            UpdateFillMiddle(v).OnComplete(() => UpdateFillTop(v));
        }

        public void Reset()
        {
            if (fillMiddle != null)
            {
                fillMiddle.fillAmount = 0;
            }
            if (fillTop != null)
            {
                fillTop.fillAmount = 0;
            }
        }

        Tween UpdateFillMiddle(float endValue)
        {
            float beginValue = fillMiddle.fillAmount;
            return DOTween.To(() => beginValue, x => beginValue = x, endValue, 0.5f);
        }

        void UpdateFillTop(float endValue)
        {
            float beginValue = fillMiddle.fillAmount;
            DOTween.To(() => beginValue, x => beginValue = x, endValue, 0.5f);
        }
    }
}

