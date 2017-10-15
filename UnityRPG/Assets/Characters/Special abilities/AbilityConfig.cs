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

        protected AbilityBehavior behavior;

        public abstract AbilityBehavior GetBehaviorComponent(GameObject gameobjectToAttachTo);

        public void AttachAbilityTo(GameObject gameObjectToattachTo)
        {
            AbilityBehavior behaviorComponent = GetBehaviorComponent(gameObjectToattachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
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
        

    }
}