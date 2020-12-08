using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "s_UserData", menuName = "Data/s_UserData", order = 0)]

public class ScriptableUserData : ScriptableObject
{
    [SerializeField]
    private UserData userData;

    public void Set(UserData _userData)
    {
        userData = _userData;
    }

    public UserData Get()
    {
        return userData;
    }
}