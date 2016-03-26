using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AIFlyingCannonEnemy
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] public FlyingCannonEnemy ControlledEntity;
        [SerializeField] public GameObject Target;
        [SerializeField] public float MaxDistance;
        [SerializeField] public float MinDistance;
        [SerializeField] public float MaxHeight;
        [SerializeField] public float MinHeight;
        [SerializeField] public float DistanceAndHeightThreshold;

        public AIStateBase<AIController> CurrentState;

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
                CurrentState.OnFixedUpdate();
            }
        }
    }
}
