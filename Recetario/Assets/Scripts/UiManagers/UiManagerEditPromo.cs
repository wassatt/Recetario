using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerEditPromo : MonoBehaviour
{
    public DataBaseManager dbManager;
    public Promo promo;
    [SerializeField]
    private Image imgPromo;
    [SerializeField]
    private InputField ifUrl;

    void OnEnable()
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
    
}
