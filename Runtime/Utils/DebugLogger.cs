using UnityEngine;

namespace GottaAsk
{
    public static class DebugLogger
    {
        public static void Log(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(message);
#endif
        }

        public static void LogError(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError(message);
#endif
        }
    }
}
