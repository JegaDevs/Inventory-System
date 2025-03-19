using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using JegaCore;

namespace Jega.InventorySystem
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static SlotSwitch OnRequestOwnedSlotsSwitch;
        public static ItemTransaction OnItemRemoved;
        public delegate void SlotSwitch(Inventory inventory, InventorySlot slotOrigin, InventorySlot slotDestination);
        public delegate void ItemTransaction(InventoryItem item, int amount);

        public Action OnSlotUpdated;
        public Action OnPointerEnterEvent;
        public Action OnPointerExitEvent;

        [SerializeField] private bool isShop;

        private bool isEmpty;
        private int itemAmount;
        private DragAndDropItem dragAndDropItem;

        private int slotIndex;
        private Inventory inventoryManager;
        private InventoryItem inventoryItem;
        private SessionService sessionService;

        public Inventory InventoryManager => inventoryManager;
        public InventoryItem InventoryItem => inventoryItem;
        public int ItemAmount => itemAmount;


        private void Awake()
        {
            sessionService = ServiceProvider.GetService<SessionService>();
        }
        private void OnDestroy()
        {
            if (inventoryManager)
            {
                inventoryManager.OnSlotUpdated -= UpdateInfo;
            }
        }

        public void RegisterManager(Inventory inventory)
        {
            Assert.IsTrue(inventory != null);

            inventoryManager = inventory;
            inventoryManager.OnSlotUpdated += UpdateInfo;
        }
        private void UpdateInfo(Inventory inventory, InventorySlot slot, Inventory.StartingItem startingItem, int slotIndex)
        {
            if (slot != this) return;

            itemAmount = startingItem.IsValid ? startingItem.Item.GetCustomSavedAmount(inventory.InventorySaveKey, startingItem.Amount) : 0;
            isEmpty = itemAmount <= 0;
            inventoryItem = startingItem.Item;
            inventoryManager = inventory;
            this.slotIndex = slotIndex;
            OnSlotUpdated?.Invoke();
        }
        public void ReleaseItem()
        {
            if (isEmpty) return;
            dragAndDropItem = Instantiate(inventoryItem.Prefab, sessionService.ItemsParent).GetComponent<DragAndDropItem>();
            dragAndDropItem.transform.position = sessionService.BackpackTransform.position + (Vector3.up * 2f);
            dragAndDropItem = null;
            OnItemRemoved?.Invoke(inventoryItem, 1);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isEmpty) return;

            OnPointerEnterEvent?.Invoke();

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isEmpty) return;
            InventoryItem item = inventoryItem;

            if (isEmpty)
                OnPointerExitEvent?.Invoke();
        }
        
    }
}
