using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;
        protected Animator animator;
        protected PlayerMovement player;
        protected AudioSource audioSource = null;
        protected AnimatorOverrideController animController;

        const float PARTICLE_DESTROY_DELAY = 10f;

        public abstract void Use(GameObject target = null);

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected IEnumerator PlayParticleEffect()
        {
            yield return new WaitForSeconds(config.GetAbilityDelay()); //Wait a certain amount of time
            
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);

            //Should the object be on the player?
            if (config.GetInstantiateOnPlayer() == true)
            {
                particleObject.transform.parent = gameObject.transform;
            }
            ParticleSystem[] myParticleSystem = particleObject.GetComponentsInChildren<ParticleSystem>();

            particleObject.transform.position = transform.position;
            particleObject.transform.rotation = transform.rotation;

            for (int i = 0; i < myParticleSystem.Length; i++)
            {
                
                myParticleSystem[i].Play();
                Destroy(particleObject, PARTICLE_DESTROY_DELAY);
            }
            
        }

        protected void SetUpRuntimeAnim()
        {
            animator = GetComponent<Animator>();
            animController = player.GetOverrideController();
            animator.runtimeAnimatorController = animController;
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }

    }
}

