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
        float _desiredHeight;

        Vector3 _targetHeading;
        float _targetDistance;
        Vector3 _targetDirection;    

        public HuntingState(AIController aiController)
            : base(aiController)
        {
            _frameMethods = new Dictionary<Func<bool>, bool>()
            {
                { LookAtTarget, false },
                { MoveTowardTarget, false },
                { MoveToCorrectHeight, false }
            };
            _desiredDistance = UnityEngine.Random.Range(_aiController.MinDistance, _aiController.MaxDistance);
            _desiredHeight = UnityEngine.Random.Range(_aiController.MinHeight, _aiController.MaxHeight);
        }

        protected override void OnFixedUpdate()
        {           
            _targetHeading = _aiController.Target.transform.position - _controlledEntity.RigidBody.position;
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
                Mathf.Abs(desiredRotationDegrees.x - currentRotationDegrees.x) > _aiController.DistanceAndHeightThreshold ? (desiredRotationDegrees.x > currentRotationDegrees.x ? 1 : -1) : 0,
                Mathf.Abs(desiredRotationDegrees.y - currentRotationDegrees.y) > _aiController.DistanceAndHeightThreshold ? (desiredRotationDegrees.y > currentRotationDegrees.y ? 1 : -1) : 0,
                Mathf.Abs(desiredRotationDegrees.z - currentRotationDegrees.z) > _aiController.DistanceAndHeightThreshold ? (desiredRotationDegrees.z > currentRotationDegrees.z ? 1 : -1) : 0
            );

            return _controlledEntity.DesiredBodyRotation.magnitude == 0;
        }

        bool MoveTowardTarget()
        {
            if (Mathf.Abs(_desiredDistance - _targetDistance) < _aiController.DistanceAndHeightThreshold)
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    0,
                    _controlledEntity.DesiredMovement.y,
                    0
                );
                return true;
            }

            Vector3 localTargetDirection = _controlledEntity.RigidBody.transform.InverseTransformDirection(_targetDirection);
            _controlledEntity.DesiredMovement = new Vector3
            (
                localTargetDirection.x,
                _controlledEntity.DesiredMovement.y,
                localTargetDirection.z
            );

            return false;
        }

        bool MoveToCorrectHeight()
        {
            if (Mathf.Abs(_desiredHeight - _controlledEntity.RigidBody.position.y) < _aiController.DistanceAndHeightThreshold)
            {
                _controlledEntity.DesiredMovement = new Vector3
                (
                    _controlledEntity.DesiredMovement.x,
                    0,
                    _controlledEntity.DesiredMovement.z
                );
                return true;
            }

            _controlledEntity.DesiredMovement = new Vector3
            (
                _controlledEntity.DesiredMovement.x,
                (_desiredHeight - _controlledEntity.RigidBody.position.y) > 0 ? 1 : -1,
                _controlledEntity.DesiredMovement.z
            );

            return false;
        }
    }
}
