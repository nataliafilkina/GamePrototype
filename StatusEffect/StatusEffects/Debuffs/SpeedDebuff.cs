using Stats;
using UnityEngine;

namespace StatusEffectSystem
{
    public class SpeedDebuff : StatusEffect
    {
        private SpeedDebuffScriptableEffect _effectData;
        private Speed speedComponent;

        public SpeedDebuff( IChangingStat source, ScriptableEffect effectData, GameObject target) : base(source, effectData, target)
        {
            speedComponent = _target.GetComponent<CharacterStats>().Speed;
            _effectData = (SpeedDebuffScriptableEffect)effectData;
        }

        protected override void ApplyEffect()
        {
            base.ApplyEffect();

            speedComponent.SlowDown(_effectData.Decrease);
        }

        public override void End()
        {
            base.End();

            speedComponent.SpeedUp(_effectData.Decrease, false);
        }
    }
}
