using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class MoveProperties
    {
        [Tooltip("Select the direction(s) that these properties apply to.")]
        [SerializeField] public MoveDirection ApplicableDirection;

        [Tooltip("Maximum velocity over which acceleration will no longer be added")]
        [SerializeField] public float MaximumSpeed;

        [Tooltip("Velocity to jump to when starting to move from either a standstill or below Minimum Speed")]
        [SerializeField] public float MinimumSpeed;


        [Header("Specify a value for only ONE of the below two properties")]
        [Tooltip("Amount of velocity to add every FixedUpdate until MaximumSpeed is reached. 0 = no acceleration, set to < 0 if using Exponential Acceleration Rate.")]
        [SerializeField] public float LinearAccelerationRate;

        [Tooltip("Amount to multiply current velocity by to get velocity to add every FixedUpdate. Requires MinimumSpeed > 0, 0 =  no acceleration, set to < 0 if using Linear Acceleration Rate.")]
        [SerializeField] public float ExponentialAccelerationRate;
    }
}
