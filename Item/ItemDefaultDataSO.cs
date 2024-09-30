#if UNITY_EDITOR
using InspectorDraw;
using UnityEditor;
#endif
using InventorySystem;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDefault", menuName = "Gameplay/Item/DefaultItem")]
public class ItemDefaultDataSO : ScriptableObject
{
#if UNITY_EDITOR
    [field: RandomIdDrawer]
#endif
    [field: SerializeField] 
    public string Id { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    [field: TextArea(15, 20)]
    public string Description { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Icon of the item in the world")]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Icon of the item in the slot")]
    public Sprite UIIcon { get; private set; }

    [field: SerializeField]
    [field: Range(1, 1000, order = 1)]
    [field: Tooltip("Max amount of item in inventory slot")]
    public int MaxStackItem { get; private set; }

    [field: SerializeField]
    public InventoryItemType InventoryType { get; private set; }

    [field: SerializeField]
    public string FileName { get; private set; }

    private void Awake()
    {
        MaxStackItem = 1;

#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this);
        FileName =  Path.GetFileNameWithoutExtension(assetPath);
#endif
    }
}
