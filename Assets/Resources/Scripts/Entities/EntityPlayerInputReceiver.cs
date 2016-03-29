using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Testing class to help test out Entity movement without writing AI :D
    /// </summary>
    public class EntityPlayerInputReceiver : MonoBehaviour
    {
        public EntityMovementBase Moving;

        public EntityMovementBase Turning;

        public void FixedUpdate()
        {
            Vector3 movement = new Vector3
            (
                0,
                Input.GetAxis("Jump") - Input.GetAxis("Handbrake"),
                Input.GetAxis("Vertical")
            );

            Vector3 turning = new Vector3
            (
                0,
                Input.GetAxis("Horizontal"),
                0
            );

            Moving.MoveInDirection(movement);
            Turning.MoveInDirection(turning);
        }
    }
}
