using System.Collections.Generic;
using Jega.Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using JegaCore;

namespace Jega.InventorySystem
{
    [RequireComponent(typeof(InventorySlot))]
    public class UIInventorySlotVisual : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Vector2 draggingOffset;
        [SerializeField] private float draggingZOffset;

        [Header("Shop interactions")]
        [SerializeField] private Image unAvailable;
        [SerializeField] private GameObject pricePopUp;
        [SerializeField] private GameObject notAffordableIndicador;
        [SerializeField] private TextMeshProUGUI priceText;

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

            unAvailable.gameObject.SetActive(false);
            pricePopUp.SetActive(false);

            inventorySlot.OnSlotUpdated += UpdateSlot;
            inventorySlot.OnSlotUpdated += UpdateAvailability;
            sessionService.OnCoinsUpdate += UpdateAvailability;

            inventorySlot.OnStartDrag += StartDrag;
            inventorySlot.OnStayDrag += StayDrag;
            inventorySlot.OnExitDrag += ExitDrag;
            inventorySlot.OnPointerEnterEvent += OnEnterPointer;
            inventorySlot.OnPointerExitEvent += OnExitPointer;
        }
        private void OnDestroy()
        {
            inventorySlot.OnSlotUpdated -= UpdateSlot;
            inventorySlot.OnSlotUpdated -= UpdateAvailability;
            sessionService.OnCoinsUpdate -= UpdateAvailability;

            inventorySlot.OnStartDrag -= StartDrag;
            inventorySlot.OnStayDrag -= StayDrag;
            inventorySlot.OnExitDrag -= ExitDrag;
            inventorySlot.OnPointerEnterEvent -= OnEnterPointer;
            inventorySlot.OnPointerExitEvent -= OnExitPointer;
        }

        private void OnDisable()
        {
            if (pricePopUp.gameObject.activeSelf)
                pricePopUp.gameObject.gameObject.SetActive(false);
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
                }
            }
            ResetIconPosition();
        }

        private void UpdateAvailability()
        {
            if (InventoryItem == null)
            {
                unAvailable.gameObject.SetActive(false);
                return;
            }
        }
        private void ResetIconPosition()
        {
            iconTransform.anchoredPosition = originalPosition;
        }


        private void StartDrag(PointerEventData eventData)
        {
            iconTransform.SetParent(UIService.Service.CurrentUIParent.transform, false);
            iconTransform.SetAsLastSibling();
            textMesh.gameObject.SetActive(false);
        }
        private void StayDrag(PointerEventData eventData)
        {
            Vector3 newPosition = eventData.position;
            iconTransform.position = newPosition;
            iconTransform.anchoredPosition += draggingOffset;
        }
        private void ExitDrag(bool sucess)
        {
            iconTransform.SetParent(transform, false);
            iconTransform.SetAsFirstSibling();
            textMesh.gameObject.SetActive(true);
            if (!sucess)
                ResetIconPosition();
        }


        private void OnEnterPointer()
        {
            pricePopUp.gameObject.SetActive(true);
            int price = 0;
            priceText.text = price.ToString();
        }
        private void OnExitPointer()
        {
            if (pricePopUp.gameObject.activeSelf)
                pricePopUp.gameObject.gameObject.SetActive(false);
        }
    }

}
