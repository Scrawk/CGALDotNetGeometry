﻿using System;
using System.Runtime.InteropServices;

using CGALDotNetGeometry.Numerics;

using REAL = System.Double;
using VECTOR2 = CGALDotNetGeometry.Numerics.Vector2d;
using POINT2 = CGALDotNetGeometry.Numerics.Point2d;
using CIRCLE2 = CGALDotNetGeometry.Shapes.Circle2d;
using SEGMENT2 = CGALDotNetGeometry.Shapes.Segment2d;
using BOX2 = CGALDotNetGeometry.Shapes.Box2d;
using MATRIX2 = CGALDotNetGeometry.Numerics.Matrix2x2d;
using MATRIX3 = CGALDotNetGeometry.Numerics.Matrix3x3d;
using MATRIX4 = CGALDotNetGeometry.Numerics.Matrix4x4d;

namespace CGALDotNetGeometry.Shapes
{
    /// <summary>
    /// A 2D ray.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Ray2d : IEquatable<Ray2d>
    {
        /// <summary>
        /// The rays position.
        /// </summary>
        public POINT2 Position;

        /// <summary>
        /// The rays direction.
        /// </summary>
        public VECTOR2 Direction;

        /// <summary>
        /// Construct a new ray.
        /// </summary>
        /// <param name="position">The rays position.</param>
        /// <param name="direction">The rays directio</param>
        public Ray2d(POINT2 position, VECTOR2 direction)
        {
            Position = position;
            Direction = direction;
        }

        /// <summary>
        /// Does the shape contain no non finite points.
        /// </summary>
        public bool IsFinite
        {
            get
            {
                if (!Direction.IsFinite) return false;
                if (!Position.IsFinite) return false;
                return true;
            }
        }

        /// <summary>
        /// Is the ray degenerate.
        /// </summary>
        public bool IsDegenerate
        {
            get
            {
                if (Direction == VECTOR2.Zero) return true;
                if (!IsFinite) return true;
                return false;
            }
        }

        public static bool operator ==(Ray2d r1, Ray2d r2)
        {
            return r1.Position == r2.Position && r1.Direction == r2.Direction;
        }

        public static bool operator !=(Ray2d r1, Ray2d r2)
        {
            return r1.Position != r2.Position || r1.Direction != r2.Direction;
        }

        /// <summary>
        /// Is the ray equal to the other object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>Is the ray equal to the other object.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Ray2d)) return false;
            Ray2d ray = (Ray2d)obj;
            return this == ray;
        }

        /// <summary>
        /// Is the ray equal to the other ray.
        /// </summary>
        /// <param name="ray">The other ray.</param>
        /// <returns>Is the ray equal to the other ray.</returns>
        public bool Equals(Ray2d ray)
        {
            return this == ray;
        }

