using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter; // So can inspected when paused
        [SerializeField] float damage = 5f;

        const float DESTROY_DELAY = 0.01f;
        float damageCaused;
        HealthSystem healthSystem;

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerMovement>())
            {
                healthSystem = other.GetComponent<HealthSystem>();
                healthSystem.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 5f);
            }

        }
    }
}