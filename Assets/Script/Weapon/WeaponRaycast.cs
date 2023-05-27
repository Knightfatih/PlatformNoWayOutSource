using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRaycast : MonoBehaviour
{
    public WeaponActivate.WeaponParents weaponParents;
    public ParticleSystem muzzleSystem;
    public ParticleSystem hitEffect;
    //
    public ParticleSystem zombieHitEffect;
    //
    public TrailRenderer bulletEffect;
    public Transform raycastPoint;
    public WeaponRecoil recoil;
    public GameObject mag;

    public string weaponName;
    public bool isFiring = false;
    public int fireRate = 25;
    public int ammoCount = 30;
    public int clipSize = 30;
    public int clipCount = 2;
    public float damage = 10;
    float accumulatedTime;

    Ray ray;
    RaycastHit rayHit;


    private void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }

    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0.0f;
        recoil.Reset();
    }

    public void UpdateFiring(float deltaTime, Vector3 target)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate; 
        while(accumulatedTime >= 0.0f)
        {
            FireBullet(target);
            accumulatedTime -= fireInterval;
        }
    }

    private void FireBullet(Vector3 target)
    {
        if (!PauseMenu.gameIsPaused)
        {

            if(ammoCount <= 0)
            {
                return;
            }
            ammoCount--;

            muzzleSystem.Emit(1);

            ray.origin = raycastPoint.position;
            ray.direction = target - raycastPoint.position;

            var tracer = Instantiate(bulletEffect, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);

            if (Physics.Raycast(ray, out rayHit))
            {
                hitEffect.transform.position = rayHit.point;
                hitEffect.transform.forward = rayHit.normal;
                hitEffect.Emit(1);
                AudioManager.instance.PlaySound("Impact", transform.position);

                tracer.transform.position = rayHit.point;

                var rb = rayHit.collider.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.AddForceAtPosition(ray.direction * 20, rayHit.point, ForceMode.Impulse);
                }


                if (rayHit.collider != null && rayHit.transform.tag == "Ragdoll")
                {
                    //
                    zombieHitEffect.transform.position = rayHit.point;
                    zombieHitEffect.transform.forward = rayHit.normal;
                    zombieHitEffect.Emit(40);
                    //
                    var hitBox = rayHit.collider.GetComponent<HitBox>();
                    var capsule = rayHit.collider.GetComponent<CapsuleCollider>();
                    var box = rayHit.collider.GetComponent<BoxCollider>();
                    var sphere = rayHit.collider.GetComponent<SphereCollider>();

                    if (capsule)
                    {
                        hitBox.OnRaycastHitCapsule(this, ray.direction);
                    }
                    if (box)
                    {
                        hitBox.OnRaycastHitBox(this, ray.direction);
                    }
                    if (sphere)
                    {
                        hitBox.OnRaycastHitSphere(this, ray.direction);
                    }
                }
                recoil.GenerateRecoil(weaponName);
            }
        }
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public bool ShouldReload()
    {
        return ammoCount == 0 && clipCount > 0;
    }

    public bool IsLowAmmo()
    {
        return ammoCount == 0 && clipCount == 0;
    }

    public void RefillAmmo()
    {
        ammoCount = clipSize;
        clipCount--;
    }
}
