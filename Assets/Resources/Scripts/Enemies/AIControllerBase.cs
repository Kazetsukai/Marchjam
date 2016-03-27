using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Base type for all AI controllers
/// </summary>
/// <typeparam name="ControlledEntityType">Type of entity that this AI Controller controls</typeparam>
public abstract class AIControllerBase<ControlledEntityType> : MonoBehaviour
{
    /// <summary>
    /// Entity that this AI Controller sends its inputs to
    /// </summary>
    public abstract ControlledEntityType ControlledEntity
    {
        get;
    }
}
