using UnityEngine;
using UnityEngine.UI;

public class UiManagerSignin : MonoBehaviour
{
    public InputField userMail;
    public InputField userPassword;
    public InputField userName;
    public ScriptableString signinMail;
    public ScriptableString signinPass;
    public ScriptableString signinName;
    public Button registerButton;

    void Start()
    {
        userMail.onValueChanged.AddListener(delegate { CheckValidInput(); });
        userPassword.onValueChanged.AddListener(delegate { CheckValidInput(); });
    }

    public void CheckValidInput()
    {
        if (userName.text.Length >= 0 && userMail.text.Contains("@") && userPassword.text.Length >= 8)
        {
            registerButton.interactable = true;
            signinMail.Set(userMail.text);
            signinPass.Set(userPassword.text);
            signinName.Set(userName.text);
        }
        else
        {
            registerButton.interactable = false;
        }
    }
}
