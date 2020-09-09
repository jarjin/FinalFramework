using UnityEngine;
using System.Collections;

namespace FirClient.Component
{
    public class CameraBase : GameBehaviour {
        protected override void OnAwake()
        {
            var camera = transform.Find("Camera");
            if (camera != null)
            {
                camera.GetComponent<Camera>().eventMask = 0;
            }
            base.OnAwake();
        }
    }
}