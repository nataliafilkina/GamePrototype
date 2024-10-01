using Player;
using StorageService;
using System.Collections.Generic;
using UI.UIPopup.UIUpgrade;

namespace Upgrade
{
    public class UpgradeTree
    {
        private UpgradeSaveData _data;

        private readonly string key = "UpgradeTree";

        private IStorageService _storageService;
        private Experience _experience;
        private Forces _forces;

        public UpgradeTree(IStorageService storageService, Experience experience, Forces forces)
        {
            _storageService = storageService;
            _experience = experience;
            _forces = forces;
            Load();
        }

        private void Load()
        {
            _storageService.Load<UpgradeSaveData>(key, data =>
            {
                if (data != null)
                {
                    _data = new UpgradeSaveData(data.StateImprovements);
                }
            });
        }

        public void Save() 
        {
            _storageService.Save(key, _data);
        }

        
        public void SetUITree(List<UIImprovement> uiImprovements)
        {
            if (_data == null)
            {
                CreateData(uiImprovements);
                _storageService.Save(key, _data);
                return;
            }

            foreach(var slot in uiImprovements)
            {
                var improvementState = _data.StateImprovements.Find(el => el.Id == slot.Id);
                if(improvementState != null) 
                {
                    SetTreeSlot(slot, improvementState);
                }
            }
        }


        private void CreateData(List<UIImprovement> uiImprovements)
        {
            var improvementsStates = new List<ImprovementState>();

            foreach(var slot in uiImprovements)
            {
                var improvementState = new ImprovementState(slot.Id);
                improvementsStates.Add(improvementState);
                SetTreeSlot(slot, improvementState);
            }

            if(improvementsStates.Count > 0)
                _data = new UpgradeSaveData(improvementsStates);
        }

        private void SetTreeSlot(UIImprovement slot, ImprovementState improvementState)
        {
            var improvementGroup = new ImprovementGroup(slot.Id, _forces, slot.ImprovementGroupSO, improvementState.Level, _experience);
            slot.SetImprovement(improvementGroup);      
        }

        public void LevelUp(List<ImprovementGroup> improvements)
        {
            foreach (var improvement in improvements)
                improvement.Apply();

            _data.Refresh(improvements);
            Save();
        }
    }
}
