using System;
using System.Collections.Generic;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Extensions
{
    public static class PrimativeExtensions
    {
        public static bool IsNullIndex(this int i)
        {
            return i == MathUtil.NULL_INDEX;
        }
    }
}
