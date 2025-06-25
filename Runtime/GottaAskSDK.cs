using System.Collections.Generic;
using GottaAsk;
using UnityEngine;
using UnityEngine.Networking;

namespace GottaAsk
{
    public delegate void OnSurveyCompletedDelegate(GottaAskSurveyCompletedResponse response);

    public class GottaAskSDK : MonoBehaviour
    {
        public static GottaAskSDK _sdkInstance;
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

        private static string USER_ID = "";
        private static string API_KEY = "";

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
#if UNITY_EDITOR  || UNITY_WEBGL || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
        #region Other Platforms

        /// <summary>
        /// Initializes the GottaAsk SDK with required user and API key.
        /// </summary>
        /// <param name="userId">The user ID within from the calling app</param>
        /// <param name="apiKey">The API key for the specific app where this SDK is being used.</param>
        public static void Init(string userId, string apiKey)
        {
            DebugLogger.Log(
                "GottaAskSDK wont work in the here. Please run on an Android or iOS device."
            );
        }

        /// <summary>
        /// Sets the delegate for the OnSurveyCompleted event.
        /// </summary>
        private static void SetOnSurveyCompletedDelegate() { 
                        DebugLogger.Log(
                "GottaAskSDK wont work in here. Please run on an Android or iOS device."
            );
         }

        /// <summary>
        /// Opens a full screen browser window and loads surveys from the GottaAsk server.
        /// </summary>
        /// <remarks>
        /// This method will not work in the Unity Editor, only on Android and iOS devices.
        /// </remarks>
        public static void ShowSurvey() {
            DebugLogger.Log(
                "GottaAskSDK wont work in here. Please run on an Android or iOS device."
            );
         }

        /// <summary>
        /// Checks if there are surveys available to be shown. Can be called before calling ShowSurvey().
        /// </summary>
        /// <remarks>
        /// This method will not work in the Unity Editor, only on Android and iOS devices.
        /// </remarks>
        public static IEnumerator<object> HaveSurveys(System.Action<bool> callback)
        {
            DebugLogger.Log(
                "GottaAskSDK wont work in here. Please run on an Android or iOS device."
            );
            callback(false);
            yield return null;
        }

        /// <summary>
        /// Sets the user attributes for the current user.
        /// This is helpful to target specific demographics with surveys.
        /// </summary>
        public static void SetUserAttributes(GottaAskDemographicData data) { 
            DebugLogger.Log(
                "GottaAskSDK wont work in here. Please run on an Android or iOS device."
            );
         }

        /// <summary>
        /// Sets the user attributes for the current user.
        /// This is helpful to target specific demographics with surveys.
        /// </summary>
        public static void SetUserAttributes(int age = 0, string country = "", int income = 0) { 
            DebugLogger.Log(
                "GottaAskSDK wont work in here. Please run on an Android or iOS device."
            );
         }

        private static void AddDemographicData(GottaAskDemographicData data) { 
            DebugLogger.Log(
                "GottaAskSDK wont work in here. Please run on an Android or iOS device."
            );
         }

        #endregion

#elif UNITY_ANDROID && !UNITY_EDITOR
        #region Android

        private static AndroidJavaObject _androidSDKReference;

        public static void Init(string userId, string apiKey)
        {
            DebugLogger.Log("Android: Init");
            USER_ID = userId;
            API_KEY = apiKey;
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
                    DebugLogger.LogError("UnityPlayer is null");
                }
            }
        }

        public static void SetUserAttributes(GottaAskDemographicData data)
        {
            AddDemographicData(data);
        }

        public static void SetUserAttributes(int age = 0, string country = null, int income = 0)
        {
            var data = new GottaAskDemographicData
            {
                age = age,
                country = country,
                income = income,
            };
            AddDemographicData(data);
        }

        private static void AddDemographicData(GottaAskDemographicData data)
        {
            if (_androidSDKReference != null)
            {
                var jsonData = JsonUtility.ToJson(data);
                _androidSDKReference.CallStatic("setGottaAskDemographicDataJson", jsonData);
            }
            else
            {
                DebugLogger.LogError("Android Bridge is null. Did you forget to call Init()?");
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
            DebugLogger.Log("Android: ShowSurvey");
            if (_androidSDKReference != null)
            {
                _androidSDKReference.CallStatic("showSurvey");
            }
            else
            {
                DebugLogger.LogError("Android Reference is null. Did you forget to call Init()?");
            }
        }

        /// <summary>
        /// Checks if there are surveys available on an Android device.
        /// </summary>
        public static IEnumerator<object> HaveSurveys(System.Action<bool> callback)
        {
            // Create the UnityWebRequest
            UnityWebRequest request = UnityWebRequest.Get("https://app.gottaask.io/v1/survey");
            request.SetRequestHeader("Content-Type", "application/json"); // Required for JSON requests
            request.SetRequestHeader("X-GOTTAASK-API-KEY", API_KEY);
            request.SetRequestHeader("X-GOTTAASK-USER-ID", USER_ID);

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (
                request.result == UnityWebRequest.Result.Success
                && request.responseCode >= 200
                && request.responseCode < 300
            )
            {
                callback(true);
                DebugLogger.Log($"Response code: {request.responseCode}");
            }
            else
            {
                callback(false);
                DebugLogger.LogError($"Error: {request.error}, Code: {request.responseCode}");
            }
        }

        #endregion
#elif UNITY_IOS && !UNITY_EDITOR
        #region iOS
        public static void Init(string userId, string apiKey)
        {
            DebugLogger.Log("iOS: Init");
        }

        public static void SetUserAttributes(int age = 0, string country = null, int income = 0)
        {
            DebugLogger.Log("iOS: SetUserAttributes");
        }

        public static void SetUserAttributes(GottaAskDemographicData data)
        {
            DebugLogger.Log("iOS: SetUserAttributes");
        }

        private static void AddDemographicData(GottaAskDemographicData data)
        {
            DebugLogger.Log("iOS: AddDemographicData");
        }

        private static void SetOnSurveyCompletedDelegate()
        {
            DebugLogger.Log("iOS: SetOnSurveyCompletedDelegate");
        }

        public static void ShowSurvey()
        {
            DebugLogger.Log("iOS: ShowSurvey");
        }

        public static IEnumerator<object> HaveSurveys(System.Action<bool> callback)
        {
            DebugLogger.Log("iOS: HaveSurveys");
            yield return null;
        }
        #endregion
#endif
    }
}
