using UnityEngine;

namespace Upgrade
{
    [CreateAssetMenu(fileName = "Level", menuName = "Gameplay/Level")]

    public class LevelSO : ScriptableObject
    {
        [field: SerializeField]
        public int Number { get; private set; }

        [field: SerializeField]
        public int NeedExperience { get; private set; }

        [field: SerializeField]
        public int PointsGivesLevel { get; private set; }

        [field: SerializeField]
        public LevelSO NextLevel { get; private set; }
    }
}
