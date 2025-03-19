using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jega
{
    public class ItemsParentSetter : MonoBehaviour
    {
        private void Awake()
        {
            SessionService.Service.ItemsParent = transform;
        }
    }
}
