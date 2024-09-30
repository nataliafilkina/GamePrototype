using EquipmentSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Gameplay/Item/Equipment")]
public class ItemEquipmentDataSO : ItemDefaultDataSO
{
    [field: SerializeField]
    public EquipmentType EquipmentType { get; private set; }

    [field: SerializeField]
    public string SpriteLibreryLabel { get; private set; }
}
