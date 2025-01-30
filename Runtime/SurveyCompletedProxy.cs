using System;
using UnityEngine;

namespace GottaAsk
{
    public class SurveyCompletedProxy : AndroidJavaProxy
    {
        private OnSurveyCompletedDelegate _callback;

        // Constructor that takes a delegate as a parameter
        public SurveyCompletedProxy(OnSurveyCompletedDelegate callback)
            : base("com.advey.gottaask.callbacks.JsonSurveyCompletedListener") // Java interface
        {
            _callback =
                callback
                ?? throw new ArgumentNullException(nameof(callback), "Callback cannot be null.");
        }

        // This method matches the Java interface method `onSurveyCompleted`
        public void onSurveyCompleted(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
            {
                DebugLogger.LogError(
                    "Survey completion failed: Received an empty or null JSON string."
                );
                _callback?.Invoke(
                    new GottaAskSurveyCompletedResponse { error = "Received empty response" }
                );
                return;
            }

            try
            {
                GottaAskSurveyCompletedResponse response =
                    JsonUtility.FromJson<GottaAskSurveyCompletedResponse>(result);

                if (response == null)
                {
                    DebugLogger.LogError(
                        "Survey completion failed: Deserialized response is null."
                    );
                    _callback?.Invoke(
                        new GottaAskSurveyCompletedResponse { error = "Invalid response format" }
                    );
                    return;
                }

                _callback?.Invoke(response);
            }
            catch (Exception ex)
            {
                DebugLogger.LogError(
                    $"Survey completion failed: JSON deserialization error - {ex.Message}\n{ex.StackTrace}"
                );
                _callback?.Invoke(
                    new GottaAskSurveyCompletedResponse { error = "JSON parsing error" }
                );
            }
        }
    }
}
