using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Enemies
{
    public class InputReceiver : MonoBehaviour
    {
        public InputType[] AcceptedInputTypes;

        public InputReceiver()
        {
            _inputValues = AcceptedInputTypes != null ? AcceptedInputTypes.ToDictionary(inputType => inputType, value => 0f) : new Dictionary<InputType, float>();
        }

        private Dictionary<InputType, float> _inputValues;

        public float this[InputType input]
        {
            get
            {
                return _inputValues[input];
            }
            set
            {
                _inputValues[input] = value;
            }
        }

        public void ResetAllInputs()
        {
            if (_inputValues == null)
            {
                return;
            }

            foreach (InputType inputType in AcceptedInputTypes)
            {
                _inputValues[inputType] = 0f;
            }
        }
    }
}
