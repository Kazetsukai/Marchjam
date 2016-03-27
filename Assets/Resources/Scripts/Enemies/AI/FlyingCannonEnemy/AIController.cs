using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AIFlyingCannonEnemy
{
    public class AIController : AIControllerBase<FlyingCannonEnemy>
    {
        [SerializeField] FlyingCannonEnemy ControlledEnemy;
        [SerializeField] public GameObject Target;
        [SerializeField] public float MaxDistance;
        [SerializeField] public float MinDistance;
        [SerializeField] public float DistanceAndHeightThreshold;

        AIStateBase<AIController, FlyingCannonEnemy> _currentState;
        public AIStateBase<AIController, FlyingCannonEnemy> CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                Debug.Log("Changed state to " + _currentState.GetType().Name);
                ControlledEnemy.ResetInputs();
            }
        }

        public override FlyingCannonEnemy ControlledEntity
        {
            get
            {
                return ControlledEnemy;
            }
        }

        // Use this for initialization
        void Start()
        {
            CurrentState = new HuntingState(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if (CurrentState != null)
            {
                ControlledEnemy.ResetInputs();
                CurrentState.TriggerFixedUpdate();
            }
        }
    }
}
