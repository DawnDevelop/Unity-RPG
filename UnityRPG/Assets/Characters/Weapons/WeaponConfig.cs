using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float Attackspeed = .5f;
        [SerializeField] float additionalDamage = 10f;

        public float GetAttackspeed()
        {
            return Attackspeed;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab() { return weaponPrefab; }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        //So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}

