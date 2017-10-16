﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip[] attackAnimation = null;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float timeBetweenAnimation = .5f;
        public float additionalDamage = 10f; //tobechanged
        [SerializeField] float damageDelay = .5f;

        public float GetTimeBetweenAnimation()
        {
            return timeBetweenAnimation;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
        }

        public GameObject GetWeaponPrefab() { return weaponPrefab; }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation[Random.Range(0, attackAnimation.Length)];
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        //So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            for (int i = 0; i < attackAnimation.Length; i++)
            {
                attackAnimation[i].events = new AnimationEvent[0];
            }
            
        }
    }
}

