using System;
using System.Runtime.InteropServices;

using CGALDotNetGeometry.Numerics;

using REAL = System.Double;
using POINT3 = CGALDotNetGeometry.Numerics.Point3d;
using BOX3 = CGALDotNetGeometry.Shapes.Box3d;
using MATRIX3 = CGALDotNetGeometry.Numerics.Matrix3x3d;
using MATRIX4 = CGALDotNetGeometry.Numerics.Matrix4x4d;

namespace CGALDotNetGeometry.Shapes
{
    /// <summary>
    /// A 3D tetrahedron.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Tetrahedron3d : IEquatable<Tetrahedron3d>
    {
        /// <summary>
        /// The tetrahedrons first point.
        /// </summary>
        public POINT3 A;

        /// <summary>
        /// The tetrahedrons second point.
        /// </summary>
        public POINT3 B;

        /// <summary>
        /// The tetrahedrons third point.
        /// </summary>
        public POINT3 C;

        /// <summary>
        /// The tetrahedrons fourth point.
        /// </summary>
        public POINT3 D;

        /// <summary>
        /// Create a new tetrahedron.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <param name="c">The third point.</param>
        /// <param name="d">The fourth point.</param>
        public Tetrahedron3d(POINT3 a, POINT3 b, POINT3 c, POINT3 d)
        {
            A = a;
            B = b;
            C = c;
            D = c;
        }

        /// <summary>
        /// Does the shape contain no non finite points.
        /// </summary>
        public bool IsFinite
        {
            get
            {
                if (!A.IsFinite) return false;
                if (!B.IsFinite) return false;
                if (!C.IsFinite) return false;
                if (!D.IsFinite) return false;
                return true;
            }
        }

        /// <summary>
        /// The bounding box of the tetrahedron.
        /// </summary>
        public BOX3 Bounds
        {
            get
            {
                var xmin = MathUtil.Min(A.x, B.x, C.x, D.x);
                var xmax = MathUtil.Max(A.x, B.x, C.x, D.x);
                var ymin = MathUtil.Min(A.y, B.y, C.y, D.y);
                var ymax = MathUtil.Max(A.y, B.y, C.y, D.y);
                var zmin = MathUtil.Min(A.z, B.z, C.z, D.z);
                var zmax = MathUtil.Max(A.z, B.z, C.z, D.z);

                return new BOX3(new POINT3(xmin, ymin, zmin), new POINT3(xmax, ymax, zmin));
            }
        }

        /// <summary>
        /// Array acess to the tetrahedrons points.
        /// </summary>
        /// <param name="i">The index of the point to access (0-3)</param>
        /// <returns>The point at index i.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        unsafe public POINT3 this[int i]
        {
            get
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Tetrahedron3d index out of range.");

                fixed (Tetrahedron3d* array = &this) { return ((POINT3*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Tetrahedron3d index out of range.");

                fixed (POINT3* array = &A) { array[i] = value; }
            }
        }

        public static Tetrahedron3d operator +(Tetrahedron3d tri, REAL s)
        {
            return new Tetrahedron3d(tri.A + s, tri.B + s, tri.C + s, tri.D + s);
        }

        public static Tetrahedron3d operator +(Tetrahedron3d tri, POINT3 v)
        {
            return new Tetrahedron3d(tri.A + v, tri.B + v, tri.C + v, tri.D + v);
        }

        public static Tetrahedron3d operator -(Tetrahedron3d tri, REAL s)
        {
            return new Tetrahedron3d(tri.A - s, tri.B - s, tri.C - s, tri.D - s);
        }

        public static Tetrahedron3d operator -(Tetrahedron3d tri, POINT3 v)
        {
            return new Tetrahedron3d(tri.A - v, tri.B - v, tri.C - v, tri.D - v);
        }

        public static Tetrahedron3d operator *(Tetrahedron3d tri, REAL s)
        {
            return new Tetrahedron3d(tri.A * s, tri.B * s, tri.C * s, tri.D * s);
        }

        public static Tetrahedron3d operator /(Tetrahedron3d tri, REAL s)
        {
            return new Tetrahedron3d(tri.A / s, tri.B / s, tri.C / s, tri.D / s);
        }

        public static Tetrahedron3d operator *(MATRIX3 m, Tetrahedron3d tet)
        {
            return new Tetrahedron3d(m * tet.A, m * tet.B, m * tet.C, m * tet.D);
        }

        public static Tetrahedron3d operator *(MATRIX4 m, Tetrahedron3d tet)
        {
            return new Tetrahedron3d(m * tet.A, m * tet.B, m * tet.C, m * tet.D);
        }

        public static implicit operator Tetrahedron3d(Tetrahedron3f tet)
        {
            return new Tetrahedron3d(tet.A, tet.B, tet.C, tet.D);
        }

        public static bool operator ==(Tetrahedron3d t1, Tetrahedron3d t2)
        {
            return t1.A == t2.A && t1.B == t2.B && t1.C == t2.C && t1.D == t2.D;
        }

        public static bool operator !=(Tetrahedron3d t1, Tetrahedron3d t2)
        {
            return t1.A != t2.A || t1.B != t2.B || t1.C != t2.C || t1.D != t2.D;
        }

        /// <summary>
        /// Is the tetrahedron equal to this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Is the tetrahedron equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Tetrahedron3d)) return false;
            Tetrahedron3d tri = (Tetrahedron3d)obj;
            return this == tri;
        }

        /// <summary>
        /// Is the tetrahedron equal to the other tetrahedron.
        /// </summary>
        /// <param name="tri">The other tetrahedron.</param>
        /// <returns>Is the tetrahedron equal to the other tetrahedron.</returns>
        public bool Equals(Tetrahedron3d tri)
        {
            return this == tri;
        }

        /// <summary>
        /// The tetrahedrons hash code.
        /// </summary>
        /// <returns>The tetrahedrons hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ A.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ B.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ C.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ D.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// The tetrahedron as a string.
        /// </summary>
        /// <returns>The tetrahedron as a string.</returns>
        public override string ToString()
        {
            return string.Format("[Tetrahedron3d: A={0}, B={1}, C={2}, C={D}]", A, B, C, D);
        }

        /// <summary>
        /// Round the tetrahedrons points.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public void Round(int digits)
        {
            A = A.Rounded(digits);
            B = B.Rounded(digits);
            C = C.Rounded(digits);
            D = D.Rounded(digits);
        }

    }
}