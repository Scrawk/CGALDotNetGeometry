using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using CGALDotNetGeometry.Shapes;

using REAL = System.Single;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct HPoint3f : IEquatable<HPoint3f>
    {
        public REAL x, y, z, w;

        /// <summary>
        /// The unit x point.
        /// </summary>
	    public readonly static HPoint3f UnitX = new HPoint3f(1, 0, 0);

        /// <summary>
        /// The unit y point.
        /// </summary>
	    public readonly static HPoint3f UnitY = new HPoint3f(0, 1, 0);

        /// <summary>
        /// The unit z point.
        /// </summary>
        public readonly static HPoint3f UnitZ = new HPoint3f(0, 0, 1);

        /// <summary>
        /// A point of zeros.
        /// </summary>
	    public readonly static HPoint3f Zero = new HPoint3f(0);

        /// <summary>
        /// A point of ones.
        /// </summary>
	    public readonly static HPoint3f One = new HPoint3f(1);

        /// <summary>
        /// A point of 0.5.
        /// </summary>
        public readonly static HPoint3f Half = new HPoint3f(0.5f);

        /// <summary>
        /// A point of positive infinity.
        /// </summary>
        public readonly static HPoint3f PositiveInfinity = new HPoint3f(REAL.PositiveInfinity);

        /// <summary>
        /// A point of negative infinity.
        /// </summary>
        public readonly static HPoint3f NegativeInfinity = new HPoint3f(REAL.NegativeInfinity);


        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint3f(REAL v)
        {
            this.x = v;
            this.y = v;
            this.z = v;
            this.w = 1;
        }

        /// <summary>
        /// A point all with the value v.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint3f(REAL v, REAL w)
        {
            this.x = v;
            this.y = v;
            this.z = v;
            this.w = w;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint3f(REAL x, REAL y, REAL z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint3f(REAL x, REAL y, REAL z, REAL w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// A point from the varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HPoint3f(double x, double y, double z, double w)
        {
            this.x = (REAL)x;
            this.y = (REAL)y;
            this.z = (REAL)z;
            this.w = (REAL)w;
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
                    throw new IndexOutOfRangeException("HPoint3f index out of range.");

                fixed (HPoint3f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("HPoint3f index out of range.");

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
                if (REAL.IsNaN(z)) return true;
                if (REAL.IsNaN(w)) return true;
                return false;
            }
        }

        /// <summary>
        /// Make a point with no nan conponents.
        /// </summary>
        public HPoint3f NoNAN
        {
            get
            {
                var p = new HPoint3f(x, y, z, w);
                if (REAL.IsNaN(p.x)) p.x = 0;
                if (REAL.IsNaN(p.y)) p.y = 0;
                if (REAL.IsNaN(p.z)) p.z = 0;
                if (REAL.IsNaN(p.w)) p.w = 0;
                return p;
            }
        }

        /// <summary>
        /// Convert from homogenous to cartesian space.
        /// </summary>
        public Point3f Cartesian
        {
            get
            {
                if (w != 0)
                    return new Point3f(x / w, y / w, z / w);
                else
                    return new Point3f(x, y, z);
            }
        }

        /// <summary>
        /// Point as vector.
        /// </summary>
        public Vector3f Vector3f => new Vector3f(x, y, z);

        /// <summary>
        /// Point as vector.
        /// </summary>
        public Vector4d Vector4d => new Vector4d(x, y, z, w);

        /// <summary>
        /// Add two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator +(HPoint3f v1, HPoint3f v2)
        {
            return new HPoint3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator +(HPoint3f v1, REAL s)
        {
            return new HPoint3f(v1.x + s, v1.z + s, v1.z + s, v1.w + s);
        }

        /// <summary>
        /// Add point and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator +(REAL s, HPoint3f v1)
        {
            return new HPoint3f(s + v1.x, s + v1.y, s + v1.z, s + v1.w);
        }

        /// <summary>
        /// Multiply two points.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator *(HPoint3f v1, HPoint3f v2)
        {
            return new HPoint3f(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator *(HPoint3f v, REAL s)
        {
            return new HPoint3f(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Multiply a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator *(REAL s, HPoint3f v)
        {
            return new HPoint3f(v.x * s, v.y * s, v.z * s, v.w * s);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator /(HPoint3f v, REAL s)
        {
            return new HPoint3f(v.x / s, v.y / s, v.z / s, v.w / s);
        }

        /// <summary>
        /// Divide a point and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HPoint3f operator /(REAL s, HPoint3f v)
        {
            return new HPoint3f(s / v.x, s / v.y, s / v.z, s / v.w);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HPoint3f v1, HPoint3f v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
        }

        /// <summary>
        /// Are these points not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HPoint3f v1, HPoint3f v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is HPoint3f)) return false;
            HPoint3f v = (HPoint3f)obj;
            return this == v;
        }

        /// <summary>
        /// Are these points equal.
        /// </summary>
        public bool Equals(HPoint3f v)
        {
            return this == v;
        }

        /// <summary>
        /// Are these points equal given the error.
        /// </summary>
        public static bool AlmostEqual(HPoint3f v0, HPoint3f v1, REAL eps = MathUtil.EPS_32)
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
        /// A rounded point.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        /// <returns>The rounded point</returns>
        public HPoint3f Rounded(int digits)
        {
            REAL x = MathUtil.Round(this.x, digits);
            REAL y = MathUtil.Round(this.y, digits);
            REAL z = MathUtil.Round(this.z, digits);
            REAL w = MathUtil.Round(this.w, digits);
            return new HPoint3f(x, y, z, w);
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

        /// <summary>
        /// Create a array of random points.
        /// </summary>
        /// <param name="seed">The seed</param>
        /// <param name="count">The number of points to create.</param>
        /// <param name="weight">The number of points weight.</param>
        /// <param name="range">The range of the points.</param>
        /// <returns>The point array.</returns>
        public static HPoint3f[] RandomPoints(int seed, int count, REAL weight, Box3f range)
        {
            var points = new HPoint3f[count];
            var rnd = new Random(seed);

            for (int i = 0; i < count; i++)
            {
                REAL x = range.Min.x + (REAL)rnd.NextDouble() * range.Max.x;
                REAL y = range.Min.y + (REAL)rnd.NextDouble() * range.Max.y;
                REAL z = range.Min.z + (REAL)rnd.NextDouble() * range.Max.z;

                points[i] = new HPoint3f(x, y, z, weight);
            }

            return points;
        }

    }
}
