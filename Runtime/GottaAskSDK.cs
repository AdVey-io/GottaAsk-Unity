using System.Collections.Generic;
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

        /// <summary>
        /// Initializes the GottaAsk SDK with required user and API key.
        /// </summary>
        /// <param name="userId">The user ID within from the calling app</param>
        /// <param name="apiKey">The API key for the specific app where this SDK is being used.</param>
        public static void Init(string userId, string apiKey)
        {
            Debug.Log(
                "GottaAskSDK wont work in the editor. Please run on an Android or iOS device."
            );
        }

        /// <summary>
        /// Sets the delegate for the OnSurveyCompleted event.
        /// </summary>
        private static void SetOnSurveyCompletedDelegate() { }

        /// <summary>
        /// Opens a full screen browser window and loads surveys from the GottaAsk server.
        /// </summary>
        /// <remarks>
        /// This method will not work in the Unity Editor, only on Android and iOS devices.
        /// </remarks>
        public static void ShowSurvey() { }

        /// <summary>
        /// Checks if there are surveys available to be shown. Can be called before calling ShowSurvey().
        /// </summary>
        /// <remarks>
        /// This method will not work in the Unity Editor, only on Android and iOS devices.
        /// </remarks>
        public static void HaveSurveys() { }

        public static void SetUserAttributes(
            string age = "",
            string country = "",
            string income = ""
        )
        {
            Debug.Log("Editor: SetUserAttributes");
        }

        public static void SetUserAttributes(Dictionary<string, string> attributes)
        {
            Debug.Log("Editor: SetUserAttributes");
        }

        #endregion
#elif UNITY_ANDROID && !UNITY_EDITOR
        #region Android

        private static AndroidJavaObject _androidSDKReference;

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
                    _androidSDKReference = new AndroidJavaObject("com.advey.gottaask.GottaAskSDK");
                    _androidSDKReference.CallStatic("init", javaActivity, userId, apiKey);
                    SetOnSurveyCompletedDelegate();
                }
                else
                {
                    Debug.LogError("UnityPlayer is null");
                }
            }
        }

        public static void SetUserAttributes(
            string age = null,
            string country = null,
            string income = null
        )
        {
            if (_androidSDKReference != null)
            {
                _androidSDKReference.CallStatic("setUserAttributes", age, country, income);
            }
            else
            {
                Debug.LogError("Android Bridge is null. Did you forget to call Init()?");
            }
        }

        public static void SetUserAttributes(map<string, string> attributes)
        {
            if (_androidSDKReference != null)
            {
                _androidSDKReference.CallStatic("setUserAttributes", attributes);
            }
            else
            {
                Debug.LogError("Android Bridge is null. Did you forget to call Init()?");
            }
        }

        private static void SetOnSurveyCompletedDelegate()
        {
            if (onSurveyCompleted != null && _androidSDKReference != null)
            {
                // Set the callback from the Android code
                var callbackProxy = new SurveyCompletedProxy(onSurveyCompleted);
                _androidSDKReference.CallStatic("addSurveyCompletedListener", callbackProxy);
            }
        }

        public static void ShowSurvey()
        {
            Debug.Log("Android: ShowSurvey");
            if (_androidSDKReference != null)
            {
                _androidSDKReference.CallStatic("loadSurvey");
            }
            else
            {
                Debug.LogError("Android Reference is null. Did you forget to call Init()?");
            }
        }

        /// <summary>
        /// Checks if there are surveys available on an Android device.
        /// </summary>
        public static void HaveSurveys()
        {
            if (_androidSDKReference != null)
            {
                _androidSDKReference.CallStatic("haveSurveys");
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

        public static void SetUserAttributes(
            string age = "",
            string country = "",
            string income = ""
        )
        {
            Debug.Log("iOS: SetUserAttributes");
        }

        public static void SetUserAttributes(Dictionary<string, string> attributes)
        {
            Debug.Log("iOS: SetUserAttributes");
        }

        private static void SetOnSurveyCompletedDelegate()
        {
            Debug.Log("iOS: SetOnSurveyCompletedDelegate");
        }

        public static void ShowSurvey()
        {
            Debug.Log("iOS: ShowSurvey");
        }

        public static void HaveSurveys()
        {
            Debug.Log("iOS: HaveSurveys");
        }
        #endregion
#endif
    }
}
