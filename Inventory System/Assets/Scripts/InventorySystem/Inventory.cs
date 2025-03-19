using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using JegaCore;
using UnityEngine.Events;

namespace Jega.InventorySystem
{
    /// <summary>
    /// Generic Inventory class. Handles all main slot management logic, should be used 
    /// as a base class for inventory variants.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        public SlotUpdated OnSlotUpdated;
        public delegate void SlotUpdated(Inventory inventory, InventorySlot slot, StartingItem startingItem, int slotIndex);

        [SerializeField] private InventoryData inventoryData;
        [SerializeField] private Transform slotsParent;
        [SerializeField] private InventorySlot slotPrefab;

        [SerializeField] protected List<Slot> slots;
        [SerializeField] private UnityEvent OnServerAddItemSuccess;
        [SerializeField] private UnityEvent OnServerAddItemFail;
        [SerializeField] private UnityEvent OnServerRemoveItemSuccess;
        [SerializeField] private UnityEvent OnServerRemoveFail;
        protected SessionService sessionService;

        public string InventorySaveKey => inventoryData.inventorySaveKey;
        public List<Slot> Slots => new List<Slot>(slots);
        protected ReadOnlyCollection<InventoryItem> ItemCollection => inventoryData.itemCollection.Collection;
        protected int NumberOfSlots => inventoryData.numberOfSlots;
        private List<StartingItem> StartingItems => inventoryData.startingItems;
        private const string SlotSaveKey = "_Slot_";

        protected virtual void Awake()
        {
            sessionService = ServiceProvider.GetService<SessionService>();
            InitialInventorySetup();
            
            InventorySlot.OnItemRemoved += LoseItemAmount;
        }

        protected virtual void OnDestroy()
        {
            InventorySlot.OnItemRemoved -= LoseItemAmount;
        }

        protected virtual void OnEnable()
        {
            UpdateAllSlots();
        }

        #region Initial Setup
        private void InitialInventorySetup()
        {
            slots = new List<Slot>();
            List<StartingItem> unfilledStartingItems = new List<StartingItem>();
            foreach (StartingItem startingItem in StartingItems)
                if (startingItem.Item.GetCustomSavedAmount(InventorySaveKey, startingItem.Amount) > 0)
                    unfilledStartingItems.Add(startingItem);

            //Fill saved slots
            for (int i = 0; i < NumberOfSlots; i++)
            {
                StartingItem startingItem = default;
                int storedItemIndex = PlayerPrefs.GetInt(InventorySaveKey + SlotSaveKey + i, -1);
                if (storedItemIndex >= 0)
                {
                    InventoryItem storedItem = ItemCollection[storedItemIndex];
                    int startingItemIndex = StartingItems.FindIndex(a => a.Item == storedItem);
                    int startingAmount = startingItemIndex >= 0 ? StartingItems[startingItemIndex].Amount : 0;
                    if (storedItem.GetCustomSavedAmount(InventorySaveKey, startingAmount) > 0)
                    {
                        startingItem = new StartingItem(storedItem, startingAmount);
                        unfilledStartingItems.Remove(startingItem);
                    }
                }
                InventorySlot slotManager = CreateNewSlots(i);
                slots.Add(new Slot(slotManager, i, startingItem, InventorySaveKey, storedItemIndex));
                OnSlotUpdated?.Invoke(this, slotManager, startingItem, i);
            }

            if (unfilledStartingItems.Count <= 0)
                return;

            //Fill unsaved slots (should happen only once to fill initial values on a empty save)
            foreach (StartingItem startingItem in unfilledStartingItems)
            {
                for (int i = 0; i < NumberOfSlots; i++)
                {
                    if (slots[i].IsEmpty)
                    {
                        Slot currentSlot = slots[i];
                        startingItem.Item.SetCustomSavedAmount(InventorySaveKey, startingItem.Amount);
                        int storedItemIndex = ItemCollection.IndexOf(startingItem.Item);
                        slots[i] = new Slot(currentSlot.SlotManager, currentSlot.Index, startingItem, InventorySaveKey, storedItemIndex);
                        OnSlotUpdated?.Invoke(this, currentSlot.SlotManager, startingItem, i);
                        break;
                    }
                }
            }
            Debug.Log("Filled unsaved slots. \n Attention! This should happen only once when there's no saved data!");
        }
        private InventorySlot CreateNewSlots(int index)
        {
            InventorySlot slotManager = Instantiate(slotPrefab, slotsParent);
            slotManager.RegisterManager(this);
            slotManager.name = slotPrefab.name + index;
            return slotManager;
        }

