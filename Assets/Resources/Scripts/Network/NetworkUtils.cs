using UnityEngine;
using System.Collections;

public static class NetworkUtils
{
    public static bool IsDifferentEnoughTo(this Vector3 me, Vector3 other)
    {
        return (me - other).magnitude > 0.5;
    }

    public static bool IsDifferentEnoughTo(this Quaternion me, Quaternion other)
    {
        return Quaternion.Angle(me, other) > 1;
    }
}

