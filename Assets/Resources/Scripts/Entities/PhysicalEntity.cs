using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicalEntity : MonoBehaviour
    {
        public Rigidbody Body
        {
            get
            {
                return GetComponent<Rigidbody>();
            }
        }
    }
}
