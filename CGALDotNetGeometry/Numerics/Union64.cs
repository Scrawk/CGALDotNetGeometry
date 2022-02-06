using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct Union64 : IEquatable<Union64>
    {

        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(2)]
        public byte Byte2;

        [FieldOffset(3)]
        public byte Byte3;

        [FieldOffset(4)]
        public byte Byte4;

        [FieldOffset(5)]
        public byte Byte5;

        [FieldOffset(6)]
        public byte Byte6;

        [FieldOffset(7)]
        public byte Byte7;

        [FieldOffset(0)]
        public int Int1;

        [FieldOffset(4)]
        public int Int2;

        [FieldOffset(0)]
        public uint UInt1;

        [FieldOffset(4)]
        public uint UInt2;

        [FieldOffset(0)]
        public UInt64 UInt64;

        [FieldOffset(0)]
        public Int64 Int64;

        [FieldOffset(0)]
        public double Double;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union64(uint ui)
        {
            var u = new Union64();
            u.UInt1 = ui;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union64(int i)
        {
            var u = new Union64();
            u.Int1 = i;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union64(UInt64 ui)
        {
            var u = new Union64();
            u.UInt64 = ui;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union64(Int64 i)
        {
            var u = new Union64();
            u.Int64 = i;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union64(double d)
        {
            var u = new Union64();
            u.Double = d;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Union64 u1, Union64 u2)
        {
            return u1.Int64 == u2.Int64;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Union64 u1, Union64 u2)
        {
            return u1.Int64 != u2.Int64;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Union64)) return false;
            Union64 v = (Union64)obj;
            return this == v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Union64 v)
        {
            return this == v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Int64.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Union64: Int64={0}, UInt64={1}, Double={2}]", Int64, UInt64, Double);
        }

    }
}
