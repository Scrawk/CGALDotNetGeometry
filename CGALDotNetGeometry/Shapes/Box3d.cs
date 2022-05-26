﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using CGALDotNetGeometry.Numerics;

using REAL = System.Double;
using POINT3 = CGALDotNetGeometry.Numerics.Point3d;
using POINT4 = CGALDotNetGeometry.Numerics.Point4d;
using MATRIX3 = CGALDotNetGeometry.Numerics.Matrix3x3d;
using MATRIX4 = CGALDotNetGeometry.Numerics.Matrix4x4d;

namespace CGALDotNetGeometry.Shapes
{
    /// <summary>
    /// A 3D box represented by its min and max values.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box3d : IEquatable<Box3d>, IShape3d
    {

        /// <summary>
        /// The boxes min point.
        /// </summary>
        public POINT3 Min;

        /// <summary>
        /// The boxes max point.
        /// </summary>
        public POINT3 Max;

        /// <summary>
        /// Construct a new box.
        /// </summary>
        /// <param name="min">The boxes min point.</param>
        /// <param name="max">The boxes max point.</param>
        public Box3d(REAL min, REAL max)
        {
            Min = new POINT3(min);
            Max = new POINT3(max);
        }

        public Box3d(REAL minX, REAL minY, REAL minZ, REAL maxX, REAL maxY, REAL maxZ)
        {
            Min = new POINT3(minX, minY, minZ);
            Max = new POINT3(maxX, maxY, maxZ);
        }

        /// <summary>
        /// Construct a new box.
        /// </summary>
        /// <param name="min">The boxes min point.</param>
        /// <param name="max">The boxes max point.</param>
        public Box3d(POINT3 min, POINT3 max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// The boxes bounding box is just itself.
        /// Needed for the IShape interface.
        /// </summary>
        public Box3d Bounds => this;

        /// <summary>
        /// Does the shape contain no non finite points.
        /// </summary>
        public bool IsFinite
        {
            get
            {
                if (!Min.IsFinite) return false;
                if (!Max.IsFinite) return false;
                return true;
            }
        }

        /// <summary>
        /// The center of the box.
        /// </summary>
        public POINT3 Center
        {
            get { return (Min + Max) * 0.5; }
        }

        /// <summary>
        /// The size of the boxes sides.
        /// </summary>
        public POINT3 Size
        {
            get { return new POINT3(Width, Height, Depth); }
        }

        /// <summary>
        /// The size of the box on the x axis.
        /// </summary>
        public REAL Width
        {
            get { return Max.x - Min.x; }
        }

        /// <summary>
        /// The size of the box on the y axis.
        /// </summary>
        public REAL Height
        {
            get { return Max.y - Min.y; }
        }

        /// <summary>
        /// The size of the box on the z axis.
        /// </summary>
        public REAL Depth
        {
            get { return Max.z - Min.z; }
        }

        /// <summary>
        /// The volume of the box.
        /// </summary>
        public REAL Volume
        {
            get { return (Max.x - Min.x) * (Max.y - Min.y) * (Max.z - Min.z); }
        }

        /// <summary>
        /// THe boxes surface area.
        /// </summary>
        public REAL SurfaceArea
        {
            get
            {
                POINT3 d = Max - Min;
                return 2.0 * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }

        public static Box3d operator +(Box3d box, REAL s)
        {
            return new Box3d(box.Min + s, box.Max + s);
        }

        public static Box3d operator +(Box3d box, POINT3 v)
        {
            return new Box3d(box.Min + v, box.Max + v);
        }

        public static Box3d operator -(Box3d box, REAL s)
        {
            return new Box3d(box.Min - s, box.Max - s);
        }

        public static Box3d operator -(Box3d box, POINT3 v)
        {
            return new Box3d(box.Min - v, box.Max - v);
        }

        public static Box3d operator *(Box3d box, REAL s)
        {
            return new Box3d(box.Min * s, box.Max * s);
        }

        public static Box3d operator /(Box3d box, REAL s)
        {
            return new Box3d(box.Min / s, box.Max / s);
        }

        public static Box3d operator *(Box3d box, MATRIX3 m)
        {
            return new Box3d(m * box.Min, m * box.Max);
        }

        public static Box3d operator *(Box3d box, MATRIX4 m)
        {
            return new Box3d(m * box.Min, m * box.Max);
        }

        public static implicit operator Box3d(Box3f box)
        {
            return new Box3d(box.Min, box.Max);
        }

        public static implicit operator Box3d(Box3i box)
        {
            return new Box3d(box.Min, box.Max);
        }

        public static bool operator ==(Box3d b1, Box3d b2)
        {
            return b1.Min == b2.Min && b1.Max == b2.Max;
        }

        public static bool operator !=(Box3d b1, Box3d b2)
        {
            return b1.Min != b2.Min || b1.Max != b2.Max;
        }

        /// <summary>
        /// Is the box equal to this obj.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Is the box equal to this obj.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Box3d)) return false;
            Box3d box = (Box3d)obj;
            return this == box;
        }

        /// <summary>
        /// Is the box equal to the other box.
        /// </summary>
        /// <param name="box">The other box.</param>
        /// <returns>Is the box equal to the other box.</returns>
        public bool Equals(Box3d box)
        {
            return this == box;
        }

        /// <summary>
        /// The boxes hash code.
        /// </summary>
        /// <returns>The boxes hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ Min.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ Max.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Box3d: Min={0}, Max={1}, Width={2}, Height={3}, Depth={4}]", Min, Max, Width, Height, Depth);
        }

        /// <summary>
        /// Get the boxes corner points as a array.
        /// </summary>
        /// <returns>The boxes corner points as a array</returns>
        public POINT3[] GetCorners()
        {
            var corners = new POINT3[8];
            corners[0] = new POINT3(Min.x, Min.y, Min.z);
            corners[1] = new POINT3(Max.x, Min.y, Min.z);
            corners[2] = new POINT3(Max.x, Min.y, Max.z);
            corners[3] = new POINT3(Min.x, Min.y, Max.z);

            corners[4] = new POINT3(Min.x, Max.y, Min.z);
            corners[5] = new POINT3(Max.x, Max.y, Min.z);
            corners[6] = new POINT3(Max.x, Max.y, Max.z);
            corners[7] = new POINT3(Min.x, Max.y, Max.z);
            return corners;
        }

        /// <summary>
        /// Copy the boxes corner points in the array.
        /// </summary>
        /// <param name="corners">A array that has a size of at least 8.</param>
        public void GetCorners(IList<POINT3> corners)
        {
            corners[0] = new POINT3(Min.x, Min.y, Min.z);
            corners[1] = new POINT3(Max.x, Min.y, Min.z);
            corners[2] = new POINT3(Max.x, Min.y, Max.z);
            corners[3] = new POINT3(Min.x, Min.y, Max.z);

            corners[4] = new POINT3(Min.x, Max.y, Min.z);
            corners[5] = new POINT3(Max.x, Max.y, Min.z);
            corners[6] = new POINT3(Max.x, Max.y, Max.z);
            corners[7] = new POINT3(Min.x, Max.y, Max.z);
        }

        /// <summary>
        /// Copy the boxes corner points in the array.
        /// </summary>
        /// <param name="corners">A array that has a size of at least 8.</param>
        public void GetCorners(IList<POINT4> corners)
        {
            corners[0] = new POINT4(Min.x, Min.y, Min.z, 1);
            corners[1] = new POINT4(Max.x, Min.y, Min.z, 1);
            corners[2] = new POINT4(Max.x, Min.y, Max.z, 1);
            corners[3] = new POINT4(Min.x, Min.y, Max.z, 1);

            corners[4] = new POINT4(Min.x, Max.y, Min.z, 1);
            corners[5] = new POINT4(Max.x, Max.y, Min.z, 1);
            corners[6] = new POINT4(Max.x, Max.y, Max.z, 1);
            corners[7] = new POINT4(Min.x, Max.y, Max.z, 1);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public static Box3d Enlarge(Box3d box, POINT3 p)
        {
            var b = new Box3d();
            b.Min.x = Math.Min(box.Min.x, p.x);
            b.Min.y = Math.Min(box.Min.y, p.y);
            b.Min.z = Math.Min(box.Min.z, p.z);
            b.Max.x = Math.Max(box.Max.x, p.x);
            b.Max.y = Math.Max(box.Max.y, p.y);
            b.Max.z = Math.Max(box.Max.z, p.z);
            return b;
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public static Box3d Enlarge(Box3d box0, Box3d box1)
        {
            var b = new Box3d();
            b.Min.x = Math.Min(box0.Min.x, box1.Min.x);
            b.Min.y = Math.Min(box0.Min.y, box1.Min.y);
            b.Min.z = Math.Min(box0.Min.z, box1.Min.z);
            b.Max.x = Math.Max(box0.Max.x, box1.Max.x);
            b.Max.y = Math.Max(box0.Max.y, box1.Max.y);
            b.Max.z = Math.Max(box0.Max.z, box1.Max.z);
            return b;
        }

        /// <summary>
        /// Return a new box expanded by the amount.
        /// </summary>
        /// <param name="box">The box to expand.</param>
        /// <param name="amount">The amount to expand.</param>
        /// <returns>The expanded box.</returns>
        public static Box3d Expand(Box3d box, REAL amount)
        {
            return new Box3d(box.Min - amount, box.Max + amount);
        }

        /// <summary>
        /// Returns true if this box intersects the other box.
        /// </summary>
        /// <param name="box">The other box.</param>
        /// <param name="includeBorder">True if on border counts as inside.</param>
        /// <returns>Returns true if this box intersects the other box.</returns>
        public bool Intersects(Box3d box, bool includeBorder)
        {
            if (includeBorder)
            {
                if (Max.x < box.Min.x || Min.x > box.Max.x) return false;
                if (Max.y < box.Min.y || Min.y > box.Max.y) return false;
                if (Max.z < box.Min.z || Min.z > box.Max.z) return false;
                return true;
            }
            else
            {
                if (Max.x <= box.Min.x || Min.x >= box.Max.x) return false;
                if (Max.y <= box.Min.y || Min.y >= box.Max.y) return false;
                if (Max.z <= box.Min.z || Min.z >= box.Max.z) return false;
                return true;
            }
        }

        /// <summary>
        /// Does the box fully contain the other box.
        /// </summary>
        /// <param name="box">The other box.</param>
        /// <param name="includeBorder">True if on border counts as inside.</param>
        /// <returns>Does the box fully contain the other box.</returns>
        public bool Contains(Box3d box, bool includeBorder)
        {
            if (includeBorder)
            {
                if (box.Max.x > Max.x || box.Min.x < Min.x) return false;
                if (box.Max.y > Max.y || box.Min.y < Min.y) return false;
                if (box.Max.z > Max.z || box.Min.z < Min.z) return false;
                return true;
            }
            else
            {
                if (box.Max.x >= Max.x || box.Min.x <= Min.x) return false;
                if (box.Max.y >= Max.y || box.Min.y <= Min.y) return false;
                if (box.Max.z >= Max.z || box.Min.z <= Min.z) return false;
                return true;
            }
        }

        /// <summary>
        /// Does the box contain the point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="includeBorder">True if on border counts as inside.</param>
        /// <returns>True if the box contains the point.</returns>
        public bool Contains(POINT3 point, bool includeBorder)
        {
            if (includeBorder)
            {
                if (point.x > Max.x || point.x < Min.x) return false;
                if (point.y > Max.y || point.y < Min.y) return false;
                if (point.z > Max.z || point.z < Min.z) return false;
                return true;
            }
            else
            {
                if (point.x >= Max.x || point.x <= Min.x) return false;
                if (point.y >= Max.y || point.y <= Min.y) return false;
                if (point.z >= Max.z || point.z <= Min.z) return false;
                return true;
            }
        }

        /// <summary>
        /// Find the closest point to the box.
        /// If point inside box return point.
        /// </summary>
        public POINT3 Closest(POINT3 p)
        {
            POINT3 c;

            if (p.x < Min.x)
                c.x = Min.x;
            else if (p.x > Max.x)
                c.x = Max.x;
            else
                c.x = p.x;

            if (p.y < Min.y)
                c.y = Min.y;
            else if (p.y > Max.y)
                c.y = Max.y;
            else
                c.y = p.y;

            if (p.z < Min.z)
                c.z = Min.z;
            else if (p.z > Max.z)
                c.z = Max.z;
            else
                c.z = p.z;

            return c;
        }

        /// <summary>
        /// Return the signed distance to the point. 
        /// If point is outside box field is positive.
        /// If point is inside box field is negative.
        /// </summary>
        public REAL SignedDistance(POINT3 p)
        {
            POINT3 d = (p - Center).Absolute - Size * 0.5;
            POINT3 max = POINT3.Max(d, 0);
            return max.Magnitude + Math.Min(MathUtil.Max(d.x, d.y, d.z), 0);
        }

        /// <summary>
        /// Round the boxes components.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            Min.Round(digits);
            Max.Round(digits);
        }

        /// <summary>
        /// Caculate the bounding box of the points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The bounding box.</returns>
        public static Box3d CalculateBounds(IList<POINT3> points)
        {
            POINT3 min = POINT3.PositiveInfinity;
            POINT3 max = POINT3.NegativeInfinity;

            int count = points.Count;
            for (int i = 0; i < count; i++)
            {
                var v = points[i];
                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;
                if (v.z < min.z) min.z = v.z;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
                if (v.z > max.z) max.z = v.z;
            }

            return new Box3d(min, max);
        }

        /// <summary>
        /// Calculate the bounds of 2 points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point</param>
        /// <returns>The bounding box.</returns>
        public static Box3d CalculateBounds(POINT3 a, POINT3 b)
        {
            REAL xmin = Math.Min(a.x, b.x);
            REAL xmax = Math.Max(a.x, b.x);
            REAL ymin = Math.Min(a.y, b.y);
            REAL ymax = Math.Max(a.y, b.y);
            REAL zmin = Math.Min(a.z, b.z);
            REAL zmax = Math.Max(a.z, b.z);

            var min = new POINT3(xmin, ymin, zmin);
            var max = new POINT3(xmax, ymax, zmax);

            return new Box3d(min, max);
        }

        /// <summary>
        /// Caculate the bounding box of a set of segments.
        /// </summary>
        /// <param name="segments">The segments.</param>
        /// <returns>The bounding box</returns>
        public static Box3d CalculateBounds(IList<Segment3d> segments)
        {
            POINT3 min = POINT3.PositiveInfinity;
            POINT3 max = POINT3.NegativeInfinity;

            int count = segments.Count;
            for (int i = 0; i < count; i++)
            {
                var seg = segments[i];

                if (seg.A.x < min.x) min.x = seg.A.x;
                if (seg.A.y < min.y) min.y = seg.A.y;
                if (seg.A.z < min.z) min.z = seg.A.z;

                if (seg.B.x < min.x) min.x = seg.B.x;
                if (seg.B.y < min.y) min.y = seg.B.y;
                if (seg.B.z < min.z) min.z = seg.B.z;

                if (seg.A.x > max.x) max.x = seg.A.x;
                if (seg.A.y > max.y) max.y = seg.A.y;
                if (seg.A.z > max.z) max.z = seg.A.z;

                if (seg.B.x > max.x) max.x = seg.B.x;
                if (seg.B.y > max.y) max.y = seg.B.y;
                if (seg.B.z > max.z) max.z = seg.B.z;
                }

            return new Box3d(min, max);
        }
    }

}




















