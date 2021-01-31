using System;
using System.Windows.Media;

namespace MiscUtils
{
    namespace Grafics
    {
        public class Colors
        {
            /// <summary>
            /// Blends two colors to find a third color somewhere between them
            /// </summary>
            /// <param name="start">interpolate from this color</param>
            /// <param name="stop">interpolate to this color</param>
            /// <param name="amt">amt number between 0 and 1</param>
            /// <returns>interpolated color</returns>
            public static Color LerpColor(Color start, Color stop, double amt)
            {
                double[] fromArray = new double[4];
                double[] toArray = new double[4];

                fromArray[0] = (double)start.A / 255;
                fromArray[1] = (double)start.R / 255;
                fromArray[2] = (double)start.G / 255;
                fromArray[3] = (double)start.B / 255;

                toArray[0] = (double)stop.A / 255;
                toArray[1] = (double)stop.R / 255;
                toArray[2] = (double)stop.G / 255;
                toArray[3] = (double)stop.B / 255;

                // Prevent extrapolation.
                amt = Math.Max(Math.Min(amt, 1), 0);

                // Perform interpolation.
                var l0 = Numerics.Numerics.Lerp(fromArray[0], toArray[0], amt);
                var l1 = Numerics.Numerics.Lerp(fromArray[1], toArray[1], amt);
                var l2 = Numerics.Numerics.Lerp(fromArray[2], toArray[2], amt);
                var l3 = Numerics.Numerics.Lerp(fromArray[3], toArray[3], amt);

                // Scale components.
                l0 *= 255;
                l1 *= 255;
                l2 *= 255;
                l3 *= 255;

                return Color.FromArgb((byte)l0, (byte)l1, (byte)l2, (byte)l3);
            }

            /// <summary>
            /// Структура
            /// </summary>
            public struct ColorRGB
            {
                public byte R;
                public byte G;
                public byte B;

                //public ColorRGB(Color value)
                //{
                //    this.R = value.R;
                //    this.G = value.G;
                //    this.B = value.B;
                //}

                //public static implicit operator Color(ColorRGB rgb)
                //{
                //    Color c = Color.FromRgb(rgb.R, rgb.G, rgb.B);
                //    return c;
                //}

                //public static explicit operator ColorRGB(Color c)
                //{
                //    return new ColorRGB(c);
                //}
            }

            /// <summary>
            /// HSL to RGB
            /// Пример: var hsl = HSL.HSL2RGB(hu/360, 0.5, 0.5);
            /// </summary>
            /// <param name="h">тон (hue). Обязательно делится на 360</param>
            /// <param name="sl">насыщенность (saturation)</param>
            /// <param name="l">светлота (lightness)</param>
            /// <returns></returns>
            public static ColorRGB HSL2RGB(double h, double sl, double l)
            {
                double v;
                double r, g, b;

                r = l;   // default to gray
                g = l;
                b = l;

                v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

                if (v > 0)
                {
                    double m;
                    double sv;
                    int sextant;
                    double fract, vsf, mid1, mid2;

                    m = l + l - v;
                    sv = (v - m) / v;
                    h *= 6.0;
                    sextant = (int)h;
                    fract = h - sextant;
                    vsf = v * sv * fract;
                    mid1 = m + vsf;
                    mid2 = v - vsf;

                    switch (sextant)
                    {
                        case 0:
                            r = v;
                            g = mid1;
                            b = m;
                            break;

                        case 1:
                            r = mid2;
                            g = v;
                            b = m;
                            break;

                        case 2:
                            r = m;
                            g = v;
                            b = mid1;
                            break;

                        case 3:
                            r = m;
                            g = mid2;
                            b = v;
                            break;

                        case 4:
                            r = mid1;
                            g = m;
                            b = v;
                            break;

                        case 5:
                            r = v;
                            g = m;
                            b = mid2;
                            break;
                    }
                }

                ColorRGB rgb;
                rgb.R = (byte)(r * 255);
                rgb.G = (byte)(g * 255);
                rgb.B = (byte)(b * 255);

                return rgb;
            }

        }
    }
}
