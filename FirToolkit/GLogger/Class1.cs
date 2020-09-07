
namespace UnityEngine
{
    public static class GLogger
    {
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        public static void Error(object message)
        {
            Debug.LogError(message);
        }

        public static void Warning(object message)
        {
            Debug.LogWarning(message);
        }

        public static void White(object message)
        {
            Debug.Log("<color=white>" + message + "</color>");
        }

        public static void Gray(object message)
        {
            Debug.Log("<color=gray>" + message + "</color>");
        }

        public static void Green(object message)
        {
            Debug.Log("<color=green>" + message + "</color>");
        }

        public static void Purple(object message)
        {
            Debug.Log("<color=#9400D3>" + message + "</color>");
        }

        public static void Yellow(object message)
        {
            Debug.Log("<color=yellow>" + message + "</color>");
        }

        public static void Red(object message)
        {
            Debug.Log("<color=red>" + message + "</color>");
        }

        public static void Blue(object message)
        {
            Debug.Log("<color=blue>" + message + "</color>");
        }

        public static void Magenta(object message)
        {
            Debug.Log("<color=magenta>" + message + "</color>");
        }

        public static void Cyan(object message)
        {
            Debug.Log("<color=cyan>" + message + "</color>");
        }
    }
}
