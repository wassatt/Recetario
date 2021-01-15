using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerSettings : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private Button btnRemoveAllFavorites;
    [SerializeField]
    private Button btnClearCart;

    void Start()
    {
        btnRemoveAllFavorites.onClick.AddListener(delegate { RemoveAllFavorites(); });
        btnClearCart.onClick.AddListener(delegate { ClearCart(); });
    }

    public void RemoveAllFavorites()
    {
        Favorite favorite = new Favorite();

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetFavorites, AuthManager.currentUserId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

            foreach (JSONNode obj in jsonString)
            {
                string objString = obj.ToString();
                //Debug.Log(objString);
                favorite = JsonUtility.FromJson<Favorite>(objString);

                StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteFavorite, $"{AuthManager.currentUserId}/{favorite.recipeId}", callbackReturn =>
                {
                    //Debug.Log(callbackReturn);
                }));
            }
        }));
    }

    public void ClearCart()
    {
        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetCart, AuthManager.currentUserId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

            int itemsCount = 0;
            foreach (JSONNode obj in jsonString)
            {
                itemsCount++;
            }

            foreach (JSONNode obj in jsonString)
            {
                string objString = obj.ToString();
                //Debug.Log(objString);
                var jsonObj = JSON.Parse(objString);
                string ingredientPath = jsonObj["ingredientPath"];
                string ingredientId = ingredientPath.Split('/')[1];
                //Debug.Log(ingredientId);

                StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteCartItem, $"{AuthManager.currentUserId}/{ingredientId}", callbackReturn =>
                {
                    //Debug.Log(callbackReturn);
                }));
            }
        }));
    }
}
