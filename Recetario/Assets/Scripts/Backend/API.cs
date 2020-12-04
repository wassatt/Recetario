public static class API
{
    private const string urlApi = "https://us-central1-pocketmenu-385db.cloudfunctions.net/api";
    //private const string urlApi = "http://localhost:5000/pocketmenu-385db/us-central1/api";     

    public const string urlGetUser = urlApi + "/user";
    public const string urlPostNewUser = urlApi + "/newUser";
    public const string urlPostUserProfileImage = urlApi + "/uploadUserProfileImage";
    public const string urlUpdateUserName = urlApi + "/updateUserName";
}
