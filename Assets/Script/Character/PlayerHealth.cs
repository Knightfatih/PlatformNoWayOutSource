using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : Health
{
    Ragdoll ragdoll;
    WeaponActivate weaponActivate;
    CharacterAiming characterAiming;
    PostProcessVolume postProcessVolume;
    CharacterControl characterControl;
    public GameObject gameOver;

    protected override void OnStart()
    {
        ragdoll = GetComponent<Ragdoll>();
        weaponActivate = GetComponent<WeaponActivate>();
        characterAiming = GetComponent<CharacterAiming>();
        characterControl = GetComponent<CharacterControl>();
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
    }
    protected override void OnDeath(Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
        weaponActivate.DropWeapon();
        characterAiming.enabled = false;
        characterControl.enabled = false;
        GameOver();
    }
    protected override void OnDamage(Vector3 direction)
    {
        UpdateVignette();
    }
    protected override void OnHeal(float amount)
    {
        UpdateVignette();
    }
    private void UpdateVignette()
    {
        Vignette vignette;
        if (postProcessVolume.profile.TryGetSettings(out vignette))
        {
            float percent = 1.0f - (currentHealth / maxHealth);
            vignette.intensity.value = percent * 1f;
            vignette.smoothness.value = 1f;
        }
    }
    
    void GameOver()
    {
        gameOver.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(UI());
    }
    IEnumerator UI()
    {
        yield return new WaitForSeconds(2);

        Time.timeScale = 0f;
    }
}
