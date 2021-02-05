using SimpleJSON;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiManagerEditRecipe : MonoBehaviour
{
    public Recipe recipe;
    public DataBaseManager dbManager;
    [SerializeField]
    private Image imgDish;
    [SerializeField]
    private Dropdown ddCategory;
    [SerializeField]
    private Dropdown ddDifficulty;
    [SerializeField]
    private Dropdown ddHours;
    [SerializeField]
    private Dropdown ddMinutes;
    [SerializeField]
    private InputField ifName;
    [SerializeField]
    private InputField ifDescription;

    [SerializeField]
    private GameObject contentIngredientsObj;
    [SerializeField]
    private GameObject pfb_grp_ingredient;

    [SerializeField]
    private GameObject contentInstructionsObj;
    [SerializeField]
    private GameObject pfb_grp_instruction;

    public UnityEvent onBackToMyRecipes;

    void OnEnable()
    {
        ReloadPanelValues();
        InstantiateIngredients();
        InstantiateInstructions();
    }

    public void ReloadPanelValues()
    {
        if (!string.IsNullOrEmpty(recipe.imageUrl))
        {
            StartCoroutine(dbManager.endpointsTools.GetImageCoroutine(recipe.imageUrl, returnValue =>
            {
                var _texture = new Texture2D(1, 1);
                _texture.LoadImage(returnValue);
                Sprite sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                imgDish.overrideSprite = sprite;
            }));
        }

        ifName.text = recipe.name;
        ifDescription.text = recipe.description;
        ddCategory.SetValueWithoutNotify(recipe.category);
        ddDifficulty.SetValueWithoutNotify(recipe.difficulty);

        if (!string.IsNullOrEmpty(recipe.prepTime))
        {
            string[] aPrepTime = recipe.prepTime.Split('-');
            int hours = int.Parse(aPrepTime[0]);
            int minutes = int.Parse(aPrepTime[1]);
            ddHours.SetValueWithoutNotify(hours);
            ddMinutes.SetValueWithoutNotify(minutes);
        }
    }

    public void AddNewIngredient()
    {
        StartCoroutine(dbManager.endpointsTools.PostJsonWithParam(API.urlPostNewIngredient, $"{recipe.id}", "{}", returnValue =>
        {
            //Debug.Log(returnValue);
            InstantiateIngredients();
        }));
    }

    public void AddNewInstruction()
    {
        StartCoroutine(dbManager.endpointsTools.PostJsonWithParam(API.urlPostNewInstruction, $"{recipe.id}", "{}", returnValue =>
        {
            //Debug.Log(returnValue);
            InstantiateInstructions();
        }));
    }

    private void InstantiateIngredients()
    {
        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetIngredients, recipe.id, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);
            recipe.listIngredients.Clear();

            foreach (JSONNode itemObj in jsonString)
            {
                string itemString = itemObj.ToString();
                Ingredient item = JsonUtility.FromJson<Ingredient>(itemString);
                recipe.listIngredients.Add(item);
                //Debug.Log(itemString);
            }

            foreach (Transform child in contentIngredientsObj.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Ingredient item in recipe.listIngredients)
            {
                InstantiateIngredient(item);
            }

        }));
    }

    private void InstantiateIngredient(Ingredient ingredient)
    {
        GameObject obj = Instantiate(pfb_grp_ingredient, contentIngredientsObj.transform);

        InputField ifName = obj.transform.Find("if_name").GetComponent<InputField>();
        ifName.text = ingredient.name;
        InputField ifQty = obj.transform.Find("if_qty").GetComponent<InputField>();
        ifQty.text = ingredient.qty;

        ifName.onEndEdit.AddListener(delegate
        {
            string json = "{\"name\" : \"" + ifName.text + "\", \"qty\" : \"" + ifQty.text + "\"}";
            //Debug.Log($"{recipe.id}/{ingredient.id}");
            StartCoroutine(dbManager.endpointsTools.PatchWithParam(API.urlUpdateIngredient, $"{recipe.id}/{ingredient.id}", json, returnValue =>
            {
                //Debug.Log(returnValue);
            }));
        });

        ifQty.onEndEdit.AddListener(delegate
        {
            string json = "{\"name\" : \"" + ifName.text + "\", \"qty\" : \"" + ifQty.text + "\"}";
            //Debug.Log($"{recipe.id}/{ingredient.id}");
            StartCoroutine(dbManager.endpointsTools.PatchWithParam(API.urlUpdateIngredient, $"{recipe.id}/{ingredient.id}", json, returnValue =>
            {
                //Debug.Log(returnValue);
            }));
        });

        obj.transform.Find("btn_delete").GetComponent<Button>().onClick.AddListener(delegate
        {
            //Debug.Log($"{recipe.id}/{ingredient.id}");
            StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteIngredient, $"{recipe.id}/{ingredient.id}", returnValue =>
            {
                //Debug.Log(returnValue);
                InstantiateIngredients();
            }));
        });
    }

    private void InstantiateInstructions()
    {
        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetInstructions, recipe.id, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);
            recipe.listInstructions.Clear();

            foreach (JSONNode itemObj in jsonString)
            {
                string itemString = itemObj.ToString();
                Instruction item = JsonUtility.FromJson<Instruction>(itemString);
                recipe.listInstructions.Add(item);
                //Debug.Log(itemString);
            }

            foreach (Transform child in contentInstructionsObj.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (Instruction item in recipe.listInstructions)
            {
                InstantiateInstruction(item);
            }
        }));
    }

    private void InstantiateInstruction(Instruction instruction)
    {
        GameObject obj = Instantiate(pfb_grp_instruction, contentInstructionsObj.transform);

        InputField ifText = obj.transform.Find("if_InstructionText").GetComponent<InputField>();
        ifText.text = instruction.text;

        ifText.onEndEdit.AddListener(delegate
        {
            string cleanText = Regex.Replace(ifText.text, @"\n", "\\n");
            cleanText = Regex.Replace(cleanText, @"\r", "\\r");

            string json = "{\"text\" : \"" + cleanText + "\"}";

            //Debug.Log($"{recipe.id}/{instruction.id}");
            StartCoroutine(dbManager.endpointsTools.PatchWithParam(API.urlUpdateInstruction, $"{recipe.id}/{instruction.id}", json, returnValue =>
            {
                //Debug.Log(returnValue);
            }));
        });


        obj.transform.Find("btn_delete").GetComponent<Button>().onClick.AddListener(delegate
        {
            Debug.Log($"{recipe.id}/{instruction.id}");
            StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteInstruction, $"{recipe.id}/{instruction.id}", returnValue =>
            {
                //Debug.Log(returnValue);
                InstantiateInstructions();
            }));
        });

    }

    public void UpdateRecipe()
    {
        recipe.name = ifName.text;
        recipe.description = ifDescription.text;
        recipe.category = ddCategory.value;
        recipe.difficulty = ddDifficulty.value;
        recipe.prepTime = $"{ddHours.value}-{ddMinutes.value}";
        string json = JsonUtility.ToJson(recipe);

        StartCoroutine(dbManager.endpointsTools.PatchWithParam(API.urlUpdateRecipe, recipe.id, json, returnValue =>
        {
            //Debug.Log(returnValue);
            onBackToMyRecipes.Invoke();
        }));
    }

    public void UpdateRecipeImage(ScriptableString imagePath)
    {
        var bytes = System.IO.File.ReadAllBytes(imagePath.Get());
        StartCoroutine(dbManager.endpointsTools.PostFileWithParam(API.urlPostRecipeImage, recipe.id, bytes, returnValue =>
        {
            //Debug.Log(returnValue);
        }));
    }
}
