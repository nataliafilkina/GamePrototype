using UICommon;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EquipmentSystem
{
    public class UIEquipmentSlot : UISlot, IHoldPortableItem
    {
        [field: SerializeField]
        public EquipmentType equipmentType { get; private set; }

        private EquipmentSlot _slot;
        public EquipmentSlot Slot
        { 
            get => _slot; 
            set {
#if DEBUG
                if (equipmentType != value.Type)
                    Debug.Log("The UISlot type does not match the saved ones. The UISlot type: " + equipmentType + 
                        ". Saved slot type: " + value.Type);
#endif
                _slot = value;
                Refresh();
            } 
        }

        protected override void Start()
        {
            base.Start();

            if (Slot != null)
                Slot.OnChanged += Refresh;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if(Slot!= null) 
                Slot.OnChanged += Refresh;
        }

        private void OnDisable()
        {
            Slot.OnChanged -= Refresh;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
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

        public void DragItem()
        {
            if (!Slot.IsEmpty)
            {
                _carrierItem.StartDrag(Slot.ItemInfo, Slot.Capacity, this);
                _uiItem.Clear();
            }
        }

        public void DropItem()
        {
            if(_carrierItem.Item is ItemEquipmentDataSO dropItem 
                && dropItem.EquipmentType == equipmentType 
                && Slot.IsEmpty)
            {
                Slot.SetItem(_carrierItem.Item);
                _carrierItem.StopDrag(Slot.Capacity);
            }
            else
                _carrierItem.StopDrag();

        }

        public void Remove(int amount)
        {
            Slot.Clear();
        }

        public void AddItem(ItemDefaultDataSO item, int amount)
        {
            Slot.SetItem(item);
        }

        public override void Refresh()
        {
            if (!Slot.IsEmpty)
            {
                _uiItem.SetView(Slot.ItemInfo.UIIcon, Slot.Capacity);
                return;
            }
            else
                _uiItem.Clear();
        }
    }
}
