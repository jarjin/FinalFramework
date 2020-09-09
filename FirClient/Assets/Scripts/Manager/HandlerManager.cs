using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirClient.Manager
{
    public class HandlerManager : BaseManager
    {
        public bool isRunning = false;
        private int clickCount = 0;
        private bool isPresseUp = false;
        private KeyCode[] KeyCodes = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };

        public override void Initialize()
        {
            isOnUpdate = true;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!isRunning)
            {
                return;
            }
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                SendMoveAction(new Vector2(-0.5f, 0.5f));
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                SendMoveAction(new Vector2(0.5f, 0.5f));
            }
            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                SendMoveAction(new Vector2(-0.5f, -0.5f));
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                SendMoveAction(new Vector2(0.5f, -0.5f));
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    SendMoveAction(new Vector2(0, 1));
                }
                if (Input.GetKey(KeyCode.S))
                {
                    SendMoveAction(new Vector2(0, -1));
                }
                if (Input.GetKey(KeyCode.A))
                {
                    SendMoveAction(new Vector2(-1, 0));
                }
                if (Input.GetKey(KeyCode.D))
                {
                    SendMoveAction(new Vector2(1, 0));
                }
            }
            //计数
            for (int i = 0; i < KeyCodes.Length; i++)
            {
                if (Input.GetKeyDown(KeyCodes[i]))
                {
                    clickCount++;
                    isPresseUp = false;
                }
                if (Input.GetKeyUp(KeyCodes[i]))
                {
                    clickCount--;
                }
            }
            if (clickCount == 0 && !isPresseUp)
            {
                isPresseUp = true;
                SendStopAction();
            }
        }

        void SendMoveAction(Vector2 vec)
        {
        }

        void SendStopAction()
        {
        }

        public override void OnDispose()
        {
            throw new System.NotImplementedException();
        }
    }
}