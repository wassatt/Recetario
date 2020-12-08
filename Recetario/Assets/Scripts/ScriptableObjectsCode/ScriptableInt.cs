using UnityEngine;
[CreateAssetMenu(fileName = "s_Int_", menuName = "Variables/s_Int", order = 0)]

public class ScriptableInt : ScriptableObject
{
    public int var;

    public void Set(int value)
    {
        var = value;
    }

    public int Get()
    {
        return var;
    }
}