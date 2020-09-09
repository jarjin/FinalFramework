using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T GetChild<T>(this GameObject go, string subnode) where T : UnityEngine.Component
        {
            if (go != null)
            {
                Transform sub = go.transform.Find(subnode);
                if (sub != null)
                    return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T GetChild<T>(this Transform go, string subnode) where T : UnityEngine.Component
        {
            if (go != null)
            {
                Transform sub = go.Find(subnode);
                if (sub != null)
                    return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject GetChild(this Transform go, string subnode)
        {
            Transform tran = go.Find(subnode);
            if (tran == null)
                return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject GetPeer(this Transform go, string subnode)
        {
            Transform tran = go.parent.Find(subnode);
            if (tran == null)
                return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(this Transform go)
        {
            if (go == null)
                return;
            for (int i = go.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 设置层
        /// </summary>
        public static void SetLayer(this Transform root, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (root != null)
            {
                Stack<Transform> children = new Stack<Transform>();
                children.Push(root);
                while (children.Count > 0)
                {
                    Transform currentTransform = children.Pop();
                    currentTransform.gameObject.layer = layer;
                    foreach (Transform child in currentTransform)
                    {
                        children.Push(child);
                    }
                }
            }
        }

        public static void SetLayer(this GameObject gameObj, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            var trans = gameObj.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trans)
            {
                t.gameObject.layer = layer;
            }
        }
    }
}
