using System.Collections;
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

    public UnityEvent onBackToMyRecipes;

    // Start is called before the first frame update
    void OnEnable()
    {
        ReloadPanelValues();
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
            Debug.Log(returnValue);
            //ReloadPanelValues();
            onBackToMyRecipes.Invoke();
        }));
    }
}
