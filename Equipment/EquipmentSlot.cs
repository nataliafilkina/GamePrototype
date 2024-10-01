

using System;
using UnityEngine;

namespace EquipmentSystem
{
    public class EquipmentSlot
    {
        public event Action OnChanged;

        public readonly EquipmentType Type;
        public ItemDefaultDataSO ItemInfo { get; set; }
        public bool IsEmpty => ItemInfo == null;

        public int Capacity { get; private set; } = 1;

        private string _pathToItemData = "ScriptableObjects/Items/";

        public EquipmentSlot(EquipmentType type) => Type = type;

        public EquipmentSlot(EquipmentType type, string itemName)
        {
            ItemInfo = Resources.Load(_pathToItemData + itemName) as ItemDefaultDataSO;
            Type = type;
        }

        public void SetItem(ItemDefaultDataSO item)
        {
            ItemInfo = item;
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            ItemInfo = null;
            OnChanged?.Invoke();
        }
    }
}
