using UnityEngine;
[CreateAssetMenu(fileName = "s_Bool_", menuName = "Variables/s_Bool", order = 2)]

public class ScriptableBool : ScriptableObject
{
    public bool var;

    public void Set(bool value)
    {
        var = value;
    }

    public bool Get()
    {
        return var;
    }
}
