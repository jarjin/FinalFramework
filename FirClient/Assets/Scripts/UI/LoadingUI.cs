using FirClient.UI.View;
using FirClient.Utility;
using System;
using UnityEngine;

namespace FirClient.UI
{
    public class LoadingUI : BaseBehaviour
    {
        private LoadingPanel panel;
        private static LoadingUI _instance;

        public static LoadingUI Instance()
        {
            if (_instance == null)
            {
                _instance = new LoadingUI();
            }
            return _instance;
        }

        public void Open(Action execOK)
        {
            string panelPath = "Prefabs/UI/LoadingPanel";
            var prefab = resMgr.LoadResAsset<GameObject>(panelPath);
            if (prefab != null)
            {
                var parent = GameObject.FindWithTag("UICanvas");
                var gameObj = Instantiate<GameObject>(prefab);
                gameObj.transform.SetParent(parent.transform);
                gameObj.transform.localPosition = Vector3.zero;
                gameObj.transform.localScale = Vector3.one;

                panel = gameObj.AddComponent<LoadingPanel>();
                if (execOK != null) execOK();
            }
        }

        public void UpdateLoadingProgress(float currValue, float maxValue)
        {
            if (panel.slider_loadingBar != null)
            {
                panel.slider_loadingBar.value = currValue / maxValue;
            }
        }

        public void UpdateLoadingText(string text)
        {
            if (panel.txt_status != null)
            {
                panel.txt_status.text = text;
            }
        }

        public void Close()
        {
            Util.UnloadAsset(panel.gameObject);
            Destroy(panel.gameObject, 0.5f);
        }
    }
}