using System;
using System.Runtime.InteropServices;

using CGALDotNetGeometry.Numerics;

using REAL = System.Double;
using POINT3 = CGALDotNetGeometry.Numerics.Point3d;
using VECTOR3 = CGALDotNetGeometry.Numerics.Vector3d;
using BOX3 = CGALDotNetGeometry.Shapes.Box3d;
using MATRIX3 = CGALDotNetGeometry.Numerics.Matrix3x3d;
using MATRIX4 = CGALDotNetGeometry.Numerics.Matrix4x4d;

namespace CGALDotNetGeometry.Shapes
{
    /// <summary>
    /// A 3D segment.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment3d : IEquatable<Segment3d>, IShape3d
    {
        /// <summary>
        /// The segments first (aka source) point.
        /// </summary>
        public POINT3 A;

        /// <summary>
        /// The segments second (aka target) point.
        /// </summary>
        public POINT3 B;

        /// <summary>
        /// Create a new segment.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        public Segment3d(POINT3 a, POINT3 b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Create a new segment.
        /// </summary>
        /// <param name="ax">The first points x value.</param>
        /// <param name="ay">The first points y value.</param>
        /// <param name="az">The first points z value.</param>
        /// <param name="bx">The second points x value.</param>
        /// <param name="by">The second points y value.</param>
        /// <param name="bz">The second points z value.</param>
        public Segment3d(REAL ax, REAL ay, REAL az, REAL bx, REAL by, REAL bz)
        {
            A = new POINT3(ax, ay, az);
            B = new POINT3(bx, by, bz);
        }

        /// <summary>
        /// The length of the segment.
        /// </summary>
        public REAL Length => POINT3.Distance(A, B);

        /// <summary>
        /// The square length of the segment.
        /// </summary>
        public REAL SqrLength => POINT3.SqrDistance(A, B);

        /// <summary>
        /// The segment flipped, a is now b, b is now a.
        /// </summary>
        public Segment3d Reversed => new Segment3d(B, A);

        /// <summary>
        /// The bounding box of the segment.
        /// </summary>
        public BOX3 Bounds
        {
            get
            {
                var xmin = MathUtil.Min(A.x, B.x);
                var xmax = MathUtil.Max(A.x, B.x);
                var ymin = MathUtil.Min(A.y, B.y);
                var ymax = MathUtil.Max(A.y, B.y);
                var zmin = MathUtil.Min(A.z, B.z);
                var zmax = MathUtil.Max(A.z, B.z);

                return new BOX3(new POINT3(xmin, ymin, zmin), new POINT3(xmax, ymax, zmin));
            }
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
                return true;
            }
        }

        /// <summary>
        /// Array acess to the segments points.
        /// </summary>
        /// <param name="i">The index of the point to access (0-2)</param>
        /// <returns>The point at index i.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        unsafe public POINT3 this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Segment3d index out of range.");

                fixed (Segment3d* array = &this) { return ((POINT3*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Segment3d index out of range.");

                fixed (POINT3* array = &A) { array[i] = value; }
            }
        }

        public static Segment3d operator +(Segment3d seg, REAL s)
        {
            return new Segment3d(seg.A + s, seg.B + s);
        }

        public static Segment3d operator +(Segment3d seg, POINT3 v)
        {
            return new Segment3d(seg.A + v, seg.B + v);
        }

        public static Segment3d operator -(Segment3d seg, REAL s)
        {
            return new Segment3d(seg.A - s, seg.B - s);
        }

        public static Segment3d operator -(Segment3d seg, POINT3 v)
        {
            return new Segment3d(seg.A - v, seg.B - v);
        }

        public static Segment3d operator *(Segment3d seg, REAL s)
        {
            return new Segment3d(seg.A * s, seg.B * s);
        }

        public static Segment3d operator /(Segment3d seg, REAL s)
        {
            return new Segment3d(seg.A / s, seg.B / s);
        }

        public static Segment3d operator *(MATRIX3 m, Segment3d seg)
        {
            return new Segment3d(m * seg.A, m * seg.B);
        }

        public static Segment3d operator *(MATRIX4 m, Segment3d seg)
        {
            return new Segment3d(m * seg.A, m * seg.B);
        }

        public static implicit operator Segment3d(Segment3f seg)
        {
            return new Segment3d(seg.A, seg.B);
        }

        public static bool operator ==(Segment3d s1, Segment3d s2)
        {
            return s1.A == s2.A && s1.B == s2.B;
        }

        public static bool operator !=(Segment3d s1, Segment3d s2)
        {
            return s1.A != s2.A || s1.B != s2.B;
        }

        /// <summary>
        /// Is the segment equal to this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Is the segment equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Segment3d)) return false;
            Segment3d seg = (Segment3d)obj;
            return this == seg;
        }

