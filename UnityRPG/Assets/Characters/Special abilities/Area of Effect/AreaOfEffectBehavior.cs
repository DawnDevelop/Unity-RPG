using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;


namespace RPG.Characters
{
    public class AreaOfEffectBehavior : AbilityBehaviour
    {
       

        void Start()
        {
            player = GetComponent<PlayerMovement>();
            audioSource = GetComponent<AudioSource>();
            SetUpRuntimeAnim();
        }

        private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e)
        {
            Debug.Log(e.Hit.transform.name); //will print collided object name to the console.
        }

        public void SetConfig(AreaOfEffectConfig configToSet)
        {
            this.config = configToSet;
        }

        public override void Use(GameObject target)
        {
            animController["SpecialAbility1"] = config.GetAnimationClip();
            animator.SetTrigger("AOE1");
            StartCoroutine(DealRadialDamageToEnemy());
            StartCoroutine(PlayParticleEffect());
            PlayAbilitySound();
        }
   


        IEnumerator DealRadialDamageToEnemy()
        {
            yield return new WaitForSeconds(config.GetHitDelay()); //Einstellbar wie lange die verzögerung sein soll


            if(config.GetIsConeAbility() == false)
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
            else if(config.GetIsConeAbility() == true)
            {
                RaycastHit[] hits = Physics.CapsuleCastAll(transform.position,
                    config.GetAbilityLength(),
                    (config as AreaOfEffectConfig).GetRadius(), 
                    Input.mousePosition);

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
    }
}