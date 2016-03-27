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
        Vector3 _strafeDirection;

        public CombatState(AIController aiController)
            : base(aiController)
        {
            _frameMethods = new Dictionary<Func<bool>, bool>()
            {
                { AimBodyAtTarget, false },
                { AimCannonAtTarget, false }
            };
        }

        protected override void OnFixedUpdate()
        {
            _targetHeading = _aiController.Target.transform.position - _controlledEntity._rigidBody.position;
            _targetDistance = _targetHeading.magnitude;
            _targetDirection = _targetHeading / _targetDistance;

            if (_targetDistance > _aiController.MaxDistance)
            {
                _aiController.CurrentState = new HuntingState(_aiController);
                return;
            }

            if (_targetDistance < _aiController.MinDistance)
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    _controlledEntity.DesiredMovement.x,
                    _controlledEntity.DesiredMovement.y,
                    -0.5f
                );
            }
            else
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    _controlledEntity.DesiredMovement.x,
                    _controlledEntity.DesiredMovement.y,
                    0
                );
            }

            if (ExecuteFrameMethods())
            {
                _controlledEntity.Firing = true;
            }
            else
            {
                _controlledEntity.Firing = false;
            }

            if (_controlledEntity.CurrentCooldown > 0)
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    _strafeDirection.x,
                    _controlledEntity.DesiredMovement.y,
                    _controlledEntity.DesiredMovement.z
                );
            }
            else
            {
                SetNewStrafeDirection();
                _controlledEntity.DesiredMovement = new Vector3
                (
                    0,
                    _controlledEntity.DesiredMovement.y,
                    _controlledEntity.DesiredMovement.z
                );
            }
        }

        void SetNewStrafeDirection()
        {
            _strafeDirection = new Vector3
            (
                1 * UnityEngine.Random.Range(0, 1f) > 0.5f ? 1 : -1,
                0
            );
        }

        bool AimBodyAtTarget()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);
            Vector3 desiredRotationDegreesDelta = new Vector3
            (
                Mathf.DeltaAngle(_controlledEntity._rigidBody.rotation.eulerAngles.x, desiredRotation.eulerAngles.x),
                Mathf.DeltaAngle(_controlledEntity._rigidBody.rotation.eulerAngles.y, desiredRotation.eulerAngles.y),
                Mathf.DeltaAngle(_controlledEntity._rigidBody.rotation.eulerAngles.z, desiredRotation.eulerAngles.z)
            );

            _controlledEntity.DesiredBodyRotation = new Vector3
            (
                Mathf.Abs(desiredRotationDegreesDelta.x) > _aiController.DistanceAndHeightThreshold ? desiredRotationDegreesDelta.x / Mathf.Abs(desiredRotationDegreesDelta.x) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.y) > _aiController.DistanceAndHeightThreshold ? desiredRotationDegreesDelta.y / Mathf.Abs(desiredRotationDegreesDelta.y) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.z) > _aiController.DistanceAndHeightThreshold ? desiredRotationDegreesDelta.z / Mathf.Abs(desiredRotationDegreesDelta.z) : 0
            );

            return _controlledEntity.DesiredBodyRotation.magnitude <= _controlledEntity.MaxCannonAngle;
        }

        bool AimCannonAtTarget()
        {
            Vector3 targetHeadingFromCannon = _aiController.Target.transform.position - _controlledEntity.Cannon.position;
            float targetDistanceFromCannon = targetHeadingFromCannon.magnitude;
            Vector3 targetDirectionFromCannon = targetHeadingFromCannon / targetDistanceFromCannon;

            Quaternion desiredRotation = Quaternion.LookRotation(targetDirectionFromCannon, Vector3.up);
            Vector2 desiredRotationDegreesDelta = new Vector2
            (
                Mathf.DeltaAngle(_controlledEntity.Cannon.rotation.eulerAngles.x, desiredRotation.eulerAngles.x), 
                Mathf.DeltaAngle(_controlledEntity.Cannon.rotation.eulerAngles.y, desiredRotation.eulerAngles.y)
            );

            _controlledEntity.DesiredCannonRotation = new Vector2
            (
                Mathf.Abs(desiredRotationDegreesDelta.x) > 0 ? desiredRotationDegreesDelta.x / Mathf.Abs(desiredRotationDegreesDelta.x) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.y) > 0 ? desiredRotationDegreesDelta.y / Mathf.Abs(desiredRotationDegreesDelta.y) : 0
            );

            return _controlledEntity.DesiredCannonRotation.magnitude <= 5;
        }
    }
}
