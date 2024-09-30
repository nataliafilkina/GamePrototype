using System;
using UnityEngine;

namespace InventorySystem
{
    public class InventorySlot : IInventorySlot
    {
        #region Fields

        public event Action OnChanged;

        public bool IsFull => !IsEmpty && _data.Amount == _data.Capacity;
        public bool IsEmpty => ItemInfo == null;

        public ItemDefaultDataSO ItemInfo { get; private set; }

        private InventorySlotData _data = new();
        public InventorySlotData Data => _data.Copy();

        private string _pathToItemData = "ScriptableObjects/Items/";

        #endregion


        public InventorySlot() { }

        public InventorySlot(InventorySlotData data) 
        {
            _data = data.Copy();
            ItemInfo = Resources.Load(_pathToItemData + data.ItemFileName) as ItemDefaultDataSO;
        }

        public int SetOrAddItem(ItemDefaultDataSO itemInfo, int amount)
        {
            if(IsEmpty)
            {
                //ItemID = itemInfo.Id;
                ItemInfo = itemInfo;
                _data.Capacity = itemInfo.MaxStackItem;
                _data.ItemFileName = itemInfo.FileName;
            }

            var spaceAvailable = _data.Capacity - _data.Amount;
            var leftAfterAdding = (spaceAvailable >= amount) ? 0 : amount - spaceAvailable;

            _data.Amount += amount - leftAfterAdding;

            OnChanged?.Invoke();

            return leftAfterAdding;
        }

        public void Clear()
        {
            if (IsEmpty)
                return;

            //ItemID = string.Empty;
            ItemInfo = null;
            _data.Clear();

            OnChanged?.Invoke();
        }

        public int RemoveItem(int amount)
        {
            int removed = (amount > _data.Amount) ? _data.Amount : amount;

            if (amount >= _data.Amount)
                Clear();
            else
                _data.Amount -= amount;

            OnChanged?.Invoke();

            return removed;
        }
    }
}
