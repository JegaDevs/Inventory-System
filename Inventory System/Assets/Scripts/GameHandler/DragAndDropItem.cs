using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jega.InventorySystem;
using UnityEngine.Networking;

namespace Jega
{
    public class DragAndDropItem : MonoBehaviour
    {
        public static Action<InventoryItem> OnAddToBackpack;
        
        [SerializeField] private InventoryItem item;
        
        private Transform backpack;
        private Camera mainCamera;
        private OutlineOnHover outlineOnHover;
        
        private Collider bodyCollider;
        private Rigidbody rb;
        private RaycastHit[] hits;
        private Vector3 mousePosition;

        private void Awake()
        {
            hits = new RaycastHit[5];
            rb = GetComponent<Rigidbody>();
            bodyCollider = GetComponentInChildren<Collider>();
            outlineOnHover = GetComponent<OutlineOnHover>();
        }


        private void Start()
        {
            mainCamera = SessionService.Service.GameCamera;
            backpack = SessionService.Service.BackpackTransform;
        }

        private Vector3 GetScreenPosition()
        {
            var newPosition = transform.position;
            newPosition.z = backpack.position.z;
            return mainCamera.WorldToScreenPoint(newPosition);
        }

        public void OnMouseDown()
        {
            if (mainCamera == null || backpack == null)
                Start();
            mousePosition = Input.mousePosition - GetScreenPosition();
            transform.rotation = Quaternion.identity;
            rb.isKinematic = true;
            outlineOnHover.ToggleOutline(true);
            bodyCollider.isTrigger = true;
            Cursor.visible = false;
        }

        public void OnMouseDrag()
        {
            var newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            newPosition.z = backpack.position.z;
            newPosition.y = Mathf.Clamp(newPosition.y, 0.5f, 15f);
            transform.position = newPosition;
        }

        public void OnMouseUp()
        {
            Cursor.visible = true;  
            bool success = false;
            if (Physics.SphereCastNonAlloc(transform.position, 1.5f, Vector3.up, hits) > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null && hits[i].collider.CompareTag("Backpack"))
                    {
                        success = true;
                        break;
                    }
                }
            }

            if (success)
            {
                OnAddToBackpack?.Invoke(item);
                Destroy(gameObject);
            }
            else
            {
                rb.isKinematic = false;
                bodyCollider.isTrigger = false;
                outlineOnHover.ToggleOutline(false);
            }
        }
        
        private void OnMouseEnter()
        {
            outlineOnHover.ToggleOutline(true);
        }

        private void OnMouseExit()
        {
            if(Cursor.visible)
                outlineOnHover.ToggleOutline(false);
        }
    }
}
