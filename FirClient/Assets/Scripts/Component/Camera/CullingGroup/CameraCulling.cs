using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace FirClient.Component
{
    public class CameraCulling : GameBehaviour
    {
        static readonly object _lock = new object();

        public class Target
        {
            private const float size = 1;
            public Target(GameObject obj, Transform transform)
            {
                image = obj.GetComponent<Image>();
                tweenAnim = obj.GetComponent<TweenAnimation>();
                bound = new BoundingSphere(transform.position, size);
            }
            public Image image { get; private set; }
            public TweenAnimation tweenAnim { get; set; }
            public BoundingSphere bound { get; private set; }
        }
        private static CullingGroup cullingGroup;
        private static BoundingSphere[] cullingPoints;
        private static List<Target> targets = new List<Target>();

        void Awake()
        {
            if (!AppConst.DebugMode)
            {
                cullingGroup = new CullingGroup();
                cullingGroup.onStateChanged += OnCullingEvent;
                cullingGroup.targetCamera = GetComponent<Camera>();
            }
        }

        void Update()
        {
            if (cullingGroup != null && cullingPoints != null && targets.Count == cullingPoints.Length)
            {
                for (var i = 0; i < targets.Count; i++)
                {
                    var trans = targets[i].image.transform;
                    cullingPoints[i].position = trans.position;
                }
            }
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="target"></param>
        public static void AddTarget(Target target)
        {
            lock (_lock)
            {
                targets.Add(target);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public static void UpdateAll()
        {
            cullingPoints = targets.Select(c => c.bound).ToArray();
            cullingGroup.SetBoundingSpheres(cullingPoints);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="target"></param>
        public static void RemoveTarget(Target target)
        {
            lock (_lock)
            {
                targets.Remove(target);
            }
        }

        void OnCullingEvent(CullingGroupEvent ev)
        {
            var target = targets[ev.index];
            if (target != null)
            {
                target.image.enabled = ev.isVisible;
                target.tweenAnim.SetPlay(ev.isVisible);
            }
        }

        void OnDestroy()
        {
            if (cullingGroup != null)
            {
                cullingGroup.onStateChanged -= OnCullingEvent;
                cullingGroup.Dispose();
                cullingGroup = null;
            }
        }
    }
}