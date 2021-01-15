using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListContentManager : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private GameObject contentCartObj;
    [SerializeField]
    private GameObject pfb_grp_CartItem;
    [SerializeField]
    private VerticalLayoutGroup layoutGroup;
    [SerializeField]
    private VerticalLayoutGroup layoutGroupScroll;

    private void OnEnable()
    {
        GetCart();
    }

    public void GetCart()
    {
        foreach (Transform child in contentCartObj.transform)
        {
            Destroy(child.gameObject);
        }

        //List<Ingredient> ingredients = new List<Ingredient>();
        //ingredients.Clear();

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetCart, AuthManager.currentUserId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

            int itemsCount = 0;
            foreach (JSONNode obj in jsonString)
            {
                itemsCount++;
            }

            int itemsCountDone = 0;

            foreach (JSONNode obj in jsonString)
            {
                string objString = obj.ToString();
                //Debug.Log(objString);
                var jsonObj = JSON.Parse(objString);
                var ingredientPath = jsonObj["ingredientPath"];
                //Debug.Log(ingredientPath);

                StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetIngredient, ingredientPath, "", ingredientValue =>
                {
                    //Debug.Log(ingredientValue);
                    Ingredient ingredient = JsonUtility.FromJson<Ingredient>(ingredientValue);
                    //ingredients.Add(ingredient);
                    InstantiateCartItem(ingredient);
                    itemsCountDone++;

                    if (itemsCountDone == itemsCount)
                    {
                        //foreach (Ingredient item in ingredients)
                        //{
                        //    InstantiateCartItem(item);
                        //}
                        StartCoroutine(FixContentSizeScroll());
                    }
                }));
            }
        }));

    }

    void InstantiateCartItem(Ingredient ingredient)
    {
        GameObject obj = Instantiate(pfb_grp_CartItem, contentCartObj.transform);
        Text txtQty = obj.transform.Find("txt_quantity").GetComponent<Text>();
        txtQty.text = ingredient.qty;
        Text txtName = txtQty.gameObject.transform.Find("txt_name").GetComponent<Text>();
        txtName.text = ingredient.name;

        txtName.gameObject.transform.Find("btn_remove").GetComponent<Button>().onClick.AddListener(delegate {
            StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteCartItem, $"{AuthManager.currentUserId}/{ingredient.id}", returnValue =>
            {
                //Debug.Log(returnValue);
                Destroy(obj);
                StartCoroutine(FixContentSize());
                StartCoroutine(FixContentSizeScroll());
            }));
        });

        StartCoroutine(FixContentSize());
    }

    IEnumerator FixContentSize()
    {
        layoutGroup.enabled = false;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = true;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = false;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = true;
    }

    IEnumerator FixContentSizeScroll()
    {
        yield return new WaitForSeconds(.5f);
        layoutGroupScroll.enabled = false;
        yield return new WaitForEndOfFrame();
        layoutGroupScroll.enabled = true;
        yield return new WaitForEndOfFrame();
        layoutGroupScroll.enabled = false;
        yield return new WaitForEndOfFrame();
        layoutGroupScroll.enabled = true;
    }
}
