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

    public void InitUiValues(Recipe recipe)
    {
        if(recipe.difficulty < spritesDiff.Length)
            imgDifficulty.overrideSprite = spritesDiff[recipe.difficulty];

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

        txtName.text = recipe.name;
        txtPrepTime.text = recipe.prepTime;
        txtDescription.text = recipe.description;
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
