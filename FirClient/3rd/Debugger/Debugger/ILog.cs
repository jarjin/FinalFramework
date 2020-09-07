using UnityEngine;

namespace UnityEngine
{
    public interface ICmd
    {
        void Log(string msg);
        void LogWarning(string msg);
        void LogError(string msg);
        void Show(bool flag);
    }

    public interface IULogger
    {
        void Log(string msg, string stack, LogType type);
    }
}