        /// <summary>
        /// Is the segment equal to the other segment.
        /// </summary>
        /// <param name="seg">The other segment.</param>
        /// <returns>Is the segment equal to the other segment.</returns>
        public bool Equals(Segment3d seg)
        {
            return this == seg;
        }

        /// <summary>
        /// The segments hash code.
        /// </summary>
        /// <returns>The segments hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ A.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ B.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// The segment as a string.
        /// </summary>
        /// <returns>The segment as a string.</returns>
        public override string ToString()
        {
            return string.Format("[Segment3d: A={0}, B={1}]", A, B);
        }

        /// <summary>
        /// Round the segments points.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public void Round(int digits)
        {
            A = A.Rounded(digits);
            B = B.Rounded(digits);
        }

        /// <summary>
        /// Does the point line on the segemnts.
        /// </summary>
        /// <param name="p">The point</param>
        /// <param name="eps">A small value to give the segment some width.</param>
        /// <returns>Does the point line on the segemnts.</returns>
        public bool Contains(POINT3 p, REAL eps)
        {
            var c = Closest(p);
            return POINT3.AlmostEqual(c, p, eps);
        }

        /// <summary>
        /// Does the point line on the segemnts.
        /// </summary>
        /// <param name="p">The point</param>
        /// <param name="includeBorder">NA here. Needed for IShape interface.</param>
        /// <returns>Does the point line on the segemnts.</returns>
        public bool Contains(POINT3 p, bool includeBorder)
        {
            var c = Closest(p);
            return POINT3.AlmostEqual(c, p, MathUtil.EPS_64);
        }

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="includeBorder"></param>
        /// <returns>Does the shape intersect the box.</returns>
        public bool Intersects(BOX3 box, bool includeBorder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the signed distance to the point. 
        /// Always positive.
        /// </summary>
        public REAL SignedDistance(POINT3 point)
        {
            return POINT3.Distance(Closest(point), point);
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="point">point</param>
        public POINT3 Closest(POINT3 point)
        {
            REAL t;
            Closest(point, out t);
            return A + (B - A) * t;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="point">point</param>
        /// <param name="t">closest point = A + t * (B - A)</param>
        public void Closest(POINT3 point, out REAL t)
        {
            t = 0.0f;
            POINT3 ab = B - A;
            POINT3 ap = point - A;

            REAL len = ab.x * ab.x + ab.y * ab.y;
            if (MathUtil.IsZero(len)) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;
            t = MathUtil.Clamp01(t);
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        public Segment3d Closest(Segment3d seg)
        {
            REAL s, t;
            Closest(seg, out s, out t);
            return new Segment3d(A + (B - A) * s, seg.A + (seg.B - seg.A) * t);
        }

        /// <summary>
        /// The closest segment spanning two other segments.
        /// </summary>
        /// <param name="seg">the other segment</param>
        /// <param name="s">closest point = A + s * (B - A)</param>
        /// <param name="t">other closest point = seg.A + t * (seg.B - seg.A)</param>
        public void Closest(Segment3d seg, out REAL s, out REAL t)
        {
            VECTOR3 ab0 = POINT3.Direction(A, B, false);
            VECTOR3 ab1 = POINT3.Direction(seg.A, seg.B, false);
            VECTOR3 a01 = POINT3.Direction(seg.A, A, false);

            REAL d00 = VECTOR3.Dot(ab0, ab0);
            REAL d11 = VECTOR3.Dot(ab1, ab1);
            REAL d1 = VECTOR3.Dot(ab1, a01);

            s = 0;
            t = 0;

            //Check if either or both segments degenerate into points.
            if (MathUtil.IsZero(d00) && MathUtil.IsZero(d11))
                return;

            if (MathUtil.IsZero(d00))
            {
                //First segment degenerates into a point.
                s = 0;
                t = MathUtil.Clamp01(d1 / d11);
            }
            else
            {
                REAL c = VECTOR3.Dot(ab0, a01);

                if (MathUtil.IsZero(d11))
                {
                    //Second segment degenerates into a point.
                    s = MathUtil.Clamp01(-c / d00);
                    t = 0;
                }
                else
                {
                    //The generate non degenerate case starts here.
                    REAL d2 = VECTOR3.Dot(ab0, ab1);
                    REAL denom = d00 * d11 - d2 * d2;

                    //if segments not parallel compute closest point and clamp to segment.
                    if (!MathUtil.IsZero(denom))
                        s = MathUtil.Clamp01((d2 * d1 - c * d11) / denom);
                    else
                        s = 0;

                    t = (d2 * s + d1) / d11;

                    if (t < 0.0f)
                    {
                        t = 0.0f;
                        s = MathUtil.Clamp01(-c / d00);
                    }
                    else if (t > 1.0f)
                    {
                        t = 1.0f;
                        s = MathUtil.Clamp01((d2 - c) / d00);
                    }
                }
            }
        }

    }
}

