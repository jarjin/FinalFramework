using System.Collections.Generic;
using log4net;
using FirServer.Interface;
using LiteNetLib;
using System;
using FirCommon.Define;

namespace FirServer.Manager
{
    public class HandlerManager : BaseManager
    {
        static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(HandlerManager));
        Dictionary<string, IHandler> mHandlers = new Dictionary<string, IHandler>();

        public override void Initialize()
        {
        }

        /// <summary>
        /// 添加处理器
        /// </summary>
        public void AddHandler(string protocal, IHandler handler)
        {
            if (mHandlers.ContainsKey(protocal))
            {
                return;
            }
            mHandlers.Add(protocal, handler);
        }

        /// <summary>
        /// 添加处理器
        /// </summary>
        public IHandler GetHandler(string protocal)
        {
            if (!mHandlers.ContainsKey(protocal))
            {
                return null;
            }
            return mHandlers[protocal];
        }

        /// <summary>
        /// 移除处理器
        /// </summary>
        public void RemoveHandler(string protocal)
        {
            mHandlers.Remove(protocal);
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        public void OnRecvData(NetPeer peer, NetPacketReader reader)
        {
            var protoType = (ProtoType)reader.GetByte();
            var protoName = reader.GetString();
            logger.Info("OnRecvData[commandid]:" + protoType + " protoName:" + protoName);

            if (!mHandlers.ContainsKey(protoName))
            {
                logger.Error("Proto ["+ protoName + "] not found!~~Reset to default!!!~");
                protoName = Protocal.Default;
            }
            var count = reader.GetInt();
            byte[] bytes = new byte[count];
            reader.GetBytes(bytes, count);

            IHandler handler = null;
            if (mHandlers.TryGetValue(protoName, out handler))
            {
                try
                {
                    if (handler != null)
                    {
                        var clientPeer = clientPeerMgr.GetClientPeer(peer);
                        if (clientPeer == null)
                        {
                            logger.Error("clientPeer was null!!!!");
                            return;
                        }
                        handler.OnMessage(clientPeer, bytes);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }
    }
}
