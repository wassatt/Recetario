using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManagerEditPromo : MonoBehaviour
{
    public DataBaseManager dbManager;
    public Promo promo;
    [SerializeField]
    private Image imgPromo;
    [SerializeField]
    private InputField ifUrl;
    [SerializeField]
    private UnityEvent onBackToPromos;

    void OnEnable()
    {
        ReloadPanelValues();
    }

    private void ReloadPanelValues()
    {
        ifUrl.text = promo.url;

        if (!string.IsNullOrEmpty(promo.imageUrl) && gameObject.activeInHierarchy)
        {
            StartCoroutine(dbManager.endpointsTools.GetImageCoroutine(promo.imageUrl, returnValue =>
            {
                var _texture = new Texture2D(1, 1);
                _texture.LoadImage(returnValue);
                Sprite sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                imgPromo.overrideSprite = sprite;
            }));
        }
    }

    public void UpdatePromo()
    {
        promo.url = ifUrl.text;
        string json = JsonUtility.ToJson(promo);

        StartCoroutine(dbManager.endpointsTools.PatchWithParam(API.urlUpdatePromo, promo.id, json, returnValue =>
        {
            //Debug.Log(returnValue);
            onBackToPromos.Invoke();
        }));
    }

    public void UpdatePromoImage(ScriptableString imagePath)
    {
        var bytes = System.IO.File.ReadAllBytes(imagePath.Get());
        StartCoroutine(dbManager.endpointsTools.PostFileWithParam(API.urlPostPromoImage, promo.id, bytes, returnValue =>
        {
            //Debug.Log(returnValue);
        }));
    }
}
