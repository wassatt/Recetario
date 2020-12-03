using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    public IEnumerator PatchWithParam(string url, string param, string jsonData, System.Action<string> callback)
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
            callback(www.error);
        }
        else
        {
            yield return null;
            callback(www.downloadHandler.text);
        }
    }

    public IEnumerator PostJsonWithParam(string url, string param, string jsonData, System.Action<string> callback)
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
            callback(www.error);
        }
        else
        {
            yield return null;
            callback(www.downloadHandler.text);
        }
    }

    public IEnumerator PostFileWithParam(string url, string param, byte[] file, System.Action<string> callback)
    {
        url = url + "/" + param;
        var request = new UnityWebRequestAsyncOperation();

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("picture", file, "image.jpg", "image/jpeg"));
        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        request = www.SendWebRequest();

        yield return new WaitUntil(() => request.isDone);

        if (www.isNetworkError || www.isHttpError)
        {
            callback(www.error);
        }
        else
        {
            yield return null;
            callback(www.downloadHandler.text);
        }
    }

    public IEnumerator DeleteWithParam(string url, string param, System.Action<string> callback)
    {
        url = url + "/" + param;

        var request = new UnityWebRequestAsyncOperation();

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.method = "DELETE";

        request = www.SendWebRequest();
        yield return new WaitUntil(() => request.isDone);

        if (www.isNetworkError || www.isHttpError)
        {
            callback(www.error);
        }
        else
        {
            yield return null;
            callback(www.downloadHandler.text);
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

