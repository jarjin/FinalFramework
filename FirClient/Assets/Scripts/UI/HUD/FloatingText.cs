using FirClient.Extensions;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace FirClient.HUD
{
    public class FloatingText : GameBehaviour
    {
        private TextMeshProUGUI textMesh;

        public static void Create(GameObject parent, Vector3 pos, int amount)
        {
            var textPrefab = Utility.Util.GetFloatingTextPrefab();
            if (textPrefab != null)
            {
                var newPos = pos + new Vector3(0, 1f);
                var gameObj = Instantiate<GameObject>(textPrefab, newPos, Quaternion.identity);
                gameObj.transform.SetParent(parent.transform);
                gameObj.transform.localScale = Vector3.one;
                gameObj.GetComponent<FloatingText>().Setup(amount);     //设置
            }

        }

        protected override void OnAwake()
        {
            textMesh = transform.GetChild<TextMeshProUGUI>("Text");
        }

        public void Setup(int damageAmount)
        {
            if (textMesh != null)
            {
                textMesh.SetText(damageAmount.ToString());
            }
            var rect = transform as RectTransform;
            if (rect)
            {
                rect.DOAnchorPosY(3, 0.5f).SetEase(Ease.InBack).OnComplete(delegate () { Destroy(gameObject); });
            }
        }
    }
}