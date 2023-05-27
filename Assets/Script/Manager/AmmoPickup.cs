using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int clipAmount = 2;

    private void OnTriggerEnter(Collider other)
    {
        WeaponActivate weaponActivate = other.GetComponent<WeaponActivate>();

        if (weaponActivate != null)
        {
            var weapon = weaponActivate.GetActiveWeapon();
            if (weapon)
            {
                weaponActivate.RefillAmmo(clipAmount);
                gameObject.SetActive(false);
            }
        }
    }
}
