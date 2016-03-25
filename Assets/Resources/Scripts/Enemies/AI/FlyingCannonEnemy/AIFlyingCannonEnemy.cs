using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class AIFlyingCannonEnemy : MonoBehaviour {
    enum AIState
    {
        Hunting
    }

    Dictionary<AIState, Type> AIStates = new Dictionary<AIState, Type>()
    {
        { AIState.Hunting, typeof(HuntingState) }
    };

    [SerializeField] FlyingCannonEnemy ControlledEntity;
    [SerializeField] GameObject Target;

    AIStateBase<FlyingCannonEnemy, AIFlyingCannonEnemy> _currentState;

    // Use this for initialization
    void Start () {
        _currentState = AIStateBase<FlyingCannonEnemy, AIFlyingCannonEnemy>.InstantiateState(AIStates[AIState.Hunting], ControlledEntity, this);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }
}
