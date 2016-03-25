using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class AIFlyingCannonEnemy : MonoBehaviour
{
    class HuntingState : AIStateBase<FlyingCannonEnemy, AIFlyingCannonEnemy>
    {
        public HuntingState(FlyingCannonEnemy controlledEntity, AIFlyingCannonEnemy stateOwner)
            : base(controlledEntity, stateOwner)
        {
            
        }

        public override void OnFixedUpdate()
        {
               
        }
    }
}
