using UnityEngine;

namespace StatusEffectSystem
{
    [CreateAssetMenu(fileName = "FreezeDebuff", menuName = "Gameplay/StatusEffect/Debuffs/FreezeDebuff")]

    public class FreezeDebuffScriptableEffect : ScriptableEffect
    {
        public override StatusEffect InitializeEffect(IChangingStat source, GameObject entity)
        {
            return new FreezeDebuff(source, this, entity);
        }
    }
}
