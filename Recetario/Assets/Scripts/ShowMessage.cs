using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    public GameObject messageObj;

    public void _ShowMessage(string message)
    {
#if UNITY_IOS && !UNITY_EDITOR
        _ShowGenericMessage(message);
#elif UNITY_ANDROID && !UNITY_EDITOR
        _ShowAndroidToastMessage(message);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
        _ShowGenericMessage(message);
#else
        _ShowGenericMessage(message);
#endif
    }

    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    private void _ShowGenericMessage(string message)
    {
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        messageObj.GetComponentInChildren<Text>().text = message;
        messageObj.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        messageObj.SetActive(false);
    }
}
