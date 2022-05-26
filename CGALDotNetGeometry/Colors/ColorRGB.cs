using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using CGALDotNetGeometry.Numerics;

namespace CGALDotNetGeometry.Colors
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRGB : IEquatable<ColorRGB>
    {

        public readonly static ColorRGB Red = new ColorRGB(1, 0, 0);
        public readonly static ColorRGB Orange = new ColorRGB(1, 0.5, 0);
        public readonly static ColorRGB Olive = new ColorRGB(0.5, 0.5, 0);
        public readonly static ColorRGB VividGreen = new ColorRGB(0.5, 1, 0);
        public readonly static ColorRGB Yellow = new ColorRGB(1, 1, 0);
        public readonly static ColorRGB Green = new ColorRGB(0, 1, 0);
        public readonly static ColorRGB SpringGreen = new ColorRGB(0, 1, 0.5);
        public readonly static ColorRGB Cyan = new ColorRGB(0, 1, 1);
        public readonly static ColorRGB Azure = new ColorRGB(0, 0.5, 1);
        public readonly static ColorRGB Teal = new ColorRGB(0, 0.5, 0.5);
        public readonly static ColorRGB Blue = new ColorRGB(0, 0, 1);
        public readonly static ColorRGB Indigo = new ColorRGB(0.25, 0, 1);
        public readonly static ColorRGB Violet = new ColorRGB(0.5, 0, 1);
        public readonly static ColorRGB Purple = new ColorRGB(0.5, 0, 0.5);
        public readonly static ColorRGB Magenta = new ColorRGB(1, 0, 1);

        public readonly static ColorRGB Black = new ColorRGB(0);
        public readonly static ColorRGB DarkGrey = new ColorRGB(0.25f);
        public readonly static ColorRGB Grey = new ColorRGB(0.5f);
        public readonly static ColorRGB LightGrey = new ColorRGB(0.75f);
        public readonly static ColorRGB White = new ColorRGB(1);
        
        public float r, g, b;

        public ColorRGB(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public ColorRGB(float v) 
            : this(v,v,v)
        {

        }

        public ColorRGB(double r, double g, double b) 
            : this((float)r, (float)g, (float)b)
        {

        }

        public ColorRGB(double v) 
            : this(v, v, v)
        {

        }

        public ColorRGB rrr
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorRGB(r, r, r); }
        }

        public ColorRGB bgr
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorRGB(b, g, r); }
        }

        public ColorHSV hsv
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ToHSV(r, g, b); }
        }

        public ColorRGBA rgb1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ColorRGBA(r, g, b, 1); }
        }

        public float Magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return MathUtil.SafeSqrt(SqrMagnitude); }
        }

        public float SqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (r * r + g * g + b * b); }
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

        unsafe public float this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("ColorRGB index out of range.");

                fixed (ColorRGB* array = &this) { return ((float*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("ColorRGB index out of range.");

                fixed (float* array = &r) { array[i] = value; }
            }
        }

        /// <summary>
        /// Add two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator +(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r + v2.r, v1.g + v2.g, v1.b + v2.b);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator +(ColorRGB v1, float s)
        {
            return new ColorRGB(v1.r + s, v1.g + s, v1.b + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator +(float s, ColorRGB v1)
        {
            return new ColorRGB(v1.r + s, v1.g + s, v1.b + s);
        }

        /// <summary>
        /// Subtract two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator -(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r - v2.r, v1.g - v2.g, v1.b - v2.b);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator -(ColorRGB v1, float s)
        {
            return new ColorRGB(v1.r - s, v1.g - s, v1.b - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator -(float s, ColorRGB v1)
        {
            return new ColorRGB(s - v1.r, s - v1.g, s - v1.b);
        }

        /// <summary>
        /// Multiply two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator *(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r * v2.r, v1.g * v2.g, v1.b * v2.b);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator *(ColorRGB v, float s)
        {
            return new ColorRGB(v.r * s, v.g * s, v.b * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator *(float s, ColorRGB v)
        {
            return new ColorRGB(v.r * s, v.g * s, v.b * s);
        }

        /// <summary>
        /// Divide two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator /(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r / v2.r, v1.g / v2.g, v1.b / v2.b);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ColorRGB operator /(ColorRGB v, float s)
        {
            return new ColorRGB(v.r / s, v.g / s, v.b / s);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ColorRGB v1, ColorRGB v2)
        {
            return (v1.r == v2.r && v1.g == v2.g && v1.b == v2.b);
        }

        /// <summary>
        /// Are these colors not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ColorRGB v1, ColorRGB v2)
        {
            return (v1.r != v2.r || v1.g != v2.g || v1.b != v2.b);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorRGB)) return false;

            ColorRGB v = (ColorRGB)obj;

            return this == v;
        }

        /// <summary>
        /// Are these colors equal given the error.
        /// </summary>
        public static bool AlmostEqual(ColorRGB c0, ColorRGB c1, float eps = MathUtil.EPS_32)
        {
            if (Math.Abs(c0.r - c1.r) > eps) return false;
            if (Math.Abs(c0.g - c1.g) > eps) return false;
            if (Math.Abs(c0.b - c1.b) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public bool Equals(ColorRGB v)
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
                return hash;
            }
        }

        /// <summary>
        /// color as a string.
        /// </summary>
		public override string ToString()
        {
            return string.Format("{0},{1},{2}", r, g, b);
        }

        /// <summary>
        /// color as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2}", r.ToString(f), g.ToString(f), b.ToString(f));
        }

        /// <summary>
        /// Create a rgba color form this colors rgb 
        /// values and the provided alpha value.
        /// </summary>
        /// <param name="a">The alpha value.</param>
        /// <returns>THe RGBA color.</returns>
        public ColorRGBA RGBA(float a)
        {
            return new ColorRGBA(r, g, b, a);
        }

        /// <summary>
        /// Alpha blend the two pixels.
        /// </summary>
        /// <param name="c0">The first pixel.</param>
        /// <param name="c1">The second pixel.</param>
        /// <param name="a0">The first pixels alpha.</param>
        /// <param name="a1">The second pixels alpha.</param>
        /// <returns>The alpha blened pixel.</returns>
        public static ColorRGB AlphaBlend(ColorRGB c0, ColorRGB c1, float a0, float a1)
        {
            float a = a0 + (1.0f - a1);
            a = MathUtil.Clamp01(a);

            if (a <= 0)
                return ColorRGB.Black;

            float inv_a = 1.0f / a;
            float one_min_a = 1.0f - a0;

            var c = new ColorRGB();
            c.r = ((c0.r * a0) + (c1.r * a1) * one_min_a) * inv_a;
            c.g = ((c0.g * a0) + (c1.g * a1) * one_min_a) * inv_a;
            c.b = ((c0.b * a0) + (c1.b * a1) * one_min_a) * inv_a;

            return c;
        }

        /// <summary>
        /// color from bytes.
        /// The values will be converted from a 0-255 range to a 0-1 range.
        /// </summary>
        /// <returns>A color will values in the 0-1 range.</returns>
        public static ColorRGB FromBytes(int r, int g, int b)
        {
            int R = MathUtil.Clamp(r, 0, 255);
            int G = MathUtil.Clamp(g, 0, 255);
            int B = MathUtil.Clamp(b, 0, 255);
            return new ColorRGB(R, G, B) / 255.0f;
        }


        /// <summary>
        /// Create a color from a integer where each byte in the 
        /// integer represents a channl in the color.
        /// </summary>
        /// <param name="i">The integer.</param>
        /// <param name="bgr">are the channels packed bgr or rgb.</param>
        /// <returns>The color.</returns>
        public static ColorRGB FromInteger(int i, bool bgr = false)
        {
            Union32 u = i;
            if(bgr)
                return FromBytes(u.Byte0, u.Byte1, u.Byte2);
            else
                return FromBytes(u.Byte2, u.Byte1, u.Byte0);
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
            int A = 255;

            if (abgr)
                return (A << 24) | (B << 16) | (G << 8) | R;
            else
                return R | (G << 8) | (B << 16) | (A << 24);
        }

        /// <summary>
        /// Apply the gamma function to the color.
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
        public static float Distance(ColorRGB c0, ColorRGB c1)
        {
            return MathUtil.SafeSqrt(SqrDistance(c0, c1));
        }

        /// <summary>
        /// The square distance between two colors.
        /// </summary>
        public static float SqrDistance(ColorRGB c0, ColorRGB c1)
        {
            return (c0 - c1).SqrMagnitude;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        public static ColorRGB Min(ColorRGB col, float s)
        {
            col.r = Math.Min(col.r, s);
            col.g = Math.Min(col.g, s);
            col.b = Math.Min(col.b, s);
            return col;
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        public static ColorRGB Max(ColorRGB col, float s)
        {
            col.r = Math.Max(col.r, s);
            col.g = Math.Max(col.g, s);
            col.b = Math.Max(col.b, s);
            return col;
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        public static ColorRGB Clamp(ColorRGB col, float min, float max)
        {
            col.r = Math.Max(Math.Min(col.r, max), min);
            col.g = Math.Max(Math.Min(col.g, max), min);
            col.b = Math.Max(Math.Min(col.b, max), min);
            return col;
        }

        /// <summary>
        /// Lerp between two colors.
        /// </summary>
        public static ColorRGB Lerp(ColorRGB c1, ColorRGB c2, float a)
        {
            a = MathUtil.Clamp01(a);
            ColorRGB col = new ColorRGB();
            col.r = MathUtil.Lerp(c1.r, c2.r, a);
            col.g = MathUtil.Lerp(c1.g, c2.g, a);
            col.b = MathUtil.Lerp(c1.b, c2.b, a);
            return col;
        }

        /// <summary>
        /// BLerp between four colors.
        /// </summary>
        public static ColorRGB BLerp(ColorRGB c00, ColorRGB c10, ColorRGB c01, ColorRGB c11, float a0, float a1)
        {
            a0 = MathUtil.Clamp01(a0);
            a1 = MathUtil.Clamp01(a1);
            ColorRGB col = new ColorRGB();
            col.r = MathUtil.BLerp(c00.r, c10.r, c01.r, c11.r, a0, a1);
            col.g = MathUtil.BLerp(c00.g, c10.g, c01.g, c11.g, a0, a1);
            col.b = MathUtil.BLerp(c00.b, c10.b, c01.b, c11.b, a0, a1);

            return col;
        }

        /// <summary>
        /// Convert to HSV color space.
        /// </summary>
        public static ColorHSV ToHSV(float r, float g, float b)
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

        /// <summary>
        /// Create a palette of 6 colors from the rainbow.
        /// </summary>
        /// <returns></returns>
        public static ColorRGB[] RainbowPalatte()
        {
            return new ColorRGB[]
            {
                Red, Orange, Yellow, Green, Blue, Violet
            };
        }

        /// <summary>
        /// Create a palette of colors..
        /// </summary>
        /// <returns></returns>
        public static ColorRGB[] Palette()
        {
            var palette = new ColorRGB[]
            {
                Red,
                Orange,
                Olive,
                VividGreen,
                Yellow,
                Green,
                SpringGreen,
                Cyan,
                Azure,
                Teal,
                Blue,
                Indigo,
                Violet,
                Purple,
                Magenta,
            };

            return palette;
        }

        /// <summary>
        /// Create a custom palette of hues with the same saturation and value.
        /// </summary>
        /// <param name="hues">The number of hues in the palette.</param>
        /// <param name="saturation">The saturation of the colors.</param>
        /// <param name="value">The values of the colors.</param>
        /// <returns></returns>
        public static ColorRGB[] CustomPalette(int hues, float saturation, float value)
        {
            var palette = new ColorRGB[hues];
            for(int i = 0; i < hues; i++)
            {
                float hue = (i + 1.0f) / hues;
                palette[i] = ColorHSV.ToRGB(hue, saturation, value);
            }

            return palette;
        }

    }

}
