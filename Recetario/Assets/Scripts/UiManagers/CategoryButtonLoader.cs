using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonLoader : MonoBehaviour
{
    public CATEGORIES category;
    [SerializeField]
    private RecipesContentManager recipesContentManager;
    private Button btnRef;

    void Start()
    {
        btnRef = transform.GetComponent<Button>();
        btnRef.onClick.AddListener(delegate { GetRestaurantsByCategory(); });
    }

    void GetRestaurantsByCategory()
    {
        recipesContentManager.GetRecipesByCategory((int)category);
    }
}