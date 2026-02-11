using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITEM_OBJ : MonoBehaviour
{
    [SerializeField]
    private float fireAngle = 45.0f;
    [SerializeField]
    private float gravity = 9.8f;

    IEnumerator CoSimulateProjectile(Vector3 pos)
    {
        float targetDist = Vector3.Distance(transform.position, pos);

        float projectileVelocity = targetDist / (Mathf.Sin(2 * fireAngle * Mathf.Deg2Rad) / gravity);
    }
}
