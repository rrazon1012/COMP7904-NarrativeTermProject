using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions {
    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Extension method to find the direction between two points
    /// </summary>
    public static Vector3 DirectionTo(this Transform transform, Vector3 target) {
        return (target - transform.position).normalized;
    }

    public static Vector3 DirectionFrom(this Transform transform, Vector3 target) {
        return (transform.position - target).normalized;
    }

    public static void LookTowards(this Transform transform, Vector3 target, float turnSpeed = 1f) {
        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
    }
}
