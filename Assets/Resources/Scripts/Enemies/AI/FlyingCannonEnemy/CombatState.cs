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
            _targetHeading = _aiController.Target.transform.position - _controlledEntity.RigidBody.position;
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
                SetNewStrafeDirection();
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
                    _strafeDirection.y,
                    _controlledEntity.DesiredMovement.z
                );
            }
            else
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    0,
                    0,
                    _controlledEntity.DesiredMovement.z
                );
            }
        }

        void SetNewStrafeDirection()
        {
            _strafeDirection = new Vector3
            (
                UnityEngine.Random.Range(0.1f, 0.2f) * UnityEngine.Random.Range(0, 1f) > 0.5f ? 1 : -1,
                UnityEngine.Random.Range(0f, 0.05f)
            );
        }

        bool AimBodyAtTarget()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(_targetDirection);
            Vector3 desiredRotationDegrees = new Vector3
            (
                desiredRotation.eulerAngles.x - (desiredRotation.eulerAngles.x > 180 ? 360 : 0),
                desiredRotation.eulerAngles.y - (desiredRotation.eulerAngles.y > 180 ? 360 : 0),
                desiredRotation.eulerAngles.z - (desiredRotation.eulerAngles.z > 180 ? 360 : 0)
            );

            Vector3 currentRotationDegrees = new Vector3
            (
                _controlledEntity.RigidBody.rotation.eulerAngles.x - (_controlledEntity.RigidBody.rotation.eulerAngles.x > 180 ? 360 : 0),
                _controlledEntity.RigidBody.rotation.eulerAngles.y - (_controlledEntity.RigidBody.rotation.eulerAngles.y > 180 ? 360 : 0),
                _controlledEntity.RigidBody.rotation.eulerAngles.z - (_controlledEntity.RigidBody.rotation.eulerAngles.z > 180 ? 360 : 0)
            );

            _controlledEntity.DesiredBodyRotation = new Vector3
            (
                Mathf.Abs(desiredRotationDegrees.x - currentRotationDegrees.x) > _controlledEntity.MaxCannonAngle ? (desiredRotationDegrees.x > currentRotationDegrees.x ? 1 : -1) : 0,
                Mathf.Abs(desiredRotationDegrees.y - currentRotationDegrees.y) > _controlledEntity.MaxCannonAngle ? (desiredRotationDegrees.y > currentRotationDegrees.y ? 1 : -1) : 0,
                Mathf.Abs(desiredRotationDegrees.z - currentRotationDegrees.z) > _controlledEntity.MaxCannonAngle ? (desiredRotationDegrees.z > currentRotationDegrees.z ? 1 : -1) : 0
            );

            return _controlledEntity.DesiredBodyRotation.magnitude == 0;
        }

        bool AimCannonAtTarget()
        {
            Vector3 targetHeadingFromCannon = _aiController.Target.transform.position - _controlledEntity.Cannon.position;
            float targetDistanceFromCannon = targetHeadingFromCannon.magnitude;
            Vector3 targetDirectionFromCannon = targetHeadingFromCannon / targetDistanceFromCannon;

            Quaternion desiredRotation = Quaternion.LookRotation(targetDirectionFromCannon);
            Vector2 desiredRotationDegrees = new Vector3
            (
                desiredRotation.eulerAngles.x - (desiredRotation.eulerAngles.x > 180 ? 360 : 0),
                desiredRotation.eulerAngles.y - (desiredRotation.eulerAngles.y > 180 ? 360 : 0)
            );

            Vector2 currentRotationDegrees = new Vector3
            (
                _controlledEntity.Cannon.rotation.eulerAngles.x - (_controlledEntity.Cannon.rotation.eulerAngles.x > 180 ? 360 : 0),
                _controlledEntity.Cannon.rotation.eulerAngles.y - (_controlledEntity.Cannon.rotation.eulerAngles.y > 180 ? 360 : 0)
            );

            _controlledEntity.DesiredBodyRotation = new Vector2
            (
                Mathf.Abs(desiredRotationDegrees.x - currentRotationDegrees.x) > 0 ? (desiredRotationDegrees.x > currentRotationDegrees.x ? 1 : -1) : 0,
                Mathf.Abs(desiredRotationDegrees.y - currentRotationDegrees.y) > 0 ? (desiredRotationDegrees.y > currentRotationDegrees.y ? 1 : -1) : 0
            );

            return _controlledEntity.DesiredCannonRotation.magnitude == 0;
        }
    }
}
