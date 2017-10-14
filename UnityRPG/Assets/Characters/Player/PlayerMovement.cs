using UnityEngine;
using UnityEngine.Assertions;

// TODO consider re-wire...
using RPG.CameraUI;
using RPG.Weapons;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {


        [SerializeField] AnimatorOverrideController animatorOverrideController = null;

        [Range(0.1f, 1f)] [SerializeField] float criticalHitChance;
        [SerializeField] float criticalHitMultiplier = 1.5f;
        // Temporarily serialized for dubbing

        [SerializeField] ParticleSystem criticalHitParticle;


        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        SpecialAbilities abilities;
        Enemy enemy = null;
        Character character;
        Animator animator = null;
        CameraRaycaster cameraRaycaster = null;
        float lastHitTime = 0;
        GameObject weaponObject = null;
        WeaponSystem weaponSystem;



        void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();

            RegisterForMouseEvents();

        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraUI.CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }



        void Update()
        {
            var healthPercantage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercantage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
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
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }


    }
}