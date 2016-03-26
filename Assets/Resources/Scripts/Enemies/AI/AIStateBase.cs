using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public abstract class AIStateBase<AIController, ControlledEntityType> where AIController : AIControllerBase<ControlledEntityType>
{
    protected ControlledEntityType _controlledEntity
    {
        get
        {
            return _aiController.ControlledEntity;
        }
    }

    protected AIController _aiController
    {
        get;
        private set;
    }

    protected AIStateBase(AIController aiController)
    {
        _aiController = aiController;
        _frameMethods = new Dictionary<Func<bool>, bool>();
    }

    protected Dictionary<Func<bool>, bool> _frameMethods;

    public void TriggerFixedUpdate()
    {
        foreach (var key in _frameMethods.Keys.ToList())
        {
            _frameMethods[key] = false;
        }

        OnFixedUpdate();
    }

    protected bool ExecuteFrameMethods()
    {
        bool returnValue = true;
        foreach (var key in _frameMethods.Keys.ToList())
        {
            _frameMethods[key] = key();
            if (!_frameMethods[key])
            {
                returnValue = false;
            }
        }

        return returnValue;
    }

    protected abstract void OnFixedUpdate();

    //Not used at the moment, but may be useful later if we want to be more abstract about the types we're using
    public static AIStateBase<AIController, ControlledEntityType> InstantiateState(Type newState, AIController stateOwner)
    {
        if (!typeof(AIStateBase<AIController, ControlledEntityType>).IsAssignableFrom(newState))
        {
            throw new InvalidOperationException("Wrong type: " + newState.Name);
        }

        ConstructorInfo stateConstructor = newState.GetConstructor(new Type[] { typeof(AIController) });
        return stateConstructor.Invoke(new object[] { stateOwner }) as AIStateBase<AIController, ControlledEntityType>;
    }
}

