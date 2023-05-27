using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponActivate : MonoBehaviour
{
    public enum WeaponParents { 
        Primary = 0,
        Secondary = 1
    }

    public Transform crossTarget;
    public Transform[] weaponParents;
    //public Transform weaponLeftGrip;
    //public Transform weaponRightGrip;

    public CharacterAiming characterAiming;
    public UIAmmo uIAmmo;
    public Animator rig;
    public bool isChangingWeapon;
    WeaponRaycast[] equippedWeapons = new WeaponRaycast[2];
    
    int activeWeaponIndex;

    bool isHolstered = false;

    public GameObject guiObject;

    // Start is called before the first frame update
    void Start()
    {
        WeaponRaycast existingWeapon = GetComponentInChildren<WeaponRaycast>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
        guiObject.SetActive(false);
    }

    public bool IsFiring()
    {
        WeaponRaycast currentWeapon = GetActiveWeapon();
        if (!currentWeapon)
        {
            return false;
        }
        return currentWeapon.isFiring;
    }

    public WeaponRaycast GetActiveWeapon()
    {
        return GetWeapon(activeWeaponIndex);
    }

    WeaponRaycast GetWeapon(int index)
    {
        if (index < 0 || index >= equippedWeapons.Length)
        {
            return null;
        }
        return equippedWeapons[index];
    }

    // Update is called once per frame
    void Update()
    {
        var weapon = GetWeapon(activeWeaponIndex);
        bool notRunning = rig.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("notRunning");
        if (weapon && !isHolstered && notRunning)
        {
            guiObject.SetActive(true);
            if (Input.GetButtonDown("Fire1"))
            {
                weapon.StartFiring();
            }
            if (weapon.isFiring)
            {
                weapon.UpdateFiring(Time.deltaTime, crossTarget.position);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.StopFiring();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleActiveWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponParents.Primary);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponParents.Secondary);
        }
    }


    public void Equip(WeaponRaycast newWeapon)
    {
        int weaponParentsIndex = (int)newWeapon.weaponParents;
        var weapon = GetWeapon(weaponParentsIndex);
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.recoil.characterAiming = characterAiming;
        weapon.recoil.rig = rig;

        weapon.transform.SetParent(weaponParents[weaponParentsIndex], false);

        weapon.transform.localPosition = new Vector3(0.1427f, 0.11f, 0.616f);
        weapon.transform.localRotation = Quaternion.identity;
        

        equippedWeapons[weaponParentsIndex] = weapon;

        SetActiveWeapon(newWeapon.weaponParents);

        uIAmmo.Refresh(weapon.ammoCount, weapon.clipCount);
    }

    void ToggleActiveWeapon()
    {
        bool isHolstered = rig.GetBool("holster_weapon");
        if (isHolstered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));
        }
    }

    void SetActiveWeapon(WeaponParents weaponParents)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponParents;

        if(holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }
        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        rig.SetInteger("weapon_index", activateIndex);
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        guiObject.SetActive(false);
        isChangingWeapon = true;
        isHolstered = true;
        var weapon = GetWeapon(index);
        if(weapon)
        {
            rig.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rig.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        isChangingWeapon = false;
    }

    IEnumerator ActivateWeapon(int index)
    {
        isChangingWeapon = true;
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rig.SetBool("holster_weapon", false);
            rig.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rig.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
        }
        isChangingWeapon = false;
    }

    public void DropWeapon()
    {
        var currentWeapon = GetActiveWeapon();
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.gameObject.AddComponent<Rigidbody>();
            equippedWeapons[activeWeaponIndex] = null;
        }
    }

    public void RefillAmmo(int clipCount)
    {
        var weapon = GetActiveWeapon();
        if (weapon)
        {
            weapon.clipCount += clipCount;
            uIAmmo.Refresh(weapon.ammoCount, weapon.clipCount);
        }
    }
}
