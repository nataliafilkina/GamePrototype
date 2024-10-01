using InventorySystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIInventory
{
    public class UIInventoryBag : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private int capacity;

        [field: SerializeField]
        public InventoryItemType ItemType { get; private set; }

        [SerializeField]
        private GameObject _slotPrefab;

        [Inject]
        private PlayerController _owner;

        [Inject]
        private UIInventorySlot.Factory _slotFactory;

        public InventoryControl InventoryControl { get; private set; }

        private List<UIInventorySlot> _uISlots;
        private Inventory inventory;

        #endregion

        private void OnValidate()
        {
            if (capacity < 0)
                capacity = 0;
        }

        private void Awake()
        {
            var grid = GetComponentInChildren<GridLayoutGroup>(true);
            grid.enabled = true;

            for (int i = 0; i < capacity; ++i)
            {
                var uiSlot = _slotFactory.Create(grid.transform, grid.transform);
                uiSlot.name = "UISlot" + i.ToString();
            }

            Canvas.ForceUpdateCanvases();
        }

        private void Start()
        {
            InventoryControl = _owner.inventoryControl;
            SetInventory();
        }

        protected void OnEnable()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChangedInSlotEvent += OnInventoryStateChanged;
                InventoryControl.CurrentTab = inventory;
                Refresh();
            }
        }

        protected void OnDisable()
        {
            if (inventory != null)
                inventory.OnInventoryChangedInSlotEvent -= OnInventoryStateChanged;
        }

        protected void OnInventoryStateChanged(int indexSlot)
        {
            _uISlots[indexSlot].Refresh();
        }

        private void SetInventory()
        {
            inventory = InventoryControl.GetInventoryByType(ItemType);

            if (inventory != null)
            {
                _uISlots = GetComponentsInChildren<UIInventorySlot>(true).ToList();
                var inventorySlots = inventory.Slots;
                int idx = 0;

                foreach (var slot in inventorySlots)
                {
                    _uISlots[idx].SetSlot(slot);
                    idx++;
                }

                OnEnable();
            }
#if UNITY_EDITOR
            else
                Debug.Log("Inventory not found. Check the installed type.");
#endif
        }

        private void Refresh()
        {
            foreach (var slot in _uISlots)
            {
                slot.Refresh();
            }
        }
    }
}
