using System;
using System.Collections.Generic;
using FTRuntime;
using FTRuntime.Yields;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using static FTRuntime.SwfClipController;

namespace FirClient.Component
{
    public class AnimNames
    {
        public const string Idle = "idle";
        public const string Run = "run";
        public const string Attack = "attack";
        public const string BeAttacked = "beAttacked";
        public const string Skill = "skill";
    }

    public class CSwf : SerializedMonoBehaviour
    {
        public event Action<string> onStopPlayingEvent;

        private SwfClip swfClip;
        private SwfClipController swfCtrl;

        [SerializeField]
        Dictionary<string, SwfClipAsset> swfAssets = new Dictionary<string, SwfClipAsset>();

        void Awake()
        {
            swfClip = GetComponent<SwfClip>();
            swfClip.sortingOrder = AppConst.RoleSortLayer;

            swfCtrl = GetComponent<SwfClipController>();
            swfCtrl.autoPlay = false;
        }

        public void SetLoopMode(bool v)
        {
            swfCtrl.loopMode = v ? LoopModes.Loop : LoopModes.Once;
        }

        /// <summary>
        /// 添加动画剪辑
        /// </summary>
        /// <param name="asset"></param>
        public void AddSwfClip(SwfClipAsset asset)
        {
            var strKeys = asset.name.Split('_');
            if (swfAssets.ContainsKey(strKeys[1]))
            {
                UnityEngine.Debug.LogError(asset);
                return;
            }
            swfAssets.Add(strKeys[1], asset);
        }

        /// <summary>
        /// 获取动画剪辑
        /// </summary>
        /// <param name="clipName"></param>
        /// <returns></returns>
        public SwfClipAsset GetSwfClip(string clipName)
        {
            SwfClipAsset asset = null;
            swfAssets.TryGetValue(clipName, out asset);
            return asset;
        }

        /// <summary>
        /// 播放动画剪辑
        /// </summary>
        /// <param name="clipName"></param>
        public void PlaySwfClip(string clip, bool isLoopPlay = false)
        {
            if (swfClip != null && swfCtrl != null)
            {
                if (!swfAssets.ContainsKey(clip))
                {
                    Debug.LogError("PlaySwfClip isnot exist:>" + clip);
                    return;
                }
                var asset = swfAssets[clip];
                if (asset != null)
                {
                    swfClip.clip = asset;
                }
                swfCtrl.loopMode = isLoopPlay ? LoopModes.Loop : LoopModes.Once;

                if (isLoopPlay)
                {
                    swfCtrl.Play("Default");
                }
                else
                {
                    StartCoroutine(OnPlayClip(clip));
                }
            }
        }

        public void PlayDefault(bool isLoopPlay = false)
        {
            if (swfClip != null && swfCtrl != null)
            {
                StartCoroutine(OnPlayClip("Default"));
            }
        }

        /// <summary>
        /// 播放动画剪辑
        /// </summary>
        IEnumerator OnPlayClip(string clip)
        {
            yield return swfCtrl.PlayAndWaitStopOrRewind("Default");
            if (onStopPlayingEvent != null)
            {
                onStopPlayingEvent(clip);
            }
        }

        /// <summary>
        /// 是否在播放中
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return swfCtrl.isPlaying;
        }
    }
}