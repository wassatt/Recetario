using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class UserData
{
    public string id;
    public string imageUrl;
    public string name;

    public UserData() { }
}


[SerializeField]
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
    public List<Ingredient> ingredients;

    public Recipe() { }
}

[SerializeField]
public class Ingredient
{
    public string id;
    public int category;
    public string imgUrl;
    public string name;
    public string description;
    public string price;

    public Ingredient() { }
}

public static class Categories
{
    static string[] categories = new string[] { "Categoría no seleccionada", "Árabe", "Asiática", "China",
        "Comida rápida", "Hamburguesas", "Internacional","Italiana","Mexicana","Pescados y mariscos",
        "Pizza", "Saludable", "Sandwiches", "Sushi", "Tacos", "Vegetariano" };

    public static string GetCategoryString(int category)
    {
        return categories[category];
    }

    public static string GetCategoryString(CATEGORIES category)
    {
        return categories[(int)category];
    }
}

[SerializeField]
public enum CATEGORIES
{
    NONE,
    ARAB,
    ASIAN,
    CHINA,
    FASTFOOD,
    HAMBURGER,
    INTERNATIONAL,
    ITALIAN,
    MEXI,
    FISH,
    PIZZA,
    HEALTHY,
    SANDWICH,
    SUSHI,
    TACO,
    VEGGIE
}