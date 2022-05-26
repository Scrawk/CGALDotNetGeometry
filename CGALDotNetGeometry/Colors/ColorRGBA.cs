﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Colors
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRGBA : IEquatable<ColorRGBA>
    {

        public readonly static ColorRGBA Red = new ColorRGBA(1, 0, 0, 1);
        public readonly static ColorRGBA Orange = new ColorRGBA(1, 0.5, 0, 1);
        public readonly static ColorRGBA Olive = new ColorRGBA(0.5, 0.5, 0, 1);
        public readonly static ColorRGBA VividGreen = new ColorRGBA(0.5, 1, 0, 1);
        public readonly static ColorRGBA Yellow = new ColorRGBA(1, 1, 0, 1);
        public readonly static ColorRGBA Green = new ColorRGBA(0, 1, 0, 1);
        public readonly static ColorRGBA SpringGreen = new ColorRGBA(0, 1, 0.5, 1);
        public readonly static ColorRGBA Cyan = new ColorRGBA(0, 1, 1, 1);
        public readonly static ColorRGBA Azure = new ColorRGBA(0, 0.5, 1, 1);
        public readonly static ColorRGBA Teal = new ColorRGBA(0, 0.5, 0.5, 1);
        public readonly static ColorRGBA Blue = new ColorRGBA(0, 0, 1, 1);
        public readonly static ColorRGBA Violet = new ColorRGBA(0.5, 0, 1, 1);
        public readonly static ColorRGBA Purple = new ColorRGBA(0.5, 0, 0.5, 1);
        public readonly static ColorRGBA Magenta = new ColorRGBA(1, 0, 1, 1);

        public readonly static ColorRGBA Black = new ColorRGBA(0, 1);
        public readonly static ColorRGBA DarkGrey = new ColorRGBA(0.25f, 1);
        public readonly static ColorRGBA Grey = new ColorRGBA(0.5f, 1);
        public readonly static ColorRGBA LightGrey = new ColorRGBA(0.75f, 1);
        public readonly static ColorRGBA White = new ColorRGBA(1, 1);
        public readonly static ColorRGBA Clear = new ColorRGBA(0, 0);

        public float r, g, b, a;

        public ColorRGBA(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public ColorRGBA(float r, float g, float b) 
            : this(r,g,b,1)
        {

        }

        public ColorRGBA(float v) 
            : this(v,v,v,v)
        {

        }

        public ColorRGBA(float v, float a) 
            : this(v,v,v, 1)
        {

        }

        public ColorRGBA(double r, double g, double b, double a) 
            : this((float)r, (float)g, (float)b, (float)a)
        {

        }

        public ColorRGBA(double r, double g, double b) 
            : this(r, g, b, 1)
        {

        }

        public ColorRGBA(double v) 
            : this(v, v, v, v)
        {

        }

        public ColorRGBA(double v, double a) 
            : this(v, v, v, a)
        {

        }

        public ColorRGBA(ColorRGB col, float a) 
            : this(col.r, col.g, col.b, a)
        {

        }

        unsafe public float this[int i]
        {
            get
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("ColorRGBA index out of range.");

                fixed (ColorRGBA* array = &this) { return ((float*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("ColorRGBA index out of range.");

                fixed (float* array = &r) { array[i] = value; }
            }
        }

        public ColorRGBA rrra
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorRGBA(r, r, r, a); }
        }

        public ColorRGBA bgra
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorRGBA(b, g, r, a); }
        }

        public ColorRGB rgb
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorRGB(r, g, b); }
        }

        public ColorHSV hsv
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ColorRGB.ToHSV(r, g, b); }
        }

        public float Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (float)Math.Sqrt(SqrMagnitude); }
        }

        public float SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (r * r + g * g + b * b + a * a); }
        }

        public float Intensity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (r + g + b) / 3.0f; }
        }

        public float Luminance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 0.2126f * r + 0.7152f * g + 0.0722f * b; }
        }

        /// <summary>
        /// Add two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator +(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r + v2.r, v1.g + v2.g, v1.b + v2.b, v1.a + v2.a);
        }

        /// <summary>
        /// Add color and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator +(ColorRGBA v1, float s)
        {
            return new ColorRGBA(v1.r + s, v1.g + s, v1.b + s, v1.a + s);
        }

        /// <summary>
        /// Add color and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator +(float s, ColorRGBA v1)
        {
            return new ColorRGBA(v1.r + s, v1.g + s, v1.b + s, v1.a + s);
        }

        /// <summary>
        /// Subtract two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator -(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r - v2.r, v1.g - v2.g, v1.b - v2.b, v1.a - v2.a);
        }

        /// <summary>
        /// Subtract color and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator -(ColorRGBA v1, float s)
        {
            return new ColorRGBA(v1.r - s, v1.g - s, v1.b - s, v1.a - s);
        }

        /// <summary>
        /// Subtract color and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator -(float s, ColorRGBA v1)
        {
            return new ColorRGBA(s - v1.r,s -  v1.g, s - v1.b, s - v1.a);
        }

        /// <summary>
        /// Multiply two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator *(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r * v2.r, v1.g * v2.g, v1.b * v2.b, v1.a * v2.a);
        }

        /// <summary>
        /// Multiply a color and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator *(ColorRGBA v, float s)
        {
            return new ColorRGBA(v.r * s, v.g * s, v.b * s, v.a * s);
        }

        /// <summary>
        /// Multiply a color and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator *(float s, ColorRGBA v)
        {
            return new ColorRGBA(v.r * s, v.g * s, v.b * s, v.a * s);
        }

        /// <summary>
        /// Divide two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator /(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r / v2.r, v1.g / v2.g, v1.b / v2.b, v1.a / v2.a);
        }

        /// <summary>
        /// Divide a color and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGBA operator /(ColorRGBA v, float s)
        {
            return new ColorRGBA(v.r / s, v.g / s, v.b / s, v.a / s);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ColorRGBA v1, ColorRGBA v2)
        {
            return (v1.r == v2.r && v1.g == v2.g && v1.b == v2.b && v1.a == v2.a);
        }

        /// <summary>
        /// Are these colors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColorRGBA v1, ColorRGBA v2)
        {
            return (v1.r != v2.r || v1.g != v2.g || v1.b != v2.b || v1.a != v2.a);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorRGBA)) return false;

            ColorRGBA v = (ColorRGBA)obj;

            return this == v;
        }

        /// <summary>
        /// Are these colors equal given the error.
        /// </summary>
        public static bool AlmostEqual(ColorRGBA c0, ColorRGBA c1, float eps = MathUtil.EPS_32)
        {
            if (Math.Abs(c0.r - c1.r) > eps) return false;
            if (Math.Abs(c0.g - c1.g) > eps) return false;
            if (Math.Abs(c0.b - c1.b) > eps) return false;
            if (Math.Abs(c0.a - c1.a) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public bool Equals(ColorRGBA v)
        {
            return this == v;
        }

        /// <summary>
        /// colors hash code. 
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ r.GetHashCode();
                hash = (hash * 16777619) ^ g.GetHashCode();
                hash = (hash * 16777619) ^ b.GetHashCode();
                hash = (hash * 16777619) ^ a.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// color as a string.
        /// </summary>
		public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", r,g,b,a);
        }

        /// <summary>
        /// color as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2},{3}", r.ToString(f), g.ToString(f), b.ToString(f), a.ToString(f));
        }

        /// <summary>
        /// Alpha blend the two pixels.
        /// </summary>
        /// <param name="c0">The first pixel.</param>
        /// <param name="c1">The second pixel.</param>
        /// <returns>The alpha blened pixel.</returns>
        public static ColorRGBA AlphaBlend(ColorRGBA c0, ColorRGBA c1)
        {
            float a = c0.a + (1.0f - c1.a);
            a = MathUtil.Clamp01(a);

            if (a <= 0) 
                return ColorRGBA.Clear;

            float inv_a = 1.0f / a;
            float one_min_a = 1.0f - c0.a;

            var c = new ColorRGBA();
            c.r = ((c0.r * c0.a) + (c1.r * c1.a) * one_min_a) * inv_a;
            c.g = ((c0.g * c0.a) + (c1.g * c1.a) * one_min_a) * inv_a;
            c.b = ((c0.b * c0.a) + (c1.b * c1.a) * one_min_a) * inv_a;
            c.a = a;

            return c;
        }

        /// <summary>
        /// color from bytes.
        /// The values will be converted from a 0-255 range to a 0-1 range.
        /// </summary>
        /// <returns>A color will values in the 0-1 range.</returns>
        public static ColorRGBA FromBytes(int r, int g, int b, int a)
        {
            int R = MathUtil.Clamp(r, 0, 255);
            int G = MathUtil.Clamp(g, 0, 255);
            int B = MathUtil.Clamp(b, 0, 255);
            int A = MathUtil.Clamp(a, 0, 255);
            return new ColorRGBA(R, G, B, A) / 255.0f;
        }


        /// <summary>
        /// Create a color from a integer where each byte in the 
        /// integer represents a channl in the color.
        /// </summary>
        /// <param name="i">The integer.</param>
        /// <param name="abgr">are the channels packed bgr or rgb.</param>
        /// <returns>The color.</returns>
        public static ColorRGBA FromInteger(int i, bool abgr = false)
        {
            Union32 u = i;
            if (abgr)
                return FromBytes(u.Byte0, u.Byte1, u.Byte2, u.Byte3);
            else
                return FromBytes(u.Byte3, u.Byte2, u.Byte1, u.Byte0);
        }

        /// <summary>
        /// Convert the color to a integer where each byte 
        /// represents a channel in the color.
        /// </summary>
        /// <param name="abgr">are the channels packed bgr or rgb.</param>
        /// <returns>A integer where each byte represents a channel in the color.</returns>
        public int ToInteger(bool abgr = false)
        {
            int R = (int)MathUtil.Clamp(r * 255.0f, 0.0f, 255.0f);
            int G = (int)MathUtil.Clamp(g * 255.0f, 0.0f, 255.0f);
            int B = (int)MathUtil.Clamp(b * 255.0f, 0.0f, 255.0f);
            int A = (int)MathUtil.Clamp(a * 255.0f, 0.0f, 255.0f);

            if (abgr)
                return (A << 24) | (B << 16) | (G << 8) | R;
            else
                return R | (G << 8) | (B << 16) | (A << 24);
        }

        /// <summary>
        /// Apply the gamma function to the color.
        /// Gamma is not applied to the alpha channel.
        /// </summary>
        /// <param name="lambda">The power to raise each channel to.</param>
        /// <param name="A">The constant the result is multiplied by. Defaults to 1.</param>
        public void Gamma(float lambda, float A = 1)
        {
            r = MathUtil.Pow(r, lambda) * A;
            g = MathUtil.Pow(g, lambda) * A;
            b = MathUtil.Pow(b, lambda) * A;
        }

        /// <summary>
        /// The distance between two colors.
        /// </summary>
        public static float Distance(ColorRGBA c0, ColorRGBA c1)
        {
            return MathUtil.SafeSqrt(SqrDistance(c0, c1));
        }

        /// <summary>
        /// The square distance between two colors.
        /// </summary>
        public static float SqrDistance(ColorRGBA c0, ColorRGBA c1)
        {
            return (c0 - c1).SqrMagnitude;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        public static ColorRGBA Min(ColorRGBA col, float s)
        {
            col.r = Math.Min(col.r, s);
            col.g = Math.Min(col.g, s);
            col.b = Math.Min(col.b, s);
            col.a = Math.Min(col.a, s);
            return col;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        public static ColorRGBA Max(ColorRGBA col, float s)
        {
            col.r = Math.Max(col.r, s);
            col.g = Math.Max(col.g, s);
            col.b = Math.Max(col.b, s);
            col.a = Math.Max(col.a, s);
            return col;
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        public static ColorRGBA Clamp(ColorRGBA col, float min, float max)
        {
            col.r = Math.Max(Math.Min(col.r, max), min);
            col.g = Math.Max(Math.Min(col.g, max), min);
            col.b = Math.Max(Math.Min(col.b, max), min);
            col.a = Math.Max(Math.Min(col.a, max), min);
            return col;
        }

        /// <summary>
        /// Lerp between two colors.
        /// </summary>
        public static ColorRGBA Lerp(ColorRGBA c1, ColorRGBA c2, float a)
        {
            a = MathUtil.Clamp01(a);
            ColorRGBA col = new ColorRGBA();
            col.r = MathUtil.Lerp(c1.r, c2.r, a);
            col.g = MathUtil.Lerp(c1.g, c2.g, a);
            col.b = MathUtil.Lerp(c1.b, c2.b, a);
            col.a = MathUtil.Lerp(c1.a, c2.a, a);
            return col;
        }

        /// <summary>
        /// BLerp between four colors.
        /// </summary>
        public static ColorRGBA BLerp(ColorRGBA c00, ColorRGBA c10, ColorRGBA c01, ColorRGBA c11, float a0, float a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            ColorRGBA col = new ColorRGBA();
            col.r = MathUtil.BLerp(c00.r, c10.r, c01.r, c11.r, a0, a1);
            col.g = MathUtil.BLerp(c00.g, c10.g, c01.g, c11.g, a0, a1);
            col.b = MathUtil.BLerp(c00.b, c10.b, c01.b, c11.b, a0, a1);
            col.a = MathUtil.BLerp(c00.a, c10.a, c01.a, c11.a, a0, a1);

            return col;
        }

        /// <summary>
        /// Convert to HSV color space.
        /// </summary>
        public ColorHSV ToHSV()
        {
            float delta, min;
            float h = 0, s, v;

            float R = MathUtil.Clamp(r * 255.0f, 0.0f, 255.0f);
            float G = MathUtil.Clamp(g * 255.0f, 0.0f, 255.0f);
            float B = MathUtil.Clamp(b * 255.0f, 0.0f, 255.0f);

            min = Math.Min(Math.Min(R, G), B);
            v = Math.Max(Math.Max(R, G), B);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0f;
            else
            {
                if (R == v)
                    h = (G - B) / delta;
                else if (G == v)
                    h = 2 + (B - R) / delta;
                else if (B == v)
                    h = 4 + (R - G) / delta;

                h *= 60;
                if (h < 0.0)
                    h = h + 360;
            }

            return new ColorHSV(h / 360.0f, s, v / 255.0f);
        }

    }

}
