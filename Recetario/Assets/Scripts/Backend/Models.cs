﻿using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public string id;
    public string imageUrl;
    public string name;
    public List<Favorite> listFavorites;

    public UserData() { }
}

[Serializable]
public class Recipe
{
    public string id;
    public string imageUrl;
    public string name;
    public string description;
    public string prepTime;
    public int category;
    public int difficulty;
    public int likes;
    public List<Ingredient> listIngredients;
    public List<Instruction> listInstructions;

    public Recipe() { }
}

[Serializable]
public class Ingredient
{
    public string id;
    public string name;
    public string qty;

    public Ingredient() { }
}

[Serializable]
public class Instruction
{
    public string id;
    public string text;

    public Instruction() { }
}

[Serializable]
public class Favorite
{
    public string recipeId;
    public Favorite() { }
}

[Serializable]
public class Promo
{
    public string id;
    public string imageUrl;
    public string url;
    public Promo() { }
}

public static class Categories
{
    static string[] categories = new string[] 
        { "Aderazos y Salsas", "Arroces", "Carnes", "Ensaladas",
           "Pescados", "Pollos","Postres","Sopas",
           "Vegetales y Guarniciones", "Masas, Panes y Otros", "Dulces", "Galletas" };

    public static string GetCategoryString(int category)
    {
        return categories[category];
    }

    public static string GetCategoryString(CATEGORIES category)
    {
        return categories[(int)category];
    }
}

[Serializable]
public enum CATEGORIES
{
    CAKES,
    CUPCAKES,
    TARTS,
    PIES,
    JELLIES,
    MOUSSE,
    ICING,
    FLAN,
    SAUCES,
    BREAD,
    CANDIES,
    COOKIES
}

[Serializable]
public class PersistantRecipe
{
    public string id;
    public List<PersistantInstruction> listInstructions;

    public PersistantRecipe() { listInstructions = new List<PersistantInstruction>(); }
}

[Serializable]
public class PersistantInstruction
{
    public string id;
    public bool toggleIsOn;

    public PersistantInstruction() { }
}