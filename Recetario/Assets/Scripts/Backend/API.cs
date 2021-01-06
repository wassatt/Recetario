public static class API
{
    private const string urlApi = "https://us-central1-recetario-79536.cloudfunctions.net/api";
    //private const string urlApi = "http://localhost:5000/recetario-79536/us-central1/api";     

    public const string urlGetUser = urlApi + "/user";
    public const string urlPostNewUser = urlApi + "/newUser";
    public const string urlPostUserProfileImage = urlApi + "/uploadUserProfileImage";
    public const string urlUpdateUserName = urlApi + "/updateUserName";

    public const string urlGetRecipes = urlApi + "/recipes";
    public const string urlGetRecipe = urlApi + "/recipe";
    public const string urlPostNewRecipe = urlApi + "/newRecipe";
    public const string urlPostRecipeImage = urlApi + "/uploadRecipeImage";
    public const string urlUpdateRecipe = urlApi + "/updateRecipe";
    public const string urlLikeRecipe = urlApi + "/likeRecipe";
    public const string urlGetLikes = urlApi + "/likesCount";
    public const string urlDeleteRecipe = urlApi + "/deleteRecipe";
    
    public const string urlGetIngredients = urlApi + "/ingredients";
    public const string urlPostNewIngredient = urlApi + "/newIngredient";
    public const string urlUpdateIngredient = urlApi + "/updateIngredient";
    public const string urlDeleteIngredient = urlApi + "/deleteIngredient";

    public const string urlGetInstructions = urlApi + "/instructions";
    public const string urlPostNewInstruction = urlApi + "/newInstruction";
    public const string urlUpdateInstruction = urlApi + "/updateInstruction";
    public const string urlDeleteInstruction = urlApi + "/deleteInstruction";

    public const string urlGetFavorites = urlApi + "/favorites";
    public const string urlAddFavorite = urlApi + "/newFavorite";
    public const string urlDeleteFavorite = urlApi + "/deleteFavorite";

    public const string urlGetRecipesSearch = urlApi + "/search";

    public const string urlGetCart = urlApi + "/cart";
    public const string urlAddToCart = urlApi + "/newCartItem";
    public const string urlDeleteCartItem = urlApi + "/deleteCartItem";
}
