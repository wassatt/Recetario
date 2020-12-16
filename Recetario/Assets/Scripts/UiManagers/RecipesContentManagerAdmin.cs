using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Events;

public class RecipesContentManagerAdmin : MonoBehaviour
{
    [SerializeField]
    private DataBaseManager dbManager;
    [SerializeField]
    private GameObject contentRecipesObj;
    [SerializeField]
    private GameObject pfb_grp_recipe_preview;
    [SerializeField]
    private UiManagerEditRecipe editPanel;


    public UnityEvent onEditRecipe;

    private void OnEnable()
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
        }));
    }

    private void InstantiatRecipePreview(Recipe recipe)
    {
        GameObject obj = Instantiate(pfb_grp_recipe_preview, contentRecipesObj.transform);
        RecipeData recipeData = obj.GetComponent<RecipeData>();
        recipeData.recipe = recipe;
        UIManagerRecipePreview recipePreview = obj.GetComponent<UIManagerRecipePreview>();
        recipePreview.InitUiValues(recipeData.recipe);

        obj.transform.Find("btn_edit").GetComponent<Button>().onClick.AddListener(delegate {
            editPanel.recipe = recipeData.recipe;
            editPanel.dbManager = dbManager;
            //open menu editor
            onEditRecipe.Invoke();
        });

        obj.transform.Find("btn_erase").GetComponent<Button>().onClick.AddListener(delegate {
            //endpoint delete recipe
            //GetResRecipes();
        });
    }
}