using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ImageUpdater : MonoBehaviour
{
    public Image img;
    [SerializeField]
    private ScriptableString imagePath;
    public string imgName;
    public UnityEvent onImageUpdated;

    public void UpdateImage(string _imgPath, string _imgName)
    {
        Texture2D _texture = NativeGallery.LoadImageAtPath(_imgPath, 1024);
        Sprite sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        imagePath.Set(_imgPath);
        imgName = _imgName;
        img.overrideSprite = sprite;
        onImageUpdated.Invoke();
    }
}
