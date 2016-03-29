using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public class EntityLinearTransformRotation : EntityMovementBase
    {
        protected override void AddRelativeForce(Vector3 force)
        {
            transform.Rotate(force, Space.Self);
        }

        protected override Vector3 GetCurrentVelocity()
        {
            return Vector3.zero;
        }
    }
}
