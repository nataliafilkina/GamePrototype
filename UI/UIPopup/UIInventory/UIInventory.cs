using InventorySystem;
using UnityEngine;
using Zenject;

namespace UIInventory
{
    public class UIInventory : MonoBehaviour
    {
        //TODO
        [Inject]
        private PlayerController _owner;

        public InventoryControl InventoryControl { get; private set; }

        private void Start()
        {
            InventoryControl = _owner.inventoryControl;
        }

        private void OnDisable()
        {
            InventoryControl.Save();
        }
    }
}
