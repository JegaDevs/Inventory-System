namespace Jega.InventorySystem
{
    public class ClientInventory : Inventory
    {
        protected override void Awake()
        {
            base.Awake();
            sessionService.RegisterActiveClientInventory(this);
            DragAndDropItem.OnAddToBackpack += AddItemToInventory;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DragAndDropItem.OnAddToBackpack -= AddItemToInventory;
        }

        private void AddItemToInventory(InventoryItem item)
        {
            GainItemAmount(item, 1);
        }
    }
}
