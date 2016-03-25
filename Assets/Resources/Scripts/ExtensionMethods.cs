using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{
    #region Vector3
    public static void SetX(this Vector3 thisVector3, float value)
    {
        thisVector3.Set(value, thisVector3.y, thisVector3.z); 
    }
    public static void SetY(this Vector3 thisVector3, float value)
    {
        thisVector3.Set(thisVector3.x, value, thisVector3.z);
    }
    public static void SetZ(this Vector3 thisVector3, float value)
    {
        thisVector3.Set(thisVector3.x, thisVector3.y, value);
    }

    //public static void AddX(this Vector3 thisVector3, float value)
    //{
    //    thisVector3 = new Vector3(thisVector3.x + value, thisVector3.y, thisVector3.z);
    //}
    //public static void AddY(this Vector3 thisVector3, float value)
    //{
    //    thisVector3 = new Vector3(thisVector3.x, thisVector3.y + value, thisVector3.z);
    //}
    //public static void AddZ(this Vector3 thisVector3, float value)
    //{
    //    thisVector3 = new Vector3(thisVector3.x, thisVector3.y, thisVector3.z + value);
    //}

    //public static void MultiplyX(this Vector3 thisVector3, float factor)
    //{
    //    thisVector3 = new Vector3(thisVector3.x * factor, thisVector3.y, thisVector3.z);
    //}
    //public static void MultiplyY(this Vector3 thisVector3, float factor)
    //{
    //    thisVector3 = new Vector3(thisVector3.x, thisVector3.y * factor, thisVector3.z);
    //}
    //public static void MultiplyZ(this Vector3 thisVector3, float factor)
    //{
    //    thisVector3 = new Vector3(thisVector3.x, thisVector3.y, thisVector3.z * factor);
    //}

    //public static void DivideX(this Vector3 thisVector3, float factor)
    //{
    //    thisVector3 = new Vector3(thisVector3.x / factor, thisVector3.y, thisVector3.z);
    //}
    //public static void DivideY(this Vector3 thisVector3, float factor)
    //{
    //    thisVector3 = new Vector3(thisVector3.x, thisVector3.y / factor, thisVector3.z);
    //}
    //public static void DivideZ(this Vector3 thisVector3, float factor)
    //{
    //    thisVector3 = new Vector3(thisVector3.x, thisVector3.y, thisVector3.z / factor);
    //}

    #endregion

    #region Transform
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }
    #endregion

    #region GameObject
    

    #endregion

    #region AnimationCurve
    public static float Duration(this AnimationCurve curve)
    {
        return curve.keys[curve.keys.Length - 1].time;
    }

    public static float StartValue(this AnimationCurve curve)
    {        
        return (curve.keys.Length > 0) ? curve.keys[0].value : 0;
    }

    #endregion


}
