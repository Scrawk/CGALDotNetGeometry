using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Single;
using BOX2 = CGALDotNetGeometry.Shapes.Box2f;
using VECTOR2 = CGALDotNetGeometry.Numerics.Vector2f;

namespace CGALDotNetGeometry.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Point2f : IEquatable<Point2f>
    {
        public REAL x, y;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static Point2f UnitX = new Point2f(1, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static Point2f UnitY = new Point2f(0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static Point2f Zero = new Point2f(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static Point2f One = new Point2f(1);

        /// <summary>
        /// A point of 0.5.
        /// </summary>
        public readonly static Point2f Half = new Point2f(0.5f);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static Point2f PositiveInfinity = new Point2f(REAL.PositiveInfinity);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static Point2f NegativeInfinity = new Point2f(REAL.NegativeInfinity);

        /// <summary>
        /// 2D point to 3D point with z as 0.
        /// </summary>
        public Point3f xy0 => new Point3f(x, y, 0);

        /// <summary>
        /// 2D point to 3D point with y as z.
        /// </summary>
        public Point3f x0y => new Point3f(x, 0, y);

        /// <summary>
        /// 2D point to 3D point with z as 1.
        /// </summary>
        public Point3f xy1 => new Point3f(x, y, 1);

        /// <summary>
        /// 2D point to 4D point with z as 0 and w as 0.
        /// </summary>
        public Point4f xy00 => new Point4f(x, y, 0, 0);

        /// <summary>
        /// 2D point to 4D point with z as 0 and w as 1.
        /// </summary>
        public Point4f xy01 => new Point4f(x, y, 0, 1);

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(REAL v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2f(double x, double y)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
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
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Point2f index out of range.");

                fixed (Point2f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Point2f index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// Are all the components of point finite.
        /// </summary>
        public bool IsFinite
        {
            get
            {
                if (!MathUtil.IsFinite(x)) return false;
                if (!MathUtil.IsFinite(y)) return false;
                return true;
            }
        }

        /// <summary>
        /// Make a point with no non finite conponents.
        /// </summary>
        public Point2f Finite
        {
            get
            {
                var p = new Point2f(x, y);
                if (!MathUtil.IsFinite(p.x)) p.x = 0;
                if (!MathUtil.IsFinite(p.y)) p.y = 0;
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
                return false;
            }
        }

        /// <summary>
        /// Make a point with no nan conponents.
        /// </summary>
        public Point2f NoNAN
        {
            get
            {
                var p = new Point2f(x, y);
                if (REAL.IsNaN(p.x)) p.x = 0;
                if (REAL.IsNaN(p.y)) p.y = 0;
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
                return x + y;
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
                return x * y;
            }
        }

        /// <summary>
        /// The points absolute values.
        /// </summary>
        public Point2f Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Point2f(Math.Abs(x), Math.Abs(y));
            }
        }

        /// <summary>
        /// The length of the point.
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
                return (x * x + y * y);
            }
        }

        /// <summary>
        /// Add two point and vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Point2f v1, Vector2f v2)
        {
            return new Point2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(Point2f v1, REAL s)
        {
            return new Point2f(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator +(REAL s, Point2f v1)
        {
            return new Point2f(s + v1.x, s + v1.y);
        }

        /// <summary>
        /// Negate point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(Point2f v)
        {
            return new Point2f(-v.x, -v.y);
        }

        /// <summary>
        /// Subtract two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract a point and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2d operator -(Point2f v1, Vector2f v2)
        {
            return new Point2d(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(Point2f v1, REAL s)
        {
            return new Point2f(v1.x - s, v1.y - s);
        }

        /// <summary>
        /// Subtract point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator -(REAL s, Point2f v1)
        {
            return new Point2f(s - v1.x, s - v1.y);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator *(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator *(Point2f v, REAL s)
        {
            return new Point2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator *(REAL s, Point2f v)
        {
            return new Point2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Divide two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator /(Point2f v1, Point2f v2)
        {
            return new Point2f(v1.x / v2.x, v1.y / v2.y);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator /(Point2f v, REAL s)
        {
            return new Point2f(v.x / s, v.y / s);
        }

        /// <summary>
        /// Divide a scalar and a point.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point2f operator /(REAL s, Point2f v)
        {
            return new Point2f(s / v.x, s / v.y);
        }

        /// <summary>
        /// Cast from Point2d to Point2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2f(Point2d v)
        {
            return new Point2f(v.x, v.y);
        }

        /// <summary>
        /// Cast from Point2i to Point2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2f(Point2i v)
        {
            return new Point2f(v.x, v.y);
        }

        /// <summary>
        /// Cast from Vector2d to Point2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Point2f(Vector2d v)
        {
            return new Point2f(v.x, v.y);
        }

        /// <summary>
        /// Cast from Vector2f to Point2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Point2f(Vector2f v)
        {
            return new Point2f(v.x, v.y);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point2f v1, Point2f v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point2f v1, Point2f v2)
        {
            return (v1.x != v2.x || v1.y != v2.y);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Point2f)) return false;
            Point2f v = (Point2f)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal given the error.
        /// </summary>
        public static bool AlmostEqual(Point2f v0, Point2f v1, REAL eps = MathUtil.EPS_32)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(Point2f v)
        {
            return this == v;
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
                return hash;
            }
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1}", x, y);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1}", x.ToString(f), y.ToString(f));
        }

        /// <summary>
        /// Distance between two points.
        /// </summary>
        public static REAL Distance(Point2f v0, Point2f v1)
        {
            return MathUtil.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// Square distance between two points.
        /// </summary>
        public static REAL SqrDistance(Point2f v0, Point2f v1)
        {
            REAL x = v0.x - v1.x;
            REAL y = v0.y - v1.y;
            return x * x + y * y;
        }

        /// <summary>
        /// Direction between two points.
        /// </summary>
        /// <param name="v0">The first point.</param>
        /// <param name="v1">The second point.</param>
        /// <param name="normalize">Should the vector be normalized.</param>
        /// <returns>The vector from v0 to v1.</returns>
        public static Vector2f Direction(Point2f v0, Point2f v1, bool normalize = true)
        {
            Vector2f v = v1 - v0;

            if (normalize)
                return v.Normalized;
            else
                return v;
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 180.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        public static Degree Angle180(Point2f a, Point2f b, Point2f c)
        {
            VECTOR2 u = Direction(b, a);
            VECTOR2 v = Direction(c, a);

            REAL dp = VECTOR2.Dot(u, v);
            REAL m = u.Magnitude * v.Magnitude;
            REAL angle = MathUtil.ToDegrees(MathUtil.SafeAcos(MathUtil.SafeDiv(dp, m)));

            return new Degree(angle);
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 360.
        /// Angle represents moving ccw from a to b.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        public static Degree Angle360(Point2f a, Point2f b, Point2f c)
        {
            VECTOR2 u = Direction(b, a);
            VECTOR2 v = Direction(c, a);

            REAL angle = MathUtil.Atan2(u.y, u.x) - MathUtil.Atan2(v.y, v.x);

            if (angle <= 0.0f)
                angle = MathUtil.PI_32 * 2.0f + angle;

            angle = 360.0f - MathUtil.ToDegrees(angle);

            return new Degree(angle >= 360.0 ? 0 : angle);
        }

        /// <summary>
        /// The minimum value between s and each component in point.
        /// </summary>
        public static Point2f Min(Point2f v, REAL s)
        {
            v.x = MathUtil.Min(v.x, s);
            v.y = MathUtil.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in points.
        /// </summary>
        public static Point2f Min(Point2f v0, Point2f v1)
        {
            v0.x = MathUtil.Min(v0.x, v1.x);
            v0.y = MathUtil.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in point.
        /// </summary>
        public static Point2f Max(Point2f v, REAL s)
        {
            v.x = MathUtil.Max(v.x, s);
            v.y = MathUtil.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in points.
        /// </summary>
        public static Point2f Max(Point2f v0, Point2f v1)
        {
            v0.x = MathUtil.Max(v0.x, v1.x);
            v0.y = MathUtil.Max(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point2f Clamp(Point2f v, REAL min, REAL max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max), min);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Point2f Clamp(Point2f v, Point2f min, Point2f max)
        {
            v.x = MathUtil.Max(MathUtil.Min(v.x, max.x), min.x);
            v.y = MathUtil.Max(MathUtil.Min(v.y, max.y), min.y);
            return v;
        }

        /// <summary>
        /// Lerp between two points.
        /// </summary>
        public static Point2f Lerp(Point2f p0, Point2f p1, REAL a)
        {
            a = MathUtil.Clamp01(a);
            Point2f p = new Point2f();
            p.x = MathUtil.Lerp(p0.x, p1.x, a);
            p.y = MathUtil.Lerp(p0.y, p1.y, a);

            return p;
        }

        /// <summary>
        /// BLerp between four points.
        /// </summary>
        public static Point2f BLerp(Point2f p00, Point2f p10, Point2f p01, Point2f p11, REAL a0, REAL a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            Point2f p = new Point2f();
            p.x = MathUtil.BLerp(p00.x, p10.x, p01.x, p11.x, a0, a1);
            p.y = MathUtil.BLerp(p00.y, p10.y, p01.y, p11.y, a0, a1);

            return p;
        }

        /// <summary>
        /// A rounded point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        /// <returns>The rounded point</returns>
        public Point2f Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            return new Point2f(x, y);
        }

        /// <summary>
        /// Round the point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            x = MathUtil.Round(x, digits);
            y = MathUtil.Round(y, digits);
        }

        /// <summary>
        /// Floor each component if point.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
        }

        /// <summary>
        /// Ceilling each component if point.
        /// </summary>
        public void Ceilling()
        {
            x = MathUtil.Ceilling(x);
            y = MathUtil.Ceilling(y);
        }

        /// <summary>
        /// Create a array of random points.
        /// </summary>
        /// <param name="seed">The seed</param>
        /// <param name="count">The number of points to create.</param>
        /// <param name="range">The range of the points.</param>
        /// <returns>The point array.</returns>
        public static Point2f[] RandomPoints(int seed, int count, BOX2 range)
        {
            var points = new Point2f[count];
            var rnd = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                REAL x = (REAL)(range.Min.x + rnd.NextDouble() * range.Max.x);
                REAL y = (REAL)(range.Min.y + rnd.NextDouble() * range.Max.y);

                points[i] = new Point2f(x, y);
            }

            return points;
        }

    }
}
