

public class ItemInSlot
{
    public ItemDefaultDataSO Info { get; private set; }
    public int AmountInSlot { get; private set; }

    public ItemInSlot(ItemDefaultDataSO info, int amountInSlot)
    {
        Info = info;
        AmountInSlot = amountInSlot;
    }
}
