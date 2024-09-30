using System;
using UnityEngine;

namespace StatusEffectSystem
{
    [Serializable]
    public struct ProbabilityEffect
    {
        [SerializeField]
        public ScriptableEffect effect;

        [SerializeField]
        public int probability;
    }
}
