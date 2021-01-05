using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerFullRecipe : MonoBehaviour
{
    public Recipe recipe;
    public DataBaseManager dbManager;

    [SerializeField]
    private Image imgDish;
    [SerializeField]
    private Image imgDifficulty;
    [SerializeField]
    private Button btnLike;
    [SerializeField]
    private Text txtLikes;
    [SerializeField]
    private Button btnFavorite;
    [SerializeField]
    private GameObject imgIsFavorite;
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Text txtPrepTime;
    [SerializeField]
    private Text txtDescription;


    [SerializeField]
    private Sprite[] spritesDiff;

    [SerializeField]
    private GameObject contentIngredientsObj;
    [SerializeField]
    private GameObject pfb_grp_ingredient;

    [SerializeField]
    private GameObject contentInstructionsObj;
    [SerializeField]
    private GameObject pfb_grp_instruction;

    [SerializeField]
    private VerticalLayoutGroup layoutGroup;

    string[] hoursOptions = new string[] { "", "1 hr", "2 hr",
        "3 hr", "4 hr", "5 hr"};
    private string GetHoursOptions(int index)
    {
        return hoursOptions[index];
    }

    string[] minutesOptions = new string[] { "", "15 min", "25 min",
        "30 min", "45 min" };
    private string GetMinutesOptions(int index)
    {
        return minutesOptions[index];
    }

    private void OnEnable()
    {
        InitUiValues(recipe);
        StartCoroutine(FixContentSize());
        //InstantiateIngredients();
        //InstantiateInstructions();
    }

    public void InitUiValues(Recipe recipe)
    {
        if (recipe.difficulty < spritesDiff.Length)
            imgDifficulty.overrideSprite = spritesDiff[recipe.difficulty];

        if (!string.IsNullOrEmpty(recipe.imageUrl) && gameObject.activeInHierarchy)
        {
            StartCoroutine(dbManager.endpointsTools.GetImageCoroutine(recipe.imageUrl, returnValue =>
            {
                var _texture = new Texture2D(1, 1);
                _texture.LoadImage(returnValue);
                Sprite sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                imgDish.overrideSprite = sprite;
            }));
        }

        GetLikes();

        txtName.text = recipe.name;
        txtDescription.text = recipe.description;
        if (!string.IsNullOrEmpty(recipe.prepTime))
        {
            string[] aPrepTime = recipe.prepTime.Split('-');
            int hours = int.Parse(aPrepTime[0]);
            int minutes = int.Parse(aPrepTime[1]);

            txtPrepTime.text = $"{GetHoursOptions(hours)} {GetMinutesOptions(minutes)}";
        }

        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetFavorites, AuthManager.currentUserId, "", returnValue =>
        {
            //Debug.Log(returnValue);
            var jsonString = JSON.Parse(returnValue);
            btnFavorite.interactable = false;
            imgIsFavorite.SetActive(false);

            foreach (JSONNode obj in jsonString)
            {
                string objString = obj.ToString();
                //Debug.Log(objString);
                var child = JSON.Parse(objString);
                var favoriteId = child["recipeId"].Value;
                //Debug.Log(favoriteId);

                if (favoriteId == recipe.id)
                {
                    imgIsFavorite.SetActive(true);
                }
            }

            btnFavorite.interactable = true;
        }));

        btnFavorite.onClick.AddListener(delegate { ToggleFavorite(); });
        btnLike.onClick.AddListener(delegate {
            StartCoroutine(dbManager.endpointsTools.PostJsonWithParam(API.urlLikeRecipe, $"{recipe.id}/{AuthManager.currentUserId}", "{}", returnValue =>
            {
                //Debug.Log(returnValue);
                GetLikes();
            }));
        });
    }

    private void GetLikes()
    {
        StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetLikes, recipe.id, "", returnValue =>
        {
            txtLikes.text = returnValue;
        }));
    }

    public void InstantiateIngredients()
    {
        RectTransform rtContent = contentIngredientsObj.GetComponent<RectTransform>();
        float originalWidth = rtContent.rect.width;
        float height = 0; 
        rtContent.sizeDelta = new Vector2(originalWidth, height);

        foreach (Transform child in contentIngredientsObj.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Ingredient item in recipe.listIngredients)
        {
            InstantiateIngredient(item);
        }

        foreach (Transform child in contentIngredientsObj.transform)
        {
            height += child.GetComponent<RectTransform>().rect.height;
        }

        rtContent.sizeDelta = new Vector2(originalWidth, height); 
    }

    private void InstantiateIngredient(Ingredient ingredient)
    {
        GameObject obj = Instantiate(pfb_grp_ingredient, contentIngredientsObj.transform);

        Text txtName = obj.transform.Find("txt_Name").GetComponent<Text>();
        txtName.text = ingredient.name;
        Text txtQty = obj.transform.Find("txt_Qty").GetComponent<Text>();
        txtQty.text = ingredient.qty;

        Button addToCart = obj.transform.Find("btn_AddToCart").GetComponent<Button>();

        if (txtQty.text.Equals("grp"))
        {
            txtQty.gameObject.SetActive(false);
            addToCart.gameObject.SetActive(false);
            txtName.text = "\n" + ingredient.name + "\n";
        }

        //obj.transform.Find("btn_AddToCart").GetComponent<Button>().onClick.AddListener(delegate
        //{
        //    //Debug.Log($"{recipe.id}/{ingredient.id}");
        //    StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteIngredient, $"{recipe.id}/{ingredient.id}", returnValue =>
        //    {
        //        //Debug.Log(returnValue);
        //        InstantiateIngredients();
        //    }));
        //});
    }

    public void InstantiateInstructions()
    {
        RectTransform rtContent = contentInstructionsObj.GetComponent<RectTransform>();
        float originalWidth = rtContent.rect.width;
        float height = 0;
        rtContent.sizeDelta = new Vector2(originalWidth, height);

        foreach (Transform child in contentInstructionsObj.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Instruction item in recipe.listInstructions)
        {
            InstantiateInstruction(item);
        }

        rtContent.sizeDelta = new Vector2(originalWidth, height);
    }

    private void InstantiateInstruction(Instruction instruction)
    {
        GameObject obj = Instantiate(pfb_grp_instruction, contentInstructionsObj.transform);

        Text txtText = obj.GetComponent<Text>();
        txtText.text = instruction.text;

        //obj.transform.Find("tgl_Done").GetComponent<Toggle>().onValueChanged.AddListener(delegate
        //{
        //    //Debug.Log($"{recipe.id}/{instruction.id}");
        //    StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteInstruction, $"{recipe.id}/{instruction.id}", returnValue =>
        //    {
        //        //Debug.Log(returnValue);
        //        InstantiateInstructions();
        //    }));
        //});
    }

    public void ToggleFavorite()
    {
        btnFavorite.interactable = false;
        if (!imgIsFavorite.activeInHierarchy)
        {
            StartCoroutine(dbManager.endpointsTools.PostJsonWithParam(API.urlAddFavorite, $"{AuthManager.currentUserId}/{recipe.id}", "{}", returnValue =>
            {
                //Debug.Log(returnValue);
                btnFavorite.interactable = true;
                imgIsFavorite.SetActive(true);
            }));
        } else
        {
            StartCoroutine(dbManager.endpointsTools.DeleteWithParam(API.urlDeleteFavorite, $"{AuthManager.currentUserId}/{recipe.id}", returnValue =>
            {
                //Debug.Log(returnValue);
                btnFavorite.interactable = true;
                imgIsFavorite.SetActive(false);
            }));
        }
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
}
