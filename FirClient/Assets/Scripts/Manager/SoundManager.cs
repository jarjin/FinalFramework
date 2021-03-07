using UnityEngine;
using System.Collections;
using FirClient.Utility;
using System;
using System.IO;
using LuaInterface;

namespace FirClient.Manager
{
    public class SoundManager : BaseManager
    {
        private AudioSource audio = null;
        private Hashtable sounds = new Hashtable();

        [NoToLua]
        public override void Initialize()
        {
            audio = ManagementCenter.managerObject.GetComponent<AudioSource>();
        }

        [NoToLua]
        public override void OnUpdate(float deltaTime)
        {
        }

        void Add(string key, AudioClip value)
        {
            if (sounds[key] != null || value == null)
            {
                return;
            }
            sounds.Add(key, value);
        }

        AudioClip Get(string key)
        {
            if (sounds[key] == null)
            {
                return null;
            }
            return sounds[key] as AudioClip;
        }

        void LoadAudioClip(string path, Action<AudioClip> action)
        {
            AudioClip ac = Get(path);
            if (ac == null)
            {
                var name = Path.GetFileNameWithoutExtension(path);
                resMgr.LoadAssetAsync<AudioClip>(path, new[] { name }, delegate (UnityEngine.Object[] objs)
                {
                    if (objs.Length == 0 || objs[0] == null)
                    {
                        return;
                    }
                    var clip = objs[0] as AudioClip;
                    if (clip != null)
                    {
                        Add(path, clip);
                        action(clip);
                    }
                });
            }
            else
            {
                action(ac);
            }
        }

        public bool CanPlayBackSound()
        {
            string key = AppConst.AppPrefix + "BackSound";
            int i = PlayerPrefs.GetInt(key, 1);
            return i == 1;
        }

        public void PlayBacksound(string name, bool canPlay)
        {
            if (audio.clip != null)
            {
                if (name == audio.clip.name)
                {
                    if (!canPlay)
                    {
                        audio.Stop();
                        audio.clip = null;
                        Util.ClearMemory();
                    }
                    return;
                }
            }
            if (canPlay)
            {
                LoadAudioClip(name, delegate(AudioClip clip)
                {
                    audio.clip = clip;
                    audio.loop = true;
                    audio.Play();
                });
            }
            else
            {
                audio.Stop();
                audio.clip = null;
                Util.ClearMemory();
            }
        }

        public bool CanPlaySoundEffect()
        {
            string key = AppConst.AppPrefix + "SoundEffect";
            int i = PlayerPrefs.GetInt(key, 1);
            return i == 1;
        }

        void PlayInternal(AudioClip clip, Vector3 position)
        {
            if (!CanPlaySoundEffect())
            {
                return;
            }
            AudioSource.PlayClipAtPoint(clip, position);
        }

        public void Play(string path)
        {
            LoadAudioClip(path, delegate (AudioClip clip)
            {
                if (clip != null)
                {
                    this.PlayInternal(clip, Vector3.zero);
                }
            });
        }

        [NoToLua]
        public override void OnDispose()
        {
        }
    }
}