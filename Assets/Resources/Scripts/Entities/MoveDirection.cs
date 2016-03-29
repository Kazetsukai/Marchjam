using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    //IMPORTANT: these two enums need to have the same values, except that MoveDirection can have numbers 
    //that have the same bits as others, while MoveDirectionDistinct should have a single bit for each number

    //The only reason MoveDirection exists is for the Unity editor so that multiple bits can be specified without have to duplicate info

    public enum MoveDirectionDistinct
    {
        XAxisPositive = 1,      // 0000 0001
        XAxisNegative = 2,      // 0000 0010
        YAxisPositive = 4,      // 0000 0100
        YAxisNegative = 8,      // 0000 1000
        ZAxisPositive = 16,     // 0001 0000
        ZAxisNegative = 32      // 0010 0000
    }

    public enum MoveDirection
    {
        All = 63,               // 0011 1111
        XAxisPositive = 1,      // 0000 0001
        XAxisNegative = 2,      // 0000 0010
        XAxis = 3,              // 0000 0011
        YAxisPositive = 4,      // 0000 0100
        YAxisNegative = 8,      // 0000 1000
        YAxis = 12,             // 0000 1100
        ZAxisPositive = 16,     // 0001 0000
        ZAxisNegative = 32,     // 0010 0000
        ZAxis = 48              // 0011 0000
    }
}
