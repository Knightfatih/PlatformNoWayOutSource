using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReload : MonoBehaviour
{
    public Animator rig;
    public WeaponAnimEvent animationEvent;
    public WeaponActivate weaponActivate;
    public Transform leftHand;
    public UIAmmo uIAmmo;
    public bool isReloading;

    GameObject HandMag;

    // Start is called before the first frame update
    void Start()
    {
        animationEvent.WeaponAnimEvnt.AddListener(OnAnimationEvent);

    }

    // Update is called once per frame
    void Update()
    {
        //AutoReloading
        WeaponRaycast weapon = weaponActivate.GetActiveWeapon();
        if (weapon)
        {
            if(weapon.clipCount > 0)
            {
                isReloading = false;
                if (Input.GetKeyDown(KeyCode.R) || weapon.ShouldReload())
                {
                    isReloading = true;
                    rig.SetTrigger("reload_weapon");
                }
            }
            if (weapon.isFiring)
            {
                uIAmmo.Refresh(weapon.ammoCount, weapon.clipCount);
            }

        }
        
    }

    void OnAnimationEvent(string eventName)
    {
        //Debug.Log(eventName);
        switch (eventName)
        {
            case "detach_mag":
                DetachMag();
                break;
            case "drop_mag":
                DropMag();
                break;
            case "refill_mag":
                RefillMag();
                break;
            case "attach_mag":
                AttachMag();
                break;
        }
    }

    void DetachMag()
    {
        WeaponRaycast weapon = weaponActivate.GetActiveWeapon();
        HandMag = Instantiate(weapon.mag, leftHand, true);
        weapon.mag.SetActive(false);
    }

    void DropMag()
    {
        GameObject dropMag = Instantiate(HandMag, HandMag.transform.position, HandMag.transform.rotation);
        dropMag.AddComponent<Rigidbody>();
        dropMag.AddComponent<BoxCollider>();
        HandMag.SetActive(false);
    }
    
    void RefillMag()
    {
        HandMag.SetActive(true);
    }

    void AttachMag()
    {
        WeaponRaycast weapon = weaponActivate.GetActiveWeapon();
        weapon.mag.SetActive(true);
        Destroy(HandMag);
        weapon.RefillAmmo();
        rig.ResetTrigger("reload_weapon");

        uIAmmo.Refresh(weapon.ammoCount, weapon.clipCount);
        isReloading = false;
    }
}
