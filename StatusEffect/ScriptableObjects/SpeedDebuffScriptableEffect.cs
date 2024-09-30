using UnityEngine;

namespace StatusEffectSystem
{
    [CreateAssetMenu(fileName = "SpeedDebuff", menuName = "Gameplay/StatusEffect/Debuffs/SpeedDebuff")]

    public class SpeedDebuffScriptableEffect : ScriptableEffect
    {
        [field: Header("Speed debuff settings")]

        [field: SerializeField]
        public float Decrease { get; private set; }

        public override StatusEffect InitializeEffect(IChangingStat source, GameObject entity)
        {
            return new SpeedDebuff(source, this, entity);
        }
    }
}