using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jega.BlueGravity
{
    [CreateAssetMenu(menuName = "Jega/CharacterStats")]
    public class CharacterStatsData : ScriptableObject
    {
        [SerializeField] private float movementVelocity;
        [SerializeField] private float rotationVelocity;

        public float MovementVelocity => movementVelocity;
        public float RotationVelocity => rotationVelocity;
    }
}
