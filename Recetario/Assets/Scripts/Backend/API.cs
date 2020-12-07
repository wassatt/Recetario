﻿public static class API
{
    private const string urlApi = "https://us-central1-recetario-79536.cloudfunctions.net/api";
    //private const string urlApi = "http://localhost:5000/recetario-79536/us-central1/api";     

    public const string urlGetUser = urlApi + "/user";
    public const string urlPostNewUser = urlApi + "/newUser";
    public const string urlPostUserProfileImage = urlApi + "/uploadUserProfileImage";
    public const string urlUpdateUserName = urlApi + "/updateUserName";
}