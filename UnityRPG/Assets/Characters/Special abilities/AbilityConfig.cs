using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip[] audioClip = null;
        [SerializeField] AnimationClip animationClip;
        [Header("ADVANCED")]
        [SerializeField] float hitDelay = 1f;
        [SerializeField] float abilityDelay = 1f;
        [SerializeField] bool isConeAbility = false;
        [SerializeField] Vector3 abilityLength;
        [SerializeField] bool instantiateOnPlayer;

        protected AbilityBehaviour behavior;

        public abstract AbilityBehaviour GetBehaviorComponent(GameObject gameobjectToAttachTo);

        public void AttachAbilityTo(GameObject gameObjectToattachTo)
        {
            AbilityBehaviour behaviorComponent = GetBehaviorComponent(gameObjectToattachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public Vector3 GetAbilityLength()
        {
            return abilityLength;
        }
        public void Use(GameObject target)
        {
            behavior.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClip[Random.Range(0, audioClip.Length)];
        }

        public AnimationClip GetAnimationClip()
        {
            animationClip.events = new AnimationEvent[0];
            return animationClip;
        }

        public float GetHitDelay()
        {
            return hitDelay;
        }

        public bool GetIsConeAbility()
        {
            return isConeAbility;
        }

        public float GetAbilityDelay()
        {
            return abilityDelay;
        }
        public bool GetInstantiateOnPlayer()
        {
            return instantiateOnPlayer;
        }
        

    }
}