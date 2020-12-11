using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class DataBaseManager : MonoBehaviour
{
    [SerializeField]
    private string dataBaseUrl;
    public EndpointsTools endpointsTools;
    public ScriptableUserData sUserData;

    public UnityEvent onDataSent;
    public UnityEvent onDataRetreived;
    public UnityEvent onReloadPanels;
    public UnityEvent onConnectionError;

    void Start()
    {
    }

    public void PostNewUserData()
    {
        string userId = AuthManager.currentUserId;
        UserData userData = new UserData();
        userData.id = AuthManager.currentUserId;
        userData.name = AuthManager.currentUserName;
        string json = JsonUtility.ToJson(userData);

        StartCoroutine(endpointsTools.PostJsonWithParam(API.urlPostNewUser, userId, json, returnValue =>
        {
            Debug.Log(returnValue);
            sUserData.Set(userData);
            onReloadPanels.Invoke();
        }));
    }

    public void GetUserData(bool reloadPanels)
    {
        string userId = AuthManager.currentUserId;
        StartCoroutine(endpointsTools.GetWithParam(API.urlGetUser, userId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            UserData userData = JsonUtility.FromJson<UserData>(returnValue);
            sUserData.Set(userData); 

            onDataRetreived.Invoke();
            if (reloadPanels)
            {
                onReloadPanels.Invoke();
            }
        }));
    }

    public void UpdateUserName(string userName)
    {
        string userId = AuthManager.currentUserId;

        UserData userData = new UserData();
        userData.name = userName;
        string json = JsonUtility.ToJson(userData);

        StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateUserName, userId, json, returnValue =>
        {
            //Debug.Log(returnValue);
            sUserData.Set(userData);
            onReloadPanels.Invoke();
        }));
    }

    public void UploadProfileImage(ScriptableString imagePath)
    {
        string userId = AuthManager.currentUserId;
        var bytes = System.IO.File.ReadAllBytes(imagePath.Get());
        StartCoroutine(endpointsTools.PostFileWithParam(API.urlPostUserProfileImage, userId, bytes, returnValue =>
        {
            //Debug.Log(returnValue);
        }));
    }
}