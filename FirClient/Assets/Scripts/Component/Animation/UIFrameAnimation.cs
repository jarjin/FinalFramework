using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FirClient.Component
{
    public class UIFrameAnimation : MonoBehaviour
    {
        public float time = 0.1f;
        public bool loop = true;
        public Sprite[] sprites;

        private int index;
        private float frameTime = 0;
        private Image image;

        // Use this for initialization
        void Awake()
        {
            Init();
        }

        void Init()
        {
            index = 0;
            image = Get<Image>(gameObject, "Animation");
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public T Get<T>(GameObject go, string subnode) where T : UnityEngine.Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null)
                    return sub.GetComponent<T>();
            }
            return null;
        }

        // Update is called once per frame
        void Update()
        {
            if (image != null && frameTime >= time)
            {
                image.sprite = sprites[index++];
                if (index == sprites.Length - 1)
                {
                    index = 0;
                    frameTime = 0;
                }
            }
            frameTime += Time.deltaTime;
        }
    }
}