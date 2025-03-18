using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jega
{
    public class Backpack : MonoBehaviour
    {
        [SerializeField] private GameObject InventoryToggle;
        [SerializeField] private GameObject backPackVisual;

        private Ray ray;
        private RaycastHit hit;

        private void Awake()
        {
            SessionService.Service.BackpackTransform = backPackVisual.transform;
        }

        private void Start()
        {
            InventoryToggle.SetActive(false);
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
    }
}
