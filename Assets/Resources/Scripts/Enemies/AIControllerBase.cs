using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class AIControllerBase<ControlledEntityType> : MonoBehaviour
{


    public abstract ControlledEntityType ControlledEntity
    {
        get;
    }
}
