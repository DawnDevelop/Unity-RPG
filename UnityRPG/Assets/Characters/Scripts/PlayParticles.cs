using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

    public class PlayParticles : MonoBehaviour
    {

        ParticleSystem[] particles;
        AudioSource audio;
        bool hasPlayed = false;

        private void Start()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
            audio = GetComponentInChildren<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!hasPlayed)
            {

                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Play();
                }
                audio.Play();
                hasPlayed = true;
            }


        }
    }
}
