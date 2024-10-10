using StatusEffectSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Weapon
{
    [CreateAssetMenu(fileName = "ForceWeaponSO", menuName = "Gameplay/Weapon/Force")]
    public class ForceWeaponSO : ScriptableObject, IForceWeaponInfo
    {
        #region SerializeField
        [field: SerializeField]
        public SchoolOfMagic SchoolOfMagic { get; set; }

        [field: SerializeField]
        public float ProjectileSpeed { get; set; }

        [field: SerializeField]
        public float Damage { get; set; }

        [field: SerializeField]
        public float RateOfFire { get; set; }

        [field: SerializeField]
        public int ExpendEnergy { get; set; }

        [field: SerializeField]
        public float AttackRange { get; set; }

        [field: SerializeField]
        private List<ProbabilityEffect> _effects;
        #endregion

        public ReadOnlyCollection<ProbabilityEffect> Effects => _effects.AsReadOnly();

        public float CurrentDamage { get; private set; } = 0;

        private void OnEnable() => CurrentDamage += Damage;
    }
}

