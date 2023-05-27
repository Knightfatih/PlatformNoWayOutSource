using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    Ragdoll ragdoll;
    AIControl aIControl;
    UIEnemyHealth enemyHealthUI;

    protected override void OnStart()
    {
        ragdoll = GetComponent<Ragdoll>();
        aIControl = GetComponent<AIControl>();
        enemyHealthUI = GetComponentInChildren<UIEnemyHealth>();
    }
    protected override void OnDeath(Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
        direction.y = 1;
        ragdoll.ApplyForce(direction * dieForce);
        enemyHealthUI.gameObject.SetActive(false);
        aIControl.enemyDead = true;
        gameObject.tag = ("RagdollDead");
        AudioManager.instance.PlaySound("Enemy Death", transform.position);

        Destroy(gameObject, 30f);
    }
    protected override void OnDamage(Vector3 direction)
    {
        enemyHealthUI.SetHealthBarPercentage(currentHealth / maxHealth);
    }
}
