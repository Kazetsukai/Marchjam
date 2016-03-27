using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AIFlyingCannonEnemy
{
    /// <summary>
    /// Specifies the AI used for FlyingCannonEnemy type enemies
    /// </summary>
    public class AIController : AIControllerBase<FlyingCannonEnemy>
    {
        [SerializeField] FlyingCannonEnemy ControlledEnemy;

        [Tooltip("The current target the FlyingCannonEnemy is pursuing")]
        [SerializeField] public GameObject Target;

        [Tooltip("The maximum distance that the enemy can be from the target before engaging")]
        [SerializeField] public float MaxDistance;

        [Tooltip("The minimum distance that the enemy can be from the target before backing away")]
        [SerializeField] public float MinDistance;

        [Tooltip("Tolerance for thresholds where a condition will be considered met even if it is not exactly met, for example a threshold of 5 will mean that MaxDistance = 30 will be accepted even if distance is 26")]
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
                //Reset inputs before running AI states so that there's no pollution of controls between states
                ControlledEnemy.ResetInputs();
                CurrentState.TriggerFixedUpdate();
            }
        }

        //Crude way of getting nearest vehicle for now
        public GameObject FindNearestVehicle()
        {
            Vehicle nearestVehicle = FindObjectsOfType<Vehicle>().OrderBy(v => Vector3.Distance(ControlledEntity.Body.position, v.transform.position)).FirstOrDefault();
            return nearestVehicle != null ? nearestVehicle.gameObject : null;
        }
    }
}
