using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jega
{
    public class DragAndDropItem : MonoBehaviour
    {
        [SerializeField]private float zVelocity;
        
        private Camera mainCamera;
        private Transform backpack;
        private Vector3 mousePosition;
        private Collider bodyCollider;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            bodyCollider = GetComponent<Collider>();
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

        private void OnMouseDown()
        {
            mousePosition = Input.mousePosition - GetScreenPosition();
            rb.isKinematic = true;
            bodyCollider.isTrigger = true;
        }

        private void OnMouseDrag()
        {
            var newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            newPosition.z = backpack.position.z;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, zVelocity * Time.deltaTime);
        }

        private void OnMouseUp()
        {
            rb.isKinematic = false;
            bodyCollider.isTrigger = false;
        }
    }
}
