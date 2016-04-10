using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IWeaponController
{
    // Called on all clients when a weapon is fired
    void FireWeapon(Vector3 position, Vector3 direction, float serverTime);
}
