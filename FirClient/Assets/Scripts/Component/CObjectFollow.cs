using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Component
{
    public enum FollowType
    {
        ObjectFollowCamera,     //对象跟随相机
        CameraFollowRole,       //相机跟随角色
    }

    [Serializable]
    class FollowInfo
    {
        public string itemName;
        public FollowType followType;
        public float offset_x = 0.2f;
        public Transform cameraTarget = null;
        public Transform followTarget = null;
    }

    public class CObjectFollow : MonoBehaviour
    {
        [SerializeField] List<FollowInfo> follows = new List<FollowInfo>();

        public void AddItem(string name, FollowType type, float offset_x, Transform camera, Transform target)
        {
            foreach(var de in follows)
            {
                if (de.itemName == name)
                {
                    return;     //避免重复添加
                }
            }
            if (camera == null)
            {
                camera = Camera.main.transform;
            }
            follows.Add(new FollowInfo()
            {
                itemName = name,
                followType = type,
                offset_x = offset_x,
                cameraTarget = camera,
                followTarget = target,
            });
        }

        public void RemoveItem(string strKey)
        {
            for(int i = follows.Count - 1; i >= 0; i--)
            {
                if (follows[i].itemName == strKey)
                {
                    follows.RemoveAt(i);
                }
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (follows.Count > 0)
            {
                foreach (FollowInfo info in follows)
                {
                    switch (info.followType)
                    {
                        case FollowType.ObjectFollowCamera:
                            OnObjectFollowCamera(info);
                        break;
                        case FollowType.CameraFollowRole:
                            OnCameraFollowRole(info);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 相机跟随角色
        /// </summary>
        private void OnCameraFollowRole(FollowInfo info)
        {
            if (info.followTarget != null && info.cameraTarget != null)
            {
                var velocity = info.followTarget.position;
                velocity.y = info.cameraTarget.position.y;
                info.cameraTarget.position = velocity;
            }
        }

        /// <summary>
        /// 对象跟随相机
        /// </summary>
        private void OnObjectFollowCamera(FollowInfo info)
        {
            if (info.followTarget != null && info.cameraTarget != null)
            {
                var velocity = info.followTarget.position;
                var x = info.cameraTarget.position.x * info.offset_x;
                info.followTarget.position = new Vector3(x, velocity.y, velocity.z);
            }
        }
    }
}
