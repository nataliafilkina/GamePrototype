using System;

namespace Upgrade
{
    [Serializable]
    public class ImprovementState
    {
        public readonly string Id;
        public int Level;

        public ImprovementState(string id, int level = 0)
        {
            Id = id;
            Level = level;
        }
    }
}
