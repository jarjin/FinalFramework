using FirClient.Component;
using FirClient.Utility;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace FirClient.UI
{
    public class LoadingUI : BaseBehaviour
    {
        private static LoadingUI _instance;

        private GameObject gameObject;
        private CPrefabVar mPrefabVar;
        private Text txt_status;
        private Slider slider_loadingBar;

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
                gameObject = Instantiate<GameObject>(prefab);
                gameObject.name = "LoadingPanel";
                gameObject.transform.SetParent(parent.transform);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;

                mPrefabVar = gameObject.GetComponent<CPrefabVar>();
                txt_status = mPrefabVar.GetVar<Text>("txt_status", VarType.Text);
                slider_loadingBar = mPrefabVar.GetVar<Slider>("slider_loadingBar", VarType.Slider);
                if (execOK != null) execOK();
            }
        }

        public void UpdateLoadingProgress(float currValue, float maxValue)
        {
            if (slider_loadingBar != null)
            {
                slider_loadingBar.value = currValue / maxValue;
            }
        }

        public void UpdateLoadingText(string text)
        {
            if (txt_status != null)
            {
                txt_status.text = text;
            }
        }

        public void Close()
        {
            Util.UnloadAsset(gameObject);
            Destroy(gameObject, 0.5f);
        }
    }
}