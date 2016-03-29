using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public interface IRigidBody
    {
        Rigidbody Body
        {
            get;
        }
    }
}
