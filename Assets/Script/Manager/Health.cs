using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    SkinnedMeshRenderer skinnedMeshRenderer;

    public float blinkIntesity;
    public float blinkDuration;
    public bool playerDead = false;
    public float dieForce;
    float blinkTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in rbs)
        {
            HitBox hitBox = rb.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }

        OnStart();
    }
    internal void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHeal(amount);
        
        blinkTimer = blinkDuration;
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        OnDamage(direction);
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }

        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction)
    {
        OnDeath(direction);
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntesity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }

    protected virtual void OnStart()
    {

    }
    protected virtual void OnDeath(Vector3 direction)
    {

    }
    protected virtual void OnDamage(Vector3 direction)
    {

    }

    protected virtual void OnHeal(float amount)
    {

    }
}
