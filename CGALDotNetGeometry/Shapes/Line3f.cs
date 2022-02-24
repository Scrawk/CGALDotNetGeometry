using System;
using System.Runtime.InteropServices;

using CGALDotNetGeometry.Numerics;

using REAL = System.Single;
using POINT3 = CGALDotNetGeometry.Numerics.Point3f;
using VECTOR3 = CGALDotNetGeometry.Numerics.Vector3f;
using MATRIX3 = CGALDotNetGeometry.Numerics.Matrix3x3f;
using MATRIX4 = CGALDotNetGeometry.Numerics.Matrix4x4f;

namespace CGALDotNetGeometry.Shapes
{
    /// <summary>
    /// A 3f Line struct represented by a position and a direction.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Line3f : IEquatable<Line3f>
    {
        /// <summary>
        /// The Lines position.
        /// </summary>
        public POINT3 Position;

        /// <summary>
        /// The Lines direction.
        /// Might not be normalized.
        /// </summary>
        public VECTOR3 Direction;

        /// <summary>
        /// Construct a Line from a point and the direction.
        /// </summary>
        /// <param name="position">The Lines position.</param>
        /// <param name="direction">The Lines direction (will be normalized)</param>
        public Line3f(POINT3 position, VECTOR3 direction)
        {
            Position = position;
            Direction = direction.Normalized;
        }

        public static Line3f operator *(MATRIX3 m, Line3f Line)
        {
            return new Line3f(m * Line.Position, m * Line.Direction);
        }

        public static Line3f operator *(MATRIX4 m, Line3f Line)
        {
            return new Line3f(m * Line.Position, m * Line.Direction);
        }

        public static explicit operator Line3d(Line3f Line)
        {
            return new Line3d(Line.Position, Line.Direction);
        }

        /// <summary>
        /// Check if the two Lines are equal.
        /// </summary>
        /// <param name="l1">The first Line.</param>
        /// <param name="l2">The second Line.</param>
        /// <returns>True if the two Lines are equal.</returns>
        public static bool operator ==(Line3f l1, Line3f l2)
        {
            return l1.Position == l2.Position && l1.Direction == l2.Direction;
        }

        /// <summary>
        /// Check if the two Lines are not equal.
        /// </summary>
        /// <param name="l1">The first Line.</param>
        /// <param name="l2">The second Line.</param>
        /// <returns>True if the two Lines are not equal.</returns>
        public static bool operator !=(Line3f l1, Line3f l2)
        {
            return l1.Position != l2.Position || l1.Direction != l2.Direction;
        }

        /// <summary>
        /// Is the Line equal to this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Is the Line equal to this object.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Line3f)) return false;
            Line3f Line = (Line3f)obj;
            return this == Line;
        }

        /// <summary>
        /// Is the Line equal to the other Line.
        /// </summary>
        /// <param name="Line">The over Line.</param>
        /// <returns>Is the Line equal to the other Line.</returns>
        public bool Equals(Line3f Line)
        {
            return this == Line;
        }

        /// <summary>
        /// The Lines hashcode.
        /// </summary>
        /// <returns>The Lines hashcode.</returns>
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

        /// <summary>
        /// The Lines as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Line3f: Position={0}, Direction={1}]", Position, Direction);
        }

        /// <summary>
        /// Normalize the lines direction.
        /// </summary>
        public void Normalize()
        {
            Direction.Normalize();
        }

        /// <summary>
        /// Round the Lines position and direction.
        /// </summary>
        /// <param name="digits">number of digits to round to.</param>
        public void Round(int digits)
        {
            Position = Position.Rounded(digits);
            Direction = Direction.Rounded(digits);
        }
    }
}

