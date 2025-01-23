using System;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    public static bool AreAquals(Vector3 oV1, Vector3 oV2, float fEpsilon = 0f)
    {
        return GetDistance(oV1, oV2) <= fEpsilon;
    }

    public static float GetDistance(Vector4 oV1, Vector4 oV2)
    {
        return GetTranslation(oV1, oV2).magnitude;
    }

    public static Vector3 GetTranslation(Vector3 oV1, Vector3 oV2)
    {
        return (oV1 - oV2);
    }

    public static Vector3 GetTranslationTo(GameObject oFrom, GameObject oTo)
    {
        return oTo.transform.position - oFrom.transform.position;
    }

    public static float EaseOutQuad(float start, float end, float value)
    {
        if (value > 1f)
            value = 1f;

        end -= start;
        return -end * value * (value - 2) + start;
    }

    public static bool IsNaN(ref Vector3 oVector)
    {
        return float.IsNaN(oVector.x) || float.IsNaN(oVector.y) || float.IsNaN(oVector.z);
    }

    public class RaycastDistanceComparer : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            return x.distance.CompareTo(y.distance);
        }
    }

    public static RaycastHit[] GetSortedRaycastHit(Vector3 oStart, Vector3 oDirection, float fMaxDistance = float.PositiveInfinity)
    {
        RaycastHit[] oHitsInfo = Physics.RaycastAll(oStart, oDirection, fMaxDistance);

        if (oHitsInfo.Length == 0)
            return null;

        Array.Sort(oHitsInfo, 0, oHitsInfo.Length, new RaycastDistanceComparer());

        return oHitsInfo;
    }
}