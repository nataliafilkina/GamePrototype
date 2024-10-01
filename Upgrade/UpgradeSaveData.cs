using System;
using System.Collections.Generic;

namespace Upgrade
{
    [Serializable]
    public class UpgradeSaveData
    {
        public List<ImprovementState> StateImprovements;

        public UpgradeSaveData(List<ImprovementState> stateImprovements) 
        {
            StateImprovements = stateImprovements;
        }

        public void Refresh(List<ImprovementGroup> improvements)
        {
            foreach(var improvement in improvements) 
            {
                var state = StateImprovements.Find(state => state.Id == improvement.Id);
                if (state != null)
                    state.Level = improvement.Level;
                else
                    StateImprovements.Add(new ImprovementState(improvement.Id, improvement.Level));
            }
        }
    }
}