        /// <summary>
        /// The rays hashcode.
        /// </summary>
        /// <returns>The rays hashcode.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                hash = (hash * MathUtil.HASH_PRIME_2) ^ Position.GetHashCode();
                hash = (hash * MathUtil.HASH_PRIME_2) ^ Direction.GetHashCode();
                return hash;
            }
        }

        public static Ray2d operator *(MATRIX2 m, Ray2d ray)
        {
            return new Ray2d(m * ray.Position, m * ray.Direction);
        }

        public static Ray2d operator *(MATRIX3 m, Ray2d ray)
        {
            return new Ray2d(m * ray.Position, m * ray.Direction);
        }

        public static Ray2d operator *(MATRIX4 m, Ray2d ray)
        {
            return new Ray2d(m * ray.Position, m * ray.Direction);
        }

        public static implicit operator Ray2d(Ray2f ray)
        {
            return new Ray2d(ray.Position, ray.Direction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Ray2d: Position={0}, Direction={1}]", Position, Direction);
        }

        /// <summary>
        /// Normalize the lines direction.
        /// </summary>
        public void Normalize()
        {
            Direction.Normalize();
        }

        /// <summary>
        /// Round the rays components.
        /// </summary>
        /// <param name="digits">The number of digits to round to.</param>
        public void Round(int digits)
        {
            Position.Round(digits);
            Direction.Round(digits);
        }

        /// <summary>
        /// Get the position offset along the ray at t.
        /// </summary>
        /// <param name="t">The amount to offset.</param>
        /// <returns>The position at t.</returns>
        public POINT2 GetPosition(REAL t)
        {
            return Position + t * Direction;
        }

        /// <summary>
        /// Intersection point between two rays.
        /// Presumes there is a intersection.
        /// </summary>
        /// <param name="ray">The other ray.</param>
        /// <returns></returns>
        public POINT2 Intersection(Ray2d ray)
        {
            REAL s, t;
            Intersects(ray, out s, out t);
            return GetPosition(s);
        }

        /// <summary>
        /// Intersection between two rays.
        /// </summary>
        /// <param name="ray">The other ray</param>
        /// <param name="s">Intersection point = Position + s * Direction.</param>
        /// <param name="t">Intersection point = ray.Position + t * ray.Direction.</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(Ray2d ray, out REAL s, out REAL t)
        {
            s = 0;
            t = 0;
            REAL dx = ray.Position.x - Position.x;
            REAL dy = ray.Position.y - Position.y;

            REAL det = ray.Direction.x * Direction.y - ray.Direction.y * Direction.x;
            if (MathUtil.IsZero(det)) return false;

            s = (dy * ray.Direction.x - dx * ray.Direction.y) / det;
            t = (dy * Direction.x - dx * Direction.y) / det;

            return s > 0 && t > 0;
        }

        /// <summary>
        /// Intersection between ray and segment.
        /// </summary>
        /// <param name="seg">the segment</param>
        /// <param name="s">Intersection point = Position + s * Direction</param>
        /// <param name="t">Intersection point = A + t * (B - A)</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(SEGMENT2 seg, out REAL s, out REAL t)
        {
            s = t = 0;

            REAL dx = seg.A.x - Position.x;
            REAL dy = seg.A.y - Position.y;

            REAL len = POINT2.Distance(seg.A, seg.B);
            if (MathUtil.IsZero(len)) return false;

            VECTOR2 n1;
            n1.x = (seg.B.x - seg.A.x) / len;
            n1.y = (seg.B.y - seg.A.y) / len;

            REAL det = n1.x * Direction.y - n1.y * Direction.x;
            if (MathUtil.IsZero(det)) return false;

            s = (dy * n1.x - dx * n1.y) / det;
            t = (dy * Direction.x - dx * Direction.y) / det;
            t /= len;

            return s > 0 && t > 0 && t < 1.0;
        }

        /// <summary>
        /// Intersection between ray and circle.
        /// </summary>
        /// <param name="circle">the circle</param>
        /// <param name="t">Intersection point = Position + t * Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(CIRCLE2 circle, out REAL t)
        {
            t = 0;
            VECTOR2 m = Position - circle.Center;
            REAL b = VECTOR2.Dot(m, Direction);
            REAL c = VECTOR2.Dot(m, m) - circle.Radius2;

            if (c > 0.0 && b > 0.0) return false;

            REAL discr = b * b - c;
            if (discr < 0.0) return false;

            t = -b - MathUtil.Sqrt(discr);

            if (t < 0.0) t = 0;
            return true;
        }

        /// <summary>
        /// Intersection between ray and box.
        /// </summary>
        /// <param name="box">the box</param>
        /// <param name="t">Intersection point = Position + t * Direction</param>
        /// <returns>If rays intersect</returns>
        public bool Intersects(BOX2 box, out REAL t)
        {
            t = 0;
            REAL tmin = 0;
            REAL tmax = REAL.PositiveInfinity;

            for (int i = 0; i < 2; i++)
            {
                if (MathUtil.IsZero(Direction[i]))
                {
                    if (Position[i] < box.Min[i] || Position[i] > box.Max[i])
                        return false;
                }
                else
                {
                    REAL ood = 1.0f / Direction[i];
                    REAL t1 = (box.Min[i] - Position[i]) * ood;
                    REAL t2 = (box.Max[i] - Position[i]) * ood;

                    if (t1 > t2)
                    {
                        REAL tmp = t1;
                        t1 = t2;
                        t2 = tmp;
                    }

                    tmin = Math.Max(tmin, t1);
                    tmax = Math.Min(tmax, t2);

                    if (tmin > tmax) return false;
                }
            }

            t = tmin;
            return true;
        }

    }
}

