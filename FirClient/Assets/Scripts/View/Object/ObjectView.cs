using UnityEngine;

namespace FirClient.View
{
    public class ObjectView : BaseBehaviour, IObjectView
    {
        public GameObject gameObject;
        public ViewObject viewObject;

        public virtual void OnAwake()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnDispose()
        {
            if (viewObject != null)
            {
                Destroy(viewObject);
            }
        }
    }
}

