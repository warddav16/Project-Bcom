using UnityEngine;
using System.Collections;

public class TypeMap : ScriptableObject
{
    public TypeMap[] StrongAgainst;
    public TypeMap[] WeakAgainst;

    public bool IsStrongAgainst(TypeMap checkAgainst)
    {
        foreach(var type in StrongAgainst )
        {
            if (type == checkAgainst)
                return true;
        }
        return false;
    }

    public bool IsWeakAgainst(TypeMap checkAgainst)
    {
        foreach (var type in WeakAgainst)
        {
            if (type == checkAgainst)
                return true;
        }
        return false;
    }
}
