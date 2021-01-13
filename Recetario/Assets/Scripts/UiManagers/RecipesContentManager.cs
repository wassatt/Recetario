using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Events;

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
    [SerializeField]
    private UiManagerFullRecipe fullRecipe;

    public UnityEvent onOpenFullRecipe;
    public UnityEvent onSearchDone;

    // Start is called before the first frame update
    void Start()
    {
        GetRecipes();
    }

    public void GetRecipesBySearch()
    {
        foreach (Transform child in contentRecipesObj.transform)
        {
            Destroy(child.gameObject);
        }

        Recipe recipe = new Recipe();

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetRecipesSearch, ifSearch.text, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);
            onSearchDone.Invoke();

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

                InstantiatRecipePreview(recipe);
            }
        }));
    }

    public void GetRecipes()
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
            onSearchDone.Invoke();

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

                InstantiatRecipePreview(recipe);
            }
        }));
    }

    public void GetRecipesByCategory(int category)
    {
        foreach (Transform child in contentRecipesObj.transform)
        {
            Destroy(child.gameObject);
        }

        Recipe recipe = new Recipe();

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetRecipes, $"{category}", "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);

            onSearchDone.Invoke();

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

                InstantiatRecipePreview(recipe);
            }
        }));
    }

    private void InstantiatRecipePreview(Recipe recipe)
    {
        GameObject obj = Instantiate(pfb_grp_recipe_preview, contentRecipesObj.transform);
        RecipeData recipeData = obj.GetComponent<RecipeData>();
        recipeData.recipe = recipe;
        UiManagerRecipePreview recipePreview = obj.GetComponent<UiManagerRecipePreview>();
        recipePreview.dbManager = dbManager;
        recipePreview.InitUiValues(recipeData.recipe);

        obj.transform.Find("btn_OpenRecipe").GetComponent<Button>().onClick.AddListener(delegate {
            fullRecipe.recipe = recipeData.recipe;
            fullRecipe.dbManager = dbManager;
            //fullRecipe.InstantiateIngredients();
            //fullRecipe.InstantiateInstructions();

            onOpenFullRecipe.Invoke();
        });
    }
}
