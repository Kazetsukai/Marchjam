using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

abstract class AIStateBase<EntityType, AIType>
{
    public static AIStateBase<EntityType, AIType> InstantiateState(Type newState, EntityType controlledEntity, AIType stateOwner)
    {
        if (!typeof(AIStateBase<EntityType, AIType>).IsAssignableFrom(newState))
        {
            throw new InvalidOperationException("Wrong type: " + newState.Name);
        }

        ConstructorInfo stateConstructor = newState.GetConstructor(new Type[] { typeof(EntityType), typeof(AIType) });
        return stateConstructor.Invoke(new object[] { controlledEntity, stateOwner }) as AIStateBase<EntityType, AIType>;
    }

    protected EntityType _controlledEntity;
    protected AIType _stateOwner;

    protected AIStateBase(EntityType controlledEntity, AIType stateOwner)
    {
        _controlledEntity = controlledEntity;
        _stateOwner = stateOwner;
    }

    public abstract void OnFixedUpdate();
}

