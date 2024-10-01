
namespace UICommon
{
    public interface IHoldPortableItem
    {
        public void DragItem();
        public void DropItem();
        public void Remove(int amount);
        public void AddItem(ItemDefaultDataSO item, int amount);
        public void Refresh();
    }
}
