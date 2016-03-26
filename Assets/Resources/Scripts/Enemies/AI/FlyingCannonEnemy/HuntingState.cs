using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AIFlyingCannonEnemy
{
    class HuntingState : AIStateBase<AIController>
    {
        Vector3 _targetLocation;
        bool _updateTargetLocation = true;

        float _desiredDistance;
        bool _updateDesiredDistance = true;

        float _desiredHeight;
        bool _updateDesiredHeight = true;

        Vector3 _targetHeading;
        float _targetDistance;
        Vector3 _targetDirection;

        Dictionary<Func<bool>, bool> _frameResults;

        FlyingCannonEnemy _controlledEntity
        {
            get
            {
                return _aiController.ControlledEntity;
            }
        }

        public HuntingState(AIController aiController)
            : base(aiController)
        {
            _frameResults = new Dictionary<Func<bool>, bool>()
            {
                { LookAtTarget, false },
                { MoveTowardTarget, false },
                { MoveToCorrectHeight, false }
            };
        }

        public override void OnFixedUpdate()
        {
            foreach (var key in _frameResults.Keys.ToList())
            {
                _frameResults[key] = false;
            }

            bool targetLocationUpdated = false;
            if (_updateTargetLocation)
            {
                _updateTargetLocation = false;
                _targetLocation = _aiController.Target.transform.position;
                targetLocationUpdated = true;
                Debug.Log("Updated target location");
            }

            if (_updateDesiredDistance)
            {
                _updateDesiredDistance = false;
                _desiredDistance = UnityEngine.Random.Range(_aiController.MinDistance, _aiController.MaxDistance);
            }

            if (_updateDesiredHeight)
            {
                _updateDesiredDistance = false;
                _desiredHeight = UnityEngine.Random.Range(_aiController.MinHeight, _aiController.MaxHeight);
            }

            _targetHeading = _targetLocation - _controlledEntity.RigidBody.position;
            _targetDistance = _targetHeading.magnitude;
            _targetDirection = _targetHeading / _targetDistance;

            foreach (var key in _frameResults.Keys.ToList())
            {
                _frameResults[key] = key();
            }

            if (_frameResults.All(r => r.Value))
            {
                if (targetLocationUpdated)
                {
                    //Debug.Log("Hunting finished");
                    //_aiController.CurrentState = null;
                }
                else
                {
                    Debug.Log("Target in range");
                    _updateTargetLocation = true;
                }
            }
        }

        bool LookAtTarget()
        {
            Quaternion desiredRotation = Quaternion.LookRotation(_targetDirection);
            Vector3 desiredRotationDegrees = desiredRotation.eulerAngles;
            Vector3 currentRotationDegrees = _controlledEntity.RigidBody.rotation.eulerAngles;

            _controlledEntity.DesiredBodyRotation = new Vector3
            (
                Mathf.Abs(desiredRotationDegrees.x - currentRotationDegrees.x) < _aiController.DistanceAndHeightThreshold ? (desiredRotationDegrees.x > currentRotationDegrees.x ? 1 : 0) : 0,
                Mathf.Abs(desiredRotationDegrees.y - currentRotationDegrees.y) < _aiController.DistanceAndHeightThreshold ? (desiredRotationDegrees.y > currentRotationDegrees.y ? 1 : 0) : 0,
                Mathf.Abs(desiredRotationDegrees.z - currentRotationDegrees.z) < _aiController.DistanceAndHeightThreshold ? (desiredRotationDegrees.z > currentRotationDegrees.z ? 1 : 0) : 0
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
