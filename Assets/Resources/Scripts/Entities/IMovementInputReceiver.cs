using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public interface IMovementInputReceiver
    {
        void MoveInDirection(Vector3 directionAndDesiredSpeedPercent);

        void MoveInDirection(float desiredSpeedPercent, MoveDirection direction);

        MovementProperties this[MoveDirectionDistinct direction]
        {
            get;
        }
    }
}
