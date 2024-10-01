using UnityEngine;

namespace UICommon.Colors
{
    public class IndicatorColors : MonoBehaviour
    {
        [field: SerializeField]
        public Color IceSchoolDamageIndicator { get; private set; }

        [field:SerializeField]
        public Color FireSchoolDamageIndicator { get; private set; }

        [field: SerializeField]
        public Color ElectricitySchoolDamageIndicator { get; private set; }

        public Color GetColorBySchoolOfMagic(SchoolOfMagic schoolOfMagic)
        {
            return schoolOfMagic switch
            {
                SchoolOfMagic.ice => IceSchoolDamageIndicator,
                SchoolOfMagic.fire => FireSchoolDamageIndicator,
                SchoolOfMagic.electricity => ElectricitySchoolDamageIndicator,
                _ => Color.black,
            };
        }
    }
}
