using Stats;
using UnityEngine;

namespace StatusEffectSystem
{
    public class DamageDebuff : StatusEffect
    {
        private DamageDebuffScriptableEffect _effectData;
        private Health _healthComponent;

        public DamageDebuff(IChangingStat source, ScriptableEffect effectData, GameObject target) : base(source, effectData, target)
        {
            _healthComponent = _target.GetComponent<CharacterStats>().Health;
            _effectData = (DamageDebuffScriptableEffect)effectData;
        }

        protected override void ApplyEffect()
        {
            base.ApplyEffect();

            _healthComponent.TakeDamage(_source, _effectData.Damage);
        }

        public override void End()
        {
            base.End();
        }
    }
}
