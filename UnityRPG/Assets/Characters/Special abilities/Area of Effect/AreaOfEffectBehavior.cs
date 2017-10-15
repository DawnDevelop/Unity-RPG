using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaOfEffectBehavior : AbilityBehavior
{

    public override void Use(GameObject target)
    {
        DealRadialDamage();
        PlayParticleEffect();
        PlayAbilityAnimation();
    }

    private void DealRadialDamage()
    {
        // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaOfEffectConfig).GetRadius(),
            Vector3.up,
            (config as AreaOfEffectConfig).GetRadius()
        );

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = (config as AreaOfEffectConfig).GetDamageToEachTarget();
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}