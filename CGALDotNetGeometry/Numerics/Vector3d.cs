using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Double;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3d : IEquatable<Vector3d>, IComparable<Vector3d>
    {
		public REAL x, y, z;

        /// <summary>
        /// The unit x vector.
        /// </summary>
        public readonly static Vector3d UnitX = new Vector3d(1, 0, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector3d UnitY = new Vector3d(0, 1, 0);

        /// <summary>
        /// The unit z vector.
        /// </summary>
	    public readonly static Vector3d UnitZ = new Vector3d(0, 0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector3d Zero = new Vector3d(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
        public readonly static Vector3d One = new Vector3d(1);

        /// <summary>
        /// A vector of 0.5.
        /// </summary>
        public readonly static Vector3d Half = new Vector3d(0.5);

        /// <summary>
        /// A vector of positive infinity.
        /// </summary>
        public readonly static Vector3d PositiveInfinity = new Vector3d(REAL.PositiveInfinity);

        /// <summary>
        /// A vector of negative infinity.
        /// </summary>
        public readonly static Vector3d NegativeInfinity = new Vector3d(REAL.NegativeInfinity);

        /// <summary>
        /// 3D vector to 3D swizzle vector.
        /// </summary>
        public Vector3d xzy => new Vector3d(x, z, y);

        /// <summary>
        /// 3D vector to 2D vector.
        /// </summary>
        public Vector2d xy => new Vector2d(x, y);

        /// <summary>
        /// 3D vector to 2D vector.
        /// </summary>
        public Vector2d xz => new Vector2d(x, z);

        /// <summary>
        /// 3D vector to 2D vector.
        /// </summary>
        public Vector2d zy => new Vector2d(z, y);

        /// <summary>
        /// 3D vector to 4D vector with w as 0.
        /// </summary>
        public Vector4d xyz0 => new Vector4d(x, y, z, 0);

        /// <summary>
        /// 3D vector to 4D vector with w as 1.
        /// </summary>
        public Vector4d xyz1 => new Vector4d(x, y, z, 1);

        /// <summary>
        /// A vector all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(REAL v) 
		{
			this.x = v; 
			this.y = v; 
			this.z = v;
		}

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(REAL x, REAL y, REAL z) 
		{
			this.x = x; 
			this.y = y;
			this.z = z;
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(REAL x, REAL y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// A vector from a 2d vector and the z varible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3d(Vector2d v, REAL z) 
		{ 
			x = v.x; 
			y = v.y; 
			this.z = z;
		}

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Vector3d index out of range.");

                fixed (Vector3d* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Vector3d index out of range.");

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
                if (!MathUtil.IsFinite(z)) return false;
                return true;
            }
        }

        /// <summary>
        /// Make a vector with no non finite conponents.
        /// </summary>
        public Vector3d Finite
        {
            get
            {
                var v = new Vector3d(x, y, z);
                if (!MathUtil.IsFinite(v.x)) v.x = 0;
                if (!MathUtil.IsFinite(v.y)) v.y = 0;
                if (!MathUtil.IsFinite(v.z)) v.z = 0;
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
                if (REAL.IsNaN(z)) return true;
                return false;
            }
        }

        /// <summary>
        /// Make a vector with no nan conponents.
        /// </summary>
        public Vector3d NoNAN
        {
            get
            {
                var v = new Vector3d(x, y, z);
                if (REAL.IsNaN(v.x)) v.x = 0;
                if (REAL.IsNaN(v.y)) v.y = 0;
                if (REAL.IsNaN(v.z)) v.z = 0;
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
                return x + y + z;
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
                return x * y * z;
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
        /// The vector normalized.
        /// </summary>
        public Vector3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL invLength = MathUtil.SafeInvSqrt(1.0, x * x + y * y + z * z);
                return new Vector3d(x * invLength, y * invLength, z * invLength);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector3d Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector3d(Math.Abs(x), Math.Abs(y), Math.Abs(z));
            }
        }

        /// <summary>
        /// Convert a normalized vector to tangent space.
        /// </summary>
        public Vector3d TangentSpaceNormal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL x = this.x * 0.5 + 0.5;
                REAL y = this.z * 0.5 + 0.5;
                REAL z = this.y;

                return new Vector3d(x, y, z);
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(Vector3d v1, REAL s)
        {
            return new Vector3d(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator +(REAL s, Vector3d v1)
        {
            return new Vector3d(v1.x + s, v1.y + s, v1.z + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d v)
        {
            return new Vector3d(-v.x, -v.y, -v.z);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(Vector3d v1, REAL s)
        {
            return new Vector3d(v1.x - s, v1.y - s, v1.z - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator -(REAL s, Vector3d v1)
        {
            return new Vector3d(s - v1.x, s - v1.y, s - v1.z);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(Vector3d v, REAL s)
        {
            return new Vector3d(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator *(REAL s, Vector3d v)
        {
            return new Vector3d(v.x * s, v.y * s, v.z * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d v1, Vector3d v2)
        {
            return new Vector3d(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(Vector3d v, REAL s)
        {
            return new Vector3d(v.x / s, v.y / s, v.z / s);
        }

        /// <summary>
        /// Divide a scalar and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3d operator /(REAL s, Vector3d v)
        {
            return new Vector3d(s / v.x, s / v.y, s / v.z);
        }

        /// <summary>
        /// Cast from Vector3f to Vector3d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3d(Vector3f v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Point3f to Vector3d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3d(Point3f v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        /// <summary>
        /// Cast from Point3d to Vector3d.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3d(Point3d v)
        {
            return new Vector3d(v.x, v.y, v.z);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3d v1, Vector3d v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3d v1, Vector3d v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public override bool Equals (object obj)
		{
			if(!(obj is Vector3d)) return false;
			Vector3d v = (Vector3d)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal given the error.
        /// </summary>
        public static bool AlmostEqual(Vector3d v0, Vector3d v1, REAL eps = MathUtil.EPS_64)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            if (Math.Abs(v0.z - v1.z) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public bool Equals(Vector3d v)
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
                hash = (hash * MathUtil.HASH_PRIME_2) ^ z.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compare two vectors by axis.
        /// </summary>
        public int CompareTo(Vector3d other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            else if (z != other.z)
                return z < other.z ? -1 : 1;
            return 0;
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
        /// The dot product of two vectors.
        /// </summary>
        public static REAL Dot(Vector3d v0, Vector3d v1)
		{
			return (v0.x*v1.x + v0.y*v1.y + v0.z*v1.z);
		}

        /// <summary>
        /// The dot product of vector and point.
        /// </summary>
        public static REAL Dot(Vector3d v0, Point3d v1)
        {
            return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);
        }

        /// <summary>
        /// The dot product of two points.
        /// </summary>
        public static REAL Dot(Point3d v0, Point3d v1)
        {
            return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);
        }

        /// <summary>
        /// The abs dot product of two vectors.
        /// </summary>
        public static REAL AbsDot(Vector3d v0, Vector3d v1)
        {
            return Math.Abs(v0.x * v1.x + v0.y * v1.y + v0.z * v1.z);
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        public void Normalize()
		{
            REAL invLength = MathUtil.SafeInvSqrt(1.0, x * x + y * y + z * z);
	    	x *= invLength;
			y *= invLength;
			z *= invLength;
		}

        /// <summary>
        /// Angle between two vectors in degrees from 0 to 180.
        /// A and b origin treated as 0,0 and do not need to be normalized.
        /// </summary>
        public static Degree Angle180(Vector3d a, Vector3d b)
        {
            REAL dp = Dot(a, b);
            REAL m = a.Magnitude * b.Magnitude;
            REAL angle = MathUtil.ToDegrees(MathUtil.SafeAcos(MathUtil.SafeDiv(dp, m)));
            return new Degree(angle);
        }

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        public Vector3d Cross(Vector3d v)
		{
			return new Vector3d(y*v.z - z*v.y, z*v.x - x*v.z, x*v.y - y*v.x);
		}

        /// <summary>
        /// Cross two vectors.
        /// </summary>
        public static Vector3d Cross(Vector3d v0, Vector3d v1)
		{
			return new Vector3d(v0.y*v1.z - v0.z*v1.y, v0.z*v1.x - v0.x*v1.z, v0.x*v1.y - v0.y*v1.x);
		}

        /// <summary>
        /// Cross a vector andpoint.
        /// </summary>
        public static Vector3d Cross(Vector3d v0, Point3d v1)
        {
            return new Vector3d(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);
        }

        /// <summary>
        /// Cross two points.
        /// </summary>
        public static Vector3d Cross(Point3d v0, Point3d v1)
        {
            return new Vector3d(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);
        }

        /// <summary>
        /// Project vector v onto u.
        /// </summary>
        public static Vector3d Project(Vector3d u, Vector3d v)
        {
            return Dot(u, v) / u.SqrMagnitude * u;
        }

        /// <summary>
        /// Given an incident vector i and a normal vector n.
        /// </summary>
        public static Vector3d Reflect(Vector3d i, Vector3d n)
        {
            return i - 2 * n * Dot(i, n);
        }

        /// <summary>
        /// Returns the refraction vector given the incident vector i, 
        /// the normal vector n and the refraction index eta.
        /// </summary>
        /// <param name="i">The incident vector</param>
        /// <param name="n">The normal vector</param>
        /// <param name="eta">The refraction index</param>
        /// <param name="r">The reflected ray.</param>
        /// <returns>True if there is a solution.</returns>
        public static bool Refract(Vector3d i, Vector3d n, REAL eta, out Vector3d r)
        {
            REAL ni = Dot(n, i);
            REAL k = 1.0f - eta * eta * (1.0f - ni * ni);

            if (k > 0)
            {
                r = eta * i - (eta * ni + MathUtil.SafeSqrt(k)) * n;
                return true;
            }
            else
            {
                r = Zero;
                return false;
            }
        }

        /// <summary>
        /// Create a set of orthonormal vectors.
        /// </summary>
        public static void Orthonormal(ref Vector3d a, ref Vector3d b, out Vector3d c)
        {
            a.Normalize();
            c = Cross(a, b);

            if (MathUtil.IsZero(c.SqrMagnitude))
                throw new ArgumentException("a and b are parallel");

            c.Normalize();
            b = Cross(c, a);
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        public static Vector3d Min(Vector3d v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            v.z = Math.Min(v.z, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        public static Vector3d Min(Vector3d v0, Vector3d v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            v0.z = Math.Min(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        public static Vector3d Max(Vector3d v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            v.z = Math.Max(v.z, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        public static Vector3d Max(Vector3d v0, Vector3d v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            v0.z = Math.Max(v0.z, v1.z);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Vector3d Clamp(Vector3d v, REAL min, REAL max)
        {
            v.x = Math.Max(Math.Min(v.x, max), min);
            v.y = Math.Max(Math.Min(v.y, max), min);
            v.z = Math.Max(Math.Min(v.z, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Vector3d Clamp(Vector3d v, Vector3d min, Vector3d max)
        {
            v.x = Math.Max(Math.Min(v.x, max.x), min.x);
            v.y = Math.Max(Math.Min(v.y, max.y), min.y);
            v.z = Math.Max(Math.Min(v.z, max.z), min.z);
            return v;
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        public static Vector3d Lerp(Vector3d v0, Vector3d v1, REAL a)
        {
            a = MathUtil.Clamp01(a);
            Vector3d v = new Vector3d();
            v.x = MathUtil.Lerp(v0.x, v1.x, a);
            v.y = MathUtil.Lerp(v0.y, v1.y, a);
            v.z = MathUtil.Lerp(v0.z, v1.z, a);
            return v;
        }

        /// <summary>
        /// BLerp between four vectors.
        /// </summary>
        public static Vector3d BLerp(Vector3d v00, Vector3d v10, Vector3d v01, Vector3d v11, REAL a0, REAL a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            Vector3d v = new Vector3d();
            v.x = MathUtil.BLerp(v00.x, v10.x, v01.x, v11.x, a0, a1);
            v.y = MathUtil.BLerp(v00.y, v10.y, v01.y, v11.y, a0, a1);
            v.z = MathUtil.BLerp(v00.z, v10.z, v01.z, v11.z, a0, a1);

            return v;
        }

        /// <summary>
        /// Slerp between two vectors arc.
        /// </summary>
        public static Vector3d Slerp(Vector3d from, Vector3d to, REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            if (t == 0.0f) return from;
            if (t == 1.0f) return to;
            if (to.x == from.x && to.y == from.y && to.z == from.z) return to;

            REAL m = from.Magnitude * to.Magnitude;
            if (MathUtil.IsZero(m)) return Vector3d.Zero;

            REAL theta = MathUtil.Acos(Dot(from, to) / m);

            if (theta == 0.0) return to;

            REAL sinTheta = MathUtil.Sin(theta);
            REAL st1 = MathUtil.Sin((1.0 - t) * theta) / sinTheta;
            REAL st = MathUtil.Sin(t * theta) / sinTheta;

            Vector3d v = new Vector3d();
            v.x = from.x * st1 + to.x * st;
            v.y = from.y * st1 + to.y * st;
            v.z = from.z * st1 + to.z * st;

            return v;
        }

        /// <summary>
        /// Round vector.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public Vector3d Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            REAL z = MathUtil.Round(this.z, digits);
            return new Vector3d(x, y, z);
        }

        /// <summary>
        /// Round the vector.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            x = MathUtil.Round(x, digits);
            y = MathUtil.Round(y, digits);
            z = MathUtil.Round(z, digits);
        }

        /// <summary>
        /// Floor each component of vector.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
            z = MathUtil.Floor(z);
        }

        /// <summary>
        /// Ceilling each component of vector.
        /// </summary>
        public void Ceilling()
        {
            x = MathUtil.Ceilling(x);
            y = MathUtil.Ceilling(y);
            z = MathUtil.Ceilling(z);
        }

    }

}


































