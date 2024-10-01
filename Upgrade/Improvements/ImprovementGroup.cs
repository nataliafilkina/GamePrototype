using Player;
using System.Collections.Generic;
using Weapon;

namespace Upgrade
{
    public class ImprovementGroup
    {
        private List<Improvement> _improvements = new();

        public int Level { get; private set; }
        public readonly string Id;

        private bool IsMaxLevel = false;

        public ImprovementGroup(string id, Forces target, ImprovementGroupSO improvementsInfo, int level, Experience experience)
        {
            Level = level;
            Id = id;

            foreach (var improvement in improvementsInfo.ImprovementsInfo)
            {
                _improvements.Add(improvement.Instatiate(target, experience));
            }

            ApplyLoad();
        }

        public ForceImprovementSO GetNextLevelInfo()
        {
            return IsMaxLevel ? _improvements[Level - 1].Info
                              : _improvements[Level].Info;
        }

        private void ApplyLoad()
        {
            if(Level > 0)
                _improvements[Level - 1].Apply();

            if (Level == _improvements.Count)
                IsMaxLevel = true;
        }

        public void Apply()
        {
            if (IsMaxLevel)
                return;

            _improvements[Level].Apply();
            Level += 1;

            if (Level == _improvements.Count)
                IsMaxLevel= true;
        }

        public bool IsLevelupAvailable()
        {
            return !IsMaxLevel && _improvements[Level].IsAvailable;
        }
    }
}
