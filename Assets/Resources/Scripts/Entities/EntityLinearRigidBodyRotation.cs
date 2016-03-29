using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class EntityLinearRigidBodyRotation : EntityMovementBase, IRigidBody
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
            Body.AddRelativeTorque(force, ForceMode.Acceleration);
        }

        protected override Vector3 GetCurrentVelocity()
        {
            return Body.transform.InverseTransformDirection(Body.angularVelocity);
        }
    }
}
