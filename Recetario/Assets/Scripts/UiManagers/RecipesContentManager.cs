using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class RecipesContentManager : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private InputField ifSearch;
    [SerializeField]
    private GameObject contentRecipesObj;
    [SerializeField]
    private GameObject pfb_grp_recipe_preview;

    // Start is called before the first frame update
    void Start()
    {
        GetResRecipes();
    }

    private void GetResRecipes()
    {
        foreach (Transform child in contentRecipesObj.transform)
        {
            Destroy(child.gameObject);
        }

        Recipe recipe = new Recipe();

        //TODO: make get random restaurants endpoint
        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetRecipes, "", "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

            foreach (JSONNode obj in jsonString)
            {
                string objString = obj.ToString();
                //Debug.Log(objString);
                recipe = JsonUtility.FromJson<Recipe>(objString);
                InstantiatRecipePreview(recipe);
            }

            //foreach (Transform child in contentRecipesObj.transform)
            //{
                //child.gameObject.GetComponent<RestaurantImageLoader>().Init();
                //child.gameObject.GetComponent<RestaurantImageLoader>().LoadImage();
            //}
        }));
    }

    private void InstantiatRecipePreview(Recipe recipe)
    {
        GameObject obj = Instantiate(pfb_grp_recipe_preview, contentRecipesObj.transform);
        RecipeData recipeData = obj.GetComponent<RecipeData>();
        recipeData.recipe = recipe;
        UIManagerRecipePreview recipePreview = obj.GetComponent<UIManagerRecipePreview>();
        recipePreview.InitUiValues(recipeData.recipe);
    }
}
