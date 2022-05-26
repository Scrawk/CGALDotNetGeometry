using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using REAL = System.Double;
using VECTOR3 = CGALDotNetGeometry.Numerics.Vector3d;
using POINT3 = CGALDotNetGeometry.Numerics.Point3d;
using MATRIX3 = CGALDotNetGeometry.Numerics.Matrix3x3d;
using MATRIX4 = CGALDotNetGeometry.Numerics.Matrix4x4d;

namespace CGALDotNetGeometry.Numerics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion3d : IEquatable<Quaternion3d>
    {

        public REAL x, y, z, w;

        public readonly static Quaternion3d Identity = new Quaternion3d(0, 0, 0, 1);

        public readonly static Quaternion3d Zero = new Quaternion3d(0, 0, 0, 0);

        /// <summary>
        /// A Quaternion from varibles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion3d(REAL x, REAL y, REAL z, REAL w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
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
                    throw new IndexOutOfRangeException("Quaternion3fd index out of range.");

                fixed (Quaternion3d* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Quaternion3d index out of range.");

                fixed (REAL* array = &x) { array[i] = value; }
            }
        }

        /// <summary>
        /// Returns the conjugate of a quaternion value.
        /// </summary>
        public Quaternion3d Conjugate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Quaternion3d(-x, -y, -z, w);
            }
        }

        /// <summary>
        /// The inverse of the quaternion.
        /// </summary>
        public Quaternion3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL im = MathUtil.SafeInv(SqrMagnitude);
                return new Quaternion3d(im * -x, im * -y, im * -z, im * w);
            }
        }

        /// <summary>
        /// The length of the quaternion.
        /// </summary>
        REAL Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MathUtil.SafeSqrt(SqrMagnitude);
            }
        }

        /// <summary>
        /// The sqr length of the quaternion.
        /// </summary>
        REAL SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * x + y * y + z * z + w * w;
            }
        }

        /// <summary>
        /// The a normalized quaternion.
        /// </summary>
        public Quaternion3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                REAL inv = MathUtil.SafeInv(Magnitude);
                return new Quaternion3d(x * inv, y * inv, z * inv, w * inv);
            }
        }

        /// <summary>
        /// Subtract a quaternion and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator -(Quaternion3d q, REAL s)
        {
            return new Quaternion3d(q.x - s, q.y - s, q.z - s, q.w - s);
        }

        /// <summary>
        /// Negate a quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator -(Quaternion3d q)
        {
            return new Quaternion3d(-q.x, -q.y, -q.z, -q.w);
        }

        /// <summary>
        /// Subtract two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator -(Quaternion3d q1, Quaternion3d q2)
        {
            return new Quaternion3d(q1.x - q2.x, q1.y - q2.y, q1.z - q2.z, q1.w - q2.w);
        }

        /// <summary>
        /// Add a quaternion and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator +(Quaternion3d q, REAL s)
        {
            return new Quaternion3d(q.x + s, q.y + s, q.z + s, q.w + s);
        }

        /// <summary>
        /// Add two quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator +(Quaternion3d q1, Quaternion3d q2)
        {
            return new Quaternion3d(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
        }

        /// <summary>
        /// Multiply a quaternion and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator *(Quaternion3d q, REAL s)
        {
            return new Quaternion3d(q.x * s, q.y * s, q.z * s, q.w * s);
        }

        /// <summary>
        /// Divide a quaternion and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator /(Quaternion3d q, REAL s)
        {
            return new Quaternion3d(q.x / s, q.y / s, q.z / s, q.w / s);
        }

        /// <summary>
        /// Multiply two quternions together.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion3d operator *(Quaternion3d q1, Quaternion3d q2)
        {
            return new Quaternion3d(q2.w * q1.x + q2.x * q1.w + q2.y * q1.z - q2.z * q1.y,
                                    q2.w * q1.y - q2.x * q1.z + q2.y * q1.w + q2.z * q1.x,
                                    q2.w * q1.z + q2.x * q1.y - q2.y * q1.x + q2.z * q1.w,
                                    q2.w * q1.w - q2.x * q1.x - q2.y * q1.y - q2.z * q1.z);
        }

        /// <summary>
        /// Multiply a quaternion and a vector together.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator *(Quaternion3d q, VECTOR3 v)
        {
            VECTOR3 xyz = new VECTOR3(q.x, q.y, q.z);
            VECTOR3 t = 2 * VECTOR3.Cross(xyz, v);
            return v + q.w * t + VECTOR3.Cross(xyz, t);
        }

        /// <summary>
        /// Multiply a quaternion and a vector together.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VECTOR3 operator *(VECTOR3 v, Quaternion3d q)
        {
            VECTOR3 xyz = new VECTOR3(q.x, q.y, q.z);
            VECTOR3 t = 2 * VECTOR3.Cross(xyz, v);
            return v + q.w * t + VECTOR3.Cross(xyz, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Quaternion3d(Quaternion3f q)
        {
            return new Quaternion3d(q.x, q.y, q.z, q.w);
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Quaternion3d v1, Quaternion3d v2)
        {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);
        }

        /// <summary>
        /// Are these Quaternions not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Quaternion3d v1, Quaternion3d v2)
        {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z || v1.w != v2.w);
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quaternion3d q)
        {
            return this == q;
        }

        /// <summary>
        /// Are these Quaternions equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Quaternion3d)) return false;
            Quaternion3d v = (Quaternion3d)obj;
            return this == v;
        }

        /// <summary>
        /// Quaternions hash code. 
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
        /// Quaternion as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", x, y, z, w);
        }

        /// <summary>
        /// Quaternion as a string.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2},{3}", x.ToString(f), y.ToString(f), z.ToString(f), w.ToString(f));
        }

        /// <summary>
        /// Convert to a single precision 3 dimension matrix.
        /// </summary>
        public MATRIX3 ToMatrix3x3d()
        {
            REAL xx = x * x,
                    xy = x * y,
                    xz = x * z,
                    xw = x * w,
                    yy = y * y,
                    yz = y * z,
                    yw = y * w,
                    zz = z * z,
                    zw = z * w;

            return new MATRIX3
            (
                1.0 - 2.0 * (yy + zz), 2.0 * (xy - zw), 2.0 * (xz + yw),
                2.0 * (xy + zw), 1.0 - 2.0 * (xx + zz), 2.0 * (yz - xw),
                2.0 * (xz - yw), 2.0 * (yz + xw), 1.0 - 2.0 * (xx + yy)
            );
        }

        /// <summary>
        /// Convert to a single precision 4 dimension matrix.
        /// </summary>
        public MATRIX4 ToMatrix4x4d()
        {
            REAL xx = x * x,
                    xy = x * y,
                    xz = x * z,
                    xw = x * w,
                    yy = y * y,
                    yz = y * z,
                    yw = y * w,
                    zz = z * z,
                    zw = z * w;

            return new MATRIX4
            (
                1.0 - 2.0 * (yy + zz), 2.0 * (xy - zw), 2.0 * (xz + yw), 0.0,
                2.0 * (xy + zw), 1.0 - 2.0 * (xx + zz), 2.0 * (yz - xw), 0.0,
                2.0 * (xz - yw), 2.0 * (yz + xw), 1.0 - 2.0 * (xx + yy), 0.0,
                0.0, 0.0, 0.0, 1.0
            );
        }

        /// <summary>
        /// Extract the rotation from a matrix.
        /// </summary>
        /// <param name="m"></param>
        /// <returns>The rotation as a quaternion from the matrix.</returns>
        public static Quaternion3d FromMatrix(MATRIX4 m)
        {
            REAL trace = m[0, 0] + m[1, 1] + m[2, 2];
            var q = new Quaternion3d();

            if (trace > 0)
            {
                // Compute w from matrix trace, then xyz
                // 4w^2 = m[0][0] + m[1][1] + m[2][2] + m[3][3] (but m[3][3] == 1)
                REAL s = (REAL)MathUtil.Sqrt(trace + 1.0);

                q.w = s / 2.0f;
                s = 0.5f / s;
                q.x = (m[2, 1] - m[1, 2]) * s;
                q.y = (m[0, 2] - m[2, 0]) * s;
                q.z = (m[1, 0] - m[0, 1]) * s;

            }
            else
            {
                // Compute largest of x, y, or z, then remaining components
                int[] nxt = { 1, 2, 0 };

                int i = 0;
                if (m[1, 1] > m[0, 0]) i = 1;
                if (m[2, 2] > m[i, i]) i = 2;

                int j = nxt[i];
                int k = nxt[j];
                REAL s = (REAL)MathUtil.Sqrt((m[i, i] - (m[j, j] + m[k, k])) + 1.0);

                q[i] = s * 0.5f;
                if (s != 0) s = 0.5f / s;

                q.w = (m[k, j] - m[j, k]) * s;
                q[j] = (m[j, i] + m[i, j]) * s;
                q[k] = (m[k, i] + m[i, k]) * s;
            }

            return q;
        }

        /// <summary>
        /// The normalize the quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            REAL invLength = MathUtil.SafeInv(Magnitude);
            x *= invLength;
            y *= invLength;
            z *= invLength;
            w *= invLength;
        }

        /// <summary>
        /// The dot product of two quaternion..
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static REAL Dot(Quaternion3d q0, Quaternion3d q1)
        {
            return q0.x * q1.x + q0.y * q1.y + q0.z * q1.z + q0.w * q1.w;
        }

        /// <summary>
        /// Create a rotation out of a vector.
        /// Uses Unity euler axis (+x right, +y up, +z forward)
        /// </summary>
        public static Quaternion3d FromEuler(VECTOR3 euler)
        {
            REAL heading = MathUtil.ToRadians(euler.y);
            REAL attitude = MathUtil.ToRadians(euler.z);
            REAL bank = MathUtil.ToRadians(euler.x);

            REAL c1 = MathUtil.Cos(heading / 2);
            REAL s1 = MathUtil.Sin(heading / 2);
            REAL c2 = MathUtil.Cos(attitude / 2);
            REAL s2 = MathUtil.Sin(attitude / 2);
            REAL c3 = MathUtil.Cos(bank / 2);
            REAL s3 = MathUtil.Sin(bank / 2);
            REAL c1c2 = c1 * c2;
            REAL s1s2 = s1 * s2;

            Quaternion3d q;
            q.w = (c1c2 * c3 - s1s2 * s3);
            q.x = (c1c2 * s3 + s1s2 * c3);
            q.y = (s1 * c2 * c3 + c1 * s2 * s3);
            q.z = (c1 * s2 * c3 - s1 * c2 * s3);

            return q;
        }

        /// <summary>
        /// Returns a 4x4 matrix that rotates around the x-axis by a given number of degrees.
        /// </summary>
        /// <param name="radian">
        /// The clockwise rotation angle when looking along the x-axis towards the origin in degrees.
        /// </param>
        public static Quaternion3f RotateX(Radian radian)
        {
            REAL a = (REAL)radian.angle * 0.5f;
            REAL sina = MathUtil.Sin(a);
            REAL cosa = MathUtil.Cos(a);
            return new Quaternion3f(sina, 0.0f, 0.0f, cosa);
        }

        /// <summary>
        /// Returns a 4x4 matrix that rotates around the y-axis by a given number of degrees.
        /// </summary>
        /// <param name="radian">
        /// The clockwise rotation angle when looking along the y-axis towards the origin in degrees.
        /// </param>
        public static Quaternion3f RotateY(Radian radian)
        {
            REAL a = (REAL)radian.angle * 0.5f;
            REAL sina = MathUtil.Sin(a);
            REAL cosa = MathUtil.Cos(a);
            return new Quaternion3f(0.0f, sina, 0.0f, cosa);
        }

        /// <summary>
        /// Returns a 4x4 matrix that rotates around the z-axis by a given number of degrees.
        /// </summary>
        /// <param name="radian">
        /// The clockwise rotation angle when looking along the z-axis towards the origin in degrees.
        /// </param>
        public static Quaternion3f RotateZ(Radian radian)
        {
            REAL a = (REAL)radian.angle * 0.5f;
            REAL sina = MathUtil.Sin(a);
            REAL cosa = MathUtil.Cos(a);
            return new Quaternion3f(0.0f, 0.0f, sina, cosa);
        }

        /// <summary>
        /// A Quaternion from a vector axis and angle.
        /// The axis is the up direction and the angle is the rotation.
        /// </summary>
        /// <param name="axis">The up axis.</param>
        /// <param name="radian">The amount to rotate in radians.</param>
        /// <returns></returns>
        public static Quaternion3d Rotate(VECTOR3 axis, Radian radian)
        {
            VECTOR3 axisN = axis.Normalized;
            REAL a = (REAL)radian.angle * 0.5f;
            REAL sina = MathUtil.Sin(a);
            REAL cosa = MathUtil.Cos(a);

            var q = new Quaternion3d();
            q.x = axisN.x * sina;
            q.y = axisN.y * sina;
            q.z = axisN.z * sina;
            q.w = cosa;

            return q;
        }

        /// <summary>
        /// A quaternion with the rotation required to
        /// rotation from the from direction to the to direction.
        /// </summary>
        /// <param name="from">The vector to start from.</param>
        /// <param name="to">The vector to slerp to.</param>
        /// <returns></returns>
        public static Quaternion3d Slerp(VECTOR3 from, VECTOR3 to)
        {
            var q = new Quaternion3d();
            var f = from.Normalized;
            var t = to.Normalized;

            var dotProdPlus1 = 1.0f + VECTOR3.Dot(f, t);

            if (MathUtil.IsZero(dotProdPlus1))
            {
                q.w = 0;
                if (MathUtil.Abs(f.x) < 0.6f)
                {
                    REAL norm = MathUtil.Sqrt(1 - f.x * f.x);
                    q.x = 0;
                    q.y = f.z / norm;
                    q.z = -f.y / norm;
                }
                else if (MathUtil.Abs(f.y) < 0.6f)
                {
                    REAL norm = MathUtil.Sqrt(1 - f.y * f.y);
                    q.x = -f.z / norm;
                    q.y = 0;
                    q.z = f.x / norm;
                }
                else
                {
                    REAL norm = MathUtil.Sqrt(1 - f.z * f.z);
                    q.x = f.y / norm;
                    q.y = -f.x / norm;
                    q.z = 0;
                }
            }
            else
            {
                REAL s = MathUtil.Sqrt(0.5f * dotProdPlus1);
                VECTOR3 tmp = (VECTOR3.Cross(f, t)) / (2.0f * s);
                q.x = tmp.x;
                q.y = tmp.y;
                q.z = tmp.z;
                q.w = s;
            }

            return q;
        }

        /// <summary>
        /// Slerp the quaternion from the from rotation to the to rotation by t.
        /// </summary>
        public static Quaternion3d Slerp(Quaternion3d from, Quaternion3d to, REAL t)
        {
            if (t <= 0.0)
            {
                return from;
            }
            else if (t >= 1.0)
            {
                return to;
            }
            else
            {
                REAL cosom = from.x * to.x + from.y * to.y + from.z * to.z + from.w * to.w;
                REAL absCosom = MathUtil.Abs(cosom);

                REAL scale0;
                REAL scale1;

                if (!MathUtil.IsZero(1.0 - absCosom))
                {
                    REAL omega = MathUtil.SafeAcos(absCosom);
                    REAL sinom = 1.0 / MathUtil.Sin(omega);
                    scale0 = MathUtil.Sin((1.0 - t) * omega) * sinom;
                    scale1 = MathUtil.Sin(t * omega) * sinom;
                }
                else
                {
                    scale0 = 1 - t;
                    scale1 = t;
                }

                Quaternion3d res = new Quaternion3d(scale0 * from.x + scale1 * to.x,
                                                    scale0 * from.y + scale1 * to.y,
                                                    scale0 * from.z + scale1 * to.z,
                                                    scale0 * from.w + scale1 * to.w);

                return res.Normalized;
            }
        }

    }

}
























