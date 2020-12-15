using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UiManagerEditRecipe : MonoBehaviour
{
    public Recipe recipe;

    [SerializeField]
    private Image imgDish;
    [SerializeField]
    private InputField ifName;
    [SerializeField]
    private InputField ifDescription;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (!string.IsNullOrEmpty(recipe.imageUrl))
            StartCoroutine(GetImageCoroutine(recipe.imageUrl, imgDish));

        ifName.text = recipe.name;
        ifDescription.text = recipe.description;
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
