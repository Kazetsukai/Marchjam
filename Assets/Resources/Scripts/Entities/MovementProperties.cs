using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class MovementProperties
    {
        [Tooltip("Maximum velocity over which acceleration will no longer be added, set to 0 to disable movement in this direction")]
        [SerializeField]
        public float MaximumSpeed;

        [Header("Specify a value for only ONE of the below two properties")]
        [Tooltip("Amount of velocity to add every second until MaximumSpeed is reached. 0 = no acceleration, if ExponentialAccelerationRate is set, Linear Acceleration Rate will define the starting rate from < Linear Acceleration Rate")]
        [SerializeField]
        public float LinearAccelerationRate;

        [Tooltip("Amount to multiply current velocity by to get velocity to add every second. Requires MinimumSpeed > 0, 0 =  no acceleration, set to <= 0 if using only Linear Acceleration Rate.")]
        [SerializeField]
        public float ExponentialAccelerationRate;

        /// <summary>
        /// Returns a MovementProperties object where all invalid values are either populated by the default or zero'd
        /// </summary>
        /// <param name="applicableProperties">MovementProperties object which may or may not contain &lt; 0 values to be replaced with defaults.</param>
        /// <param name="defaultProperties">MovementProperties object containing defaults</param>
        /// <returns></returns>
        public static MovementProperties GetActiveProperties(MovementProperties applicableProperties, MovementProperties defaultProperties)
        {
            if (applicableProperties == defaultProperties)
            {
                return applicableProperties;
            }

            MovementProperties activeProperties = new MovementProperties()
            {
                MaximumSpeed = Mathf.Max(0, applicableProperties.MaximumSpeed >= 0 ? applicableProperties.MaximumSpeed : defaultProperties.MaximumSpeed),
                LinearAccelerationRate = Mathf.Max(0, applicableProperties.LinearAccelerationRate >= 0 ? applicableProperties.LinearAccelerationRate : defaultProperties.LinearAccelerationRate),
                ExponentialAccelerationRate = 0
            };

            //Exponential rate can only work if Linear rate is > 0
            if (activeProperties.LinearAccelerationRate > 0)
            {
                activeProperties.ExponentialAccelerationRate = applicableProperties.ExponentialAccelerationRate >= 0 ? applicableProperties.ExponentialAccelerationRate : defaultProperties.ExponentialAccelerationRate;
            }

            return activeProperties;
        }

        public static MovementProperties Empty
        {
            get
            {
                return new MovementProperties()
                {
                    MaximumSpeed = 0,
                    LinearAccelerationRate = 0,
                    ExponentialAccelerationRate = 0
                };
            }
        }
    }
}
