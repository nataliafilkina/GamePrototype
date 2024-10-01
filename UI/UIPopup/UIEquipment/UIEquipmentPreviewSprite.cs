using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace EquipmentSystem
{
    public class UIEquipmentPreviewSprite : MonoBehaviour
    {
        [SerializeField]
        private EquipmentType _equipmentType;
        [SerializeField]
        private ItemEquipmentDataSO _defaultItem;

        private EquipmentSlot slot;
        private SpriteResolver _resolver;
        private string _category;

        [Inject]
        private Equipment _equipment;

        private void Start()
        {
            _resolver = GetComponent<SpriteResolver>();
            _category = _resolver.GetCategory();

            slot = _equipment.GetSlotByType(_equipmentType);
            SetItem();

            OnEnable();
        }

        private void OnEnable()
        {
            if(slot != null)
                slot.OnChanged += SetItem;
        }

        private void OnDisable()
        {
            if (slot != null)
                slot.OnChanged -= SetItem;
        }

        private void SetItem()
        {
            if (slot.ItemInfo == null)
            {      
                _resolver.SetCategoryAndLabel(_resolver.GetCategory(), _defaultItem.SpriteLibreryLabel);
                return;
            }

            if (slot.ItemInfo is ItemEquipmentDataSO item)
            {
                _resolver.SetCategoryAndLabel(_resolver.GetCategory(), item.SpriteLibreryLabel);
            }
        }
    }
}
