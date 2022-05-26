using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


using REAL = System.Single;
using BOX3 = CGALDotNetGeometry.Shapes.Box3f;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point3f : IEquatable<Point3f>
    {
        public REAL x, y, z;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static Point3f UnitX = new Point3f(1, 0, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static Point3f UnitY = new Point3f(0, 1, 0);

        /// <summary>
        /// The unit z point.
        /// </summary>
        public readonly static Point3f UnitZ = new Point3f(0, 0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static Point3f Zero = new Point3f(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static Point3f One = new Point3f(1);

        /// <summary>
        /// A point of 0.5.
        /// </summary>
        public readonly static Point3f Half = new Point3f(0.5f);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static Point3f PositiveInfinity = new Point3f(REAL.PositiveInfinity);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static Point3f NegativeInfinity = new Point3f(REAL.NegativeInfinity);

        /// <summary>
        /// 3D point to 3D swizzle point.
        /// </summary>
        public Point3f xzy => new Point3f(x, z, y);

        /// <summary>
        /// 3D point to 2D point.
        /// </summary>
        public Point2f xy => new Point2f(x, y);

        /// <summary>
        /// 3D point to 2D point.
        /// </summary>
        public Point2f xz => new Point2f(x, z);

        /// <summary>
        /// 3D point to 2D point.
        /// </summary>
        public Point2f zy => new Point2f(z, y);

        /// <summary>
        /// 3D point to 4D point with w as 0.
        /// </summary>
        public Point4f xyz0 => new Point4f(x, y, z, 0);

        /// <summary>
        /// 3D point to 4D point with w as 1.
        /// </summary>
        public Point4f xyz1 => new Point4f(x, y, z, 1);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3f(REAL v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3f(REAL x, REAL y, REAL z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point3f(double x, double y, double z)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
            this.z = (REAL)z;
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
                    throw new IndexOutOfRangeException("Point3f index out of range.");

                fixed (Point3f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Point3f index out of range.");

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
                return true;
            }
        }

        /// <summary>
        /// Make a point with no non finite conponents.
        /// </summary>
        public Point3f Finite
        {
            get
            {
                var p = new Point3f(x, y, z);
                if (!MathUtil.IsFinite(p.x)) p.x = 0;
                if (!MathUtil.IsFinite(p.y)) p.y = 0;
                if (!MathUtil.IsFinite(p.z)) p.z = 0;
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
                return false;
            }
        }

        /// <summary>
        /// Make a point with no nan conponents.
        /// </summary>
        public Point3f NoNAN
        {
            get
            {
                var p = new Point3f(x, y, z);
                if (REAL.IsNaN(p.x)) p.x = 0;
                if (REAL.IsNaN(p.y)) p.y = 0;
                if (REAL.IsNaN(p.z)) p.z = 0;
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
                return x + y + z;
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
                return x * y * z;
            }
        }

        /// <summary>
        /// The points absolute values.
        /// </summary>
        public Point3f Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Point3f(Math.Abs(x), Math.Abs(y), Math.Abs(z));
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
                return (x * x + y * y + z * z);
            }
        }

        /// <summary>
        /// Add two point and vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(Point3f v1, Vector3f v2)
        {
            return new Point3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(Point3f v1, REAL s)
        {
            return new Point3f(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator +(REAL s, Point3f v1)
        {
            return new Point3f(s + v1.x, s + v1.y, s + v1.z);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v)
        {
            return new Point3f(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract a point and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v1, Vector3f v2)
        {
            return new Point3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(Point3f v1, REAL s)
        {
            return new Point3f(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator -(REAL s, Point3f v1)
        {
            return new Point3f(s - v1.x, s - v1.y, s - v1.z);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator *(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator *(Point3f v, REAL s)
        {
            return new Point3f(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator *(REAL s, Point3f v)
        {
            return new Point3f(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator /(Point3f v1, Point3f v2)
        {
            return new Point3f(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator /(Point3f v, REAL s)
        {
            return new Point3f(v.x / s, v.y / s, v.z / s);
        }

        /// <summary>
        /// Divide a scalar and a point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point3f operator /(REAL s, Point3f v)
        {
            return new Point3f(s / v.x, s / v.y, s / v.z);
        }

        /// <summary>
        /// Cast from Point3d to Point3f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3f(Point3d v)
        {
            return new Point3f(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Point3i to Point3f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3f(Point3i v)
        {
            return new Point3f(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Vector3d to Point3f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point3f(Vector3d v)
        {
            return new Point3f(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Vector3f to Point3f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point3f(Vector3f v)
        {
            return new Point3f(v.x, v.y, v.z);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point3f v1, Point3f v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point3f v1, Point3f v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Point3f)) return false;
            Point3f v = (Point3f)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(Point3f v)
        {
            return this == v;
        }

        /// <summary>
        /// Are these points equal given the error.
        /// </summary>
        public static bool AlmostEqual(Point3f v0, Point3f v1, REAL eps = MathUtil.EPS_32)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            if (Math.Abs(v0.z - v1.z) > eps) return false;
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
                return hash;
            }
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1},{2}", x, y, z);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2}", x.ToString(f), y.ToString(f), z.ToString(f));
        }

        /// <summary>
        /// Distance between two points.
        /// </summary>
        public static REAL Distance(Point3f v0, Point3f v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        public static REAL SqrDistance(Point3f v0, Point3f v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            REAL z = v0.z - v1.z;
            return x * x + y * y + z * z;
        }

        /// <summary>
        /// Direction between two points.
        /// </summary>
        /// <param name="v0">The first point.</param>
        /// <param name="v1">The second point.</param>
        /// <param name="normalize">Should the vector be normalized.</param>
        /// <returns>The vector from v0 to v1.</returns>
        public static Vector3f Direction(Point3f v0, Point3f v1, bool normalize = true)
        {
            Vector3f v = v1 - v0;

            if (normalize)
                return v.Normalized;
            else
                return v;
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        public static Point3f Min(Point3f v, REAL s)
        {
            v.x = MathUtil.Min(v.x, s);
            v.y = MathUtil.Min(v.y, s);
            v.z = MathUtil.Min(v.z, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        public static Point3f Min(Point3f v0, Point3f v1)
        {
            v0.x = MathUtil.Min(v0.x, v1.x);
            v0.y = MathUtil.Min(v0.y, v1.y);
            v0.z = MathUtil.Min(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        public static Point3f Max(Point3f v, REAL s)
        {
            v.x = MathUtil.Max(v.x, s);
            v.y = MathUtil.Max(v.y, s);
            v.z = MathUtil.Max(v.z, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        public static Point3f Max(Point3f v0, Point3f v1)
        {
            v0.x = MathUtil.Max(v0.x, v1.x);
            v0.y = MathUtil.Max(v0.y, v1.y);
            v0.z = MathUtil.Max(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point3f Clamp(Point3f v, REAL min, REAL max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max), min);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max), min);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point3f Clamp(Point3f v, Point3f min, Point3f max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
            v.z = MathUtil.Max(MathUtil.Min(v.z, max.z), min.z);
            return v;
        }

        /// <summary>
        /// Lerp between two points.
        /// </summary>
        public static Point3f Lerp(Point3f p0, Point3f p1, REAL a)
        {
            a = MathUtil.Clamp01(a);
            Point3f p = new Point3f();
            p.x = MathUtil.Lerp(p0.x, p1.x, a);
            p.y = MathUtil.Lerp(p0.y, p1.y, a);
            p.z = MathUtil.Lerp(p0.z, p1.z, a);

            return p;
        }

        /// <summary>
        /// BLerp between four points.
        /// </summary>
        public static Point3f BLerp(Point3f p00, Point3f p10, Point3f p01, Point3f p11, REAL a0, REAL a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            Point3f p = new Point3f();
            p.x = MathUtil.BLerp(p00.x, p10.x, p01.x, p11.x, a0, a1);
            p.y = MathUtil.BLerp(p00.y, p10.y, p01.y, p11.y, a0, a1);
            p.z = MathUtil.BLerp(p00.z, p10.z, p01.z, p11.z, a0, a1);

            return p;
        }

        /// <summary>
        /// A rounded point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        /// <returns>The rounded point</returns>
        public Point3f Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            REAL z = MathUtil.Round(this.z, digits);
            return new Point3f(x, y, z);
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
        }

        /// <summary>
        /// Floor each component if point.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
            z = MathUtil.Floor(z);
        }

        /// <summary>
        /// Ceilling each component if point.
        /// </summary>
        public void Ceilling()
        {
            x = MathUtil.Ceilling(x);
            y = MathUtil.Ceilling(y);
            z = MathUtil.Ceilling(z);
        }

        /// <summary>
        /// Create a array of random points.
        /// </summary>
        /// <param name="seed">The seed</param>
        /// <param name="count">The number of points to create.</param>
        /// <param name="range">The range of the points.</param>
        /// <returns>The point array.</returns>
        public static Point3f[] RandomPoints(int seed, int count, BOX3 range)
        {
            var points = new Point3f[count];
            var rnd = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                REAL x = (REAL)(range.Min.x + rnd.NextDouble() * range.Max.x);
                REAL y = (REAL)(range.Min.y + rnd.NextDouble() * range.Max.y);
                REAL z = (REAL)(range.Min.z + rnd.NextDouble() * range.Max.z);

                points[i] = new Point3f(x, y, z);
            }

            return points;
        }

    }
}
