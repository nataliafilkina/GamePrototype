using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Upgrade
{
    [CreateAssetMenu(fileName = "ImprovementGroup", menuName = "Gameplay/Upgrade/ImprovementGroup/ImprovementGroup")]
    public class ImprovementGroupSO : ScriptableObject
    {
        [field: SerializeField]
        private List<ForceImprovementSO> _improvementsInfo = new();

        public ReadOnlyCollection<ForceImprovementSO> ImprovementsInfo;

        private void OnEnable()
        {
            ImprovementsInfo = _improvementsInfo.AsReadOnly();
        }
    }
}
