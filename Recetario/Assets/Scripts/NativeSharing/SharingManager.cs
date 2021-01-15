using System.Collections;
using System.IO;
using UnityEngine;

public class SharingManager : MonoBehaviour
{

    private IEnumerator CoroutineTakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("Hello world!")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }

    public void ShareLink(string link)
    {
        StartCoroutine(CoroutineShareLink(link));
    }

    public void ShareText(string subject, string text)
    {
        StartCoroutine(CoroutineShareText(subject, text));
    }

    private IEnumerator CoroutineShareLink(string link)
    {
        yield return new WaitForEndOfFrame();

        new NativeShare()
            .SetSubject("Recetario").SetText(link)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }

    private IEnumerator CoroutineShareText(string subject, string text)
    {
        yield return new WaitForEndOfFrame();

        new NativeShare()
            .SetSubject(subject).SetText(text)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}
