using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Provides a base type allowing configurable movement settings, while allowing the implementation of actual movement
    /// to be defined by the inheriting class. The inheriting class only needs to provide a method which applies movement,
    /// and a method which provides the current movement.
    /// </summary>
    public abstract class EntityMovementBase : MonoBehaviour, IMovementInputReceiver
    {
        //As per the comment in MoveDirection.cs there's some weird stuff with MoveDirection and MoveDirectionDistinct
        //
        //Basically this construct exists to allow movement properties to be specified in the Unity Editor, for example
        //a prefab can be configured with a DirectionMovementProperties object with just one element, with a MoveDirection
        //specified of MoveDirection.All which allows the same properties to be applied to movement in all directions.
        //
        //Alternatively a prefab could be configured to move fast in a forward (MoveDirection.ZAxisPositive) direction, but slower
        //in all other directions.
        //
        //Once we get to actually calculating which way to move, however, we need a distinct direction and properties that apply
        //to movement in that direction. This is what MoveDirectionDistinct exists for, to provide an enumeration that lists
        //just the numbers that are distinct. We then provide a bunch of utility methods to provide for conversions between
        //MoveDirection, MoveDirectionDistinct, and Vector3

        [Header("Note that the first direction specified will be the default", order = 0)]
        [Header("Any subsequent < 0 numbers will revert to default.", order = 1)]
        public DirectionMovementProperties[] Movement = new DirectionMovementProperties[]
        {
            new DirectionMovementProperties()
            {
                ApplicableDirection = MoveDirection.All
            }
        };

        /// <summary>
        /// Inheriting types must override this method to allow for movement to be triggered
        /// </summary>
        /// <param name="force">A vector representing the force to be applied on all 3 axes</param>
        protected abstract void AddRelativeForce(Vector3 force);

        /// <summary>
        /// Inheriting types must override this method to provide the current movement
        /// </summary>
        /// <returns>Vector3 representing the current velocity on all 3 axes</returns>
        protected abstract Vector3 GetCurrentVelocity();

        private MovementProperties _defaultMovementProperties;
        public MovementProperties DefaultMovementProperties
        {
            get
            {
                if (_defaultMovementProperties == null)
                {
                    DirectionMovementProperties firstMoveProps = Movement.FirstOrDefault();
                    _defaultMovementProperties = firstMoveProps != null ? firstMoveProps.Properties : MovementProperties.Empty;
                }

                return _defaultMovementProperties;
            }
        }

        public MovementProperties this[MoveDirectionDistinct direction]
        {
            get
            {
                DirectionMovementProperties matchingProperties = Movement.FirstOrDefault(p => (p.ApplicableDirection & (MoveDirection)direction) > 0);
                return matchingProperties != null ? matchingProperties.Properties : DefaultMovementProperties;
            }
        }

        public void MoveInDirection(Vector3 directionAndDesiredSpeedPercent)
        {
            _queuedMovement += directionAndDesiredSpeedPercent;
        }

        public void MoveInDirection(float desiredSpeedPercent, MoveDirection direction)
        {
            _queuedMovement += (GetVector3DirectionFromMoveDirection(direction) * desiredSpeedPercent);
        }

        /// <summary>
        /// Moves in the directions specified by the Vector3, i.e. 1,0,0 is equal to MoveDirection.XAxisPositive at 100%, 
        /// -1,0,0 is equal to MoveDirection.XAxisNegative at 100%, 0,0.5f,0 is equal to MoveDirection.YAxisPositive at 50%
        /// </summary>
        /// <param name="directionAndDesiredSpeedPercent"></param>
        protected void ApplyMovementInDirection(Vector3 directionAndDesiredSpeedPercent)
        {
            foreach (KeyValuePair<MoveDirectionDistinct, float> direction in GetScaledMoveDirectionFromVector3Direction(directionAndDesiredSpeedPercent))
            {
                ApplyMovementInDirection(direction.Value, direction.Key);
            }
        }

        /// <summary>
        /// Moves in the specified direction targeting the specified percentage, e.g. if maximum speed is 50 and desiredSpeedPercent is 0.5, 
        /// velocity will be set to MinimumSpeed &lt;= velocity &lt;= 25
        /// </summary>
        /// <param name="desiredSpeedPercent">Percentage of maximum speed desired, from 0.0f to 1.0f</param>
        /// <param name="direction">Direction(s) to move in</param>
        protected void ApplyMovementInDirection(float desiredSpeedPercent, MoveDirection direction)
        {
            Vector3 forceModifier = Vector3.zero;

            foreach (MoveDirection distinctDirection in Enum.GetValues(typeof(MoveDirectionDistinct)))
            {
                if ((direction & distinctDirection) > 0)
                {
                    ApplyMovementInDirection(desiredSpeedPercent, (MoveDirectionDistinct)distinctDirection);
                }
            }
        }

        /// <summary>
        /// Moves in the specified direction targeting the specified percentage, e.g. if maximum speed is 50 and desiredSpeedPercent is 0.5, 
        /// velocity will be set to MinimumSpeed &lt;= velocity &lt;= 25
        /// </summary>
        /// <param name="desiredSpeedPercent">Percentage of maximum speed desired, from 0.0f to 1.0f</param>
        /// <param name="distinctDirection">Direction(s) to move in</param>
        protected void ApplyMovementInDirection(float desiredSpeedPercent, MoveDirectionDistinct distinctDirection)
        {
            if (!Enum.IsDefined(typeof(MoveDirectionDistinct), distinctDirection))
            {
                ApplyMovementInDirection(desiredSpeedPercent, (MoveDirection)distinctDirection);
                return;
            }

            AddRelativeForce(GetForceForDirection(desiredSpeedPercent, distinctDirection));
            return;
        }

        /// <summary>
        /// Calculates the relative force Vector3 to be added based on the desired speed and direction.
        /// </summary>
        /// <param name="desiredSpeedPercent">Percentage of maximum speed desired, from 0.0f to 1.0f</param>
        /// <param name="distinctDirection">Direction to move in</param>
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
                    case MoveDirectionDistinct.ZAxisPositive:
                        distinctDirection = MoveDirectionDistinct.ZAxisNegative;
                        break;
                    case MoveDirectionDistinct.ZAxisNegative:
                        distinctDirection = MoveDirectionDistinct.ZAxisPositive;
                        break;
                    case MoveDirectionDistinct.XAxisPositive:
                        distinctDirection = MoveDirectionDistinct.XAxisNegative;
                        break;
                    case MoveDirectionDistinct.XAxisNegative:
                        distinctDirection = MoveDirectionDistinct.XAxisPositive;
                        break;
                    case MoveDirectionDistinct.YAxisPositive:
                        distinctDirection = MoveDirectionDistinct.YAxisNegative;
                        break;
                    case MoveDirectionDistinct.YAxisNegative:
                        distinctDirection = MoveDirectionDistinct.YAxisPositive;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            //Normalise desired speed
            desiredSpeedPercent = Mathf.Min(1, Mathf.Abs(desiredSpeedPercent));

            //Find settings that apply to the specified direction, if there are none we can't move in this direction
            MovementProperties applicableProperties = this[distinctDirection];

            Vector3 localVelocity = GetCurrentVelocity();
            Vector3 directionAsVector = GetVector3DirectionFromMoveDirection(distinctDirection);
            localVelocity.Scale(directionAsVector);

            return directionAsVector * GetIncreaseInVelocity(applicableProperties, localVelocity.magnitude, desiredSpeedPercent);
        }

        /// <summary>
        /// Calculates the amount of relative force that should be added based on the entity's movement
        /// properties, current velocity in the direction, and desired speed in that direction.
        /// </summary>
        /// <param name="applicableProperties">MoveProperties that are applicable for this direction</param>
        /// <param name="currentVelocity">Current velocity in this direction</param>
        /// <param name="normalisedDesiredSpeedPercent">Percentage of maximum speed desired in this direction, from 0.0f to 1.0f</param>
        /// <returns></returns>
        private float GetIncreaseInVelocity(MovementProperties applicableProperties, float currentVelocity, float normalisedDesiredSpeedPercent)
        {
            MovementProperties activeProperties = MovementProperties.GetActiveProperties(applicableProperties, DefaultMovementProperties);

            if (normalisedDesiredSpeedPercent <= 0 ||
                activeProperties.MaximumSpeed <= 0 ||
                activeProperties.LinearAccelerationRate <= 0)
            {
                return 0f;
            }

            //Cap target speed at maximum speed
            float targetSpeed = Mathf.Min(normalisedDesiredSpeedPercent * activeProperties.MaximumSpeed, activeProperties.MaximumSpeed);

            //If we're already exceeding or at target speed, don't need to increase further
            if (currentVelocity >= targetSpeed)
            {
                return 0f;
            }

            float speedModifier = 0f;

            if (activeProperties.ExponentialAccelerationRate <= 0 || currentVelocity < activeProperties.LinearAccelerationRate)
            {
                speedModifier = activeProperties.LinearAccelerationRate;
            }
            else
            {
                speedModifier = (currentVelocity * activeProperties.ExponentialAccelerationRate);
            }

            //Cap speed modifier so that we don't exceed maximum speed
            if (currentVelocity + speedModifier > activeProperties.MaximumSpeed)
            {
                speedModifier = activeProperties.MaximumSpeed - currentVelocity;
            }

            return speedModifier;
        }

        /// <summary>
        /// Returns the corresponding Vector3 for the given MoveDirection
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <returns></returns>
        private Vector3 GetVector3DirectionFromMoveDirection(MoveDirection moveDirection)
        {
            Vector3 vectorDirection = Vector3.zero;
            foreach (MoveDirection distinctDirection in Enum.GetValues(typeof(MoveDirectionDistinct)))
            {
                if ((moveDirection & distinctDirection) > 0)
                {
                    vectorDirection += GetVector3DirectionFromMoveDirection((MoveDirectionDistinct)distinctDirection);
                }
            }

            return vectorDirection;
        }

        /// <summary>
        /// Returns the corresponding Vector3 for the given MoveDirection
        /// </summary>
        /// <param name="distinctDirection"></param>
        /// <returns></returns>
        private Vector3 GetVector3DirectionFromMoveDirection(MoveDirectionDistinct distinctDirection)
        {
            if (!Enum.IsDefined(typeof(MoveDirectionDistinct), distinctDirection))
            {
                return GetVector3DirectionFromMoveDirection((MoveDirection)distinctDirection);
            }

            switch (distinctDirection)
            {
                case MoveDirectionDistinct.ZAxisPositive:
                    return new Vector3(0, 0, 1);
                case MoveDirectionDistinct.ZAxisNegative:
                    return new Vector3(0, 0, -1);
                case MoveDirectionDistinct.XAxisPositive:
                    return new Vector3(1, 0, 0);
                case MoveDirectionDistinct.XAxisNegative:
                    return new Vector3(-1, 0, 0);
                case MoveDirectionDistinct.YAxisPositive:
                    return new Vector3(0, 1, 0);
                case MoveDirectionDistinct.YAxisNegative:
                    return new Vector3(0, -1, 0);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Returns the corresponding MoveDirection for the given Vector3 direction
        /// </summary>
        /// <param name="direction">A Vector3 representing the direction</param>
        /// <returns></returns>
        private MoveDirection GetMoveDirectionFromVector3Direction(Vector3 direction)
        {
            Vector3 normalisedDirection = direction.normalized;
            int response = 0;
            if (normalisedDirection.x > 0)
            {
                response += (int)MoveDirection.XAxisPositive;
            }

            if (normalisedDirection.x < 0)
            {
                response += (int)MoveDirection.XAxisNegative;
            }

            if (normalisedDirection.y > 0)
            {
                response += (int)MoveDirection.YAxisPositive;
            }

            if (normalisedDirection.y < 0)
            {
                response += (int)MoveDirection.YAxisNegative;
            }

            if (normalisedDirection.z > 0)
            {
                response += (int)MoveDirection.ZAxisPositive;
            }

            if (normalisedDirection.z < 0)
            {
                response += (int)MoveDirection.ZAxisNegative;
            }

            return (MoveDirection)response;
        }

        /// <summary>
        /// Returns a Dictionary of distinct move directions with the scale of the given Vector3 for the relevant directions
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Dictionary<MoveDirectionDistinct, float> GetScaledMoveDirectionFromVector3Direction(Vector3 direction)
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

        public void FixedUpdate()
        {
            if (_queuedMovement.sqrMagnitude > 0)
            {
                ApplyMovementInDirection(_queuedMovement);
            }

            _queuedMovement = Vector3.zero;
        }

        private Vector3 _queuedMovement = Vector3.zero;
    }
}
