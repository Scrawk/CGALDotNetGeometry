using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Single;

namespace CGALDotNetGeometry.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2f : IEquatable<Vector2f>, IComparable<Vector2f>
    {
        public REAL x, y;

        /// <summary>
        /// The unit x vector.
        /// </summary>
	    public readonly static Vector2f UnitX = new Vector2f(1, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector2f UnitY = new Vector2f(0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector2f Zero = new Vector2f(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
	    public readonly static Vector2f One = new Vector2f(1);

        /// <summary>
        /// A vector of 0.5.
        /// </summary>
        public readonly static Vector2f Half = new Vector2f(0.5f);

        /// <summary>
        /// A vector of positive infinity.
        /// </summary>
        public readonly static Vector2f PositiveInfinity = new Vector2f(REAL.PositiveInfinity);

        /// <summary>
        /// A vector of negative infinity.
        /// </summary>
        public readonly static Vector2f NegativeInfinity = new Vector2f(REAL.NegativeInfinity);

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3f x0y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3f(x, 0, y); }
        }

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3f xy0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3f(x, y, 0); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4f xy01
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, y, 0, 1); }
        }

        /// <summary>
        /// Convert to a 4 dimension vector.
        /// </summary>
        public Vector4f x0y1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, 0, y, 1); }
        }

        /// <summary>
        /// A vector all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(REAL v)
        {
            this.x = v;
            this.y = v;
        }

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2f(double x, double y)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
        }

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Vector2f index out of range.");

                fixed (Vector2f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Vector2f index out of range.");

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
                return true;
            }
        }

        /// <summary>
        /// Make a vector with no non finite conponents.
        /// </summary>
        public Vector2f Finite
        {
            get
            {
                var v = new Vector2f(x, y);
                if (!MathUtil.IsFinite(v.x)) v.x = 0;
                if (!MathUtil.IsFinite(v.y)) v.y = 0;
                return v;
            }
        }

        /// <summary>
        /// Are any of the vectors components nan.
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
        /// Make a vector with no nan conponents.
        /// </summary>
        public Vector2f NoNAN
        {
            get
            {
                var v = new Vector2f(x, y);
                if (REAL.IsNaN(v.x)) v.x = 0;
                if (REAL.IsNaN(v.y)) v.y = 0;
                return v;
            }
        }

        /// <summary>
        /// The sum of the vectors components.
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
        /// The product of the vectors components.
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
                return (x * x + y * y);
            }
        }

        /// <summary>
        /// The vector normalized.
        /// </summary>
        public Vector2f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL invLength = MathUtil.SafeInvSqrt(1.0f, x * x + y * y);
                return new Vector2f(x * invLength, y * invLength);
            }
        }

        /// <summary>
        /// Counter clock-wise perpendicular.
        /// </summary>
        public Vector2f PerpendicularCCW
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2f(-y, x);
            }
        }

        /// <summary>
        /// Clock-wise perpendicular.
        /// </summary>
        public Vector2f PerpendicularCW
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2f(y, -x);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector2f Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector2f(Math.Abs(x), Math.Abs(y));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(Vector2f v1, REAL s)
        {
            return new Vector2f(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator +(REAL s, Vector2f v1)
        {
            return new Vector2f(v1.x + s, v1.y + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f v)
        {
            return new Vector2f(-v.x, -v.y);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(Vector2f v1, REAL s)
        {
            return new Vector2f(v1.x - s, v1.y - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator -(REAL s, Vector2f v1)
        {
            return new Vector2f(s - v1.x, s - v1.y);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(Vector2f v, REAL s)
        {
            return new Vector2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator *(REAL s, Vector2f v)
        {
            return new Vector2f(v.x * s, v.y * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x / v2.x, v1.y / v2.y);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(Vector2f v, REAL s)
        {
            return new Vector2f(v.x / s, v.y / s);
        }

        /// <summary>
        /// Divide a scalar and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2f operator /(REAL s, Vector2f v)
        {
            return new Vector2f(s / v.x, s / v.y);
        }

        /// <summary>
        /// Cast from Vector2f to Vector2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2f(Vector2d v)
        {
            return new Vector2f(v.x, v.y);
        }

        /// <summary>
        /// Cast from Point2f to Vector2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2f(Point2d v)
        {
            return new Vector2f(v.x, v.y);
        }

        /// <summary>
        /// Cast from Point2f to Vector2f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2f(Point2f v)
        {
            return new Vector2f(v.x, v.y);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2f v1, Vector2f v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2f v1, Vector2f v2)
        {
            return (v1.x != v2.x || v1.y != v2.y);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2f)) return false;
            Vector2f v = (Vector2f)obj;
            return this == v;
        }

        /// <summary>
        /// Are these vectors equal given the error.
        /// </summary>
        public static bool AlmostEqual(Vector2f v0, Vector2f v1, REAL eps = MathUtil.EPS_32)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public bool Equals(Vector2f v)
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
        /// Compare two vectors by axis.
        /// </summary>
        public int CompareTo(Vector2f other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            return 0;
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
        /// The dot product of two vectors.
        /// </summary>
        public static REAL Dot(Vector2f v0, Vector2f v1)
        {
            return (v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// The dot product of vector and point.
        /// </summary>
        public static REAL Dot(Vector2f v0, Point2f v1)
        {
            return (v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// The dot product of two pointss.
        /// </summary>
        public static REAL Dot(Point2f v0, Point2f v1)
        {
            return (v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// The abs dot product of two vectors.
        /// </summary>
        public static REAL AbsDot(Vector2f v0, Vector2f v1)
        {
            return Math.Abs(v0.x * v1.x + v0.y * v1.y);
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        public void Normalize()
        {
            REAL invLength = MathUtil.SafeInvSqrt(1.0f, x * x + y * y);
            x *= invLength;
            y *= invLength;
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        public static REAL Cross(Vector2f v0, Vector2f v1)
        {
            return v0.x * v1.y - v0.y * v1.x;
        }

        /// <summary>
        /// Project vector v onto u.
        /// </summary>
        public static Vector2f Project(Vector2f u, Vector2f v)
        {
            return Dot(u, v) / u.SqrMagnitude * u;
        }

        /// <summary>
        /// Given an incident vector i and a normal vector n.
        /// </summary>
        public static Vector2f Reflect(Vector2f i, Vector2f n)
        {
            return i - 2 * n * Dot(i, n);
        }

        /// <summary>
        /// Returns the refraction vector given the incident vector i, 
        /// the normal vector n and the refraction index eta.
        /// </summary>
        public static Vector2f Refract(Vector2f i, Vector2f n, REAL eta)
        {
            REAL ni = Dot(n, i);
            REAL k = 1.0f - eta * eta * (1.0f - ni * ni);

            return (k >= 0) ? eta * i - (eta * ni + MathUtil.SafeSqrt(k)) * n : Zero;
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 180.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        public static Degree Angle180(Vector2f a, Vector2f b)
        {
            REAL dp = Dot(a, b);
            REAL m = a.Magnitude * b.Magnitude;
            REAL angle = MathUtil.ToDegrees(MathUtil.SafeAcos(MathUtil.SafeDiv(dp, m)));
            return new Degree(angle);
        }

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 360.
        /// Angle represents moving ccw from a to b.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        public static Degree Angle360(Vector2f a, Vector2f b)
        {
            REAL angle = MathUtil.Atan2(a.y, a.x) - MathUtil.Atan2(b.y, b.x);

            if (angle <= 0.0)
                angle = MathUtil.PI_32 * 2.0f + angle;

            angle = 360.0f - MathUtil.ToDegrees(angle);
            return new Degree(angle >= 360.0f ? 0.0f : angle);
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        public static Vector2f Min(Vector2f v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        public static Vector2f Min(Vector2f v0, Vector2f v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        public static Vector2f Max(Vector2f v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        public static Vector2f Max(Vector2f v0, Vector2f v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Vector2f Clamp(Vector2f v, REAL min, REAL max)
        {
            v.x = Math.Max(Math.Min(v.x, max), min);
            v.y = Math.Max(Math.Min(v.y, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Vector2f Clamp(Vector2f v, Vector2f min, Vector2f max)
        {
            v.x = Math.Max(Math.Min(v.x, max.x), min.x);
            v.y = Math.Max(Math.Min(v.y, max.y), min.y);
            return v;
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        public static Vector2f Lerp(Vector2f v0, Vector2f v1, REAL a)
        {
            a = MathUtil.Clamp01(a);
            Vector2f v = new Vector2f();
            v.x = MathUtil.Lerp(v0.x, v1.x, a);
            v.y = MathUtil.Lerp(v0.y, v1.y, a);
            return v;
        }

        /// <summary>
        /// BLerp between four vectors.
        /// </summary>
        public static Vector2f BLerp(Vector2f v00, Vector2f v10, Vector2f v01, Vector2f v11, REAL a0, REAL a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            Vector2f v = new Vector2f();
            v.x = MathUtil.BLerp(v00.x, v10.x, v01.x, v11.x, a0, a1);
            v.y = MathUtil.BLerp(v00.y, v10.y, v01.y, v11.y, a0, a1);

            return v;
        }

        /// <summary>
        /// Slerp between two vectors arc.
        /// </summary>
        public static Vector2f Slerp(Vector2f from, Vector2f to, REAL t)
        {
            if (t < 0.0) t = 0.0f;
            if (t > 1.0) t = 1.0f;

            if (t == 0.0) return from;
            if (t == 1.0) return to;
            if (to.x == from.x && to.y == from.y) return to;

            REAL m = from.Magnitude * to.Magnitude;
            if (MathUtil.IsZero(m)) return Vector2f.Zero;

            REAL theta = MathUtil.Acos(Dot(from, to) / m);

            if (theta == 0.0) return to;

            REAL sinTheta = MathUtil.Sin(theta);
            REAL st1 = MathUtil.Sin((1.0f - t) * theta) / sinTheta;
            REAL st = MathUtil.Sin(t * theta) / sinTheta;

            Vector2f v = new Vector2f();
            v.x = from.x * st1 + to.x * st;
            v.y = from.y * st1 + to.y * st;

            return v;
        }

        /// <summary>
        /// Round vector.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public Vector2f Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            return new Vector2f(x, y);
        }

        /// <summary>
        /// Round the vector.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            x = MathUtil.Round(x, digits);
            y = MathUtil.Round(y, digits);
        }

        /// <summary>
        /// Floor each component of vector.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
        }

        /// <summary>
        /// Ceilling each component of vector.
        /// </summary>
        public void Ceilling()
        {
            x = MathUtil.Ceilling(x);
            y = MathUtil.Ceilling(y);
        }
    }

}


































