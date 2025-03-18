using System;
using UnityEngine;
using JegaCore;

namespace Jega.BlueGravity
{
    [RequireComponent((typeof(Rigidbody)))]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterStatsData characterStats;
        
        private Rigidbody body;
        private Vector3 velocityVector;

        private InputService inputService;

        public Rigidbody Body => body;
        public Vector3 VelocityVector => velocityVector;

        private void Awake()
        {
            inputService = ServiceProvider.GetService<InputService>();
            body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            velocityVector = inputService.MovementVector.normalized.FromPlaneToVector3();
        }

        private void FixedUpdate()
        {
            velocityVector.x *= characterStats.HorizontalVelocity;
            velocityVector.z *= characterStats.VerticalVelocity;

            body.velocity = velocityVector;
        }
    }
}
