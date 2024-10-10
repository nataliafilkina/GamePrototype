using StatusEffectSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Upgrade;

namespace Weapon
{
    public class ForceStats : IForceWeaponInfo, IUpgradable
    {
        public SchoolOfMagic SchoolOfMagic { get; private set; }

        public float ProjectileSpeed { get; set; }

        public float Damage { get; set; }

        public float RateOfFire { get; set; }

        public int ExpendEnergy { get; set; }

        public float AttackRange { get; set; }

        public ReadOnlyCollection<ProbabilityEffect> Effects => _effects.AsReadOnly();

        private List<ProbabilityEffect> _effects;

        public void Set(IForceWeaponInfo defaultData)
        {
            SchoolOfMagic = defaultData.SchoolOfMagic;
            ProjectileSpeed = defaultData.ProjectileSpeed;
            Damage = defaultData.Damage;
            RateOfFire = defaultData.RateOfFire;
            ExpendEnergy = defaultData.ExpendEnergy;
            AttackRange = defaultData.AttackRange;
            _effects = new List<ProbabilityEffect>(defaultData.Effects);
            //Load
        }

        public void DoUpgrade(Improvement improvement)
        {
            throw new System.NotImplementedException();
        }
    }
}
