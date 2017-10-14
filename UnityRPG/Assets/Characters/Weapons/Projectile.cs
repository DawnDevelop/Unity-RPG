using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed;
        GameObject shooter; //For debug purposes

        const float DESTROY_DELAY = 0.005f;
        float damageCaused;

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        private void Start()
        {
        }

        void OnCollisionEnter(Collision collision)
        {
            //

            int layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer)
            {
                //DamageIfDamagable(collision);
            }
        }

        //TODO reimplement
        //private void DamageIfDamagable(Collision collision)
        //{
        //    Component damageableComponent = collision.gameObject.GetComponent(typeof(HealthSystem));
        //    if (damageableComponent)
        //    {
        //        (damageableComponent as IDamageable).TakeDamage(damageCaused);
        //    }
        //    Destroy(gameObject, DESTROY_DELAY);
        //}

        public float GetDefaultLauchSpeed()
        {
            return projectileSpeed;
        }
    }

}