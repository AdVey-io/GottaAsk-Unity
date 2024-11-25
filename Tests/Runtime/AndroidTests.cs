using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GottaAsk.Test

public class AndroidBridgeTests
{
    [SetUp]
    public void Setup()
    {
        // Initialize AndroidBridge before running tests
        AndroidBridge.Init();
    }

    [UnityTest]
    public System.Collections.IEnumerator Test_ShowAd_CallsAndroidMethod()
    {
        // Act
        AndroidBridge.ShowSurvey();

        // Assert
        yield return null; // Allow time for the Android bridge to process
        LogAssert.Expect(LogType.Log, "Android: ShowAd"); // Expect log output
    }

    [UnityTest]
    public System.Collections.IEnumerator Test_SurveyCompletedCallback()
    {
        // Arrange
        bool callbackInvoked = false;
        AndroidJavaProxy callbackProxy = new AndroidJavaProxy(
            "com.advey.gottaask.GottaAskSDK$SurveyCompletedListener"
        )
        {
            onSurveyCompleted = (string data) =>
            {
                callbackInvoked = true;
                Debug.Log("Survey Completed with data: " + data);
            },
        };

        AndroidBridge.SetSurveyCompletedCallback(callbackProxy);

        // Act
        yield return null; // Simulate time for callback execution

        // Assert
        Assert.IsTrue(callbackInvoked, "SurveyCompletedCallback was not invoked.");
    }
}
