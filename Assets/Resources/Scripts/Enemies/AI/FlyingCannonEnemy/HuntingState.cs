using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AIFlyingCannonEnemy
{
    class HuntingState : AIStateBase<FlyingCannonEnemy, AIController>
    {
        public HuntingState(FlyingCannonEnemy controlledEntity, AIController stateOwner)
            : base(controlledEntity, stateOwner)
        {
            
        }

        public override void OnFixedUpdate()
        {
               
        }
    }
}
