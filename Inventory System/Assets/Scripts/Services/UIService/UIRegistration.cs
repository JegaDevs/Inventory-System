using UnityEngine;

namespace Jega.Services
{
    public class UIRegistration : MonoBehaviour
    {
        private void Awake()
        {
            UIService.Service.RegisterNewScene(gameObject);
        }
    }
}