        protected void UpdateAllSlots()
        {
            int count = slots.Count;
            for (int i = 0; i < count; i++)
                UpdateTargetSlot(i);
        }
        protected void UpdateTargetSlot(int slotIndex)
        {
            Slot slot = slots[slotIndex];
            StartingItem startingItem = default;
            int storedItemIndex = PlayerPrefs.GetInt(InventorySaveKey + SlotSaveKey + slotIndex, -1);
            if (storedItemIndex >= 0)
            {
                InventoryItem storedItem = ItemCollection[storedItemIndex];
                int startingItemIndex = StartingItems.FindIndex(a => a.Item == storedItem);
                int startingAmount = startingItemIndex >= 0 ? StartingItems[startingItemIndex].Amount : 0;
                int ownedAmount = storedItem.GetCustomSavedAmount(InventorySaveKey, startingAmount);
                if (ownedAmount > 0)
                    startingItem = new StartingItem(storedItem, startingAmount);
                else
                    storedItemIndex = -1;
            }
            slots[slotIndex] = new Slot(slot.SlotManager, slot.Index, startingItem, InventorySaveKey, storedItemIndex);
            OnSlotUpdated?.Invoke(this, slots[slotIndex].SlotManager, startingItem, slotIndex);
        }
        #endregion


        #region Inventories interactions
        protected virtual void GainItemAmount(InventoryItem item, int amount)
        {
            int previousOwned = item.GetCustomSavedAmount(InventorySaveKey, 0);
            int newOwned = previousOwned + amount;
            item.SetCustomSavedAmount(InventorySaveKey, newOwned);
            bool isSlotAlreadyFilled = previousOwned > 0;
            if (isSlotAlreadyFilled)
            {
                int slotIndex = slots.FindIndex(a => a.Item == item);
                UpdateTargetSlot(slotIndex);
            }
            else
            {
                StartingItem startingItem = new StartingItem(item);
                for (int i = 0; i < NumberOfSlots; i++)
                {
                    bool isValidSlot = false;
                    if (slots[i].IsEmpty)
                    {
                        if(i < inventoryData.slotTypes.Count && 
                           (inventoryData.slotTypes[i] == InventoryItem.ItemType.Undefined || inventoryData.slotTypes[i] == item.Type))
                        isValidSlot = true;
                    }
                    if (isValidSlot)
                    {
                        Slot currentSlot = slots[i];
                        int storedItemIndex = ItemCollection.IndexOf(startingItem.Item);
                        slots[i] = new Slot(currentSlot.SlotManager, currentSlot.Index, startingItem, InventorySaveKey, storedItemIndex);
                        OnSlotUpdated?.Invoke(this, slots[i].SlotManager, startingItem, i);
                        break;
                    }
                }
            }
            ServerService.Service.RequestServerItemEvent(item, () => OnServerAddItemSuccess.Invoke(), () => OnServerAddItemFail.Invoke());
        }
        protected virtual void LoseItemAmount(InventoryItem item, int amount)
        {
            int slotIndex = slots.FindIndex(a => a.Item == item);
            int previousOwned = item.GetCustomSavedAmount(InventorySaveKey, 0);
            int newOwned = previousOwned - amount;
            item.SetCustomSavedAmount(InventorySaveKey, newOwned);
            UpdateTargetSlot(slotIndex);
            ServerService.Service.RequestServerItemEvent(item, null, null);
        }
        #endregion


        public bool GetHasSpaceForTransaction(InventoryItem item)
        {
            int ownedIndex = slots.FindIndex(a => a.Item == item);
            if (ownedIndex >= 0)
                return true;

            foreach (Slot slot in slots)
                if (slot.IsEmpty)
                    return true;

            return false;
        }

        #region public structs
        [Serializable]
        public struct Slot
        {
            public StartingItem StartingItem;
            public InventorySlot SlotManager;
            public int Index;
            public int ItemId;
            public Slot(InventorySlot uiSlot, int slotIndex, StartingItem startingItem, string customSlotSaveKey, int itemIndex)
            {
                StartingItem = startingItem;
                SlotManager = uiSlot;
                Index = slotIndex;
                ItemId = itemIndex;

                PlayerPrefs.SetInt(customSlotSaveKey + SlotSaveKey + slotIndex, itemIndex);
            }

            public readonly InventoryItem Item => StartingItem.Item;
            public readonly bool IsEmpty => Item == null;
        }

        [Serializable]
        public struct StartingItem
        {
            public InventoryItem Item;
            public int Amount;

            public StartingItem(InventoryItem item, int startingAmount = 0)
            {
                Item = item;
                Amount = startingAmount;
            }
            public StartingItem(StartingItem copy)
            {
                Item = copy.Item;
                Amount = copy.Amount;
            }

            public bool IsValid => Item != null;
        }
        #endregion
    }
}
