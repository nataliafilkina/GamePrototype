using System.Collections.Generic;

namespace EquipmentSystem
{
    public class EquipmentData
    {
        public Dictionary<EquipmentType, string> ItemByType { get; private set; } = new ();

        public EquipmentData() { }

        public EquipmentData(List<EquipmentSlot> equipmentSlots)
        {
            foreach(var slot in equipmentSlots) 
            {
                var itemName = slot.IsEmpty ? "" : slot.ItemInfo.Name;
                ItemByType.Add(slot.Type, itemName);
            }
        }
    }
}
