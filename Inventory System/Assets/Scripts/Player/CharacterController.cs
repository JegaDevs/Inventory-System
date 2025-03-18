using System;
using UnityEngine;
using JegaCore;

namespace Jega
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
            if (velocityVector.sqrMagnitude > 0)
            {
                float targetAngle = Mathf.Atan2(velocityVector.x, velocityVector.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, targetAngle, 0), characterStats.RotationVelocity * Time.deltaTime);

                body.velocity = transform.forward * characterStats.MovementVelocity;
            }
            else
                body.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (velocityVector.sqrMagnitude > 0) 
                body.velocity = transform.forward * characterStats.MovementVelocity;
            else
                body.velocity = Vector3.zero;
        }
    }
}
