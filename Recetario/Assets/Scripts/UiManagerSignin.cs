using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UiManagerSignin : MonoBehaviour
{
    public InputField userMail;
    public InputField userPassword;
    public InputField userName;
    public ToggleGroup toggleGroup;
    public ScriptableString signinMail;
    public ScriptableString signinPass;
    public ScriptableString signinName;
    public ScriptableBool isRestaurant;
    public Button registerButton;

    void Start()
    {
        userMail.onValueChanged.AddListener(delegate { CheckValidInput(); });
        userPassword.onValueChanged.AddListener(delegate { CheckValidInput(); });
        CheckIsRestaurantValue();
    }

    void OnEnable()
    {
        CheckIsRestaurantValue();
    }

    public void CheckIsRestaurantValue()
    {
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle != null)
        {
            //Debug.Log(selectedToggle, selectedToggle);
            if (selectedToggle.name == "tgl_Restaurant")
                isRestaurant.var = true;
            else
                isRestaurant.var = false;
        }
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
