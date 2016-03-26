using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public abstract class AIStateBase<AIController>
{
    protected AIController _aiController
    {
        get;
        private set;
    }

    protected AIStateBase(AIController aiController)
    {
        _aiController = aiController;
    }

    public abstract void OnFixedUpdate();

    //Not used at the moment, but may be useful later if we want to be more abstract about the types we're using
    public static AIStateBase<AIController> InstantiateState(Type newState, AIController stateOwner)
    {
        if (!typeof(AIStateBase<AIController>).IsAssignableFrom(newState))
        {
            throw new InvalidOperationException("Wrong type: " + newState.Name);
        }

        ConstructorInfo stateConstructor = newState.GetConstructor(new Type[] { typeof(AIController) });
        return stateConstructor.Invoke(new object[] { stateOwner }) as AIStateBase<AIController>;
    }
}

