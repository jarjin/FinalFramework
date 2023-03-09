using FirServer.Define;
using FirServer.Interface;
using Google.Protobuf;
using log4net;

namespace FirServer.Manager
{
    public class HandlerManager : BaseManager
    {
        private static readonly ILog logger = LogManager.GetLogger(AppConst.LogRepos?.Name, typeof(HandlerManager));
        private Dictionary<string, IHandler> mHandlers = new Dictionary<string, IHandler>();

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
        public IHandler? GetHandler(string protocal)
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
        /// 接收数据
        /// </summary>
        internal async Task OnRecvData(string protoName, MsgChannel channel, ByteString byteData)
        {
            mHandlers.TryGetValue(protoName, out IHandler? handler);
            if (handler != null)
            {
                try
                {
                    await handler.OnMessage(channel, byteData);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
            else
            {
                logger.Error("Proto [" + protoName + "] not found!~~");
            }
        }

        public override void OnDispose()
        {
        }
    }
}
