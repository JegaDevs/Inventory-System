using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jega
{
    public class OutlineOnHover : MonoBehaviour
    {
        [SerializeField] private bool isManualCalling;
        [SerializeField] private Material outlineMaterial;
        
        private MeshRenderer[]renderers;
        
        private void Awake()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }
        
        public void OnMouseEnter()
        {
            if (isManualCalling) return;
                ToggleOutline(true);
        }

        public void OnMouseExit()
        {
            if (isManualCalling) return;
                ToggleOutline(false);
        }
        
        
        public void ToggleOutline(bool toggle)
        {
            foreach (var currentRenderer in renderers)
            {
                var originalMaterial = currentRenderer.sharedMaterial;
                var newMaterials = new List<Material> { originalMaterial };
                if(toggle)
                    newMaterials.Add(outlineMaterial);
                currentRenderer.SetMaterials(newMaterials);
            }
        }
    }
}
