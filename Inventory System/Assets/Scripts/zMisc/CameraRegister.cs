using System;
using System.Collections;
using System.Collections.Generic;
using JegaCore;
using UnityEngine;

namespace Jega
{
    public class CameraRegister : MonoBehaviour
    {
        private void Awake()
        {
            ServiceProvider.GetService<SessionService>().GameCamera = GetComponent<Camera>();
        }
    }
}
