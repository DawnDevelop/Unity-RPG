using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters 
{
    public class WeaponSystem : MonoBehaviour
    {

        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig = null;
        [Range(0.1f, 1f)] [SerializeField] float criticalHitChance;
        [SerializeField] float criticalHitMultiplier = 1.5f;

        [Header("Projectile wenn vorhanden")]
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] float projSpeed = 1f;
        [SerializeField] Vector3 projRotate;

        GameObject target;
        GameObject weaponObject;
        Animator animator;
        Character character;
        float lastHitTime;
        Projectile projectile;
        Rigidbody rb;
        GameObject player;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            character = GetComponent<Character>();
            player = GameObject.FindGameObjectWithTag("Player");
            animator = GetComponent<Animator>();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        public void StopAttacking()
        {
            animator.StopPlayback();
            StopAllCoroutines();
        }

        IEnumerator SpawnProjectile()
        {
            if(projectileToUse != null)
            {
                yield return new WaitForSecondsRealtime(0.3f);

                var direction = character.transform.position - projectileSocket.transform.position;

                GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.LookRotation(direction));
                newProjectile.transform.Rotate(direction.x, -90, direction.z);
                Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
                Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
                newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projSpeed;
            }

        }

        private void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;

            if(target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                var targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }

            var characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = (characterHealth <= Mathf.Epsilon);

            if(characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            
            while (attackerStillAlive && targetStillAlive)
            {
                float weaponHitPeriod = currentWeaponConfig.GetTimeBetweenAnimation();
                float timeToWait = weaponHitPeriod * character.GetAnimationSpeedMultiplier();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if(isTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce()
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = currentWeaponConfig.GetDamageDelay();
            SetAttackAnimation();
            StartCoroutine(DamageAfterDelay(damageDelay));

            
        }

        private IEnumerator DamageAfterDelay(float damageDelay)
        {
            StartCoroutine(SpawnProjectile());

            yield return new WaitForSecondsRealtime(damageDelay);

            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); // empty hands
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on," + gameObject.name +"please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on" + gameObject.name + "please remove one");
            return dominantHands[0].gameObject;
        }




        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetTimeBetweenAnimation())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                lastHitTime = Time.time;

            }
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        private float CalculateDamage()
        {
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();
        }


        private void SetAttackAnimation()
        {
            if(!character.GetOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Please proved " + gameObject + "with an override Controller");
            }
            var animatorOverrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();

        }

    }

}

