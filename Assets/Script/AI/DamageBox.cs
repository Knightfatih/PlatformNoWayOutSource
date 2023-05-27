using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    public float amount = 50;
    Vector3 direction;

    public float damageTimer = 0f;
    public float timeBetweenDamage = 10f;

    private void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponent<Health>();
        damageTimer += Time.deltaTime;
        if (damageTimer > timeBetweenDamage && other.gameObject.tag == "Player")
        {
            health.TakeDamage(amount, direction);
            damageTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        damageTimer = 0f;
    }
}
