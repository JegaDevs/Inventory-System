using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using JegaCore;

namespace Jega.InventorySystem
{
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public static SlotSwitch OnRequestOwnedSlotsSwitch;
        public static SlotSwitchInventories OnRequestClothingInventorySwitch;
        public static ItemTransaction OnItemBought;
        public static ItemTransaction OnItemSold;
        public delegate void SlotSwitch(Inventory inventory, InventorySlot slotOrigin, InventorySlot slotDestination);
        public delegate void SlotSwitchInventories(Inventory inventoryOrigin, Inventory inventoryDestination, InventoryItem itemOrigin, InventoryItem itemDest);
        public delegate void ItemTransaction(Inventory shopInventory, InventoryItem item, int amount);

        public Action OnSlotUpdated;
        public Action<PointerEventData> OnStartDrag;
        public Action<PointerEventData> OnStayDrag;
        public Action<bool> OnExitDrag;
        public Action OnPointerEnterEvent;
        public Action OnPointerExitEvent;

        [SerializeField] private bool isShop;

        private bool isEmpty;
        private int itemAmount;
        private bool isDragging;

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
        private void OnEnable()
        {
            if (isDragging)
                OnExitDrag?.Invoke(false);
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
        public void UpdateInfo(Inventory inventory, InventorySlot slot, Inventory.StartingItem startingItem, int slotIndex)
        {
            if (slot != this) return;

            itemAmount = startingItem.IsValid ? startingItem.Item.GetCustomSavedAmount(inventory.InventorySaveKey, startingItem.Amount) : 0;
            isEmpty = itemAmount <= 0;
            inventoryItem = startingItem.Item;
            inventoryManager = inventory;
            this.slotIndex = slotIndex;
            OnSlotUpdated?.Invoke();
        }

        #region Draging Behavior
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            isDragging = true;
            OnStartDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            OnStayDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isEmpty) return;
            isDragging = false;
            GameObject destination = eventData.pointerCurrentRaycast.gameObject;
            HandleSlotSwapInteractions(destination);
        }

        private void HandleSlotSwapInteractions(GameObject destination)
        {
            bool sucess = false;
            if (destination != null && destination.TryGetComponent(out InventorySlot newSlot) && newSlot != this)
            {
                if (inventoryManager == newSlot.inventoryManager)
                {
                    OnRequestOwnedSlotsSwitch?.Invoke(inventoryManager, this, newSlot);
                    sucess = true;
                }
            }

            OnExitDrag?.Invoke(sucess);
        }

        #endregion

        #region Shop Interactions
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
        #endregion
    }
}
