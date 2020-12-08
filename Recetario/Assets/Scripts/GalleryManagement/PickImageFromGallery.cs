using UnityEditor;
using UnityEngine;

public class PickImageFromGallery : MonoBehaviour
{
    public void PickImage(ImageUpdater imageUpdater)
    {
#if UNITY_IOS && !UNITY_EDITOR
        PickImageNative(imageUpdater);
#elif UNITY_ANDROID && !UNITY_EDITOR
        PickImageNative(imageUpdater);
#elif UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
        PickImageWindows(imageUpdater);
#else
        PickImageWindows(imageUpdater);
#endif
    }

    private void PickImageWindows(ImageUpdater imageUpdater)
    {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
        var path = EditorUtility.OpenFilePanel("Select an image", "", "png,jpg,jpeg");
        string[] separatedPath = path.Split('/');
        string originalFileName = separatedPath[separatedPath.Length - 1];
        string imgPath = path;
        string imgName = originalFileName;

        imageUpdater.UpdateImage(path, imgName);
        //Debug.Log(path);
#endif
    }

    private void PickImageNative(ImageUpdater imageUpdater)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            string[] separatedPath = path.Split('/');
            string originalFileName = separatedPath[separatedPath.Length - 1];
            string imgPath = path;
            string imgName = originalFileName;

            imageUpdater.UpdateImage(path, imgName);
            //Debug.Log(path);
        }, "Select an image", "image/*");

        //Debug.Log("Permission result: " + permission);
    }
}
