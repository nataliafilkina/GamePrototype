#if UNITY_EDITOR
using InspectorDraw;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace StatusEffectSystem
{
    public abstract class ScriptableEffect : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }


        [field: SerializeField]
        public Material Material { get; private set; }


        [field: SerializeField]
        public float Duration { get; private set; }


        [field: SerializeField]
        public bool IsPeriodic { get; private set; }


        [field: SerializeField]
        private List<ScriptableEffect> IncompatibleEffects { get; set; }
        public IReadOnlyList<ScriptableEffect> EffectsNeedCancel => IncompatibleEffects.AsReadOnly();

#if UNITY_EDITOR
        [field: DrawIf("IsPeriodic", true)]
#endif
        [field: SerializeField]
        public float Period { get; private set; }


        [field: Header("Action for multiple instances")]


        [field: SerializeField]
        public bool IsUpdate { get; private set; } = true;

#if UNITY_EDITOR
        [field: DrawIf("IsUpdate", false)]
#endif
        [field: SerializeField]
        public bool IsDurationStacked { get; private set; }

#if UNITY_EDITOR
        [field: DrawIf("IsUpdate", false)]
#endif
        [field: SerializeField]
        public bool IsEffectStacked { get; private set; }



        private void OnValidate()
        {
            Duration = Mathf.Max(Duration, 0);
            Period = Mathf.Clamp(Period, 1f, Duration);
        }

        public abstract StatusEffect InitializeEffect(IChangingStat source, GameObject target);
    }
}
