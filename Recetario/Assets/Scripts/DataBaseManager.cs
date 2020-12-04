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
    public string dataBaseUrl;
    public EndpointsTools endpointsTools;
    public ScriptableBool isRestaurant;
    public ScriptableInt siCategory;
    public ScriptableString ssPhoneNumber;
    public ScriptableString ssBussinessAddress;
    public ScriptableString ssSchedule;
    public ScriptableString ssName;
    public ScriptableString ssProfileImageUrl;

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

    private void OnDestroy()
    {
        UnsubscribeDatabaseListeners();
    }

    public void SubscribeDatabaseListeners()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(AuthManager.currentUserId).ValueChanged += HandleUserValuesChanged;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(AuthManager.currentUserId).Child("isRestaurant").ValueChanged += HandleisRestaurantChanged;
    }

    public void UnsubscribeDatabaseListeners()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(AuthManager.currentUserId).ValueChanged -= HandleUserValuesChanged;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(AuthManager.currentUserId).Child("isRestaurant").ValueChanged -= HandleisRestaurantChanged;

    }

    private void HandleUserValuesChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        else
        {
            GetUserData(false);
        }
    }

    private void HandleisRestaurantChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        else
        {
            GetUserData(true);
        }
    }

    public void GetUserData(bool reloadPanels)
    {
        string userId = AuthManager.currentUserId;
        StartCoroutine(endpointsTools.GetWithParam(API.urlGetUser, userId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            UserData userData = JsonUtility.FromJson<UserData>(returnValue);
            //isRestaurant.Set(userData.isRestaurant);
            //ssPhoneNumber.Set(userData.phoneNumber);
            //ssBussinessAddress.Set(userData.businessAddress);
            //ssSchedule.Set(userData.schedule);
            //siCategory.Set(userData.category);
            //ssProfileImageUrl.Set(userData.profileImageUrl);
            onDataRetreived.Invoke();
            if (reloadPanels)
            {
                onReloadPanels.Invoke();
            }
        }));
    }

    public void GetRestaurants()
    {
        //StartCoroutine(endpointsTools.GetEndpointWithParam(urlGetRestaurants, "", "", returnValue =>
        //{ }));
    }

    public void UpdateAccountType(bool isRestaurant)
    {
        if (isRestaurant == false)
            UpdateCategory(0);

        string userId = AuthManager.currentUserId;
        UserData userData = new UserData();
        //userData.isRestaurant = isRestaurant;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateAccountType, userId, json));
    }

    public void UpdateUserName(string userName)
    {
        string userId = AuthManager.currentUserId;

        UserData userData = new UserData();
        userData.name = userName;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateUserName, userId, json));
    }

    public void UpdatePhone(string phoneNumber)
    {
        string userId = AuthManager.currentUserId;

        UserData userData = new UserData();
        //userData.phoneNumber = phoneNumber;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdatePhoneNumber, userId, json));
    }

    public void UpdateBusinessAddress(string businessAddress)
    {
        string userId = AuthManager.currentUserId;

        UserData userData = new UserData();
        //userData.businessAddress = businessAddress;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateBusinessAddress, userId, json));
    }

    public void UpdateSchedule(string schedule)
    {
        string userId = AuthManager.currentUserId;

        UserData userData = new UserData();
        //userData.schedule = schedule;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateSchedule, userId, json));
    }

    public void UpdateCategory(int category)
    {
        string userId = AuthManager.currentUserId;

        UserData userData = new UserData();
        //userData.category = category;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PatchWithParam(API.urlUpdateCategory, userId, json));
    }

    public void PostNewUserData()
    {
        string userId = AuthManager.currentUserId;
        UserData userData = new UserData();
        //userData.isRestaurant = isRestaurant.Get(); 
        userData.name = AuthManager.currentUserName;
        string json = JsonUtility.ToJson(userData);

        //StartCoroutine(endpointsTools.PostWithParam(API.urlPostNewUser, userId, json));
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