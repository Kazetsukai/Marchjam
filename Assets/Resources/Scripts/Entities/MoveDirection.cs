using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public enum MoveDirection
    {
        All = 63,
        Forward = 1,
        Backward = 2,
        ZAxis = 3,
        Right = 4,
        Left = 8,
        XAxis = 12,
        Up = 16,
        Down = 32,
        YAxis = 48
    }
}
