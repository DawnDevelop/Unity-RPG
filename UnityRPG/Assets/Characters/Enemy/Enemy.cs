using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 3f;

        [SerializeField] float FiringPeriodInSeconds = 1f;
        [SerializeField] float FiringPeriodvariation = 0.1f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float projectileDamage = 9f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;

        [SerializeField] Vector3 AimOffset = new Vector3(0, 1f, 0);

        bool isAttacking = false;

        PlayerMovement player = null;

        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        public void TakeDamage(float amount)
        {
            //todo Remove
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = FiringPeriodInSeconds * Random.Range(FiringPeriodInSeconds - FiringPeriodvariation, FiringPeriodInSeconds + FiringPeriodvariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);
            }

            if (distanceToPlayer >= attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                //aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                //aiCharacterControl.SetTarget(transform);
            }

        }

        //TODO seperate out Character firing logic
        void FireProjectile()
        {
            //Setup the damage
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetShooter(gameObject);
            projectileComponent.SetDamage(projectileDamage);
            

            //Work out what the speed is and velocity
            Vector3 unitVectorToPlayer = (player.transform.position + AimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLauchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;

        }

        void OnDrawGizmos()
        {
            //Draw movement

            //Draw attack sphere
            Gizmos.color = new Color(255f, 0, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

            Gizmos.color = new Color(0, 255, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }

}