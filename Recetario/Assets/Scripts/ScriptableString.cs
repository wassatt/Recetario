using UnityEngine;
[CreateAssetMenu(fileName = "s_String_", menuName = "Variables/s_String", order = 1)]

public class ScriptableString : ScriptableObject
{
    public string var;

    public void Set(string value)
    {
        var = value;
    }

    public string Get()
    {
        return var;
    }
}