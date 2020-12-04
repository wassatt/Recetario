using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    private DatabaseReference reference;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(dataBaseUrl);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void PostNewUserData()
    {
        string userId = AuthManager.currentUserId;
        UserData userData = new UserData();
        userData.name = AuthManager.currentUserName;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PostWithParam(API.urlPostNewUser, userId, json));
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

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateUserName, userId, json));
    }

    public void UploadProfileImage(string path)
    {
        string userId = AuthManager.currentUserId;
        var bytes = System.IO.File.ReadAllBytes(path);
        //StartCoroutine(endpointsTools.PostWithParam(API.urlPostUserProfileImage, userId, bytes));
    }

    public void UploadProfileImage(ScriptableString imagePath)
    {
        string userId = AuthManager.currentUserId;
        var bytes = System.IO.File.ReadAllBytes(imagePath.Get());
        //StartCoroutine(endpointsTools.PostWithParam(API.urlPostUserProfileImage, userId, bytes));
    }

}