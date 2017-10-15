using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; //TODO consider re-wiring

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.13f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHight = 2.5f;

        [Header("Movement")]
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1.2f;
        [SerializeField] float movinTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshhold = 1f;

        [Header("NavMeshAgent")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1f;
        [SerializeField] float navMeshStoppingDistance = 3f;
        [SerializeField] float navMeshBaseOffset = 0;

        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody myRigidbody;
        bool isAlive = true;

        float turnAmount;
        float m_ForwardAmount;

        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHight;


            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = audioSourceSpatialBlend;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.runtimeAnimatorController = animatorOverrideController;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshStoppingDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.baseOffset = navMeshBaseOffset;
            navMeshAgent.acceleration = 3f;
        }

        private void Update()
        {
            if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

        public void kill()
        {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPosition)
        {
            navMeshAgent.destination = worldPosition;
        }

        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);

            ApplyExtraTurnRotation();
            // send input and other state parameters to the animator
            UpdateAnimator();
        }

        void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction.
            if (movement.magnitude > moveThreshhold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            m_ForwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            // update the animator parameters
            animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = moveSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movinTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }


        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                navMeshAgent.SetDestination(enemy.transform.position);
            }
        }

        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                velocity.y = myRigidbody.velocity.y;
                myRigidbody.velocity = velocity;
            }
        }


    }
}