using UnityEngine;
using Weapon;
using System.Collections.Generic;
using Player;

namespace Upgrade
{
    public abstract class ForceImprovementSO : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        [field: TextArea(15, 20)]
        public string Description { get; private set; }

        [field: SerializeField]
        public int Level { get; private set; }

        [field: SerializeField]
        public int Cost { get; private set; }

        public abstract Improvement Instatiate(Forces forceStats, Experience experience);
    }
}
