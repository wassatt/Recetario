using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManagerLogin : MonoBehaviour
{
    [SerializeField]
    private AuthManager authManager;
    public InputField userMail;
    public InputField userPassword;
    public ScriptableString loginMail;
    public ScriptableString loginPass;
    public Button loginButton;
    public InputField userMailRecover;
    public Button btnSendMailRecover;

    public UnityEvent onPassResetEmailSent;

    public void Start()
    {
        userMail.onValueChanged.AddListener(delegate { CheckValidInput(); });
        userPassword.onValueChanged.AddListener(delegate { CheckValidInput(); });
        btnSendMailRecover.onClick.AddListener(delegate { SendResetPassEmail(); });
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

    public void SendResetPassEmail()
    {
        StartCoroutine(authManager.SendResetPasswordEmail(userMailRecover.text, returnCallback => {
            onPassResetEmailSent.Invoke();
        }));
    }
}