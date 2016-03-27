using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AIFlyingCannonEnemy
{
    class HuntingState : AIStateBase<AIController, FlyingCannonEnemy>
    {
        /// <summary>
        /// Random value between MinDistance and MaxDistance set when state is initialised
        /// </summary>
        float _desiredDistance;

        Vector3 _targetHeading;
        float _targetDistance;
        Vector3 _targetDirection;

        public HuntingState(AIController aiController)
            : base(aiController)
        {
            FrameMethods = new Dictionary<Func<bool>, bool>()
            {
                { LookAtTarget, false },
                { MoveTowardTarget, false }
            };
            _desiredDistance = UnityEngine.Random.Range(AiController.MinDistance, AiController.MaxDistance);
        }

        protected override void OnFixedUpdate()
        {
            //Update heading / distance / direction of target
            _targetHeading = AiController.Target.transform.position - ControlledEntity.Body.position;
            _targetDistance = _targetHeading.magnitude;
            _targetDirection = _targetHeading / _targetDistance;

            ExecuteFrameMethods();

            //Don't need to have all FrameMethods satisfied, as long as we're close enough to target we can go into CombatState
            if (FrameMethods[MoveTowardTarget])
            {
                AiController.CurrentState = new CombatState(AiController);
                return;
            }
        }

        /// <summary>
        /// Commands the ControlledEntity to rotate so that it is facing the target. Returns true if current rotation is within DistanceAndHeightThreshold of the target.
        /// </summary>
        /// <returns>True if current rotation is within DistanceAndHeightThreshold of the target.</returns>
        bool LookAtTarget()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);
            Vector3 desiredRotationDegreesDelta = new Vector3
            (
                Mathf.DeltaAngle(ControlledEntity.Body.rotation.eulerAngles.x, desiredRotation.eulerAngles.x), 
                Mathf.DeltaAngle(ControlledEntity.Body.rotation.eulerAngles.y, desiredRotation.eulerAngles.y),
                Mathf.DeltaAngle(ControlledEntity.Body.rotation.eulerAngles.z, desiredRotation.eulerAngles.z)
            );

            ControlledEntity.DesiredBodyRotation = new Vector3
            (
                Mathf.Abs(desiredRotationDegreesDelta.x) > AiController.DistanceAndHeightThreshold ? Mathf.Sign(desiredRotationDegreesDelta.x) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.y) > AiController.DistanceAndHeightThreshold ? Mathf.Sign(desiredRotationDegreesDelta.y) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.z) > AiController.DistanceAndHeightThreshold ? Mathf.Sign(desiredRotationDegreesDelta.z) : 0
            );

            return ControlledEntity.DesiredBodyRotation.magnitude == 0;
        }

        /// <summary>
        /// Commands the ControlledEntity to move toward the target. Returns true if current distance is within DistanceAndHeightThreshold of desired distance.
        /// Desired distance is calculated at the start of the state to be between MinDistance and MaxDistance.
        /// </summary>
        /// <returns></returns>
        bool MoveTowardTarget()
        {
            if (_targetDistance - _desiredDistance < AiController.DistanceAndHeightThreshold)
            {
                return true;
            }

            Vector3 localTargetDirection = ControlledEntity.Body.transform.InverseTransformDirection(_targetDirection);
            ControlledEntity.DesiredMovement = new Vector3
            (
                Mathf.Sign(localTargetDirection.x),
                Mathf.Sign(localTargetDirection.y),
                Mathf.Sign(localTargetDirection.z)
            );

            return false;
        }
    }
}
