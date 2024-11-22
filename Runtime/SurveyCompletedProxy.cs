using UnityEngine;
namespace GottaAsk
{
    public class SurveyCompletedProxy : AndroidJavaProxy
    {
        private OnSurveyCompletedDelegate _callback;

        // Constructor that takes a delegate as a parameter
        public SurveyCompletedProxy(OnSurveyCompletedDelegate callback)
            : base("com.advey.gottaask.SurveyCompletedListener") // Java interface
        {
            _callback = callback;
        }

        // This method matches the Java interface method `onTaskCompleted`
        public void onSurveyCompleted(string result)
        {
            _callback?.Invoke(result);
        }
    }
}