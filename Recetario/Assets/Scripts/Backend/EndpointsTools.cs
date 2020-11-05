using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EndpointsTools : MonoBehaviour
{
    public IEnumerator GetWithParam(string url, string param, string accessToken, System.Action<string> callback)
    {
        url = url + "/" + param;
        UnityWebRequest www = UnityWebRequest.Get(url);
        //www.SetRequestHeader("Authorization", authorization);
        //www.SetRequestHeader("Authorization", "TOKEN " + accessToken);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            yield return null;
            callback(www.downloadHandler.text);
        }
    }

    public IEnumerator GetWithParam(string url, string param, string accessToken)
    {
        url = url + "/" + param;
        UnityWebRequest www = UnityWebRequest.Get(url);
        //www.SetRequestHeader("Authorization", authorization);
        //www.SetRequestHeader("Authorization", "TOKEN " + accessToken);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }


    public IEnumerator PatchWithParam(string url, string param, string jsonData)
    {

        url = url + "/" + param;

        var request = new UnityWebRequestAsyncOperation();

        UnityWebRequest www = UnityWebRequest.Put(url, jsonData);
        www.method = "PATCH";
        www.SetRequestHeader("content-type", "application/json");
        www.uploadHandler.contentType = "application/json";
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

        request = www.SendWebRequest();
        yield return new WaitUntil(() => request.isDone);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator PostWithParam(string url, string param, string jsonData)
    {

        url = url + "/" + param;

        var request = new UnityWebRequestAsyncOperation();

        UnityWebRequest www = UnityWebRequest.Post(url, jsonData);
        www.SetRequestHeader("content-type", "application/json");
        www.uploadHandler.contentType = "application/json";
        www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));

        request = www.SendWebRequest();
        yield return new WaitUntil(() => request.isDone);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator PostWithParam(string url, string param, byte[] file)
    {
        url = url + "/" + param;
        var request = new UnityWebRequestAsyncOperation();

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("picture", file, "image.jpg", "img"));
        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        request = www.SendWebRequest();

        yield return new WaitUntil(() => request.isDone);

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
        }
    }

    public IEnumerator GetImageCoroutine(string url, System.Action<byte[]> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            callback(results);
        }
    }
}

