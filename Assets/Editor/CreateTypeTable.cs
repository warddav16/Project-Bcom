using UnityEngine;
using UnityEditor;

public class CreateTypeTable
{
    [MenuItem("Assets/Create/Type")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<TypeMap>();
    }
}
