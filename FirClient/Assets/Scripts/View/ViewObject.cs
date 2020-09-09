using System;

namespace FirClient.View
{
    public enum ViewType
    {
        Role,
        Animal,
        Bullet,
        Effect
    }

    public class ViewObject : GameBehaviour
    {
        private ViewType viewType;
        private INPCView npcView;
        private IObjectView objView;

        protected override void OnAwake()
        {
            if (npcView != null)
            {
                npcView.OnAwake();
            }
            if (objView != null)
            {
                objView.OnAwake();
            }
            base.OnAwake();
        }

        protected override void OnUpdate()
        {
            if (npcView != null)
            {
                npcView.OnUpdate();
            }
            if (objView != null)
            {
                objView.OnUpdate();
            }
            base.OnUpdate();
        }

        protected override void OnDestroyMe()
        {
            if (npcView != null)
            {
                npcView.OnDispose();
            }
            if (objView != null)
            {
                objView.OnDispose();
            }
            base.OnDestroyMe();
        }

        public void BindView(INPCView view)
        {  
            npcView = view;
            Type type = view.GetType();
            var _npcView = view as NPCView;
            if (_npcView != null)
            {
                _npcView.viewObject = this;
                _npcView.gameObject = gameObject;
            }
        }

        public void BindView(IObjectView view)
        {
            objView = view;
            var type = view.GetType();
            if (type == typeof(EffectView))
            {
                viewType = ViewType.Effect;
            }
            else if (type == typeof(BulletView))
            {
                viewType = ViewType.Bullet;
            }
            var _objView = objView as ObjectView;
            _objView.viewObject = this;
            _objView.gameObject = gameObject;
        }
    }
}

