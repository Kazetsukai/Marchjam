using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Enemies
{
    public abstract class InputReceivingEntity : PhysicalEntity
    {
        protected abstract IList<InputType> AcceptedInputTypes
        {
            get;
        }

        public InputReceivingEntity()
        {
            InputValues = AcceptedInputTypes.ToDictionary(inputType => inputType, value => 0f);
        }

        protected Dictionary<InputType, float> InputValues
        {
            get;
            private set;
        }

        public float this[InputType input]
        {
            get
            {
                return InputValues[input];
            }
            set
            {
                InputValues[input] = value;
            }
        }

        public void ResetAllInputs()
        {
            if (InputValues == null)
            {
                return;
            }

            foreach (InputType inputType in AcceptedInputTypes)
            {
                InputValues[inputType] = 0f;
            }
        }
    }
}
