using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerRecipePreview : MonoBehaviour
{
    public DataBaseManager dbManager;
    [SerializeField]
    private Image imgDish;
    [SerializeField]
    private Image imgDifficulty;
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

        txtName.text = recipe.name;
        txtDescription.text = recipe.description;
        if (!string.IsNullOrEmpty(recipe.prepTime))
        {
            string[] aPrepTime = recipe.prepTime.Split('-');
            int hours = int.Parse(aPrepTime[0]);
            int minutes = int.Parse(aPrepTime[1]);

            txtPrepTime.text = $"{GetHoursOptions(hours)} {GetMinutesOptions(minutes)}";
        }

        if (imgIsFavorite != null)
        {
            StartCoroutine(dbManager.endpointsTools.GetWithParam(API.urlGetFavorites, AuthManager.currentUserId, "", returnValue =>
            {
                //Debug.Log(returnValue);
                var jsonString = JSON.Parse(returnValue);
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
            }));
        }
    }
}
