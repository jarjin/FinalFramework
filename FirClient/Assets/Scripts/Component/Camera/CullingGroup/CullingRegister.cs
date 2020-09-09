using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FirClient.Component
{
    public class CullingRegister : GameBehaviour
    {
        CameraCulling.Target target;

        // Use this for initialization
        void Awake()
        {
            if (!AppConst.DebugMode)
            {
                GetComponent<Image>().enabled = false;

                target = new CameraCulling.Target(gameObject, transform);
                CameraCulling.AddTarget(target);
            }
        }

        void OnDestroy()
        {
            if (target != null)
            {
                CameraCulling.RemoveTarget(target);
                target = null;
            }
        }
    }
}
