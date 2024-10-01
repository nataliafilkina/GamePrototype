using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace EquipmentSystem
{
    public class UIEquipment : MonoBehaviour
    {
        private List<UIEquipmentSlot> _UISlots= new ();

        [Inject]
        public Equipment Equipment { get; private set; }

        private void Awake()
        {
            _UISlots = GetComponentsInChildren<UIEquipmentSlot>().ToList();
        }

        private void Start()
        {
            Equipment.SetSlots(_UISlots);
        }

        private void OnDisable()
        {
            Equipment.Save();
        }
    }
}
