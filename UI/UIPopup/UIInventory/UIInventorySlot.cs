using InventorySystem;
using TMPro;
using UI.ModalWindow;
using UICommon;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UIInventory
{
    [RequireComponent(typeof(UIClickHandler))]
    public class UIInventorySlot : UISlot, IHoldPortableItem
    {
        #region Fields
        public IInventorySlot Slot { get; private set; }
        private InventoryControl _inventoryControl;

        private UIModalWindowHolder _windowHolder;
        private UIInputWindow _takePartItemWindow;

        private Transform _startPosition;
        private Transform _parent;

        #endregion

        [Inject]
        public void Construct(Transform position, Transform parent, UIModalWindowHolder modalWindowHolder)
        {
            _windowHolder = modalWindowHolder;
            _startPosition = position;
            _parent = parent;
        }

        public class Factory : PlaceholderFactory<Transform, Transform, UIInventorySlot>
        {
        }

        private new void Awake()
        {
            base.Awake();

            transform.position = _startPosition.position;
            transform.SetParent(_parent, false);
        }

        protected override void Start()
        {
            base.Start();
            _inventoryControl = GetComponentInParent<UIInventoryBag>().InventoryControl;
            _takePartItemWindow = _windowHolder.InputWindow as UIInputWindow;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Slot != null)
                Slot.OnChanged += Refresh;
        }

        private void OnDisable()
        {
            Slot.OnChanged -= Refresh;
        }

        public void SetSlot(IInventorySlot slot) => Slot = slot;

        public void DragItem()
        {
            _carrierItem.StartDrag(Slot.ItemInfo, Slot.Data.Amount, this);
            _uiItem.Clear();
        }

        public void DragItem(int amount)
        {
            _carrierItem.StartDrag(Slot.ItemInfo, amount, this);
            _uiItem.SetAmount(Slot.Data.Amount - amount);
        }

        public void DropItem()
        {
            if (_carrierItem.FromSlot is UIInventorySlot fromSlot)
            {
                _carrierItem.StopDrag();
                _inventoryControl.MoveFromSlotToSlot(fromSlot.Slot, Slot, _carrierItem.ItemAmount);
            }
            else
            {
                var added = _inventoryControl.AddItemToSlot(_carrierItem.Item, _carrierItem.ItemAmount, Slot);
                _carrierItem.StopDrag(added);
            }

        }

        public void AddItem(ItemDefaultDataSO item, int amount)
        {
            _inventoryControl.AddItem(item, amount);
        }

        public void Remove(int amount)
        {
            _inventoryControl.RemoveItemFromSlot(Slot, amount);
        }

        public override void Refresh()
        {
            if (!Slot.IsEmpty)
            {
                _uiItem.SetView(Slot.ItemInfo.UIIcon, Slot.Data.Amount);
                return;
            }

            _uiItem.Clear();
        }

        public override void OnOneClick(PointerEventData eventData)
        {
            base.OnOneClick(eventData);

            if (_carrierItem.IsEmpty() && !Slot.IsEmpty)
            {
                DragItem();
                return;
            }

            if (!_carrierItem.IsEmpty())
                DropItem();
        }

        public override void OnClickWithLeftShift(PointerEventData eventData)
        {
            if (_carrierItem.IsEmpty() && Slot.Data.Amount > 1)
            {
                _takePartItemWindow.Show("Введите количесвто элементов",
                                                  "Количество элементов",
                                                  gameObject.transform.position,
                                                  TMP_InputField.ContentType.IntegerNumber,
                                                  3,
                                                  "1",
                                                  1,
                                                  Slot.Data.Amount - 1,
                                                  (string inputText) => { DragItem(int.Parse(inputText)); });
            }
        }
    }
}