using UnityEngine;
using UnityEngine.UI;
using GottaAsk;

public class ButtonBehaviour : MonoBehaviour
{
    public Button button;
    private bool ShouldShowButton = true;
    private GottaAskSDK gottaAskSDK;


    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = new GameObject("GottaAskSDK");
        ShouldShowButton = true;
        gottaAskSDK = obj.AddComponent<GottaAskSDK>();
        gottaAskSDK.Init("", "");
        gottaAskSDK.onSurveyCompleted = (data) =>
        {
            Debug.Log("Survey Completed Delegate: " + data);
        };
    }

    // Update is called once per frame
    void Update()
    {
        //   button.gameObject.SetActive(ShouldShowButton);
    }

    public void ButtonPressed()
    {
        Debug.Log("Button Pressed");
        ShouldShowButton = false;
        gottaAskSDK.ShowAd();
    }
}
