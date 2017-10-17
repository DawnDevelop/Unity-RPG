using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float damage = 5f;
        [SerializeField] bool destryOnImpact = true;
        const float DESTROY_DELAY = 0.01f;
        float damageCaused;
        HealthSystem healthSystem;

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }


        private void Update()
        {
            Destroy(gameObject, 5f);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerMovement>())
            {
                healthSystem = other.GetComponent<HealthSystem>();
                healthSystem.TakeDamage(damage);
                if(destryOnImpact)
                {
                    Destroy(gameObject);
                }
                
            }

        }
    }
}