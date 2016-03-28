using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enemies
{
    /// <summary>
    /// Types of input that an InputReceivingEntity can receive.
    /// Convention is that inputs that can receive opposite positive and negative inputs are named PositiveInput_NegativeInput
    /// </summary>
    public enum InputType
    {
        MoveForward_MoveBackward,
        MoveRight_MoveLeft,
        MoveUp_MoveDown,
        LookDown_LookUp,
        LookRight_LookLeft,
        RollLeft_RollRight,
        AimWeaponDown_AimWeaponUp,
        AimWeaponRight_AimWeaponLeft,
        Fire1
    }
}
