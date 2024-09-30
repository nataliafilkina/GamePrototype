using System;

namespace InventorySystem
{
    public interface IInventorySlot
    {
        public event Action OnChanged;

        bool IsFull { get; }
        bool IsEmpty { get; }

        InventorySlotData Data { get; }
        ItemDefaultDataSO ItemInfo { get; }

        int SetOrAddItem(ItemDefaultDataSO itemInfo, int amount);
        int RemoveItem(int amount);
        void Clear();
    }
}
