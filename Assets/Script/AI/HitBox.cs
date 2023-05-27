using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    public float capsuleDamage = 10f;
    public float boxDamage = 30f;
    public float sphereDamage = 100f;

    public void OnRaycastHitCapsule(WeaponRaycast weapon, Vector3 direction)
    {
        health.TakeDamage(weapon.damage + capsuleDamage, direction);
        //Debug.Log("limbs");
    }
    public void OnRaycastHitBox(WeaponRaycast weapon, Vector3 direction)
    {
        health.TakeDamage(weapon.damage + boxDamage, direction);
        //Debug.Log("upper");
    }
    public void OnRaycastHitSphere(WeaponRaycast weapon, Vector3 direction)
    {
        health.TakeDamage(weapon.damage + sphereDamage, direction);
        //Debug.Log("head");
    }
}
