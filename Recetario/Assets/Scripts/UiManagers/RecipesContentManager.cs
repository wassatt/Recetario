using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Events;
using System.Collections.Generic;

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
    [SerializeField]
    private ScrollRect srRecipes;

    public UnityEvent onOpenFullRecipe;
    public UnityEvent onSearchDone;
    public List<Recipe> recipes;
    int recipesAt;
    int instancesOffset;
    // Start is called before the first frame update
    void Start()
    {
        recipes = new List<Recipe>();
        instancesOffset = 4;
        srRecipes.onValueChanged.AddListener(delegate {CheckIfScrollIsAtBottom();});
        GetRecipes();
    }

    public void GetRecipesBySearch()
    {
        recipesAt = 0;

        foreach (Transform child in contentRecipesObj.transform)
        {
            Destroy(child.gameObject);
        }

        Recipe recipe = new Recipe();
        recipes.Clear();

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

                recipes.Add(recipe);
                //InstantiateRecipePreview(recipe);
            }
            InstantiateRecipes(recipesAt);
        }));
    }

    public void GetRecipes()
    {
        recipesAt = 0;

        foreach (Transform child in contentRecipesObj.transform)
        {
            Destroy(child.gameObject);
        }

        Recipe recipe = new Recipe();
        recipes.Clear();

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

                recipes.Add(recipe);
                //InstantiateRecipePreview(recipe);
            }

            //TODO: Shuffle
            InstantiateRecipes(recipesAt);
        }));
    }

    public void CheckIfScrollIsAtBottom()
    {
        if (srRecipes.verticalNormalizedPosition <= 0.05f)
        {
            // get last index
            InstantiateRecipes(recipesAt);
        }
    }

    public void GetRecipesByCategory(int category)
    {
        recipesAt = 0;

        foreach (Transform child in contentRecipesObj.transform)
        {
            Destroy(child.gameObject);
        }

        Recipe recipe = new Recipe();
        recipes.Clear();

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

                recipes.Add(recipe);
                //InstantiateRecipePreview(recipe);
            }
                InstantiateRecipes(recipesAt);
        }));
    }

    private void InstantiateRecipes(int startAt)
    {
        if(recipes.Count != 0)
        {
            if (recipes.Count > instancesOffset)
            {
                for (int i = startAt; i < startAt+ instancesOffset; i++)
                {
                    if (i < recipes.Count)
                        InstantiateRecipePreview(recipes[i]);
                }
                
                recipesAt = startAt + instancesOffset;
            }
            else
            {
                foreach (Recipe recipe in recipes)
                {
                    if (recipesAt < recipes.Count)
                        InstantiateRecipePreview(recipe);
                    recipesAt++;
                }
            }
        }
    }

    private void InstantiateRecipePreview(Recipe recipe)
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

//static class MyExtensions
//{
//    private static Random rng = new Random();

//    public static void Shuffle<T>(this IList<T> list)
//    {
//        int n = list.Count;
//        while (n > 1)
//        {
//            n--;
//            int k = rng.Next(n + 1);
//            T value = list[k];
//            list[k] = list[n];
//            list[n] = value;
//        }
//    }
//}