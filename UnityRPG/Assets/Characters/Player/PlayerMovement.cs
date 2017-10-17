using UnityEngine;
using RPG.CameraUI;
using System.Collections;
using System;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {

        // Temporarily serialized for debugging
        [SerializeField] ParticleSystem criticalHitParticle;
        [SerializeField] AnimationClip strafeLeftClip;
        [SerializeField] AnimationClip strafeRightClip;

        SpecialAbilities abilities;
        EnemyAI enemy = null;
        Character character; 
        WeaponSystem weaponSystem;
        TalkToNPC npc;


        void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();

            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraUI.CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverNPC += OnMouseOverNPC;
        }

        void OnMouseOverNPC(TalkToNPC npc)
        {

        }

        void Update()
        {
            var healthPercantage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercantage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
                ScanForAorDKey();
            }
        }

        private void ScanForAorDKey()
        {
            if(Input.GetKeyDown("w"))
            {
                var animatorOverrideController = GetComponent<Character>().GetOverrideController();
                var animator = GetComponent<Animator>();
                animator.runtimeAnimatorController = animatorOverrideController;
                var currentRotation = animator.bodyRotation;
                animator.bodyRotation = Quaternion.Euler(character.transform.position);
                animatorOverrideController["DEFAULT ATTACK"] = strafeLeftClip;
                animator.SetTrigger("Attack");
                animator.bodyRotation = currentRotation;
            }
            else if(Input.GetKeyDown("s"))
            {
                var animatorOverrideController = GetComponent<Character>().GetOverrideController();
                var animator = GetComponent<Animator>();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController["DEFAULT ATTACK"] = strafeRightClip;
                animator.SetTrigger("Attack");
            }
            else
            {
                return;
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }

        }



        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                weaponSystem.StopAttacking();
                character.SetDestination(destination);
            }
        }


        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while (!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }


    }
}