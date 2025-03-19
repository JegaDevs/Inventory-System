using System;
using System.Collections;
using System.Collections.Generic;
using Jega.InventorySystem;
using UnityEngine;

namespace Jega
{
    public class Backpack : MonoBehaviour
    {
        [SerializeField] private GameObject InventoryToggle;
        [SerializeField] private GameObject backPackVisual;
        [SerializeField] private List<BackpackVisualPair> backpackVisualPairs;

        private Inventory playerInventory;
        private Ray ray;
        private RaycastHit hit;

        [Serializable]
        private struct BackpackVisualPair
        {
            public InventoryItem item;
            public GameObject visualItem;
        }
        private void Awake()
        {
            SessionService.Service.BackpackTransform = backPackVisual.transform;
            playerInventory = SessionService.Service.CurrentClientInventory;
            playerInventory.OnSlotUpdated += UpdateBackpackVisuals;
        }

        private void OnDestroy()
        {
            playerInventory.OnSlotUpdated -= UpdateBackpackVisuals;
        }
        
        private IEnumerator Start()
        {
            InventoryToggle.SetActive(false);
            yield return null;
            UpdateBackpackVisuals();
        }

        private void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0) && hit.collider.CompareTag("Backpack"))
                    InventoryToggle.SetActive(!InventoryToggle.activeSelf);
            }
        }

        private void UpdateBackpackVisuals() => UpdateBackpackVisuals(SessionService.Service.CurrentClientInventory, null, default, 0);
        private void UpdateBackpackVisuals(Inventory inventory, InventorySlot slotAdded, Inventory.StartingItem startingitem, int slotindex)
        {
            foreach (var backpackVisualPair in backpackVisualPairs)
                backpackVisualPair.visualItem.SetActive(false);

            foreach (var slot in inventory.Slots)
            {
                var slotItem = slot.Item;
                if(slotItem == null)
                    continue;
                foreach (var backpackVisualPair in backpackVisualPairs)
                    if(slotItem.ID == backpackVisualPair.item.ID)
                        backpackVisualPair.visualItem.SetActive(true);
            }
        }
    }
}
