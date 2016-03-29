using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class DirectionMovementProperties
    {
        [Tooltip("Select the direction(s) that these properties apply to.")]
        [SerializeField] public MoveDirection ApplicableDirection;

        [Tooltip("Unset a property by setting it to < 0")]
        [SerializeField] public MovementProperties Properties;
    }
}
