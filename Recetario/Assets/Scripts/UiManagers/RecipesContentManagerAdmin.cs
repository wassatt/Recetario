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
        GetRecipes();
    }

    public void AddNewRecipe()
    {
        StartCoroutine(dbManager.endpointsTools.PostJsonWithParam(API.urlPostNewRecipe, "", "{}", returnValue =>
        {
            //Debug.Log(returnValue);
            GetRecipes();
        }));
    }

    private void GetRecipes()
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
                recipe.listIngredients.Clear();

                var jsonObj = JSON.Parse(objString);
                var ingredients = jsonObj["ingredients"];

                foreach (JSONNode itemObj in ingredients)
                {
                    string itemString = itemObj.ToString();
                    Ingredient item = JsonUtility.FromJson<Ingredient>(itemString);
                    recipe.listIngredients.Add(item);
                    //Debug.Log(itemString);
                }

                var instructions = jsonObj["instructions"];

                foreach (JSONNode itemObj in instructions)
                {
                    string itemString = itemObj.ToString();
                    Instruction item = JsonUtility.FromJson<Instruction>(itemString);
                    recipe.listInstructions.Add(item);
                    //Debug.Log(itemString);
                }

                InstantiateRecipePreview(recipe);
            }
        }));
    }

    private void InstantiateRecipePreview(Recipe recipe)
    {
        GameObject obj = Instantiate(pfb_grp_recipe_preview, contentRecipesObj.transform);
        RecipeData recipeData = obj.GetComponent<RecipeData>();
        recipeData.recipe = recipe;
        UiManagerRecipePreview recipePreview = obj.GetComponent<UiManagerRecipePreview>();
        recipePreview.dbManager = dbManager;
        recipePreview.InitUiValues(recipeData.recipe);

        obj.transform.Find("btn_edit").GetComponent<Button>().onClick.AddListener(delegate {
            editPanel.recipe = recipeData.recipe;
            editPanel.dbManager = dbManager;
            //open menu editor
            onEditRecipe.Invoke();
        });

        obj.transform.Find("btn_erase").GetComponent<Button>().onClick.AddListener(delegate {
            //endpoint delete recipe
            StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteRecipe, recipeData.recipe.id, returnValue =>
            {
                //Debug.Log(returnValue);
                GetRecipes();
            }));
        });
    }
}