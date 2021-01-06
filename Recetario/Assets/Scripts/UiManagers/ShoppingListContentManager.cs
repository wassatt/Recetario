using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
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

        Ingredient ingredient = new Ingredient();

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetCart, AuthManager.currentUserId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

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
                    ingredient = JsonUtility.FromJson<Ingredient>(ingredientValue);
                    InstantiateCartItem(ingredient);
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

        });

        StartCoroutine(FixContentSize());
    }

    IEnumerator FixContentSize()
    {
        layoutGroup.enabled = false;
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = true;
    }
}
