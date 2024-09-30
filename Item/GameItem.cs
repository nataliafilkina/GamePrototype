using UnityEngine;

public class GameItem : MonoBehaviour
{
    #region Fields
    [field: SerializeField]
    public ItemDefaultDataSO Info {  get; private set; }

    [field: SerializeField]
    public int Amount { get; private set; }

    [field: SerializeField]
    public bool CanBePickedUp { get; private set; } = true;
    #endregion

    private void OnValidate()
    {
        if(Amount < 1)
            Amount = 1;
    }

    public void PickUp(GameObject owner)
    {
        var control = owner.GetComponentInParent<PlayerController>();
        control.inventoryControl?.AddItem(Info, Amount);

        Destroy(gameObject);
    }
}
