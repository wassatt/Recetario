using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerRecipePreview : MonoBehaviour
{
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

    public Sprite[] spritesDiff;

    public void InitUiValues(Recipe recipe)
    {
        if(recipe.difficulty < spritesDiff.Length)
            imgDifficulty.overrideSprite = spritesDiff[recipe.difficulty];

        txtName.text = recipe.name;
        txtPrepTime.text = recipe.prepTime;
        txtDescription.text = recipe.description;
    }
}
