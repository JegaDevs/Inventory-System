using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jega.InventorySystem;
using JegaCore;
using TMPro;
using UnityEngine;

namespace Jega
{
    [RequireComponent(typeof(Inventory))]
    public class UIInventoryVisual : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private string preText;

        private Inventory inventory;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            inventory.OnSlotUpdated += UpdateWeightText;
        }

        private void OnDestroy()
        {
            inventory.OnSlotUpdated -= UpdateWeightText;
        }

        private void Start()
        {
            UpdateWeightText();
        }

        private void UpdateWeightText()
        {
            GlobalMonoBehaviour.HostCoroutine(UpdateWeightCoroutine());
        }

        private void UpdateWeightText(Inventory inventory1, InventorySlot slot1, Inventory.StartingItem startingitem,
            int slotindex) => UpdateWeightText();

        private IEnumerator UpdateWeightCoroutine()
        {
            yield return null;
            
            float totalWeight = 0;
            foreach (var slot in inventory.Slots)
            {
                if(slot.IsEmpty) continue;
                totalWeight += slot.Item.Weight * slot.SlotManager.ItemAmount;
            }
            weightText.text = preText + totalWeight.ToString("0.0");
        }
    }
}
