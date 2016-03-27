using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AIFlyingCannonEnemy
{
    class CombatState : AIStateBase<AIController, FlyingCannonEnemy>
    {
        Vector3 _targetHeading;
        float _targetDistance;
        Vector3 _targetDirection;

        /// <summary>
        /// Direction to move after firing, updated after firing
        /// </summary>
        Vector3 _strafeDirection;

        public CombatState(AIController aiController)
            : base(aiController)
        {
            FrameMethods = new Dictionary<Func<bool>, bool>()
            {
                { AimBodyAtTarget, false },
                { AimCannonAtTarget, false }
            };
        }

        protected override void OnFixedUpdate()
        {
            //Update heading / distance / direction of target
            _targetHeading = AiController.Target.transform.position - ControlledEntity.Body.position;
            _targetDistance = _targetHeading.magnitude;
            _targetDirection = _targetHeading / _targetDistance;

            //Revert to hunting state if target gets out of MaxDistance
            if (_targetDistance > AiController.MaxDistance)
            {
                AiController.CurrentState = new HuntingState(AiController);
                return;
            }

            //Back off if target gets too close
            if (_targetDistance < AiController.MinDistance)
            {
                ControlledEntity.DesiredMovement.Set
                (
                    ControlledEntity.DesiredMovement.x,
                    ControlledEntity.DesiredMovement.y,
                    -1f
                );
            }

            //Strafe while cannon is cooling down
            if (ControlledEntity.CurrentCooldown > 0)
            {
                ControlledEntity.DesiredMovement.Set
                (
                    _strafeDirection.x,
                    ControlledEntity.DesiredMovement.y,
                    ControlledEntity.DesiredMovement.z
                );
            }


            if (ExecuteFrameMethods())
            {
                ControlledEntity.Firing = true;
                if (ControlledEntity.CurrentCooldown == 0)
                {
                    SetNewStrafeDirection();
                }
            }
            else
            {
                ControlledEntity.Firing = false;
            }
        }

        /// <summary>
        /// Randomly sets _strafeDirection either a left or right Vector3
        /// </summary>
        void SetNewStrafeDirection()
        {
            _strafeDirection = UnityEngine.Random.Range(0, 1f) > 0.5f ? Vector3.left : Vector3.right;
        }

        /// <summary>
        /// Commands the ControlledEntity to rotate so that it is facing the target. Returns true of the current rotation is within the angle that the Cannon rotation can be aimed at the target.
        /// </summary>
        /// <returns>True of the current rotation is within the angle that the Cannon rotation can be aimed at the target.</returns>
        bool AimBodyAtTarget()
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

            return ControlledEntity.DesiredBodyRotation.magnitude <= ControlledEntity.MaxCannonAngle;
        }

        /// <summary>
        /// Commands the ControlledEntity to aim it's cannon at the target. Returns true if current cannon rotation is within DistanceAndHeightThreshold of the target.
        /// </summary>
        /// <returns>True if current cannon rotation is within DistanceAndHeightThreshold of the target.</returns>
        bool AimCannonAtTarget()
        {
            //Calculate headings / direction from cannon offset
            Vector3 targetHeadingFromCannon = AiController.Target.transform.position - ControlledEntity.Cannon.position;
            float targetDistanceFromCannon = targetHeadingFromCannon.magnitude;
            Vector3 targetDirectionFromCannon = targetHeadingFromCannon / targetDistanceFromCannon;

            Quaternion desiredRotation = Quaternion.LookRotation(targetDirectionFromCannon, Vector3.up);
            Vector2 desiredRotationDegreesDelta = new Vector2
            (
                Mathf.DeltaAngle(ControlledEntity.Cannon.rotation.eulerAngles.x, desiredRotation.eulerAngles.x), 
                Mathf.DeltaAngle(ControlledEntity.Cannon.rotation.eulerAngles.y, desiredRotation.eulerAngles.y)
            );

            ControlledEntity.DesiredCannonRotation = new Vector2
            (
                Mathf.Abs(desiredRotationDegreesDelta.x) > 0 ? Mathf.Sign(desiredRotationDegreesDelta.x) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.y) > 0 ? Mathf.Sign(desiredRotationDegreesDelta.y) : 0
            );

            return ControlledEntity.DesiredCannonRotation.magnitude <= AiController.DistanceAndHeightThreshold;
        }
    }
}
