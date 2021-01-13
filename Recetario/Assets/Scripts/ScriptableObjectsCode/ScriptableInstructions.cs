using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "s_InstructionsData", menuName = "Data/s_Instructions", order = 1)]

public class ScriptableInstructions : ScriptableObject
{
    public List<PersistantRecipe> persistantRecipes;
}