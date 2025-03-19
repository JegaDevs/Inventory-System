using Jega.InventorySystem;
using System;
using UnityEngine;
using JegaCore;

namespace Jega
{
    public class SessionService : IService
    {
        public int Priority => 0;
        private ClientInventory currentClientInventory;
        
        
        public ClientInventory CurrentClientInventory => currentClientInventory;
        public Camera GameCamera;
        public Transform BackpackTransform;
        public Transform ItemsParent;

        public static SessionService Service => ServiceProvider.GetService<SessionService>();
        public void Preprocess()
        {
        }
        public void Postprocess()
        {
        }
        
        public void RegisterActiveClientInventory(ClientInventory clientInventory)
        {
            currentClientInventory = clientInventory;
        }

        public void UnregisterClientShopInventory()
        {
            currentClientInventory = null;
        }

    }
}