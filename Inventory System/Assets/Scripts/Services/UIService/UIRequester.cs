using UnityEngine;
using JegaCore;

namespace Jega.Services
{
    public class UIRequester : MonoBehaviour
    {
        void Awake()
        {
            ServiceProvider.GetService<UIService>();
        }
    }

}


