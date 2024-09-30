using System;

namespace InventorySystem
{
    [Serializable]
    public class InventorySlotData
    {
        public int Amount { get; set; }
        public int Capacity { get; set; }
        public string ItemFileName { get; set; }

        public InventorySlotData() { }

        public InventorySlotData(int amount, int capacity, string itemName)
        {
            Amount = amount;
            Capacity = capacity;
            ItemFileName = itemName;
        }

        public InventorySlotData Copy() => new (Amount, Capacity, ItemFileName);

        public void Clear()
        {
            Amount = 0;
            Capacity = 0;
            ItemFileName = "";
        }
    }
}
