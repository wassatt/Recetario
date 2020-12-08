using UnityEngine;
using UnityEngine.UI;

public class UiManagerProfile : MonoBehaviour
{
    #region ▂▃▅▇█▓▒░VARIABLES░▒▓█▇▅▃▂

    [SerializeField]
    private AuthManager authManager;
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private ScriptableUserData sUserData;
    [SerializeField]
    private Image imgProfile;
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Text txtMail;
    [SerializeField]
    private InputField ifName;
    [SerializeField]
    private InputField ifMail;
    [SerializeField]
    private InputField ifCurrentPass;
    [SerializeField]
    private InputField ifNewPass;
    [SerializeField]
    private InputField ifPasswordMail;
    [SerializeField]
    private Button btnAcceptName;
    [SerializeField]
    private Button btnAcceptMail;
    [SerializeField]
    private Button btnAcceptPass;

    #endregion

    void Start()
    {
        GetProfileImage();
        UpdateUiTextValues();
        btnAcceptName.onClick.AddListener(delegate { UpdateUserName(); });
        btnAcceptMail.onClick.AddListener(delegate { UpdateUserMail(); });
        btnAcceptPass.onClick.AddListener(delegate { UpdateUserPassword(); });
    }

    public void UpdateUiTextValues() //Don't enter coroutines in this method
    {
        txtName.text = AuthManager.currentUserName;
        ifName.text = AuthManager.currentUserName;
        txtMail.text = AuthManager.currentUserMail;
        ifMail.text = AuthManager.currentUserMail;
        ifPasswordMail.text = "";
    }

    public void GetProfileImage()
    {
        if (!string.IsNullOrEmpty(sUserData.Get().imageUrl))
        {
            StartCoroutine(dbManager.endpointsTools.GetImageCoroutine(sUserData.Get().imageUrl, returnValue =>
            {
                var _texture = new Texture2D(1, 1);
                _texture.LoadImage(returnValue);
                Sprite sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                imgProfile.overrideSprite = sprite;
            }));
        }
    }

    public void UpdateUserName()
    {
        StartCoroutine(authManager.UpdateProfile(ifName.text, "", returnValue =>
        {
            //Debug.Log(returnValue);
            UpdateUiTextValues();
        }));
    }

    public void UpdateUserMail()
    {
        StartCoroutine(authManager.UpdateMail(AuthManager.currentUserMail, ifMail.text, ifPasswordMail.text, returnValue =>
        {
            //Debug.Log(returnValue);
            UpdateUiTextValues();
        }));
    }

    public void UpdateUserPassword()
    {
        StartCoroutine(authManager.UpdatePassword(AuthManager.currentUserMail, ifCurrentPass.text, ifNewPass.text, returnValue =>
        {
            //Debug.Log(returnValue);
        }));
    }
}