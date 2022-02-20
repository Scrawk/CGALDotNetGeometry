using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace CGALDotNetGeometry.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct Union16 : IEquatable<Union16>
    {

        [FieldOffset(0)]
        public byte Byte0;

        [FieldOffset(1)]
        public byte Byte1;

        [FieldOffset(0)]
        public ushort UShort;

        [FieldOffset(0)]
        public short Short;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union16(short i)
        {
            var u = new Union16();
            u.Short = i;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union16(ushort ui)
        {
            var u = new Union16();
            u.UShort = ui;
            return u;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Union16 u1, Union16 u2)
        {
            return u1.Short == u2.Short;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Union16 u1, Union16 u2)
        {
            return u1.Short != u2.Short;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (!(obj is Union16)) return false;
            Union16 v = (Union16)obj;
            return this == v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Union16 v)
        {
            return this == v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Short.GetHashCode();
        }

        public float Half
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get 
            {
                float half = Byte0 * 256.0f + Byte1;
                half /= (float)ushort.MaxValue;
                return half;
            }
        }

        public override string ToString()
        {
            return string.Format("[Union16: Short={0}, UShort={1}, Half={2}]", Short, UShort, Half);
        }

    }
}
