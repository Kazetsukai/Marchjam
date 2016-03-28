using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public class LinearMovingPhysicalEntity : PhysicalEntity
    {
        [Header("Note that the first direction specified will be the default", order = 0)]
        [Header("Any subsequent < 0 numbers will revert to default.", order = 1)]
        public MoveProperties[] MovementProperties = new MoveProperties[]
        {
            new MoveProperties()
            {
                ApplicableDirection = MoveDirection.All
            }
        };

        /// <summary>
        /// Returns the first MoveProperties item from MovementProperties
        /// </summary>
        public MoveProperties DefaultMoveProperties
        {
            get
            {
                return MovementProperties[0];
            }
        }

        /// <summary>
        /// Returns the corresponding Vector3 for the given MoveDirection
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <returns></returns>
        public Vector3 GetVector3DirectionFromMoveDirection(MoveDirection moveDirection)
        {
            Vector3 vectorDirection = Vector3.zero;
            foreach (MoveDirection distinctDirection in Enum.GetValues(typeof(MoveDirectionDistinct)))
            {
                if ((moveDirection & distinctDirection) > 0)
                {
                    vectorDirection += GetVector3DirectionFromMoveDirection(distinctDirection);
                }
            }

            return vectorDirection;
        }

        /// <summary>
        /// Returns the corresponding Vector3 for the given MoveDirectionDistinct
        /// </summary>
        /// <param name="distinctDirection"></param>
        /// <returns></returns>
        public Vector3 GetVector3DirectionFromMoveDirection(MoveDirectionDistinct distinctDirection)
        {
            if (!Enum.IsDefined(typeof(MoveDirectionDistinct), distinctDirection))
            {
                throw new ArgumentException("Invalid value supplied for direction:" + distinctDirection.ToString());
            }

            switch (distinctDirection)
            {
                case MoveDirectionDistinct.Forward:
                    return Vector3.forward;
                case MoveDirectionDistinct.Backward:
                    return Vector3.back;
                case MoveDirectionDistinct.Right:
                    return Vector3.right;
                case MoveDirectionDistinct.Left:
                    return Vector3.left;
                case MoveDirectionDistinct.Up:
                    return Vector3.up;
                case MoveDirectionDistinct.Down:
                    return Vector3.down;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the corresponding MoveDirection for the given Vector3 direction
        /// </summary>
        /// <param name="direction">A Vector3 representing the direction</param>
        /// <returns></returns>
        public MoveDirection GetMoveDirectionFromVector3Direction(Vector3 direction)
        {
            Vector3 normalisedDirection = direction.normalized;
            int response = 0;
            if (normalisedDirection.x > 0)
            {
                response += (int)MoveDirection.Right;
            }

            if (normalisedDirection.x < 0)
            {
                response += (int)MoveDirection.Left;
            }

            if (normalisedDirection.y > 0)
            {
                response += (int)MoveDirection.Up;
            }

            if (normalisedDirection.y < 0)
            {
                response += (int)MoveDirection.Down;
            }

            if (normalisedDirection.z > 0)
            {
                response += (int)MoveDirection.Forward;
            }

            if (normalisedDirection.z < 0)
            {
                response += (int)MoveDirection.Backward;
            }

            return (MoveDirection)response;
        }

        /// <summary>
        /// Returns a Dictionary of distinct move directions with the scale of the given Vector3 for the relevant directions
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Dictionary<MoveDirectionDistinct, float> GetScaledMoveDirectionFromVector3Direction(Vector3 direction)
        {
            MoveDirection directions = GetMoveDirectionFromVector3Direction(direction);

            Dictionary<MoveDirectionDistinct, float> scaledDirections = new Dictionary<MoveDirectionDistinct, float>();

            foreach (MoveDirection distinctDirection in Enum.GetValues(typeof(MoveDirectionDistinct)))
            {
                if ((directions & distinctDirection) > 0)
                {
                    Vector3 thisDirection = new Vector3(direction.x, direction.y, direction.z);
                    thisDirection.Scale(GetVector3DirectionFromMoveDirection((MoveDirectionDistinct)distinctDirection));
                    scaledDirections.Add((MoveDirectionDistinct)distinctDirection, thisDirection.magnitude);
                }
            }

            return scaledDirections;
        }

        /// <summary>
        /// Moves in the specified direction targeting the specified percentage, e.g. if maximum speed is 50 and desiredSpeedPercent is 0.5, 
        /// velocity will be set to MinimumSpeed &lt;= velocity &lt;= 25
        /// </summary>
        /// <param name="desiredSpeedPercent">Percentage of maximum speed desired, from 0.0f to 1.0f</param>
        /// <param name="direction">Direction(s) to move in</param>
        public void MoveInDirection(float desiredSpeedPercent, MoveDirection direction)
        {
            Vector3 forceModifier = Vector3.zero;

            foreach (MoveDirection distinctDirection in Enum.GetValues(typeof(MoveDirectionDistinct)))
            {
                if ((direction & distinctDirection) > 0)
                {
                    MoveInDirection(desiredSpeedPercent, (MoveDirectionDistinct)distinctDirection);
                }
            }
        }

        /// <summary>
        /// Moves in the specified direction targeting the specified percentage, e.g. if maximum speed is 50 and desiredSpeedPercent is 0.5, 
        /// velocity will be set to MinimumSpeed &lt;= velocity &lt;= 25
        /// </summary>
        /// <param name="desiredSpeedPercent">Percentage of maximum speed desired, from 0.0f to 1.0f</param>
        /// <param name="distinctDirection">Direction(s) to move in</param>
        public void MoveInDirection(float desiredSpeedPercent, MoveDirectionDistinct distinctDirection)
        {
            if (!Enum.IsDefined(typeof(MoveDirectionDistinct), distinctDirection))
            {
                throw new ArgumentException("Invalid value supplied for direction:" + distinctDirection.ToString());
            }

            Body.AddRelativeForce(GetForceForDirection(desiredSpeedPercent, distinctDirection));
        }

        /// <summary>
        /// Moves in the directions specified by the Vector3, i.e. 1,0,0 is equal to MoveDirection.Right at 100%, 
        /// -1,0,0 is equal to MoveDirection.Left at 100%, 0,0.5f,0 is equal to MoveDirection.Up at 50%
        /// </summary>
        /// <param name="directionAndDesiredSpeedPercent"></param>
        public void MoveInDirection(Vector3 directionAndDesiredSpeedPercent)
        {
            foreach (KeyValuePair<MoveDirectionDistinct, float> direction in GetScaledMoveDirectionFromVector3Direction(directionAndDesiredSpeedPercent))
            {
                MoveInDirection(direction.Value, (MoveDirection)direction.Key);
            }
        }

        /// <summary>
        /// Calculates the relative force Vector3 to add to the RigidBody based on the desired speed and direction.
        /// </summary>
        /// <param name="desiredSpeedPercent">Percentage of maximum speed desired, from 0.0f to 1.0f</param>
        /// <param name="distinctDirection">Distinct direction to move in</param>
        /// <returns></returns>
        private Vector3 GetForceForDirection(float desiredSpeedPercent, MoveDirectionDistinct distinctDirection)
        {
            if (!Enum.IsDefined(typeof(MoveDirectionDistinct), distinctDirection))
            {
                throw new ArgumentException("Invalid value supplied for direction:" + distinctDirection.ToString());
            }

            //Invert direction if less than 0
            if (desiredSpeedPercent < 0)
            {
                switch (distinctDirection)
                {
                    case MoveDirectionDistinct.Forward:
                        distinctDirection = MoveDirectionDistinct.Backward;
                        break;
                    case MoveDirectionDistinct.Backward:
                        distinctDirection = MoveDirectionDistinct.Forward;
                        break;
                    case MoveDirectionDistinct.Right:
                        distinctDirection = MoveDirectionDistinct.Left;
                        break;
                    case MoveDirectionDistinct.Left:
                        distinctDirection = MoveDirectionDistinct.Right;
                        break;
                    case MoveDirectionDistinct.Up:
                        distinctDirection = MoveDirectionDistinct.Down;
                        break;
                    case MoveDirectionDistinct.Down:
                        distinctDirection = MoveDirectionDistinct.Up;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            //Normalise desired speed
            desiredSpeedPercent = Mathf.Min(1, Mathf.Abs(desiredSpeedPercent));

            //Find settings that apply to the specified direction, if there are none we can't move in this direction
            MoveProperties applicableProperties = MovementProperties.FirstOrDefault(p => (p.ApplicableDirection & (MoveDirection)distinctDirection) > 0);
            if (applicableProperties == null)
            {
                return Vector3.zero;
            }

            Vector3 localVelocity = Body.transform.InverseTransformDirection(Body.velocity);
            Vector3 directionAsVector = GetVector3DirectionFromMoveDirection(distinctDirection);
            localVelocity.Scale(directionAsVector);

            return directionAsVector * GetIncreaseInVelocity(applicableProperties, localVelocity.magnitude, desiredSpeedPercent);
        }

        /// <summary>
        /// Calculates the amount of relative force that should be added to the RigidBody based on the entity's
        /// movement properties, current velocity in the direction, and desired speed in that direction.
        /// </summary>
        /// <param name="applicableProperties">MoveProperties that are applicable for this direction</param>
        /// <param name="currentVelocity">Current velocity in this direction</param>
        /// <param name="normalisedDesiredSpeedPercent">Percentage of maximum speed desired in this direction, from 0.0f to 1.0f</param>
        /// <returns></returns>
        private float GetIncreaseInVelocity(MoveProperties applicableProperties, float currentVelocity, float normalisedDesiredSpeedPercent)
        {
            MoveProperties activeProperties = GetMovePropertiesWithDefaultsApplied(applicableProperties);

            if (normalisedDesiredSpeedPercent <= 0)
            {
                return 0f;
            }

            if (activeProperties.MaximumSpeed <= 0)
            {
                return 0f;
            }

            //Boost to minimum speed if we're currently moving at less than the minimum speed
            if (activeProperties.MinimumSpeed > 0 && currentVelocity < activeProperties.MinimumSpeed)
            {
                return activeProperties.MinimumSpeed;
            }

            //Target speed must be between minimum and maximum speed
            float targetSpeed = Mathf.Max(activeProperties.MinimumSpeed, Mathf.Min(normalisedDesiredSpeedPercent * activeProperties.MaximumSpeed, activeProperties.MaximumSpeed));

            //If we're already exceeding or at target speed, don't need to increase further
            if (currentVelocity >= targetSpeed)
            {
                return 0f;
            }

            float speedModifier = 0f;

            if (activeProperties.LinearAccelerationRate >= 0)
            {
                speedModifier = activeProperties.LinearAccelerationRate;
            }
            else if (activeProperties.ExponentialAccelerationRate >= 0)
            {
                speedModifier = (currentVelocity * activeProperties.ExponentialAccelerationRate);
            }

            speedModifier *= Time.fixedDeltaTime;

            //Cap speed modifier so that we don't exceed maximum speed
            if (currentVelocity + speedModifier > activeProperties.MaximumSpeed)
            {
                speedModifier = activeProperties.MaximumSpeed - currentVelocity;
            }

            return speedModifier;
        }

        /// <summary>
        /// Returns a MoveProperties object where all &lt; 0 values are replaced with the value from the default
        /// </summary>
        /// <param name="applicableProperties">MoveProperties object which may or may not contain &lt; 0 values to be replaced with defaults.</param>
        /// <returns></returns>
        public MoveProperties GetMovePropertiesWithDefaultsApplied(MoveProperties applicableProperties)
        {
            if (applicableProperties == DefaultMoveProperties)
            {
                return applicableProperties;
            }

            return new MoveProperties()
            {
                ApplicableDirection = applicableProperties.ApplicableDirection,
                MaximumSpeed = applicableProperties.MaximumSpeed >= 0 ? applicableProperties.MaximumSpeed : DefaultMoveProperties.MaximumSpeed,
                MinimumSpeed = applicableProperties.MinimumSpeed >= 0 ? applicableProperties.MinimumSpeed : DefaultMoveProperties.MinimumSpeed,
                //The below two properties can be < 0 for valid reason, if the other property is set to >= 0, so only get the default if both values are less than 0
                LinearAccelerationRate = applicableProperties.LinearAccelerationRate >= 0 || applicableProperties.ExponentialAccelerationRate >= 0 ? applicableProperties.LinearAccelerationRate : DefaultMoveProperties.LinearAccelerationRate,
                ExponentialAccelerationRate = applicableProperties.LinearAccelerationRate >= 0 || applicableProperties.ExponentialAccelerationRate >= 0 ? applicableProperties.ExponentialAccelerationRate : DefaultMoveProperties.ExponentialAccelerationRate
            };
        }
    }
}
