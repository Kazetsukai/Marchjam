using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

/// <summary>
/// Base type for an AI State
/// </summary>
/// <typeparam name="AIController">The AIController derived from AIControllerBase that uses this state</typeparam>
/// <typeparam name="ControlledEntityType">Type of entity that this AI State applies to</typeparam>
public abstract class AIStateBase<AIController, ControlledEntityType> where AIController : AIControllerBase<ControlledEntityType>
{
    /// <summary>
    /// Shorthand access to ControlledEntity from AIController
    /// </summary>
    public ControlledEntityType ControlledEntity
    {
        get
        {
            return AiController.ControlledEntity;
        }
    }

    /// <summary>
    /// Reference to parent AIController
    /// </summary>
    public AIController AiController
    {
        get;
        private set;
    }

    protected AIStateBase(AIController aiController)
    {
        AiController = aiController;
        FrameMethods = new Dictionary<Func<bool>, bool>();
    }

    /// <summary>
    /// Dictionary of methods returning bools that will be executed each FixedUpdate, with their results stored in the Dictionary.
    /// Should be initialised in the AIState's constructor, e.g. FrameMethods = new Dictionary<Func<bool>, bool>() { FindMyEnemy, false } 
    /// with a method named FindMyEnemy that returns true when my enemy is found.
    /// </summary>
    protected Dictionary<Func<bool>, bool> FrameMethods;

    /// <summary>
    /// Resets the result of all FrameMethods before execute the FixedUpdate code for this State
    /// </summary>
    public void TriggerFixedUpdate()
    {
        foreach (var key in FrameMethods.Keys.ToList())
        {
            FrameMethods[key] = false;
        }

        OnFixedUpdate();
    }

    /// <summary>
    /// Executes and updates the result of all FrameMethods and returns true if all FrameMethods returned true
    /// </summary>
    /// <returns>True if all FrameMethods returned true</returns>
    protected bool ExecuteFrameMethods()
    {
        bool returnValue = true;
        foreach (var key in FrameMethods.Keys.ToList())
        {
            FrameMethods[key] = key();
            if (!FrameMethods[key])
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

