using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AIFlyingCannonEnemy
{
    class HuntingState : AIStateBase<AIController, FlyingCannonEnemy>
    {
        float _desiredDistance;

        Vector3 _targetHeading;
        float _targetDistance;
        Vector3 _targetDirection;

        public HuntingState(AIController aiController)
            : base(aiController)
        {
            _frameMethods = new Dictionary<Func<bool>, bool>()
            {
                { LookAtTarget, false },
                { MoveTowardTarget, false }
            };
            _desiredDistance = UnityEngine.Random.Range(_aiController.MinDistance, _aiController.MaxDistance);
        }

        protected override void OnFixedUpdate()
        {
            _targetHeading = _aiController.Target.transform.position - _controlledEntity._rigidBody.position;
            _targetDistance = _targetHeading.magnitude;
            _targetDirection = _targetHeading / _targetDistance;

            ExecuteFrameMethods();

            if (_frameMethods[MoveTowardTarget])
            {
            _aiController.CurrentState = new CombatState(_aiController);
            return;
            }
        }

        bool LookAtTarget()
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
                Mathf.Abs(desiredRotationDegreesDelta.x) > _aiController.DistanceAndHeightThreshold ? desiredRotationDegreesDelta.x / Mathf.Abs(desiredRotationDegreesDelta.x): 0,
                Mathf.Abs(desiredRotationDegreesDelta.y) > _aiController.DistanceAndHeightThreshold ? desiredRotationDegreesDelta.y / Mathf.Abs(desiredRotationDegreesDelta.y) : 0,
                Mathf.Abs(desiredRotationDegreesDelta.z) > _aiController.DistanceAndHeightThreshold ? desiredRotationDegreesDelta.z / Mathf.Abs(desiredRotationDegreesDelta.z) : 0
            );

            return _controlledEntity.DesiredBodyRotation.magnitude == 0;
        }

        bool MoveTowardTarget()
        {
            if (_targetDistance - _desiredDistance < _aiController.DistanceAndHeightThreshold)
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    0,
                    _controlledEntity.DesiredMovement.y,
                    0
                );
                return true;
            }

            Vector3 localTargetDirection = _controlledEntity._rigidBody.transform.InverseTransformDirection(_targetDirection);
            _controlledEntity.DesiredMovement = new Vector3
            (
                localTargetDirection.x,
                localTargetDirection.y,
                localTargetDirection.z
            );

            return false;
        }
    }
}
