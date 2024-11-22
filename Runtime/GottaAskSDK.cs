using GottaAsk;
using UnityEngine;

namespace GottaAsk
{
    public delegate void OnSurveyCompletedDelegate(string data);

    public class GottaAskSDK : MonoBehaviour
    {
        public static GottaAskSDK _sdkInstance;
        public delegate void SurveyCompletedCallback(string data);

        public static OnSurveyCompletedDelegate _onSurveyCompleted;
        public static OnSurveyCompletedDelegate onSurveyCompleted
        {
            get => _onSurveyCompleted;
            set
            {
                _onSurveyCompleted = value;
                SetOnSurveyCompletedDelegate();
            }
        }

        /// <summary>
        /// Initializes the Unity instance of the GottaAskSDK.
        ///
        /// <para>If an instance of GottaAskSDK does not already exist in the scene,
        /// it will find the first object of type GottaAskSDK or create a new GameObject
        /// with the GottaAskSDK component attached and mark it to not be destroyed on load.
        /// </para>
        ///
        /// It
        /// </summary>
        private static void InitUnityInstance()
        {
            if (_sdkInstance == null)
            {
                _sdkInstance = FindFirstObjectByType(typeof(GottaAskSDK)) as GottaAskSDK;
                if (_sdkInstance == null)
                {
                    _sdkInstance = new GameObject("GottaAskSDK").AddComponent<GottaAskSDK>();
                    DontDestroyOnLoad(_sdkInstance.gameObject);
                }
            }
        }

#if UNITY_EDITOR
        #region Editor

        public static void Init(string userId, string apiKey)
        {
            Debug.Log(
                "GottaAskSDK wont work in the editor. Please run on an Android or iOS device."
            );
        }

        private static void SetOnSurveyCompletedDelegate() { }

        public static void ShowSurvey() { }

        #endregion
#elif UNITY_ANDROID && !UNITY_EDITOR
        #region Android

        private static AndroidJavaObject _androidBridge;

        public static void Init(string userId, string apiKey)
        {
            Debug.Log("Android: Init");
            InitUnityInstance();
            //Screen.orientation = ScreenOrientation.Portrait; //.LandscapeLeft;
            using (var _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                if (_unityPlayer != null)
                {
                    var javaActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    _androidBridge = new AndroidJavaObject("com.advey.gottaask.GottaAskSDK");
                    _androidBridge.CallStatic("init", javaActivity, userId, apiKey);
                    SetOnSurveyCompletedDelegate();
                }
                else
                {
                    Debug.LogError("UnityPlayer is null");
                }
            }
        }

        private static void SetOnSurveyCompletedDelegate()
        {
            if (onSurveyCompleted != null && _androidBridge != null)
            {
                // Set the callback from the Android code
                var callbackProxy = new SurveyCompletedProxy(onSurveyCompleted);
                _androidBridge.CallStatic("addSurveyCompletedListener", callbackProxy);
            }
        }

        /**
         * ShowAd() is called from the Unity side to show the ad.
         */
        public static void ShowSurvey()
        {
            Debug.Log("Android: ShowSurvey");
            if (_androidBridge != null)
            {
                _androidBridge.CallStatic("loadSurvey");
            }
            else
            {
                Debug.LogError("Android Bridge is null. Did you forget to call Init()?");
            }
        }

        public static void HaveSurveys()
        {
            if (_androidBridge != null)
            {
                _androidBridge.CallStatic("haveSurveys");
            }
            else
            {
                Debug.LogError("Android Bridge is null. Did you forget to call Init()?");
            }
        }
        #endregion
#elif UNITY_IOS && !UNITY_EDITOR
        #region iOS
        public static void Init(string userId, string apiKey)
        {
            Debug.Log("iOS: Init");
        }

        private static void SetOnSurveyCompletedDelegate()
        {
            Debug.Log("iOS: SetOnSurveyCompletedDelegate");
        }

        public static void ShowSurvey()
        {
            Debug.Log("iOS: ShowSurvey");
        }

        #endregion
#endif
    }
}
