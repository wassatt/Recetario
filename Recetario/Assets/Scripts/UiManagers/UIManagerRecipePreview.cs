using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManagerRecipePreview : MonoBehaviour
{
    public DataBaseManager dbManager;
    [SerializeField]
    private Image imgDish;
    [SerializeField]
    private Image imgDifficulty;
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
    }

    IEnumerator GetImageCoroutine(string url, Image image)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            byte[] results = www.downloadHandler.data;
            var _texture = new Texture2D(1, 1);
            _texture.LoadImage(results);
            Sprite sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            image.overrideSprite = sprite;
        }
    }
}
