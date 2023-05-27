using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public WeaponRaycast weaponFab;

    private void OnTriggerEnter(Collider other)
    {
        WeaponActivate weaponActivate = other.gameObject.GetComponent<WeaponActivate>();
        if (weaponActivate)
        {
            WeaponRaycast newWeapon = Instantiate(weaponFab);
            weaponActivate.Equip(newWeapon);

            gameObject.SetActive(false);
        }

    }
}
