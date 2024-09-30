using System;
using System.Collections.Generic;
using StorageService;

namespace InventorySystem
{
    public class InventoryControl
    {
        public int Capacity { get; private set; } = 42;
        public Inventory Inventory { get; private set; }

        private IStorageService _storageService;
        private readonly string _key = "Inventory";

        private List<Inventory> _inventory = new();
        private Inventory _totalTab;
        private Inventory _currentTab;
        public Inventory CurrentTab 
        { 
            get => _currentTab; 
            set
                {
                    if(_inventory.Contains(value))
                        _currentTab = value;
                }
        }

        public InventoryControl(IStorageService storageService, int capacity) 
        {
            _storageService = storageService;
            Capacity = capacity;
            Load();
            _totalTab = _inventory.Find(inv => inv.ItemType == InventoryItemType.Total);
        }

        private void Load()
        {
            _storageService.Load<InventoryData>(_key, data =>
            {
                if (data != null)
                {
                    ParseSaveData(data);
                }
                else
                {
                    foreach(InventoryItemType type in Enum.GetValues(typeof(InventoryItemType)))
                    {
                        if (type != InventoryItemType.NotSuitable)
                            _inventory.Add(new Inventory(Capacity, type));
                    }
                }
            });
        }

        public void Save()
        {
            var saveData = new InventoryData(Capacity, _inventory);
            _storageService.Save(_key, saveData);
        }

        public Inventory GetInventoryByType(InventoryItemType type)
        {
            return _inventory.Find(inventory => inventory.ItemType == type);
        }

        public int AddItem(ItemDefaultDataSO item, int amount)
        {
            var inventory = _inventory.Find(inventory => inventory.ItemType == item.InventoryType);
            var addedAmount = inventory.AddItem(item, amount);

            if (addedAmount != 0 && inventory.ItemType != InventoryItemType.Total)
                _totalTab.AddItem(item, addedAmount);

            return addedAmount;
        }

        public int AddItemToSlot(ItemDefaultDataSO item, int amount, IInventorySlot slot) 
        {
            if(_currentTab.ItemType == InventoryItemType.Total || _currentTab.ItemType == item.InventoryType)
            {
                int added = _currentTab.AddItemToSlot(slot, item, amount);

                if(_currentTab.ItemType != InventoryItemType.Total)
                    _totalTab.AddItem(item, added);
                else
                    if(item.InventoryType != InventoryItemType.Total)
                    {
                        var inventory = _inventory.Find(inventory => inventory.ItemType == item.InventoryType);
                        inventory.AddItem(item, amount);
                    }
                return added;
            }
            else
                return AddItem(item, amount);
        }

        public void RemoveItemFromSlot(IInventorySlot slot, int amount)
        {
            var itemType = slot.ItemInfo.InventoryType;
            var itemId = slot.ItemInfo.Id;

            int removed = _currentTab.RemoveItemFromSlot(slot, amount);

            if(itemType != InventoryItemType.Total)
            {
                if (_currentTab.ItemType == itemType)
                    _totalTab.RemoveItem(itemId, removed);
                else
                {
                    var inventoryByItemType = _inventory.Find(inventory => inventory.ItemType == itemType);
                    inventoryByItemType.RemoveItem(itemId, removed);
                }
            }
        }

        public void MoveFromSlotToSlot(IInventorySlot fromSlot, IInventorySlot toSlot, int amount)
        {
            _currentTab.MoveFromSlotToSlot(fromSlot, toSlot, amount);
        }

        private void ParseSaveData(InventoryData data)
        {
            Capacity = data.Capacity;

            foreach(var inventoryData in data.SlotDataByItemType)
            {
                _inventory.Add(new Inventory(Capacity, inventoryData.Key, inventoryData.Value));
            }
        }
    }
}
