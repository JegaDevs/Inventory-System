namespace Jega.InventorySystem
{
    public class ClientInventory : Inventory
    {
        protected override void Awake()
        {
            base.Awake();
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

        protected override void OnEnable()
        {
            base.OnEnable();
            sessionService.RegisterActiveClientInventory(this);
        }
        private void OnDisable()
        {
            sessionService.UnregisterClientShopInventory();
        }
    }
}
