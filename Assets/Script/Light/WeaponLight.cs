using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLight : MonoBehaviour
{
    public GameObject weaponFlashlight;
    private bool on = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !on)
        {
            weaponFlashlight.SetActive(true);
            on = true;
        }
        else if(Input.GetKeyDown(KeyCode.F) && on)
        {
            weaponFlashlight.SetActive(false);
            on = false;
        }
    }
}
