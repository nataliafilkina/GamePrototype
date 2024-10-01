using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace EquipmentSystem
{
    public class UIEquipmentPlayer : MonoBehaviour
    {
        private List<SpriteResolver> _resolvers;
        [SerializeField]
        private string _defaultLabel;

        [Inject]
        private Equipment _equipment;

        private Dictionary<string, EquipmentType> typeByCategory  = new()
        {
            { "ForearmR", EquipmentType.Gloves},
            { "ForearmL", EquipmentType.Gloves},
        }; 

        private void Awake()
        {
            _resolvers = GetComponentsInChildren<SpriteResolver>().ToList();
        }

        private void Start()
        {
            _equipment.OnSaveNew += Refresh;
            Refresh();
        }

        private void Refresh()
        { 
            foreach (var resolver in _resolvers)
            {
                var category = resolver.GetCategory();
                if (category == null || !typeByCategory.TryGetValue(category, out var type))
                    continue;

                var slot = _equipment.GetSlotByType(type);
                if (slot.ItemInfo == null)
                {
                    resolver.SetCategoryAndLabel(resolver.GetCategory(), _defaultLabel);
                }
                else
                    if (slot.ItemInfo is ItemEquipmentDataSO item)
                    {
                        resolver.SetCategoryAndLabel(resolver.GetCategory(), item.SpriteLibreryLabel);
                    }
            }
        }
    }
}
