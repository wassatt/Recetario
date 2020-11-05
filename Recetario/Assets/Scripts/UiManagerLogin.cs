using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerLogin : MonoBehaviour
{
    //public InputField userName;
    public InputField userMail;
    public InputField userPassword;
    public ScriptableString loginMail;
    public ScriptableString loginPass;
    public Button loginButton;

    public void Start()
    {
        userMail.onValueChanged.AddListener(delegate { CheckValidInput(); });
        userPassword.onValueChanged.AddListener(delegate { CheckValidInput(); });
    }


    public void CheckValidInput()
    {
        if (/*userName.text.Length >= 0 &&*/ userMail.text.Contains("@") && userPassword.text.Length >= 8)
        {
            loginButton.interactable = true;
            loginMail.Set(userMail.text);
            loginPass.Set(userPassword.text);
        }
        else
        {
            loginButton.interactable = false;
        }
    }
}