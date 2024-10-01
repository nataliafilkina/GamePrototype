using StorageService;
using System;
using System.Collections.Generic;

namespace EquipmentSystem
{
    public class Equipment
    {
        public Action OnSaveNew;

        private List<EquipmentSlot> _equipmentSlots = new();
        private IStorageService _storageService;
        private readonly string _key = "Equipment";

        public Equipment(IStorageService storageService)
        {
            _storageService = storageService;
            Load();
        }

        private void Load() 
        {
            _storageService.Load<EquipmentData>(_key, data =>
            {
                if (data != null)
                {
                    ParseSaveData(data);
                }
            });
        }

        public void Save()
        {
            var saveData = new EquipmentData(_equipmentSlots);
            _storageService.Save(_key, saveData);

            OnSaveNew?.Invoke();
        }

        private void ParseSaveData(EquipmentData data)
        {
            foreach(var type in data.ItemByType)
            {
                var slot = new EquipmentSlot(type.Key, type.Value);
                _equipmentSlots.Add(slot);
            }
        }

        public void SetSlots(List<UIEquipmentSlot> UISlots) 
        {
            if (_equipmentSlots.Count > 0)
            {
                for (int i = 0; i < _equipmentSlots.Count; i++)
                {
                    UISlots[i].Slot = _equipmentSlots[i];
                }
            }
            else
                InitSlots(UISlots);
        }

        private void InitSlots(List<UIEquipmentSlot> UISlots)
        {
            foreach (var uiSlot in UISlots)
            {
                var slot = new EquipmentSlot(uiSlot.equipmentType);
                _equipmentSlots.Add(slot);
                uiSlot.Slot = slot;
            }
        }

        public EquipmentSlot GetSlotByType(EquipmentType type)
        {
            return _equipmentSlots?.Find(slot => slot.Type == type);
        }
    }
}
