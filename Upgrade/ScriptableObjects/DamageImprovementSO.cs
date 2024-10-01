using Player;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

namespace Upgrade
{
    [CreateAssetMenu(fileName = "Improvement", menuName = "Gameplay/Upgrade/Improvement/DamageImprovement")]
    public class DamageImprovementSO : ForceImprovementSO
    {
        [field: SerializeField]
        [field: Range(1, 100)]
        public float DamageIncrease { get; private set; }

        [field: SerializeField]
        public bool IsPersent { get; private set; }

        public override Improvement Instatiate(Forces forceStats, Experience experience)
        {
            return new ForceDamageImprovement(this, forceStats, experience);
        }
    }
}
