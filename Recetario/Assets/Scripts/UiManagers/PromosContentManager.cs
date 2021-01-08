using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class PromosContentManager : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private GameObject contentPromosObj;
    [SerializeField]
    private GameObject pfb_grp_promo;

    private void OnEnable()
    {
        GetPromos();
    }

    private void GetPromos()
    {
        foreach (Transform child in contentPromosObj.transform)
        {
            Destroy(child.gameObject);
        }

        Promo promo = new Promo();

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetPromos, "", "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

            foreach (JSONNode obj in jsonString)
            {
                string objString = obj.ToString();
                //Debug.Log(objString);
                promo = JsonUtility.FromJson<Promo>(objString);

                InstantiatePromo(promo);
            }
        }));
    }

    private void InstantiatePromo(Promo promo)
    {
        //Debug.Log($"{promo.id} {promo.imageUrl} {promo.url}");
        GameObject obj = Instantiate(pfb_grp_promo, contentPromosObj.transform);
        PromoData promoData = obj.GetComponent<PromoData>();
        promoData.promo = promo;
        Transform imageMask = obj.transform.Find("img_promotion_mask");
        Transform promoThumbnail = imageMask.Find("img_promotion_thumbnail");
        Image imgPromo = promoThumbnail.GetComponent<Image>();

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

        promoThumbnail.GetComponent<Button>().onClick.AddListener(delegate
        {
            OpenURL(promo.url);
        });
    }

    public void OpenURL(string url)
    {
        if (url != null)
            Application.OpenURL(url);
        else
        {
            //Application.OpenURL("http://unity3d.com/");
        }
    }
}
