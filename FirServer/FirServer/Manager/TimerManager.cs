using System;
using System.Collections.Generic;
using System.Timers;

namespace FirServer.Manager
{
    class TimerInfo
    {
        public string timerName;
        public int interval;
        public int refcount;
        public Action action;
    }

    public class TimerManager : BaseManager
    {
        private Timer mloopTimer = new Timer();
        private Dictionary<string, TimerInfo> mTimers = new Dictionary<string, TimerInfo>();


        public override void Initialize()
        {
            mloopTimer = new Timer(1000);
            mloopTimer.AutoReset = true;
            mloopTimer.Elapsed += new ElapsedEventHandler(OnTimer);
            mloopTimer.Start();
        }

        public void AddTimer(string timerName, int interval, Action action)
        {
            lock (mTimers)
            {
                if (mTimers.ContainsKey(timerName))
                {
                    return;
                }
                var info = new TimerInfo();
                info.action = action;
                info.timerName = timerName;
                info.interval = interval;
                info.refcount = 0;
                mTimers.Add(timerName, info);
            }
        }

        public void RemoveTimer(string timerName)
        {
            lock (mTimers)
            {
                mTimers.Remove(timerName);
            }
        }

        void OnTimer(object sender, ElapsedEventArgs e)
        {
            lock (mTimers)
            {
                foreach (var de in mTimers)
                {
                    if (de.Value != null)
                    {
                        de.Value.refcount++;
                        if (de.Value.refcount >= de.Value.interval)
                        {
                            de.Value.action();
                            de.Value.refcount = 0;
                        }
                    }
                }
            }
        }

        public void ExecOnceTimer(int interval, Action action)
        {
            var timer = new Timer(interval);
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(delegate(object sender, ElapsedEventArgs e)
            {
                action();
                timer.Dispose();
            });
        }
    }
}
