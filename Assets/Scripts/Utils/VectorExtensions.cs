using UnityEngine;
using System.Collections;

public static class VectorExtensions
{
    public static Vector2 ToVec2XZ(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    public static Vector3 UpscaleXZ(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }
}
