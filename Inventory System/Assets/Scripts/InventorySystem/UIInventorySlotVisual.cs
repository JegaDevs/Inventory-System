using System.Collections.Generic;
using Jega.Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JegaCore;
using UnityEngine.Serialization;

namespace Jega.InventorySystem
{
    [RequireComponent(typeof(InventorySlot))]
    public class UIInventorySlotVisual : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI textMesh;
        
        [Header("Shop interactions")]
        [SerializeField] private GameObject infoPopUp;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI weightText;

        private InventorySlot inventorySlot;
        private RectTransform iconTransform;
        private Vector2 originalPosition;

        private SessionService sessionService;

        private InventoryItem InventoryItem => inventorySlot.InventoryItem;

        private void Awake()
        {
            sessionService = ServiceProvider.GetService<SessionService>();
            inventorySlot = GetComponent<InventorySlot>();
            iconTransform = iconImage.GetComponent<RectTransform>();
            originalPosition = iconTransform.anchoredPosition;

            infoPopUp.SetActive(false);

            inventorySlot.OnSlotUpdated += UpdateSlot;
            
            inventorySlot.OnPointerEnterEvent += OnEnterPointer;
            inventorySlot.OnPointerExitEvent += OnExitPointer;
        }
        private void OnDestroy()
        {
            inventorySlot.OnSlotUpdated -= UpdateSlot;
            
            inventorySlot.OnPointerEnterEvent -= OnEnterPointer;
            inventorySlot.OnPointerExitEvent -= OnExitPointer;
        }

        private void OnDisable()
        {
            if (infoPopUp.gameObject.activeSelf)
                infoPopUp.gameObject.gameObject.SetActive(false);
        }

        private void UpdateSlot()
        {
            textMesh.text = string.Empty;
            iconImage.gameObject.SetActive(InventoryItem);

            if (InventoryItem != null)
            {
                if (inventorySlot.ItemAmount > 0)
                {
                    iconImage.sprite = InventoryItem.Icon;
                    textMesh.text = inventorySlot.ItemAmount.ToString();
                    nameText.text = InventoryItem.Name;
                    typeText.text = InventoryItem.Type.ToString();
                    weightText.text = InventoryItem.Weight.ToString("0.0");
                }
            }
            ResetIconPosition();
        }

        private void ResetIconPosition()
        {
            iconTransform.anchoredPosition = originalPosition;
        }


        private void OnEnterPointer()
        {
            infoPopUp.gameObject.SetActive(true);
            int price = 0;
        }
        private void OnExitPointer()
        {
            if (infoPopUp.gameObject.activeSelf)
                infoPopUp.gameObject.gameObject.SetActive(false);
        }
    }

}
