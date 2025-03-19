using System;
using System.Collections;
using System.Collections.Generic;
using Jega.InventorySystem;
using JegaCore;
using UnityEngine;
using UnityEngine.Networking;

namespace Jega
{
    public class ServerService : IService
    {
        public int Priority => 0;
        public static ServerService Service => ServiceProvider.GetService<ServerService>();
        public void Preprocess()
        {
        }

        public void Postprocess()
        {
        }

        public void RequestServerItemEvent(InventoryItem item, Action OnSucess, Action OnFail)
        {
            GlobalMonoBehaviour.HostCoroutine(RequestServer(item, OnSucess, OnFail));
        }
        private IEnumerator RequestServer(InventoryItem item, Action OnSucess, Action OnFail)
        {
            WWWForm form = new WWWForm();
            form.AddField("Bearer Token", "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP");
            form.AddField("ItemId", item.ID);
            
            UnityWebRequest www = UnityWebRequest.Post("https://wadahub.manerai.com/api/inventory/status", form);
            www.SetRequestHeader("Authorization", "Bearer " + "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                JegaDebug.LogError("Failed to send server request: " + www.error);
                OnFail?.Invoke();
            }
            else
            {
                OnSucess?.Invoke();
                JegaDebug.LogVerbose("Server success receiving data from item: " + item.name);
            }
            www.Dispose();
        }
    }
}
