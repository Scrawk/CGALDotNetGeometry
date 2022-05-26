using System;
using System.Collections;
using System.Runtime.InteropServices;

using REAL = System.Double;
using VECTOR2 = CGALDotNetGeometry.Numerics.Vector2d;
using VECTOR3 = CGALDotNetGeometry.Numerics.Vector3d;
using VECTOR4 = CGALDotNetGeometry.Numerics.Vector4d;
using POINT2 = CGALDotNetGeometry.Numerics.Point2d;
using POINT3 = CGALDotNetGeometry.Numerics.Point3d;
using POINT4 = CGALDotNetGeometry.Numerics.Point4d;
using QUATERNION = CGALDotNetGeometry.Numerics.Quaternion3d;

namespace CGALDotNetGeometry.Numerics
{
    /// <summary>
    /// Matrix is column major. Data is accessed as: row + (column*4). 
    /// Matrices can be indexed like 2D arrays but in an expression like mat[a, b], 
    /// a refers to the row index, while b refers to the column index 
    /// (note that this is the opposite way round to Cartesian coordinates).
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4d : IEquatable<Matrix4x4d>
    {

        /// <summary>
        /// The matrix
        /// </summary>
        public REAL m00, m10, m20, m30;
        public REAL m01, m11, m21, m31;
        public REAL m02, m12, m22, m32;
        public REAL m03, m13, m23, m33;

        /// <summary>
        /// The Matrix Idenity.
        /// </summary>
        static readonly public Matrix4x4d Identity = new Matrix4x4d(1, 0, 0, 0,
                                                                    0, 1, 0, 0,
                                                                    0, 0, 1, 0,
                                                                    0, 0, 0, 1);

        /// <summary>
        /// A matrix from the following varibles.
        /// </summary>
        public Matrix4x4d(REAL m00, REAL m01, REAL m02, REAL m03,
                          REAL m10, REAL m11, REAL m12, REAL m13,
                          REAL m20, REAL m21, REAL m22, REAL m23,
                          REAL m30, REAL m31, REAL m32, REAL m33)
        {
            this.m00 = m00; this.m01 = m01; this.m02 = m02; this.m03 = m03;
            this.m10 = m10; this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m20 = m20; this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m30 = m30; this.m31 = m31; this.m32 = m32; this.m33 = m33;
        }

        /// <summary>
        /// A matrix from the following column vectors.
        /// </summary>
        public Matrix4x4d(VECTOR4 c0, VECTOR4 c1, VECTOR4 c2, VECTOR4 c3)
        {
            m00 = c0.x; m01 = c1.x; m02 = c2.x; m03 = c3.x;
            m10 = c0.y; m11 = c1.y; m12 = c2.y; m13 = c3.y;
            m20 = c0.z; m21 = c1.z; m22 = c2.z; m23 = c3.z;
            m30 = c0.w; m31 = c1.w; m32 = c2.w; m33 = c3.w;
        }

        /// <summary>
        /// A matrix from the following varibles.
        /// </summary>
        public Matrix4x4d(REAL v)
        {
            m00 = v; m01 = v; m02 = v; m03 = v;
            m10 = v; m11 = v; m12 = v; m13 = v;
            m20 = v; m21 = v; m22 = v; m23 = v;
            m30 = v; m31 = v; m32 = v; m33 = v;
        }

        /// <summary>
        /// A matrix copied from a array of varibles.
        /// </summary>
        public Matrix4x4d(REAL[,] m)
        {
            m00 = m[0, 0]; m01 = m[0, 1]; m02 = m[0, 2]; m03 = m[0, 3];
            m10 = m[1, 0]; m11 = m[1, 1]; m12 = m[1, 2]; m13 = m[1, 3];
            m20 = m[2, 0]; m21 = m[2, 1]; m22 = m[2, 2]; m23 = m[2, 3];
            m30 = m[3, 0]; m31 = m[3, 1]; m32 = m[3, 2]; m33 = m[3, 3];
        }

        /// <summary>
        /// Access the varible at index i
        /// </summary>
        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 16)
                    throw new IndexOutOfRangeException("Matrix4x4d index out of range.");

