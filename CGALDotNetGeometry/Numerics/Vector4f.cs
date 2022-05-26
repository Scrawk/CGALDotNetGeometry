using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Single;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4f : IEquatable<Vector4f>, IComparable<Vector4f>
    {

        public REAL x, y, z, w;

        /// <summary>
        /// The unit x vector.
        /// </summary>
        public readonly static Vector4f UnitX = new Vector4f(1, 0, 0, 0);

        /// <summary>
        /// The unit y vector.
        /// </summary>
	    public readonly static Vector4f UnitY = new Vector4f(0, 1, 0, 0);

        /// <summary>
        /// The unit z vector.
        /// </summary>
	    public readonly static Vector4f UnitZ = new Vector4f(0, 0, 1, 0);

        /// <summary>
        /// The unit w vector.
        /// </summary>
	    public readonly static Vector4f UnitW = new Vector4f(0, 0, 0, 1);

        /// <summary>
        /// A vector of zeros.
        /// </summary>
	    public readonly static Vector4f Zero = new Vector4f(0);

        /// <summary>
        /// A vector of ones.
        /// </summary>
	    public readonly static Vector4f One = new Vector4f(1);

        /// <summary>
        /// A vector of 0.5.
        /// </summary>
        public readonly static Vector4f Half = new Vector4f(0.5f);

        /// <summary>
        /// A vector of positive infinity.
        /// </summary>
        public readonly static Vector4f PositiveInfinity = new Vector4f(REAL.PositiveInfinity);

        /// <summary>
        /// A vector of negative infinity.
        /// </summary>
        public readonly static Vector4f NegativeInfinity = new Vector4f(REAL.NegativeInfinity);

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2f xy
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2f(x, y); }
        }

        /// <summary>
        /// Convert to a 2 dimension vector.
        /// </summary>
        public Vector2f xz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector2f(x, z); }
        }

        /// <summary>
        /// Convert to a 3 dimension vector.
        /// </summary>
        public Vector3f xyz
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector3f(x, y, z); }
        }

        /// <summary>
        /// A copy of the vector with w as 0.
        /// </summary>
        public Vector4f xyz0
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Vector4f(x, y, z, 0); }
        }

        /// <summary>
        /// A vector all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(REAL v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
            this.w = v;
        }

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(REAL x, REAL y, REAL z, REAL w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// A vector from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(double x, double y, double z, double w)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
            this.z = (REAL)z;
            this.w = (REAL)w;
        }

        /// <summary>
        /// A vector from a 2d vector and the z and w varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(Vector2f v, REAL z, REAL w)
        {
            x = v.x;
            y = v.y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// A vector from a 3d vector and the w varible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4f(Vector3f v, REAL w)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            this.w = w;
        }

        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Vector4f index out of range.");

                fixed (Vector4f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Vector4f index out of range.");

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
                if (!MathUtil.IsFinite(w)) return false;
                return true;
            }
        }

        /// <summary>
        /// Make a vector with no non finite conponents.
        /// </summary>
        public Vector4f Finite
        {
            get
            {
                var v = new Vector4f(x, y, z, w);
                if (!MathUtil.IsFinite(v.x)) v.x = 0;
                if (!MathUtil.IsFinite(v.y)) v.y = 0;
                if (!MathUtil.IsFinite(v.z)) v.z = 0;
                if (!MathUtil.IsFinite(v.w)) v.w = 0;
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
                if (REAL.IsNaN(w)) return true;
                return false;
            }
        }

        /// <summary>
        /// Make a vector with no nan conponents.
        /// </summary>
        public Vector4f NoNAN
        {
            get
            {
                var v = new Vector4f(x, y, z, w);
                if (REAL.IsNaN(v.x)) v.x = 0;
                if (REAL.IsNaN(v.y)) v.y = 0;
                if (REAL.IsNaN(v.z)) v.z = 0;
                if (REAL.IsNaN(v.w)) v.w = 0;
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
                return x + y + z + w;
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
                return x * y * z * w;
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
        /// The vector normalized.
        /// </summary>
        public Vector4f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL invLength = MathUtil.SafeInvSqrt(1.0f, x * x + y * y + z * z + w * w);
                return new Vector4f(x * invLength, y * invLength, z * invLength, w * invLength);
            }
        }

        /// <summary>
        /// The vectors absolute values.
        /// </summary>
        public Vector4f Absolute
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Vector4f(Math.Abs(x), Math.Abs(y), Math.Abs(z), Math.Abs(w));
            }
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator +(Vector4f v1, Vector4f v2)
        {
            return new Vector4f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator +(Vector4f v1, REAL s)
        {
            return new Vector4f(v1.x + s, v1.y + s, v1.z + s, v1.w + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator +(REAL s, Vector4f v1)
        {
            return new Vector4f(v1.x + s, v1.y + s, v1.z + s, v1.w + s);
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator -(Vector4f v)
        {
            return new Vector4f(-v.x, -v.y, -v.z, -v.w);
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator -(Vector4f v1, Vector4f v2)
        {
            return new Vector4f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator -(Vector4f v1, REAL s)
        {
            return new Vector4f(v1.x - s, v1.y - s, v1.z - s, v1.w - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator -(REAL s, Vector4f v1)
        {
            return new Vector4f(s - v1.x, s - v1.y, s - v1.z, s - v1.w);
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(Vector4f v1, Vector4f v2)
        {
            return new Vector4f(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(Vector4f v, REAL s)
        {
            return new Vector4f(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator *(REAL s, Vector4f v)
        {
            return new Vector4f(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator /(Vector4f v1, Vector4f v2)
        {
            return new Vector4f(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator /(Vector4f v, REAL s)
        {
            return new Vector4f(v.x / s, v.y / s, v.z / s, v.w / s);
        }

        /// <summary>
        /// Divide a scalar and a vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4f operator /(REAL s, Vector4f v)
        {
            return new Vector4f(s / v.x, s / v.y, s / v.z, s / v.w);
        }

        /// <summary>
        /// Cast from Vector4d to Vector4f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4f(Vector4d v)
        {
            return new Vector4f(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Cast from Point4d to Vector4f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector4f(Point4d v)
        {
            return new Vector4f(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Cast from Point4f to Vector4f.
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector4f(Point4f v)
        {
            return new Vector4f(v.x, v.y, v.z, v.w);
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4f v1, Vector4f v2)
		{
			return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
		}

        /// <summary>
        /// Are these vectors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector4f v1, Vector4f v2)
		{
			return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
		}

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public override bool Equals (object obj)
		{
			if(!(obj is Vector4f)) return false;
			Vector4f v = (Vector4f)obj;
			return this == v;
		}

        /// <summary>
        /// Are these vectors equal given the error.
        /// </summary>
        public static bool AlmostEqual(Vector4f v0, Vector4f v1, REAL eps = MathUtil.EPS_32)
        {
            if (Math.Abs(v0.x - v1.x) > eps) return false;
            if (Math.Abs(v0.y - v1.y) > eps) return false;
            if (Math.Abs(v0.z - v1.z) > eps) return false;
            if (Math.Abs(v0.w - v1.w) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these vectors equal.
        /// </summary>
        public bool Equals(Vector4f v)
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
                hash = (hash * MathUtil.HASH_PRIME_2) ^ w.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compare two vectors by axis.
        /// </summary>
        public int CompareTo(Vector4f other)
        {
            if (x != other.x)
                return x < other.x ? -1 : 1;
            else if (y != other.y)
                return y < other.y ? -1 : 1;
            else if (z != other.z)
                return z < other.z ? -1 : 1;
            else if (w != other.w)
                return w < other.w ? -1 : 1;
            return 0;
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
        /// The dot product of two vectors.
        /// </summary>
        public static REAL Dot(Vector4f v0, Vector4f v1)
		{
			return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z + v0.w * v1.w);
		}

        /// <summary>
        /// The dot product of vector and point.
        /// </summary>
        public static REAL Dot(Vector4f v0, Point4f v1)
        {
            return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z + v0.w * v1.w);
        }

        /// <summary>
        /// The dot product of two points.
        /// </summary>
        public static REAL Dot(Point4f v0, Point4f v1)
        {
            return (v0.x * v1.x + v0.y * v1.y + v0.z * v1.z + v0.w * v1.w);
        }

        /// <summary>
        /// The abs dot product of two vectors.
        /// </summary>
        public static REAL AbsDot(Vector4f v0, Vector4f v1)
        {
            return Math.Abs(v0.x * v1.x + v0.y * v1.y + v0.z * v1.z + v0.w * v1.w);
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        public void Normalize()
        {
            REAL invLength = MathUtil.SafeInvSqrt(1.0f, x * x + y * y + z * z + w * w);
            x *= invLength;
            y *= invLength;
            z *= invLength;
            w *= invLength;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        public static Vector4f Min(Vector4f v, REAL s)
        {
            v.x = Math.Min(v.x, s);
            v.y = Math.Min(v.y, s);
            v.z = Math.Min(v.z, s);
            v.w = Math.Min(v.w, s);
            return v;
        }

        /// <summary>
        /// The minimum value between each component in vectors.
        /// </summary>
        public static Vector4f Min(Vector4f v0, Vector4f v1)
        {
            v0.x = Math.Min(v0.x, v1.x);
            v0.y = Math.Min(v0.y, v1.y);
            v0.z = Math.Min(v0.z, v1.z);
            v0.w = Math.Min(v0.w, v1.w);
            return v0;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        public static Vector4f Max(Vector4f v, REAL s)
        {
            v.x = Math.Max(v.x, s);
            v.y = Math.Max(v.y, s);
            v.z = Math.Max(v.z, s);
            v.w = Math.Max(v.w, s);
            return v;
        }

        /// <summary>
        /// The maximum value between each component in vectors.
        /// </summary>
        public static Vector4f Max(Vector4f v0, Vector4f v1)
        {
            v0.x = Math.Max(v0.x, v1.x);
            v0.y = Math.Max(v0.y, v1.y);
            v0.z = Math.Max(v0.z, v1.z);
            v0.w = Math.Max(v0.w, v1.w);
            return v0;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Vector4f Clamp(Vector4f v, REAL min, REAL max)
        {
            v.x = Math.Max(Math.Min(v.x, max), min);
            v.y = Math.Max(Math.Min(v.y, max), min);
            v.z = Math.Max(Math.Min(v.z, max), min);
            v.w = Math.Max(Math.Min(v.w, max), min);
            return v;
        }

        /// <summary>
        /// Clamp each component to specified min and max.
        /// </summary>
        public static Vector4f Clamp(Vector4f v, Vector4f min, Vector4f max)
        {
            v.x = Math.Max(Math.Min(v.x, max.x), min.x);
            v.y = Math.Max(Math.Min(v.y, max.y), min.y);
            v.z = Math.Max(Math.Min(v.z, max.z), min.z);
            v.w = Math.Max(Math.Min(v.w, max.w), min.w);
            return v;
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        public static Vector4f Lerp(Vector4f v0, Vector4f v1, REAL a)
        {
            a = MathUtil.Clamp01(a);
            Vector4f v = new Vector4f();
            v.x = MathUtil.Lerp(v0.x, v1.x, a);
            v.y = MathUtil.Lerp(v0.y, v1.y, a);
            v.z = MathUtil.Lerp(v0.z, v1.z, a);
            v.w = MathUtil.Lerp(v0.w, v1.w, a);
            return v;
        }

        /// <summary>
        /// BLerp between four vectors.
        /// </summary>
        public static Vector4f BLerp(Vector4f v00, Vector4f v10, Vector4f v01, Vector4f v11, REAL a0, REAL a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            Vector4f v = new Vector4f();
            v.x = MathUtil.BLerp(v00.x, v10.x, v01.x, v11.x, a0, a1);
            v.y = MathUtil.BLerp(v00.y, v10.y, v01.y, v11.y, a0, a1);
            v.z = MathUtil.BLerp(v00.z, v10.z, v01.z, v11.z, a0, a1);
            v.w = MathUtil.BLerp(v00.w, v10.w, v01.w, v11.w, a0, a1);

            return v;
        }

        /// <summary>
        /// Round vector.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public Vector4f Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            REAL z = MathUtil.Round(this.z, digits);
            REAL w = MathUtil.Round(this.w, digits);
            return new Vector4f(x, y, z, w);
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
            w = MathUtil.Round(w, digits);
        }

        /// <summary>
        /// Floor each component of vector.
        /// </summary>
        public void Floor()
        {
            x = MathUtil.Floor(x);
            y = MathUtil.Floor(y);
            z = MathUtil.Floor(z);
            w = MathUtil.Floor(w);
        }

        /// <summary>
        /// Ceilling each component of vector.
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


































