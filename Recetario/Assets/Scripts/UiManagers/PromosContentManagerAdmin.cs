using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Events;

public class PromosContentManagerAdmin : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private GameObject contentPromosObj;
    [SerializeField]
    private GameObject pfb_grp_promo_preview;
    //[SerializeField]
    //private UiManagerEditRecipe editPanel;

    [SerializeField]
    private UnityEvent onEditPromo;

    private void OnEnable()
    {
        GetPromos();
    }

    public void AddNewPromo()
    {
        StartCoroutine(dbManager.endpointsTools.PostJsonWithParam(API.urlPostNewPromo, "", "{}", returnValue =>
        {
            //Debug.Log(returnValue);
            GetPromos();
        }));
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

                InstantiatePromoPreview(promo);
            }
        }));
    }

    private void InstantiatePromoPreview(Promo promo)
    {
        //Debug.Log($"{promo.id} {promo.imageUrl} {promo.url}");
        GameObject obj = Instantiate(pfb_grp_promo_preview, contentPromosObj.transform);
        PromoData promoData = obj.GetComponent<PromoData>();
        promoData.promo = promo;
        UiManagerPromoPreview promoPreview = obj.GetComponent<UiManagerPromoPreview>();
        promoPreview.dbManager = dbManager;
        promoPreview.InitUiValues(promoData.promo);

        obj.transform.Find("btn_edit").GetComponent<Button>().onClick.AddListener(delegate
        {
            //editPanel.recipe = recipeData.recipe;
            //editPanel.dbManager = dbManager;
            //open menu editor
            onEditPromo.Invoke();
        });

        obj.transform.Find("btn_erase").GetComponent<Button>().onClick.AddListener(delegate
        {
            StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeletePromo, promoData.promo.id, returnValue =>
            {
                //Debug.Log(returnValue);
                GetPromos();
            }));
        });
    }
}
