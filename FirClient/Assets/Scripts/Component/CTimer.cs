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

    public class TickerInfo
    {
        public uint typeId;
        public uint frameCount;
        public uint refCount;
        public object param;
        public Action<uint, object> action;
    }

    public class CTimer : BaseObject
    {
        private static CTimer instance;
        
        private readonly object mlock = new object();

        private List<TickerInfo> tickers = new List<TickerInfo>();
        private List<TickerInfo> expireTickers = new List<TickerInfo>();

        private List<TimerInfo> timers = new List<TimerInfo>();
        private List<TimerInfo> expireTimers = new List<TimerInfo>();

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
            OnTimer(deltaTime);     //����
            OnTicker(deltaTime);    //
        }

        public TimerInfo AddTimer(float expires, float interval, Action<object> func, object param = null, bool runNow = false)
        {
            lock (mlock)
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
        }

        public TimerInfo AddLuaTimer(float expires, float interval, LuaTable self, LuaFunction func, object param, bool runNow)
        {
            lock (mlock)
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
        }

        public void RemoveTimer(TimerInfo timer)
        {
            lock (mlock)
            {
                if (timer != null)
                {
                    expireTimers.Add(timer);
                }
            }
        }

        void OnTimer(float deltaTime)
        {
            if (timers.Count > 0)
            {
                for (int i = timers.Count - 1; i >= 0; i--)
                {
                    var timer = timers[i];
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
                            if (timer.sharpfunc != null)
                            {
                                timer.sharpfunc.Invoke(timer.param);
                            }
                            if (timer.luaFunc != null)
                            {
                                timer.luaFunc.Call<LuaTable, object>(timer.luaself, timer.param);
                            }
                        }
                    }
                    else
                    {
                        if (timer.tick >= timer.interval)
                        {
                            timer.tick = 0;
                            if (timer.sharpfunc != null)
                            {
                                timer.sharpfunc.Invoke(timer.param);
                            }
                            if (timer.luaFunc != null)
                            {
                                timer.luaFunc.Call<LuaTable, object>(timer.luaself, timer.param);
                            }
                        }
                    }
                }
                lock (mlock)
                {
                    foreach (var timer in expireTimers)
                    {
                        DisposeTimer(timer);
                    }
                    expireTimers.Clear();
                }
            }
        }

        void DisposeTimer(TimerInfo timer)
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

        //////////////////////////////////////////////////////////////////////////////////////////////////
        public void CreateTicker(Dictionary<uint, uint> kv, object param, Action<uint, object> action)
        {
            lock (mlock)
            {
                foreach (var de in kv)
                {
                    var ticker = new TickerInfo();
                    ticker.typeId = de.Key;
                    ticker.frameCount = de.Value;
                    ticker.refCount = 0;
                    ticker.action = action;
                    ticker.param = param;
                    tickers.Add(ticker);
                }
            }
        }

        private void OnTicker(float daltaTime)
        {
            if (tickers.Count > 0)
            {
                for(int i = tickers.Count - 1; i >= 0; i--)
                {
                    var ticker = tickers[i];
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
                lock (mlock)
                {
                    foreach (var timer in expireTickers)
                    {
                        tickers.Remove(timer);
                    }
                    expireTickers.Clear();
                }
            }
        }

        internal void ClearAllTimer()
        {
            lock (mlock)
            {
                timers.Clear();
                expireTimers.Clear();

                tickers.Clear();
                expireTickers.Clear();
            }
        }

        public override void OnDispose()
        {
        }
    }
}