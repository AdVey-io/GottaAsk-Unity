using UnityEngine;

namespace GottaAsk.Test
{
    public static class AndroidBridge
    {
        private static AndroidJavaObject _androidBridge;

        public static void Init()
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                _androidBridge = new AndroidJavaObject("com.advey.gottaask.GottaAskSDK");
                _androidBridge.CallStatic("init", activity, "test-user-id", "test-api-token");
            }
        }

        public static void ShowSurvey()
        {
            _androidBridge?.CallStatic("showSurvey");
        }

        public static bool HaveSurveys()
        {
            _androidBridge?.CallStatic("haveSurveys");
        }

        public static void AddDemographicData(GottaAskDemographicData data)
        {
            var jsonData = JsonUtility.ToJson(data);
            _androidBridge?.CallStatic("setGottaAskDemographicDataJson", callbackProxy);
        }

        public static void SetSurveyCompletedCallback(AndroidJavaProxy callbackProxy)
        {
            _androidBridge?.CallStatic("addSurveyCompletedListener", callbackProxy);
        }
    }
}
