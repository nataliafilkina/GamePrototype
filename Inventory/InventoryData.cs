using System.Collections.Generic;

namespace InventorySystem
{
    public class InventoryData
    {
        public int Capacity { get; set; }
        public Dictionary<InventoryItemType, List<InventorySlotData>> SlotDataByItemType { get; private set; } = new();

        public InventoryData() { }

        public InventoryData(int capacity, List<Inventory> inventory)
        {
            Capacity = capacity;
            foreach(var inventoryTab in inventory)
            {
                var slots = inventoryTab.Slots;
                List<InventorySlotData> slotsData = new();

                foreach (var slot in slots)
                    slotsData.Add(slot.Data);

                SlotDataByItemType.Add(inventoryTab.ItemType, slotsData);
            }
        }

        public void Clear()
        { 
            Capacity = 0;
            SlotDataByItemType.Clear(); 
        }
    }
}