                fixed (Matrix4x4d* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 16)
                    throw new IndexOutOfRangeException("Matrix4x4d index out of range.");

                fixed (REAL* array = &m00) { array[i] = value; }
            }
        }

        /// <summary>
        /// Access the varible at index ij
        /// </summary>
        public REAL this[int i, int j]
        {
            get => this[i + j * 4];
            set => this[i + j * 4] = value;
        }
        /// <summary>
        /// Is this the identity matrix.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (x == y)
                        {
                            if (!MathUtil.IsOne(this[x, y]))
                                return false;
                        }
                        else
                        {
                            if (!MathUtil.IsZero(this[x, y]))
                                return false;
                        }

                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Does the matric have scale.
        /// </summary>
        public bool HasScale
        {
            get
            {
                if (!MathUtil.IsOne((this * VECTOR3.UnitX).SqrMagnitude)) return true;
                if (!MathUtil.IsOne((this * VECTOR3.UnitY).SqrMagnitude)) return true;
                if (!MathUtil.IsOne((this * VECTOR3.UnitZ).SqrMagnitude)) return true;

                return false;
            }
        }

        /// <summary>
        /// The transpose of the matrix. The rows and columns are flipped.
        /// </summary>
        public Matrix4x4d Transpose
        {
            get
            {
                Matrix4x4d transpose = new Matrix4x4d();
                transpose.m00 = m00;
                transpose.m01 = m10;
                transpose.m02 = m20;
                transpose.m03 = m30;

                transpose.m10 = m01;
                transpose.m11 = m11;
                transpose.m12 = m21;
                transpose.m13 = m31;

                transpose.m20 = m02;
                transpose.m21 = m12;
                transpose.m22 = m22;
                transpose.m23 = m32;

                transpose.m30 = m03;
                transpose.m31 = m13;
                transpose.m32 = m23;
                transpose.m33 = m33;

                return transpose;
            }
        }

        /// <summary>
        /// The determinate of a matrix. 
        /// </summary>
        public REAL Determinant
        {
            get
            {
                return (m00 * Minor(1, 2, 3, 1, 2, 3) -
                        m01 * Minor(1, 2, 3, 0, 2, 3) +
                        m02 * Minor(1, 2, 3, 0, 1, 3) -
                        m03 * Minor(1, 2, 3, 0, 1, 2));
            }
        }

        /// <summary>
        /// The adjoint of a matrix. 
        /// </summary>
        public Matrix4x4d Adjoint
        {
            get
            {
                return new Matrix4x4d(
                        Minor(1, 2, 3, 1, 2, 3),
                        -Minor(0, 2, 3, 1, 2, 3),
                        Minor(0, 1, 3, 1, 2, 3),
                        -Minor(0, 1, 2, 1, 2, 3),

                        -Minor(1, 2, 3, 0, 2, 3),
                        Minor(0, 2, 3, 0, 2, 3),
                        -Minor(0, 1, 3, 0, 2, 3),
                        Minor(0, 1, 2, 0, 2, 3),

                        Minor(1, 2, 3, 0, 1, 3),
                        -Minor(0, 2, 3, 0, 1, 3),
                        Minor(0, 1, 3, 0, 1, 3),
                        -Minor(0, 1, 2, 0, 1, 3),

                        -Minor(1, 2, 3, 0, 1, 2),
                        Minor(0, 2, 3, 0, 1, 2),
                        -Minor(0, 1, 3, 0, 1, 2),
                        Minor(0, 1, 2, 0, 1, 2));
            }
        }

        /// <summary>
        /// The inverse of the matrix.
        /// A matrix multipled by its inverse is the idenity.
        /// </summary>
        public Matrix4x4d Inverse
        {
            get
            {
                return Adjoint * MathUtil.SafeInv(Determinant);
            }
        }

        public REAL Trace
        {
            get
            {
                return m00 + m11 + m22 + m33;
            }
        }

        /// <summary>
        /// Add two matrices.
        /// </summary>
        public static Matrix4x4d operator +(Matrix4x4d m1, Matrix4x4d m2)
        {
            Matrix4x4d kSum = new Matrix4x4d();
            kSum.m00 = m1.m00 + m2.m00;
            kSum.m01 = m1.m01 + m2.m01;
            kSum.m02 = m1.m02 + m2.m02;
            kSum.m03 = m1.m03 + m2.m03;

            kSum.m10 = m1.m10 + m2.m10;
            kSum.m11 = m1.m11 + m2.m11;
            kSum.m12 = m1.m12 + m2.m12;
            kSum.m13 = m1.m13 + m2.m13;

            kSum.m20 = m1.m20 + m2.m20;
            kSum.m21 = m1.m21 + m2.m21;
            kSum.m22 = m1.m22 + m2.m22;
            kSum.m23 = m1.m23 + m2.m23;

            kSum.m30 = m1.m30 + m2.m30;
            kSum.m31 = m1.m31 + m2.m31;
            kSum.m32 = m1.m32 + m2.m32;
            kSum.m33 = m1.m33 + m2.m33;

            return kSum;
        }

        /// <summary>
        /// Subtract two matrices.
        /// </summary>
        public static Matrix4x4d operator -(Matrix4x4d m1, Matrix4x4d m2)
        {
            Matrix4x4d kSum = new Matrix4x4d();
            kSum.m00 = m1.m00 - m2.m00;
            kSum.m01 = m1.m01 - m2.m01;
            kSum.m02 = m1.m02 - m2.m02;
            kSum.m03 = m1.m03 - m2.m03;

            kSum.m10 = m1.m10 - m2.m10;
            kSum.m11 = m1.m11 - m2.m11;
            kSum.m12 = m1.m12 - m2.m12;
            kSum.m13 = m1.m13 - m2.m13;

            kSum.m20 = m1.m20 - m2.m20;
            kSum.m21 = m1.m21 - m2.m21;
            kSum.m22 = m1.m22 - m2.m22;
            kSum.m23 = m1.m23 - m2.m23;

            kSum.m30 = m1.m30 - m2.m30;
            kSum.m31 = m1.m31 - m2.m31;
            kSum.m32 = m1.m32 - m2.m32;
            kSum.m33 = m1.m33 - m2.m33;
            return kSum;
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        public static Matrix4x4d operator *(Matrix4x4d m1, Matrix4x4d m2)
        {
            Matrix4x4d kProd = new Matrix4x4d();

            kProd.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30;
            kProd.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31;
            kProd.m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32;
            kProd.m03 = m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33;

            kProd.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30;
            kProd.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31;
            kProd.m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32;
            kProd.m13 = m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33;

            kProd.m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30;
            kProd.m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31;
            kProd.m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32;
            kProd.m23 = m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33;

            kProd.m30 = m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30;
            kProd.m31 = m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31;
            kProd.m32 = m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32;
            kProd.m33 = m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33;
            return kProd;
        }

        /// <summary>
        /// Multiply a vector by a matrix.
        /// Acts like z is 0, and w is 0.
        /// </summary>
        public static VECTOR2 operator *(Matrix4x4d m, VECTOR2 v)
        {
            VECTOR2 kProd = new VECTOR2();

            kProd.x = m.m00 * v.x + m.m01 * v.y;
            kProd.y = m.m10 * v.x + m.m11 * v.y;
    
            return kProd;
        }

        /// <summary>
        /// Multiply a vector by a matrix.
        /// Acts like w is 0.
        /// </summary>
        public static VECTOR3 operator *(Matrix4x4d m, VECTOR3 v)
        {
            VECTOR3 kProd = new VECTOR3();

            kProd.x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z;
            kProd.y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z;
            kProd.z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z;

            return kProd;
        }

        /// <summary>
        /// Multiply a point by a matrix.
        /// Acts like z is 0, and w is 1.
        /// </summary>
        public static POINT2 operator *(Matrix4x4d m, POINT2 v)
        {
            POINT2 kProd = new POINT2();

            kProd.x = m.m00 * v.x + m.m01 * v.y + m.m03;
            kProd.y = m.m10 * v.x + m.m11 * v.y + m.m13;

            return kProd;
        }

        /// <summary>
        /// Multiply a point by a matrix.
        /// Acts like w is 1.
        /// </summary>
        public static POINT3 operator *(Matrix4x4d m, POINT3 v)
        {
            POINT3 kProd = new POINT3();

            kProd.x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03;
            kProd.y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13;
            kProd.z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23;
  
            return kProd;
        }

        /// <summary>
        /// Multiply a vector by a matrix.
        /// </summary>
        public static VECTOR4 operator *(Matrix4x4d m, VECTOR4 v)
        {
            VECTOR4 kProd = new VECTOR4();

            kProd.x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * v.w;
            kProd.y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * v.w;
            kProd.z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * v.w;
            kProd.w = m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33 * v.w;

            return kProd;
        }

        /// <summary>
        /// Multiply a point by a matrix.
        /// </summary>
        public static POINT4 operator *(Matrix4x4d m, POINT4 v)
        {
            POINT4 kProd = new POINT4();

            kProd.x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * v.w;
            kProd.y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * v.w;
            kProd.z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * v.w;
            kProd.w = m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33 * v.w;

            return kProd;
        }

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        public static Matrix4x4d operator *(Matrix4x4d m1, REAL s)
        {
            Matrix4x4d kProd = new Matrix4x4d();
            kProd.m00 = m1.m00 * s;
            kProd.m01 = m1.m01 * s;
            kProd.m02 = m1.m02 * s;
            kProd.m03 = m1.m03 * s;

            kProd.m10 = m1.m10 * s;
            kProd.m11 = m1.m11 * s;
            kProd.m12 = m1.m12 * s;
            kProd.m13 = m1.m13 * s;

            kProd.m20 = m1.m20 * s;
            kProd.m21 = m1.m21 * s;
            kProd.m22 = m1.m22 * s;
            kProd.m23 = m1.m23 * s;

            kProd.m30 = m1.m30 * s;
            kProd.m31 = m1.m31 * s;
            kProd.m32 = m1.m32 * s;
            kProd.m33 = m1.m33 * s;
            return kProd;
        }

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        public static Matrix4x4d operator *(REAL s, Matrix4x4d m1)
        {
            Matrix4x4d kProd = new Matrix4x4d();
            kProd.m00 = m1.m00 * s;
            kProd.m01 = m1.m01 * s;
            kProd.m02 = m1.m02 * s;
            kProd.m03 = m1.m03 * s;

            kProd.m10 = m1.m10 * s;
            kProd.m11 = m1.m11 * s;
            kProd.m12 = m1.m12 * s;
            kProd.m13 = m1.m13 * s;

            kProd.m20 = m1.m20 * s;
            kProd.m21 = m1.m21 * s;
            kProd.m22 = m1.m22 * s;
            kProd.m23 = m1.m23 * s;

            kProd.m30 = m1.m30 * s;
            kProd.m31 = m1.m31 * s;
            kProd.m32 = m1.m32 * s;
            kProd.m33 = m1.m33 * s;
            return kProd;
        }

        /// <summary>
        /// Cast to double matrix from a float matrix.
        /// </summary>
        /// <param name="m">The other matrix</param>
        public static implicit operator Matrix4x4d(Matrix4x4f m)
        {
            var m2 = new Matrix4x4d();
            for (int i = 0; i < 16; i++)
                m2[i] = m[i];

            return m2;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public static bool operator ==(Matrix4x4d m1, Matrix4x4d m2)
        {

            if (m1.m00 != m2.m00) return false;
            if (m1.m01 != m2.m01) return false;
            if (m1.m02 != m2.m02) return false;
            if (m1.m03 != m2.m03) return false;

            if (m1.m10 != m2.m10) return false;
            if (m1.m11 != m2.m11) return false;
            if (m1.m12 != m2.m12) return false;
            if (m1.m13 != m2.m13) return false;

            if (m1.m20 != m2.m20) return false;
            if (m1.m21 != m2.m21) return false;
            if (m1.m22 != m2.m22) return false;
            if (m1.m23 != m2.m23) return false;

            if (m1.m30 != m2.m30) return false;
            if (m1.m31 != m2.m31) return false;
            if (m1.m32 != m2.m32) return false;
            if (m1.m33 != m2.m33) return false;

            return true;
        }

        /// <summary>
        /// Are these matrices not equal.
        /// </summary>
        public static bool operator !=(Matrix4x4d m1, Matrix4x4d m2)
        {
            if (m1.m00 != m2.m00) return true;
            if (m1.m01 != m2.m01) return true;
            if (m1.m02 != m2.m02) return true;
            if (m1.m03 != m2.m03) return true;

            if (m1.m10 != m2.m10) return true;
            if (m1.m11 != m2.m11) return true;
            if (m1.m12 != m2.m12) return true;
            if (m1.m13 != m2.m13) return true;

            if (m1.m20 != m2.m20) return true;
            if (m1.m21 != m2.m21) return true;
            if (m1.m22 != m2.m22) return true;
            if (m1.m23 != m2.m23) return true;

            if (m1.m30 != m2.m30) return true;
            if (m1.m31 != m2.m31) return true;
            if (m1.m32 != m2.m32) return true;

            return false;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4x4d)) return false;

            Matrix4x4d mat = (Matrix4x4d)obj;

            return this == mat;
        }

        /// <summary>
		/// Are these matrices equal.
		/// </summary>
        public bool Equals(Matrix4x4d mat)
        {
            return this == mat;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public static bool AlmostEqual(Matrix4x4d m0, Matrix4x4d m1, REAL eps = MathUtil.EPS_64)
        {
            if (Math.Abs(m0.m00 - m1.m00) > eps) return false;
            if (Math.Abs(m0.m10 - m1.m10) > eps) return false;
            if (Math.Abs(m0.m20 - m1.m20) > eps) return false;
            if (Math.Abs(m0.m30 - m1.m30) > eps) return false;

            if (Math.Abs(m0.m01 - m1.m01) > eps) return false;
            if (Math.Abs(m0.m11 - m1.m11) > eps) return false;
            if (Math.Abs(m0.m21 - m1.m21) > eps) return false;
            if (Math.Abs(m0.m31 - m1.m31) > eps) return false;

            if (Math.Abs(m0.m02 - m1.m02) > eps) return false;
            if (Math.Abs(m0.m12 - m1.m12) > eps) return false;
            if (Math.Abs(m0.m22 - m1.m22) > eps) return false;
            if (Math.Abs(m0.m32 - m1.m32) > eps) return false;

            if (Math.Abs(m0.m03 - m1.m03) > eps) return false;
            if (Math.Abs(m0.m13 - m1.m13) > eps) return false;
            if (Math.Abs(m0.m23 - m1.m23) > eps) return false;
            if (Math.Abs(m0.m33 - m1.m33) > eps) return false;

            return true;
        }

        /// <summary>
        /// Matrices hash code. 
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)MathUtil.HASH_PRIME_1;
                for (int i = 0; i < 16; i++)
                    hash = (hash * MathUtil.HASH_PRIME_2) ^ this[i].GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// A matrix as a string.
        /// </summary>
        public override string ToString()
        {
            return this[0, 0] + "," + this[0, 1] + "," + this[0, 2] + "," + this[0, 3] + "\n" +
                    this[1, 0] + "," + this[1, 1] + "," + this[1, 2] + "," + this[1, 3] + "\n" +
                    this[2, 0] + "," + this[2, 1] + "," + this[2, 2] + "," + this[2, 3] + "\n" +
                    this[3, 0] + "," + this[3, 1] + "," + this[3, 2] + "," + this[3, 3];
        }

        /// <summary>
        /// A matrix as a string.
        /// </summary>
        public string ToString(string f)
        {
            return this[0, 0].ToString(f) + "," + this[0, 1].ToString(f) + "," + this[0, 2].ToString(f) + "," + this[0, 3].ToString(f) + "\n" +
                    this[1, 0].ToString(f) + "," + this[1, 1].ToString(f) + "," + this[1, 2].ToString(f) + "," + this[1, 3].ToString(f) + "\n" +
                    this[2, 0].ToString(f) + "," + this[2, 1].ToString(f) + "," + this[2, 2].ToString(f) + "," + this[2, 3].ToString(f) + "\n" +
                    this[3, 0].ToString(f) + "," + this[3, 1].ToString(f) + "," + this[3, 2].ToString(f) + "," + this[3, 3].ToString(f);
        }

        /// <summary>
        /// The minor of a matrix. 
        /// </summary>
        private REAL Minor(int r0, int r1, int r2, int c0, int c1, int c2)
        {
            return this[r0, c0] * (this[r1, c1] * this[r2, c2] - this[r2, c1] * this[r1, c2]) -
                    this[r0, c1] * (this[r1, c0] * this[r2, c2] - this[r2, c0] * this[r1, c2]) +
                    this[r0, c2] * (this[r1, c0] * this[r2, c1] - this[r2, c0] * this[r1, c1]);
        }

        /// <summary>
        /// The inverse of the matrix.
        /// A matrix multipled by its inverse is the idenity.
        /// </summary>
        public bool TryInverse(ref Matrix4x4d mInv)
        {

            REAL det = Determinant;

            if (MathUtil.IsZero(det))
                return false;

            mInv = Adjoint * (1.0f / det);
            return true;
        }

        /// <summary>
        /// Get the ith column as a vector.
        /// </summary>
        public VECTOR4 GetColumn(int iCol)
        {
            return new VECTOR4(this[0, iCol], this[1, iCol], this[2, iCol], this[3, iCol]);
        }

        /// <summary>
        /// Set the ith column from a vector.
        /// </summary>
        public void SetColumn(int iCol, VECTOR4 v)
        {
            this[0, iCol] = v.x;
            this[1, iCol] = v.y;
            this[2, iCol] = v.z;
            this[3, iCol] = v.w;
        }

        /// <summary>
        /// Get the ith row as a vector.
        /// </summary>
        public VECTOR4 GetRow(int iRow)
        {
            return new VECTOR4(this[iRow, 0], this[iRow, 1], this[iRow, 2], this[iRow, 3]);
        }

        /// <summary>
        /// Set the ith row from a vector.
        /// </summary>
        public void SetRow(int iRow, VECTOR4 v)
        {
            this[iRow, 0] = v.x;
            this[iRow, 1] = v.y;
            this[iRow, 2] = v.z;
            this[iRow, 3] = v.w;
        }

        /// <summary>
        /// Convert to a 3 dimension matrix.
        /// </summary>
        public Matrix3x3d ToMatrix3x3d()
        {
            Matrix3x3d mat = new Matrix3x3d();

            mat.m00 = m00; mat.m01 = m01; mat.m02 = m02;
            mat.m10 = m10; mat.m11 = m11; mat.m12 = m12;
            mat.m20 = m20; mat.m21 = m21; mat.m22 = m22;

            return mat;
        }

        /// <summary>
        /// Create a translation, rotation and scale.
        /// </summary>
        static public Matrix4x4d TranslateRotateScale(POINT3 t, QUATERNION r, POINT3 s)
        {
            Matrix4x4d T = Translate(t);
            Matrix4x4d R = r.ToMatrix4x4d();
            Matrix4x4d S = Scale(s);

            return T * R * S;
        }

        /// <summary>
        /// Create a translation and rotation.
        /// </summary>
        static public Matrix4x4d TranslateRotate(POINT3 t, QUATERNION r)
        {
            Matrix4x4d T = Translate(t);
            Matrix4x4d R = r.ToMatrix4x4d();

            return T * R;
        }

        /// <summary>
        /// Create a translation and scale.
        /// </summary>
        static public Matrix4x4d TranslateScale(POINT3 t, POINT3 s)
        {
            Matrix4x4d T = Translate(t);
            Matrix4x4d S = Scale(s);

            return T * S;
        }

        /// <summary>
        /// Create a rotation and scale.
        /// </summary>
        static public Matrix4x4d RotateScale(QUATERNION r, POINT3 s)
        {
            Matrix4x4d R = r.ToMatrix4x4d();
            Matrix4x4d S = Scale(s);

            return R * S;
        }

        /// <summary>
        /// Create a translation out of a vector.
        /// </summary>
        static public Matrix4x4d Translate(POINT3 v)
        {
            return new Matrix4x4d(1, 0, 0, v.x,
                                    0, 1, 0, v.y,
                                    0, 0, 1, v.z,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a translation out of a vector.
        /// </summary>
        static public Matrix4x4d Translate(REAL x, REAL y, REAL z)
        {
            return new Matrix4x4d(1, 0, 0, x,
                                    0, 1, 0, y,
                                    0, 0, 1, z,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a scale out of a vector.
        /// </summary>
        static public Matrix4x4d Scale(POINT3 v)
        {
            return new Matrix4x4d(v.x, 0, 0, 0,
                                    0, v.y, 0, 0,
                                    0, 0, v.z, 0,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a scale out of a vector.
        /// </summary>
        static public Matrix4x4d Scale(REAL x, REAL y, REAL z)
        {
            return new Matrix4x4d(x, 0, 0, 0,
                                    0, y, 0, 0,
                                    0, 0, z, 0,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a scale out of a vector.
        /// </summary>
        static public Matrix4x4d Scale(REAL s)
        {
            return new Matrix4x4d(s, 0, 0, 0,
                                  0, s, 0, 0,
                                  0, 0, s, 0,
                                  0, 0, 0, 1);
        }

        /// <summary>
        /// Create a rotation out of a angle in degrees.
        /// </summary>
        static public Matrix4x4d RotateX(Radian radian)
        {
            REAL a = (REAL)radian.angle;
            REAL ca = MathUtil.Cos(a);
            REAL sa = MathUtil.Sin(a);

            return new Matrix4x4d(1, 0, 0, 0,
                                    0, ca, -sa, 0,
                                    0, sa, ca, 0,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a rotation out of a angle in degrees.
        /// </summary>
        static public Matrix4x4d RotateY(Radian radian)
        {
            REAL a = (REAL)radian.angle;
            REAL ca = MathUtil.Cos(a);
            REAL sa = MathUtil.Sin(a);

            return new Matrix4x4d(ca, 0, sa, 0,
                                    0, 1, 0, 0,
                                    -sa, 0, ca, 0,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a rotation out of a angle in degrees.
        /// </summary>
        static public Matrix4x4d RotateZ(Radian radian)
        {
            REAL a = (REAL)radian.angle;
            REAL ca = MathUtil.Cos(a);
            REAL sa = MathUtil.Sin(a);

            return new Matrix4x4d(ca, -sa, 0, 0,
                                    sa, ca, 0, 0,
                                    0, 0, 1, 0,
                                    0, 0, 0, 1);
        }

        /// <summary>
        /// Create a rotation out of a vector.
        /// </summary>
        static public Matrix4x4d Rotate(VECTOR3 euler)
        {
            return QUATERNION.FromEuler(euler).ToMatrix4x4d();
        }

        /// <summary>
        /// Create a rotation from a angle and the rotation axis.
        /// </summary>
        /// <param name="radian">The rotation amount.</param>
        /// <param name="axis">The axis to rotate on.</param>
        /// <returns>The rotation matrix.</returns>
        static public Matrix4x4d Rotate(Radian radian, VECTOR3 axis)
        {
            REAL sinTheta = (REAL)MathUtil.Sin(radian);
            REAL cosTheta = (REAL)MathUtil.Cos(radian);

            Matrix4x4d m = new Matrix4x4d();

            // Compute rotation of first basis vector
            m[0, 0] = axis.x * axis.x + (1 - axis.x * axis.x) * cosTheta;
            m[0, 1] = axis.x * axis.y * (1 - cosTheta) - axis.z * sinTheta;
            m[0, 2] = axis.x * axis.z * (1 - cosTheta) + axis.y * sinTheta;
            m[0, 3] = 0;

            // Compute rotations of second and third basis vectors
            m[1, 0] = axis.x * axis.y * (1 - cosTheta) + axis.z * sinTheta;
            m[1, 1] = axis.y * axis.y + (1 - axis.y * axis.y) * cosTheta;
            m[1, 2] = axis.y * axis.z * (1 - cosTheta) - axis.x * sinTheta;
            m[1, 3] = 0;

            m[2, 0] = axis.x * axis.z * (1 - cosTheta) - axis.y * sinTheta;
            m[2, 1] = axis.y * axis.z * (1 - cosTheta) + axis.x * sinTheta;
            m[2, 2] = axis.z * axis.z + (1 - axis.z * axis.z) * cosTheta;
            m[2, 3] = 0;

            return m;
        }

        /// <summary>
        /// Create a perspective matrix.
        /// </summary>
        static public Matrix4x4d Perspective(Radian fovy, REAL aspect, REAL zNear, REAL zFar)
        {
            REAL f = 1.0f / (REAL)Math.Tan(fovy.angle / 2.0);
            return new Matrix4x4d(f / aspect, 0, 0, 0,
                                    0, f, 0, 0,
                                    0, 0, (zFar + zNear) / (zNear - zFar), (2.0f * zFar * zNear) / (zNear - zFar),
                                    0, 0, -1, 0);

            /*
            // Perform projective divide for perspective projection
            Matrix4x4 persp(1, 0, 0, 0, 
                            0, 1, 0, 0, 
                            0, 0, f / (f - n), -f * n / (f - n),
                            0, 0, 1, 0);

            // Scale canonical perspective view to specified field of view
            Float invTanAng = 1 / std::tan(Radians(fov) / 2);
            return Scale(invTanAng, invTanAng, 1) * Transform(persp);
            */
        }

        /// <summary>
        /// Create a ortho matrix.
        /// </summary>
        static public Matrix4x4d Ortho(REAL xRight, REAL xLeft, REAL yTop, REAL yBottom, REAL zNear, REAL zFar)
        {
            REAL tx, ty, tz;
            tx = -(xRight + xLeft) / (xRight - xLeft);
            ty = -(yTop + yBottom) / (yTop - yBottom);
            tz = -(zFar + zNear) / (zFar - zNear);
            return new Matrix4x4d(2.0f / (xRight - xLeft), 0, 0, tx,
                                    0, 2.0f / (yTop - yBottom), 0, ty,
                                    0, 0, -2.0f / (zFar - zNear), tz,
                                    0, 0, 0, 1);

        }

        /// <summary>
        /// Create a ortho matrix.
        /// </summary>
        static public Matrix4x4d Ortho(REAL zNear, REAL zFar)
        {
            return Scale(1, 1, 1 / (zFar - zNear)) * Translate(0, 0, -zNear);
        }

        /// <summary>
        /// Creates the matrix need to look at target from position.
        /// </summary>
        static public Matrix4x4d LookAt(POINT3 position, POINT3 target, VECTOR3 Up)
        {

            VECTOR3 zaxis = POINT3.Direction(target, position);
            VECTOR3 xaxis = VECTOR3.Cross(Up, zaxis).Normalized;
            VECTOR3 yaxis = VECTOR3.Cross(zaxis, xaxis);

            return new Matrix4x4d(xaxis.x, xaxis.y, xaxis.z, -VECTOR3.Dot(xaxis, position),
                                      yaxis.x, yaxis.y, yaxis.z, -VECTOR3.Dot(yaxis, position),
                                      zaxis.x, zaxis.y, zaxis.z, -VECTOR3.Dot(zaxis, position),
                                      0, 0, 0, 1);
        }


    }

}

























