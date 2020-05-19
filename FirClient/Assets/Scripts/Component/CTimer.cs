using LuaInterface;
using System;
using System.Collections.Generic;

namespace FirClient.Component
{
    public class TimerInfo
    {
        public string name;
        public float expire;
        public float tick;
        public float interval;
        public object param;
        public LuaTable luaself;
        public LuaFunction luaFunc;
        public Action<object> sharpfunc;
    }

    public class TimeTicker
    {
        public uint typeId;
        public uint frameCount;
        public uint refCount;
        public object param;
        public Action<uint, object> action;
    }

    public class Ticker
    {
        public uint remain;
        public uint frameCount;
        public object param;
        public Action<object> action;
    }

    public class CTimer : BaseObject
    {
        private static CTimer instance;
        
        private float interval = 0;
        private object mlock = new object();
        private List<TimeTicker> expireTickers = new List<TimeTicker>();
        private List<TimeTicker> timeTickers = new List<TimeTicker>();

        private List<TimerInfo> expireTimers = new List<TimerInfo>();
        private HashSet<TimerInfo> timers = new HashSet<TimerInfo>();

        private List<Ticker> delTickers = new List<Ticker>();
        static HashSet<Ticker> tickers = new HashSet<Ticker>();

        public static CTimer Create()
        {
            if (instance == null)
            {
                instance = new CTimer();
            }
            return instance;
        }

        public override void Initialize()
        {
            isOnUpdate = true;
        }

        public override void OnUpdate(float deltaTime)
        {
            OnTimer(deltaTime);     //运行
            OnTicker(deltaTime);
            OnTimeTicker(deltaTime);    //
        }

        /// <summary>
        /// 添加计时器事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="o"></param>
        public TimerInfo AddTimer(float expires, float interval, Action<object> func, object param = null, bool runNow = false)
        {
            var timer = new TimerInfo();
            timer.interval = interval;
            timer.sharpfunc = func;
            timer.param = param;
            timer.expire = expires;
            timer.tick = runNow ? interval : 0;
            timers.Add(timer);
            return timer;
        }

        public TimerInfo AddLuaTimer(float expires, float interval, LuaTable self, LuaFunction func, object param, bool runNow)
        {
            var timer = new TimerInfo();
            timer.interval = interval;
            timer.luaself = self;
            timer.luaFunc = func;
            timer.param = param;
            timer.expire = expires;
            timer.tick = runNow ? interval : 0;
            timers.Add(timer);
            return timer;
        }

        /// <summary>
        /// 删除计时器事件
        /// </summary>
        /// <param name="name"></param>
        public void RemoveTimer(TimerInfo timer)
        {
            if (timer != null)
            {
                expireTimers.Add(timer);
            }
        }

        /// <summary>
        /// 计时器运行
        /// </summary>
        void OnTimer(float deltaTime)
        {
            if (timers.Count == 0)
            {
                return;
            }
            foreach (var timer in timers)
            {
                if (expireTimers.Contains(timer))
                {
                    continue;
                }
                timer.tick += deltaTime;
                if (timer.expire > 0)
                {
                    if (timer.tick >= timer.expire)
                    {
                        expireTimers.Add(timer);
                        if (timer.luaFunc != null)
                        {
                            if (timer.luaself == null)
                            {
                                timer.luaFunc.Call<object>(timer.param);
                            }
                            else
                            {
                                timer.luaFunc.Call<LuaTable, object>(timer.luaself, timer.param);
                            }
                        }
                        if (timer.sharpfunc != null)
                        {
                            timer.sharpfunc.Invoke(timer.param);
                        }
                    }
                }
                else
                {
                    if (timer.tick >= timer.interval)
                    {
                        timer.tick = 0;
                        if (timer.luaFunc != null)
                        {
                            timer.luaFunc.Call<LuaTable, object>(timer.luaself, timer.param);
                        }
                        if (timer.sharpfunc != null)
                        {
                            timer.sharpfunc.Invoke(timer.param);
                        }
                    }
                }
            }
            lock (mlock)
            {
                foreach (var timer in expireTimers)
                {
                    if (timer.luaself != null)
                    {
                        timer.luaself.Dispose();
                        timer.luaself = null;
                    }
                    if (timer.luaFunc != null)
                    {
                        timer.luaFunc.Dispose();
                        timer.luaFunc = null;
                    }
                    timers.Remove(timer);
                }
                expireTimers.Clear();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 添加帧动作
        /// </summary>
        /// <param name="kv">Key:ActionType,Value:frame</param>
        /// <param name="action">Callback</param>
        public void AddFrameActions(Dictionary<uint, uint> kv, object param, Action<uint, object> action)
        {
            foreach(var de in kv)
            {
                var ticker = new TimeTicker();
                ticker.typeId = de.Key;
                ticker.frameCount = de.Value;
                ticker.refCount = 0;
                ticker.action = action;
                ticker.param = param;
                timeTickers.Add(ticker);
            }
        }

        private void OnTimeTicker(float daltaTime)
        {
            if (timeTickers.Count == 0)
            {
                return;
            }
            foreach(TimeTicker ticker in timeTickers)
            {
                if (ticker.refCount == ticker.frameCount)
                {
                    expireTickers.Add(ticker);
                    ticker.action(ticker.typeId, ticker.param);
                }
                else
                {
                    ticker.refCount++;
                }
            }
            foreach (var timer in expireTickers)
            {
                timeTickers.Remove(timer);
            }
            expireTickers.Clear();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        public static uint TimeToFrame(uint ms)
        {
            return ms / 1000 * 33;
        }

        public Ticker CreateTicker(uint ms, object param, Action<object> action)
        {
            var ticker = new Ticker();
            ticker.param = param;
            ticker.action = action;
            ticker.frameCount = ticker.remain = TimeToFrame(ms);
            tickers.Add(ticker);
            return ticker;
        }

        void OnTicker(float deltaTime)
        {
            if (tickers.Count > 0)
            {
                delTickers.Clear();
                foreach (Ticker ticker in tickers)
                {
                    if (ticker.frameCount > 0)
                    {
                        if (--ticker.remain == 0)
                        {
                            delTickers.Add(ticker);
                        }
                    }
                    if (ticker.action != null)
                    {
                        ticker.action(ticker.param);
                    }
                }
                foreach (var t in delTickers)
                {
                    tickers.Remove(t);
                }
            }
        }

        public override void OnDispose()
        {
        }
    }
}