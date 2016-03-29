using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class EntityLinearRigidBodyMovement : EntityMovementBase, IRigidBody
    {
        public Rigidbody Body
        {
            get
            {
                return GetComponent<Rigidbody>();
            }
        }

        protected override void AddRelativeForce(Vector3 force)
        {
            Body.AddRelativeForce(force, ForceMode.VelocityChange);
        }

        protected override Vector3 GetCurrentVelocity()
        {
            return Body.transform.InverseTransformDirection(Body.velocity);
        }
    }
}
