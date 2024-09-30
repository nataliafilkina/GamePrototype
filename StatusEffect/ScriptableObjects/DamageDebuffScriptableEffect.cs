using UnityEngine;

namespace StatusEffectSystem
{
    [CreateAssetMenu(fileName = "DamageDebuff", menuName = "Gameplay/StatusEffect/Debuffs/DamageDebuff")]

    public class DamageDebuffScriptableEffect : ScriptableEffect
    {
        [field: Header("Health debuff settings")]

        [field: SerializeField]
        public float Damage { get; private set; }

        public override StatusEffect InitializeEffect(IChangingStat source, GameObject entity)
        {
            return new DamageDebuff(source, this, entity);
        }
    }
}