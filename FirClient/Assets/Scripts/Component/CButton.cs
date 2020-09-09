using System;
using System.Collections;
using System.Collections.Generic;
using FirClient.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace  FirClient.Component
{
    public enum SoundType
    {
        None, 
        Button,
    }

    public class CButton : Button
    {
        public SoundType btnSoundType = SoundType.None;

        protected override void Awake() 
        {
            this.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            switch(btnSoundType) 
            {
                case SoundType.Button: break;
            }
            var soundMgr = ManagementCenter.GetManager<SoundManager>();
            soundMgr.Play("Audios/20001");
        }

        protected override void OnDestroy() {
            this.onClick.RemoveListener(OnClick);
        }
    }
}

