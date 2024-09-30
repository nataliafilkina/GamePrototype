using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory
    {
        public event Action<int> OnInventoryChangedInSlotEvent;

        public readonly InventoryItemType ItemType;
        public int Capacity { get; private set; }
        public bool IsFull => _slots.All(slots => slots.IsFull);

        private readonly List<IInventorySlot> _slots;
        public IReadOnlyCollection<IInventorySlot> Slots => _slots.AsReadOnly();


        public Inventory(int capacity, InventoryItemType itemType)
        {
            Capacity = capacity;
            ItemType = itemType;    

            _slots = new List<IInventorySlot>(Capacity);

            for (int i = 0; i < Capacity; ++i)
                _slots.Add(new InventorySlot());
            ItemType = itemType;
        }

        public Inventory(int capacity, InventoryItemType itemType, List<InventorySlotData> slotData)
        {
            Capacity = capacity;
            ItemType = itemType;
            _slots = new List<IInventorySlot>(Capacity);

            foreach(var slot in slotData)
                _slots.Add(new InventorySlot(slot));
        }

        public int AddItem(ItemDefaultDataSO item, int amount)
        {
            var addedAmount = 0;
            var requiredSlot = _slots.Find(slot => !slot.IsEmpty && slot.ItemInfo.Id == item.Id && !slot.IsFull);
            requiredSlot ??= _slots.Find(slot => slot.IsEmpty);

            if (requiredSlot != null)
            {
                var left = AddItemToSlot(requiredSlot, item, amount);
                if (left != 0)
                    addedAmount += AddItem(item, left);
                else
                    addedAmount = amount;
            }
            else
                Debug.Log($"Cannot add item ({item.Id}), amount: {amount}, because there is no place for that.");

            return addedAmount;
        }

        public int AddItemToSlot(IInventorySlot slot, ItemDefaultDataSO item, int amount) 
        {
            var left = slot.SetOrAddItem(item, amount);

            if (left != amount)
                OnInventoryChangedInSlotEvent?.Invoke(_slots.IndexOf(slot));

            return left;
        }

        public int RemoveItem(string itemId, int amount)
        {
            var removedAmount = 0;

            while (removedAmount < amount)
            {
                var slotWithItem = _slots.Find(slot => !slot.IsEmpty && slot.ItemInfo.Id == itemId);

                if (slotWithItem == null)
                    return removedAmount;

                var removed = slotWithItem.RemoveItem(amount - removedAmount);
                removedAmount += removed;
            }

            return removedAmount;
        }

        public int RemoveItemFromSlot(IInventorySlot slot, int amount)
        {
            return slot.RemoveItem(amount);
        }

        public void MoveFromSlotToSlot(IInventorySlot fromSlot, IInventorySlot toSlot, int amount)
        {
            if (toSlot.IsEmpty || fromSlot.ItemInfo.Id == toSlot.ItemInfo.Id)
            {
                int left = toSlot.SetOrAddItem(fromSlot.ItemInfo, amount);
                fromSlot.RemoveItem(amount - left);

                OnInventoryChangedInSlotEvent?.Invoke(_slots.IndexOf(fromSlot));
                OnInventoryChangedInSlotEvent?.Invoke(_slots.IndexOf(toSlot));
            }
        }
    }
}

