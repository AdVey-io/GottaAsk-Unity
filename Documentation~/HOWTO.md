# **GottaAsk SDK - Unity**

The `GottaAskSDK` provides functionality for integrating surveys and ads into Unity projects, supporting Android and iOS platforms. Below is an overview of the API, usage, and platform-specific implementations.

---

## **Namespace**
```csharp
using GottaAsk;
```

---

## **Class**: `GottaAskSDK`

The `GottaAskSDK` class is a static utility that facilitates initializing the SDK, displaying surveys, and handling survey completion callbacks.

### **Features**
- Initialize the SDK with user-specific credentials.
- Display surveys on supported platforms.
- Handle survey completion events using delegates.

---

## **Delegates**

### **OnSurveyCompletedDelegate**
```csharp
public delegate void OnSurveyCompletedDelegate(string data);
```
- A callback invoked when a survey is completed.
- **Parameter**:
  - `data`: JSON string containing survey results or other relevant information.

---

## **Properties**

### **onSurveyCompleted**
```csharp
public static OnSurveyCompletedDelegate onSurveyCompleted { get; set; }
```
- Allows you to set a callback that is triggered when a survey is completed.
- Setting this property internally calls `SetOnSurveyCompletedDelegate()`.

---

## **Methods**

### **Init**
```csharp
public static void Init(string userId, string apiToken)
```
- Initializes the SDK.
- **Parameters**:
  - `userId`: A unique identifier for the user.
  - `apiToken`: API token for authenticating with the GottaAsk service.

#### **Platform-Specific Behavior**
- **Editor**: Logs a warning indicating that the SDK does not function in the Unity Editor.
- **Android**: Initializes the Android bridge using the provided credentials.
- **iOS**: Initializes the SDK on iOS (implementation to be completed as per iOS requirements).

---

### **ShowSurvey**
```csharp
public static void ShowSurvey()
```
- Displays a survey.
- **Platform-Specific Behavior**:
  - **Editor**: No operation.
  - **Android**: Loads and displays a survey using the Android bridge.
  - **iOS**: Logs the survey display request (functionality to be implemented).

---

### **SetOnSurveyCompletedDelegate**
```csharp
private static void SetOnSurveyCompletedDelegate()
```
- Configures the delegate to handle survey completion callbacks.
- **Platform-Specific Behavior**:
  - **Editor/iOS**: Logs the action.
  - **Android**: Sets a callback proxy for receiving events from the Android bridge.

### **HaveSurveys**
```csharp
public static IEnumerator<object> HaveSurveys(System.Action<bool> callback)
```
- Checks if there are surveys available to be shown. Can be called before showing a survey.
- **Parameters**:
  - `callback`: Action that receives a boolean indicating if surveys are available.
- **Returns**:
  - An IEnumerator for coroutine support.

#### **Platform-Specific Behavior**
- **Editor**: Returns false through the callback.
- **Android**: Makes an API request to check for available surveys.
- **iOS**: Logs the action (functionality to be implemented).

---

## **Usage Example**

### **1. Setting Up the SDK**
In your Unity project, add the following setup code in a script:

```csharp
using UnityEngine;
using GottaAsk;

public class GottaAskDemo : MonoBehaviour
{
    private void Start()
    {
        // Initialize the SDK
        GottaAskSDK.Init("your-user-id", "your-api-token");

        // Set the survey completed callback
        GottaAskSDK.onSurveyCompleted = OnSurveyCompleted;
    }

    private void OnSurveyCompleted(string data)
    {
        Debug.Log("Survey completed with data: " + data);
    }

    public void ShowSurvey()
    {
      // Check if surveys are available before showing
      StartCoroutine(GottaAskSDK.HaveSurveys((hasSurveys) => {
          if (hasSurveys)
          {
              GottaAskSDK.ShowSurvey();
          }
          else
          {
              Debug.Log("No surveys available at the moment");
          }
      }));
    }
}
```

### **2. Handling Survey Completion**
Implement the `OnSurveyCompletedDelegate` to process the survey result:

```csharp
private void OnSurveyCompleted(string data)
{
    Debug.Log("Survey completed! Data: " + data);
    // Process survey data (e.g., JSON parsing)
}
```

---

## **Platform Support**

### **Editor**
- **Behavior**: Logs warnings that functionality is not available in the Unity Editor.

### **Android**
- Uses the Android Java bridge to communicate with the GottaAsk SDK.
- **Dependencies**:
  - `com.unity3d.player.UnityPlayer`
  - `com.advey.gottaask.GottaAskSDK`

### **iOS**
- Logs SDK usage (functionality implementation is required).

---

## **Error Handling**

### Common Errors
1. **Null Android Bridge**:
   - Ensure `Init()` is called before invoking other methods.

2. **No Delegate Set**:
   - Assign a value to `onSurveyCompleted` to handle survey completion events.

### Debugging
- Use `Debug.Log` outputs to trace the execution flow and catch initialization issues.

---

## **Future Enhancements**
- Full implementation for iOS functionality.
- Advanced survey analytics and customization support.

---
