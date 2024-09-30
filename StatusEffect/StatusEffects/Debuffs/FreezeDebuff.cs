using UnityEngine;

namespace StatusEffectSystem
{
    public class FreezeDebuff : StatusEffect
    {
        private Rigidbody2D _targetRigidbody;

        public FreezeDebuff(IChangingStat source, ScriptableEffect effectData, GameObject target) : base(source, effectData, target)
        {
            _targetRigidbody = target.GetComponent<Rigidbody2D>();
        }

        protected override void ApplyEffect()
        {
            base.ApplyEffect();

            /*var effectableEntity = _target.GetComponent<EffectableEntity>();

            if (effectableEntity.HasEffectByType(typeof(SpeedDebuffScriptableEffect), out var speedEffect))
            {
                effectableEntity.CancelEffect(speedEffect);
            }*/

            _targetRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

        }

        public override void End()
        {
            base.End();

            _targetRigidbody.constraints = RigidbodyConstraints2D.None;
        }
    }
}
