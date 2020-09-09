using LuaInterface;
using UnityEngine;

namespace FirClient.Component
{
    public class CLuaAnimator : MonoBehaviour
    {
        private bool canPlay = false;
        private LuaTable self;
        private LuaFunction onPlayEnd;
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Initialize(LuaTable self, LuaFunction endCall)
        {
            this.self = self;
            this.onPlayEnd = endCall;
        }

        public void Play(string animName)
        {
            if (animator != null)
            {
                canPlay = true;
                animator.Play(animName);
            }
        }

        void Update()
        {
            if (animator != null && canPlay)
            {
                var info = animator.GetCurrentAnimatorStateInfo(0);
                if (info.normalizedTime >= 1.0f)
                {
                    canPlay = false;
                    onPlayEnd.Call<LuaTable, GameObject>(self, gameObject);
                }
            }
        }

        public void Dispose()
        {
            if (self != null)
            {
                self.Dispose();
                self = null;
            }
            if (onPlayEnd != null)
            {
                onPlayEnd.Dispose();
                onPlayEnd = null;
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}