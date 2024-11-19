using UnityEngine;

namespace GottaAsk
{
    public delegate void OnSurveyCompletedDelegate(string data);

    public class GottaAskSDK : MonoBehaviour
    {
        public delegate void SurveyCompletedCallback(string data);


        private OnSurveyCompletedDelegate _onSurveyCompleted;
        public OnSurveyCompletedDelegate onSurveyCompleted
        {
            get => _onSurveyCompleted;
            set
            {
                _onSurveyCompleted = value;
                SetOnSurveyCompletedDelegate();
            }
        }
        private WebViewObject webViewObject;
        private AndroidJavaObject _androidBridge;


#if UNITY_EDITOR
        #region Editor
        public void Init(string userId, string apiToken)
        {
            webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
            webViewObject.Init(
                cb: (msg) =>
                {
                    Debug.Log(string.Format("CallFromJS[{0}]", msg));
                },
                err: (msg) =>
                {
                    Debug.LogError(string.Format("CallOnError[{0}]", msg));
                },
                ld: (msg) =>
                {
                    Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
                    webViewObject.EvaluateJS(@"
                    window.Unity = {
                        call: function(msg) {
                            window.location = 'unity:' + msg;
                        }
                    }
                ");
                },
                enableWKWebView: true
            );
        }

        private void SetOnSurveyCompletedDelegate()
        {
            Debug.Log("Editor: SetOnSurveyCompletedDelegate");
        }

        public void ShowAd()
        {
            webViewObject.LoadURL("http://localhost:8080/v1/survey");
            webViewObject.SetVisibility(true);
        }
        #endregion
#elif UNITY_ANDROID && !UNITY_EDITOR
#region Android
    public void Init(string userId, string apiToken)
    {
        Screen.orientation = ScreenOrientation.Portrait;//.LandscapeLeft;
        using(var _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            if (_unityPlayer != null) {
                var javaActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                _androidBridge = new AndroidJavaObject("com.advey.gottaask.GottaAskSDK");
                _androidBridge.Call("init", javaActivity);
                SetOnSurveyCompletedDelegate();

            }
        }
    }

    private void SetOnSurveyCompletedDelegate()
    {
        if (onSurveyCompleted != null && _androidBridge != null)
        {
            // Set the callback from the Android code
            var callbackProxy = new SurveyCompletedProxy(onSurveyCompleted);
            _androidBridge.Call("addSurveyCompletedListener", callbackProxy);
        }
    }

    public void ShowAd()
    {
        Debug.Log("Android: ShowAd");
        if (_androidBridge != null)
        {
            _androidBridge.Call("loadAd");
        } else {
            Debug.LogError("Android Bridge is null. Did you forget to call Init()?");
        }
        
    }
#endregion
#elif UNITY_IOS && !UNITY_EDITOR
#region iOS
    public void Init(string userId, string apiToken) 
    {
        Debug.Log("iOS: Init");
    }

    private void SetOnSurveyCompletedDelegate()
    {
        Debug.Log("iOS: SetOnSurveyCompletedDelegate");
    }

    public void ShowAd()
    {
        Debug.Log("iOS: ShowAd");
    }

#endregion
#endif
    }
}