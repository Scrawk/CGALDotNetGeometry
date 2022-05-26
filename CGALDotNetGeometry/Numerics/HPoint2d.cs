using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using CGALDotNetGeometry.Shapes;

using REAL = System.Double;

namespace CGALDotNetGeometry.Numerics
{
    /// <summary>
    /// A Homogenous 2D point struct.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct HPoint2d : IEquatable<HPoint2d>
    {
        public REAL x, y, w;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static HPoint2d UnitX = new HPoint2d(1, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static HPoint2d UnitY = new HPoint2d(0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static HPoint2d Zero = new HPoint2d(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static HPoint2d One = new HPoint2d(1);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint2d(REAL v)
        {
            this.x = v;
            this.y = v;
            this.w = 1;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint2d(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
            this.w = 1;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint2d(REAL x, REAL y, REAL w)
        {
            this.x = x;
            this.y = y;
            this.w = w;
        }

        /// <summary>
        /// Array accessor for variables. 
        /// </summary>
        /// <param name="i">The variables index.</param>
        /// <returns>The variable value</returns>
        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("HPoint2d index out of range.");

                fixed (HPoint2d* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("HPoint2d index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// Are all the components of vector finite.
        /// </summary>
        public bool IsFinite
        {
            get
            {
                if (!MathUtil.IsFinite(x)) return false;
                if (!MathUtil.IsFinite(y)) return false;
                if (!MathUtil.IsFinite(w)) return false;
                return true;
            }
        }

        /// <summary>
        /// Make a point with no non finite conponents.
        /// </summary>
        public HPoint2d Finite
        {
            get
            {
                var p = new HPoint2d(x, y, w);
                if (!MathUtil.IsFinite(p.x)) p.x = 0;
                if (!MathUtil.IsFinite(p.y)) p.y = 0;
                if (!MathUtil.IsFinite(p.w)) p.w = 0;
                return p;
            }
        }

        /// <summary>
        /// Are any of the points components nan.
        /// </summary>
        public bool IsNAN
        {
            get
            {
                if (REAL.IsNaN(x)) return true;
                if (REAL.IsNaN(y)) return true;
                if (REAL.IsNaN(w)) return true;
                return false;
            }
        }

        /// <summary>
        /// Make a point with no nan conponents.
        /// </summary>
        public HPoint2d NoNAN
        {
            get
            {
                var p = new HPoint2d(x, y, w);
                if (REAL.IsNaN(p.x)) p.x = 0;
                if (REAL.IsNaN(p.y)) p.y = 0;
                if (REAL.IsNaN(p.w)) p.w = 0;
                return p;
            }
        }

        /// <summary>
        /// Convert from homogenous to cartesian space.
        /// </summary>
        public Point2d Cartesian
        {
            get
            {
                if (w != 0)
                    return new Point2d(x / w, y / w);
                else
                    return new Point2d(x, y);
            }
        }

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator +(HPoint2d v1, HPoint2d v2)
        {
            return new HPoint2d(v1.x + v2.x, v1.y + v2.y, v1.w + v2.w);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator +(HPoint2d v1, REAL s)
        {
            return new HPoint2d(v1.x + s, v1.y + s, v1.w + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator +(REAL s, HPoint2d v1)
        {
            return new HPoint2d(s + v1.x, s + v1.y, s + v1.w);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator *(HPoint2d v1, HPoint2d v2)
        {
            return new HPoint2d(v1.x * v2.x, v1.y * v2.y, v1.w * v2.w);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator *(HPoint2d v, REAL s)
        {
            return new HPoint2d(v.x * s, v.y * s, v.w * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator *(REAL s, HPoint2d v)
        {
            return new HPoint2d(v.x * s, v.y * s, v.w * s);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator /(HPoint2d v, REAL s)
        {
            return new HPoint2d(v.x / s, v.y / s, v.w / s);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint2d operator /(REAL s, HPoint2d v)
        {
            return new HPoint2d(s / v.x, s / v.y, s / v.w);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HPoint2d v1, HPoint2d v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.w == v2.w);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HPoint2d v1, HPoint2d v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.w != v2.w);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is HPoint2d)) return false;
            HPoint2d v = (HPoint2d)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(HPoint2d v)
        {
            return this == v;
        }

        /// <summary>
        /// Are these points equal given the error.
        /// </summary>
        public static bool AlmostEqual(HPoint2d v0, HPoint2d v1, REAL eps = MathUtil.EPS_64)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            if (Math.Abs(v0.w - v1.w) > eps) return false;
            return true;
        }

        /// <summary>
        /// Vectors hash code. 
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ x.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ y.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ w.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", x, y, w);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2}", x.ToString(f), y.ToString(f), w.ToString(f));
        }

        /// <summary>
        /// A rounded point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        /// <returns>The rounded point</returns>
        public HPoint2d Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            REAL w = MathUtil.Round(this.w, digits);
            return new HPoint2d(x, y, w);
        }

        /// <summary>
        /// Round the point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            x = MathUtil.Round(x, digits);
            y = MathUtil.Round(y, digits);
            w = MathUtil.Round(w, digits);
        }

        /// <summary>
        /// Floor each component if point.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
            w = MathUtil.Floor(w);
        }

        /// <summary>
        /// Ceilling each component if point.
        /// </summary>
        public void Ceilling()
        {
            x = MathUtil.Ceilling(x);
            y = MathUtil.Ceilling(y);
            w = MathUtil.Ceilling(w);
        }

        /// <summary>
        /// Create a array of random points.
        /// </summary>
        /// <param name="seed">The seed</param>
        /// <param name="count">The number of points to create.</param>
        /// <param name="weight">The number of points weight.</param>
        /// <param name="range">The range of the points.</param>
        /// <returns>The point array.</returns>
        public static HPoint2d[] RandomPoints(int seed, int count, REAL weight, Box2d range)
        {
            var points = new HPoint2d[count];
            var rnd = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                REAL x = range.Min.x + rnd.NextDouble() * range.Max.x;
                REAL y = range.Min.y + rnd.NextDouble() * range.Max.y;

                points[i] = new HPoint2d(x, y, weight);
            }

            return points;
        }

    }
}
