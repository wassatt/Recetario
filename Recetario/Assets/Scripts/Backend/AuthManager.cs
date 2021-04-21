using Firebase.Auth;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{//*
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private ScriptableString signinMail;
    [SerializeField]
    private ScriptableString signinPass;
    [SerializeField]
    private ScriptableString signinName;
    [SerializeField]
    private ScriptableString loginMail;
    [SerializeField]
    private ScriptableString loginPass;

    public UnityEvent onUserLoggedPrev;
    public UnityEvent onLogInSuccess;
    public UnityEvent onLogInFailed;
    public UnityEvent onSingInSuccess;
    public UnityEvent onSingInFailed;
    public UnityEvent onUserNotLoggedPrev;
    //public UnityEvent onDataUpdated;

    public static string currentUserId;
    public static string currentUserMail;
    public static string currentUserName;

    private void Start()
    {
        ClearStrings();
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChanged;
        CheckUser();
    }

    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }

    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
        CheckUser();
    }

    private void CheckUser()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            currentUserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            //Debug.Log(currentUserId);
            currentUserMail = FirebaseAuth.DefaultInstance.CurrentUser.Email;
            //Debug.Log(currentUserMail);
            currentUserName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
            //Debug.Log(currentUserName);

            dbManager.GetUserData(true);
            onUserLoggedPrev.Invoke();
        }
        else
        {
            onUserNotLoggedPrev.Invoke();
        }
    }

    public void RegisterUser()
    {
        StartCoroutine(RegisterCoroutine(signinMail.var, signinPass.var));
    }

    private IEnumerator RegisterCoroutine(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogError(registerTask.Exception);
            onSingInFailed.Invoke();
        }
        else
        {
            //Debug.Log(registerTask.Result);

            StartCoroutine(UpdateProfile(signinName.var, "", returnValue =>
            {
                //Debug.Log(returnValue);
                ClearStrings();
                dbManager.PostNewUserData();
                onSingInSuccess.Invoke();
            }));
        }
    }

    public IEnumerator UpdateProfile(string name, string photoUrl, System.Action<string> callback)
    {
        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
        {
            DisplayName = name
        };
        
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        var updateTask = user.UpdateUserProfileAsync(profile);
        yield return new WaitUntil(() => updateTask.IsCompleted);

        if (updateTask.Exception != null)
        {
            Debug.LogError(updateTask.Exception);
            callback(updateTask.Exception.ToString());
        }
        else
        {
            currentUserName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;

            dbManager.UpdateUserName(currentUserName);
            //Debug.Log(currentUserName);
            //Debug.Log("User info updated");
            yield return null;
            callback(currentUserName);
        }
    }

    public IEnumerator UpdateMail(string currentMail, string newMail, string password, System.Action<string> callback)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(currentMail, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            //Debug.LogError(loginTask.Exception);
            yield return null;
            callback(loginTask.Exception.ToString());
            onLogInFailed.Invoke();
        }
        else
        {
            var updateTask = user.UpdateEmailAsync(newMail);
            yield return new WaitUntil(() => updateTask.IsCompleted);

            if (updateTask.Exception != null)
            {
                //Debug.LogError(updateTask.Exception);
                callback(updateTask.Exception.ToString());
            }
            else
            {
                currentUserMail = FirebaseAuth.DefaultInstance.CurrentUser.Email;
                //Debug.Log(currentUserMail);
                //Debug.Log("User info updated");
                yield return null;
                callback(currentUserMail);
            }
        }
    }

    public IEnumerator UpdatePassword(string email, string currentPass, string newPass, System.Action<string> callback)
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, currentPass);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            //Debug.LogError(loginTask.Exception);
            yield return null;
            callback(loginTask.Exception.ToString());
            onLogInFailed.Invoke();
        }
        else
        {
            var updateTask = user.UpdatePasswordAsync(newPass);
            yield return new WaitUntil(() => updateTask.IsCompleted);

            if (updateTask.Exception != null)
            {
                //Debug.LogError(updateTask.Exception);
                callback(updateTask.Exception.ToString());
            }
            else
            {
                //Debug.Log("Password changed");
                yield return null;
                callback("Password changed");
            }
        }
    }

    public IEnumerator SendResetPasswordEmail(string email, System.Action<string> callback)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var sendResetPassTask = auth.SendPasswordResetEmailAsync(email);
        yield return new WaitUntil(() => sendResetPassTask.IsCompleted);

        if (sendResetPassTask.Exception != null)
        {
            //Debug.LogError(sendResetPassTask.Exception);
            yield return null;
            callback(sendResetPassTask.Exception.ToString());
        }
        else
        {
            callback("Reset password email sent");
        }
    }

    public void LoginUser()
    {
        StartCoroutine(LoginCoroutine(loginMail.Get(), loginPass.Get()));
    }

    private IEnumerator LoginCoroutine(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);
            onLogInFailed.Invoke();
        }
        else
        {
            //Debug.Log(loginTask.Result);
            ClearStrings();
            dbManager.GetUserData(true);
            onLogInSuccess.Invoke();
        }
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        ClearStrings();
        //TODO: add ClearUserData();
        SceneManager.LoadScene("Main");
    }

    private void ClearStrings()
    {
        signinMail.Set("");
        signinPass.Set("");
        signinName.Set("");
        loginPass.Set("");
        loginPass.Set("");
    }
}
