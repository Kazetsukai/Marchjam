using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    //IMPORTANT: these two enums need to have the same values, except that MoveDirection can have numbers 
    //that have the same bits as others, while MoveDirectionDistinct should have a single bit for each number

    public enum MoveDirectionDistinct
    {
        Forward = 1,        // 0000 0001
        Backward = 2,       // 0000 0010
        Right = 4,          // 0000 0100
        Left = 8,           // 0000 1000
        Up = 16,            // 0001 0000
        Down = 32           // 0010 0000
    }

    public enum MoveDirection
    {
        All = 63,           // 0011 1111
        Forward = 1,        // 0000 0001
        Backward = 2,       // 0000 0010
        ZAxis = 3,          // 0000 0011
        Right = 4,          // 0000 0100
        Left = 8,           // 0000 1000
        XAxis = 12,         // 0000 1100
        Up = 16,            // 0001 0000
        Down = 32,          // 0010 0000
        YAxis = 48          // 0011 0000
    }
}
