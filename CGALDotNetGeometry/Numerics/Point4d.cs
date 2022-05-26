using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Double;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point4d : IEquatable<Point4d>
    {
        public REAL x, y, z, w;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static Point4d UnitX = new Point4d(1, 0, 0, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static Point4d UnitY = new Point4d(0, 1, 0, 0);

        /// <summary>
        /// The unit z point.
        /// </summary>
        public readonly static Point4d UnitZ = new Point4d(0, 0, 1, 0);

        /// <summary>
        /// The unit z point.
        /// </summary>
        public readonly static Point4d UnitW = new Point4d(0, 0, 0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static Point4d Zero = new Point4d(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static Point4d One = new Point4d(1);

        /// <summary>
        /// A point of 0.5.
        /// </summary>
        public readonly static Point4d Half = new Point4d(0.5);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static Point4d PositiveInfinity = new Point4d(REAL.PositiveInfinity);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static Point4d NegativeInfinity = new Point4d(REAL.NegativeInfinity);

        /// <summary>
        /// 4D point to 2D point.
        /// </summary>
        public Point2d xy => new Point2d(x, y);

        /// <summary>
        /// 4D point to 3D point.
        /// </summary>
        public Point3d xyz => new Point3d(x, y, z);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point4d(REAL v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
            this.w = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point4d(REAL x, REAL y, REAL z, REAL w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
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
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Point4d index out of range.");

                fixed (Point4d* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Point4d index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// Are all the components ofpoint finite.
        /// </summary>
        public bool IsFinite
        {
            get
            {
                if (!MathUtil.IsFinite(x)) return false;
                if (!MathUtil.IsFinite(y)) return false;
                if (!MathUtil.IsFinite(z)) return false;
                if (!MathUtil.IsFinite(w)) return false;
                return true;
            }
        }

        /// <summary>
        /// Make a point with no non finite conponents.
        /// </summary>
        public Point4d Finite
        {
            get
            {
                var p = new Point4d(x, y, z, w);
                if (!MathUtil.IsFinite(p.x)) p.x = 0;
                if (!MathUtil.IsFinite(p.y)) p.y = 0;
                if (!MathUtil.IsFinite(p.z)) p.z = 0;
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
                if (REAL.IsNaN(z)) return true;
                if (REAL.IsNaN(w)) return true;
                return false;
            }
        }

        /// <summary>
        /// Make a point with no nan conponents.
        /// </summary>
        public Point4d NoNAN
        {
            get
            {
                var p = new Point4d(x, y, z, w);
                if (REAL.IsNaN(p.x)) p.x = 0;
                if (REAL.IsNaN(p.y)) p.y = 0;
                if (REAL.IsNaN(p.z)) p.z = 0;
                if (REAL.IsNaN(p.w)) p.w = 0;
                return p;
            }
        }

        /// <summary>
        /// The sum of the points components.
        /// </summary>
        public REAL Sum
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x + y + z + w;
            }
        }

        /// <summary>
        /// The product of the points components.
        /// </summary>
        public REAL Product
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * y * z * w;
            }
        }

        /// <summary>
        /// The points absolute values.
        /// </summary>
        public Point4d Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Point4d(Math.Abs(x), Math.Abs(y), Math.Abs(z), Math.Abs(w));
            }
        }

        /// <summary>
        /// The length of the vector.
        /// </summary>
        public REAL Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MathUtil.SafeSqrt(SqrMagnitude);
            }
        }

        /// <summary>
        /// The length of the vector squared.
        /// </summary>
		public REAL SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (x * x + y * y + z * z + w * w);
            }
        }

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator +(Point4d v1, Point4d v2)
        {
            return new Point4d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        /// <summary>
        /// Add a point and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator +(Point4d v1, Vector4d v2)
        {
            return new Point4d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator +(Point4d v1, REAL s)
        {
            return new Point4d(v1.x + s, v1.z + s, v1.z + s, v1.w + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator +(REAL s, Point4d v1)
        {
            return new Point4d(s + v1.x, s + v1.y, s + v1.z, s + v1.w);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator -(Point4d v)
        {
            return new Point4d(-v.x, -v.y, -v.z, -v.w);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator -(Point4d v1, Point4d v2)
        {
            return new Point4d(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        /// <summary>
        /// Add a point and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator -(Point4d v1, Vector4d v2)
        {
            return new Point4d(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator -(Point4d v1, REAL s)
        {
            return new Point4d(v1.x - s, v1.y - s, v1.z - s, v1.w - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator -(REAL s, Point4d v1)
        {
            return new Point4d(s - v1.x, s - v1.y, s - v1.z, s - v1.w);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator *(Point4d v1, Point4d v2)
        {
            return new Point4d(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator *(Point4d v, REAL s)
        {
            return new Point4d(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator *(REAL s, Point4d v)
        {
            return new Point4d(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator /(Point4d v1, Point4d v2)
        {
            return new Point4d(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator /(Point4d v, REAL s)
        {
            return new Point4d(v.x / s, v.y / s, v.z / s, v.w / s);
        }

        /// <summary>
        /// Divide a scalar and a point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point4d operator /(REAL s, Point4d v)
        {
            return new Point4d(s / v.x, s / v.y, s / v.z, s / v.w);
        }

        /// <summary>
        /// Cast from Point4f to Point4d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point4d(Point4f v)
        {
            return new Point4d(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Cast from Point4i to Point4d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point4d(Point4i v)
        {
            return new Point4d(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Cast from Vector4f to Point4d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point4d(Vector4f v)
        {
            return new Point4d(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Cast from Vector4d to Point4d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point4d(Vector4d v)
        {
            return new Point4d(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point4d v1, Point4d v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point4d v1, Point4d v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Point4d)) return false;
            Point4d v = (Point4d)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(Point4d v)
        {
            return this == v;
        }

        /// <summary>
        /// Are these points equal given the error.
        /// </summary>
        public static bool AlmostEqual(Point4d v0, Point4d v1, REAL eps = MathUtil.EPS_64)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            if (Math.Abs(v0.z - v1.z) > eps) return false;
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
                hash = (hash * MathUtil.HASH_PRIME_2) ^ z.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ w.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", x, y, z, w);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2},{3}", x.ToString(f), y.ToString(f), z.ToString(f), w.ToString(f));
        }

        /// <summary>
        /// Distance between two points.
        /// </summary>
        public static REAL Distance(Point4d v0, Point4d v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>

        public static REAL SqrDistance(Point4d v0, Point4d v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            REAL z = v0.z - v1.z;
            REAL w = v0.w - v1.w;
            return x * x + y * y + z * z + w * w;
        }

        /// <summary>
        /// Direction between two points.
        /// </summary>
        /// <param name="v0">The first point.</param>
        /// <param name="v1">The second point.</param>
        /// <param name="normalize">Should the vector be normalized.</param>
        /// <returns>The vector from v0 to v1.</returns>
        public static Vector4d Direction(Point4d v0, Point4d v1, bool normalize = true)
        {
            Vector4d v = v1 - v0;

            if (normalize)
                return v.Normalized;
            else
                return v;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        public static Point4d Min(Point4d v, REAL s)
        {
            v.x = MathUtil.Min(v.x, s);
            v.y = MathUtil.Min(v.y, s);
            v.z = MathUtil.Min(v.z, s);
            v.w = MathUtil.Min(v.w, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        public static Point4d Min(Point4d v0, Point4d v1)
        {
            v0.x = MathUtil.Min(v0.x, v1.x);
            v0.y = MathUtil.Min(v0.y, v1.y);
            v0.z = MathUtil.Min(v0.z, v1.z);
            v0.w = MathUtil.Min(v0.w, v1.w);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        public static Point4d Max(Point4d v, REAL s)
        {
            v.x = MathUtil.Max(v.x, s);
            v.y = MathUtil.Max(v.y, s);
            v.z = MathUtil.Max(v.z, s);
            v.w = MathUtil.Max(v.w, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        public static Point4d Max(Point4d v0, Point4d v1)
        {
            v0.x = MathUtil.Max(v0.x, v1.x);
            v0.y = MathUtil.Max(v0.y, v1.y);
            v0.z = MathUtil.Max(v0.z, v1.z);
            v0.w = MathUtil.Max(v0.w, v1.w);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point4d Clamp(Point4d v, REAL min, REAL max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max), min);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max), min);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max), min);
            v.w = MathUtil.Max(MathUtil.Min(v.w, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point4d Clamp(Point4d v, Point4d min, Point4d max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max.z), min.z);
            v.w = MathUtil.Max(MathUtil.Min(v.w, max.w), min.w);
            return v;
        }

        /// <summary>
        /// Lerp between two points.
        /// </summary>
        public static Point4d Lerp(Point4d p0, Point4d p1, REAL a)
        {
            a = MathUtil.Clamp01(a);
            Point4d p = new Point4d();
            p.x = MathUtil.Lerp(p0.x, p1.x, a);
            p.y = MathUtil.Lerp(p0.y, p1.y, a);
            p.z = MathUtil.Lerp(p0.z, p1.z, a);
            p.w = MathUtil.Lerp(p0.w, p1.w, a);
            return p;
        }

        /// <summary>
        /// BLerp between four points.
        /// </summary>
        public static Point4d BLerp(Point4d p00, Point4d p10, Point4d p01, Point4d p11, REAL a0, REAL a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            Point4d p = new Point4d();
            p.x = MathUtil.BLerp(p00.x, p10.x, p01.x, p11.x, a0, a1);
            p.y = MathUtil.BLerp(p00.y, p10.y, p01.y, p11.y, a0, a1);
            p.z = MathUtil.BLerp(p00.z, p10.z, p01.z, p11.z, a0, a1);
            p.w = MathUtil.BLerp(p00.w, p10.w, p01.w, p11.w, a0, a1);

            return p;
        }

        /// <summary>
        /// A rounded point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        /// <returns>The rounded point</returns>
        public Point4d Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            REAL z = MathUtil.Round(this.z, digits);
            REAL w = MathUtil.Round(this.w, digits);
            return new Point4d(x, y, z, w);
        }

        /// <summary>
        /// Round the point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            x = MathUtil.Round(x, digits);
            y = MathUtil.Round(y, digits);
            z = MathUtil.Round(z, digits);
            w = MathUtil.Round(w, digits);
        }

        /// <summary>
        /// Floor each component if point.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
            z = MathUtil.Floor(z);
            w = MathUtil.Floor(w);
        }

        /// <summary>
        /// Ceilling each component if point.
        /// </summary>
        public void Ceilling()
        {
            x = MathUtil.Ceilling(x);
            y = MathUtil.Ceilling(y);
            z = MathUtil.Ceilling(z);
            w = MathUtil.Ceilling(w);
        }

    }
}
