using System;
using System.Collections.Generic;

namespace MiscUtils
{
    namespace Numerics
    {
        public class Numerics
        {
            /// <summary>
            /// Constrains a value between a minimum and maximum value
            /// </summary>
            /// <param name="n">number to constrain</param>
            /// <param name="low">low minimum limit</param>
            /// <param name="high">high maximum limit</param>
            /// <returns>constrained number</returns>
            public static double Constrain(double n, double low, double high) => Math.Max(Math.Min(n, high), low);

            /// <summary>
            /// Re-maps a number from one range to another
            /// </summary>
            /// <param name="n">value  the incoming value to be converted</param>
            /// <param name="start1">start1 lower bound of the value's current range</param>
            /// <param name="stop1">stop1  upper bound of the value's current range</param>
            /// <param name="start2">start2 lower bound of the value's target range</param>
            /// <param name="stop2">stop2  upper bound of the value's target range</param>
            /// <param name="withinBounds">[withinBounds] constrain the value to the newly mapped range</param>
            /// <returns></returns>
            public static double Map(double n, double start1, double stop1, double start2, double stop2, bool withinBounds = false)
            {
                var newval = (n - start1) / (stop1 - start1) * (stop2 - start2) + start2;
                if (!withinBounds)
                {
                    return newval;
                }
                if (start2 < stop2)
                {
                    return Constrain(newval, start2, stop2);
                }
                else
                {
                    return Constrain(newval, stop2, start2);
                }
            }

            /// <summary>
            /// Аналог функции SWAP в C++, замена аргументов местами
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            public static void Swap<T>(ref T v1, ref T v2) { T v3 = v1; v1 = v2; v2 = v3; }

            /// <summary>
            /// Замена аргументов местами
            /// </summary>
            /// <param name="matrixA">массив 1</param>
            /// <param name="matrixB">массив 2</param>
            public static void Swap<T>(ref T[,] matrixA, ref T[,] matrixB)
            {
                int aRows = matrixA.GetLength(0); int aCols = matrixA.GetLength(1);
                int bRows = matrixB.GetLength(0); int bCols = matrixB.GetLength(1);
                if (aCols != bCols || aRows != bRows)
                    throw new Exception("Размерность матриц не одинакова");

                T[,] temp = Create<T>(aRows, bCols);

                for (int i = 0; i < aRows; ++i)
                    for (int j = 0; j < aCols; ++j)
                    {
                        temp[i, j] = matrixA[i, j];
                        matrixA[i, j] = matrixB[i, j];
                        matrixB[i, j] = temp[i, j];
                    }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="matrixA"></param>
            /// <param name="matrixB"></param>
            public static void Swap<T>(ref T[] matrixA, ref T[] matrixB)
            {
                int nA = matrixA.GetLength(0);
                int nB = matrixA.GetLength(0);

                if (nA != nB)
                    throw new Exception("Размерность матриц не одинакова");

                T[] temp = Create<T>(nA);

                for (int i = 0; i < nA; ++i)
                {
                    temp[i] = matrixA[i];
                    matrixA[i] = matrixB[i];
                    matrixB[i] = temp[i];
                }
            }

            private static T[,] Create<T>(int rows, int cols) => new T[rows, cols];

            private static T[] Create<T>(int n) => new T[n];

            //static Random rnd = new Random();
            //public static T Random<T>(T val)
            //{
            //    rnd.Next(val);
            //    return val;
            //}

            /// <summary>
            /// Линейная интерполяция
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="t"></param>
            /// <returns></returns>
            public static double Lerp(double a, double b, double t)
            {
                // return a * (t - 1) + b * t; можно переписать с одним умножением (раскрыть скобки, взять в другие скобки):
                return a + (b - a) * t;
            }

            public static double Radians(double degree) => degree * Math.PI / 180;
            public static double Degrees(double radians) => radians * 180 / Math.PI;

            public static double Noise(double x, double y = 0) => new Perlin2D().Noise(x * 0.01, y * 0.01, 4);
            public static double Noise(double x, double y = 0, double z = 0) => new Perlin3D().noise(x, y, z);
        }

        /// <summary>
        /// /
        /// </summary>
        class Perlin2D
        {
            byte[] permutationTable;

            public Perlin2D(int seed = 0)
            {
                var rand = new System.Random(seed);
                permutationTable = new byte[1024];
                rand.NextBytes(permutationTable);
            }

            // Для интерполяции: Уравнение пятой степени
            static double QunticCurve(double t) => t* t * t* (t* (t* 6 - 15) + 10);

            /// <summary>
            /// хэш-функция с Простыми числами, обрезкой результата до размера массива со случайными байтами
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            double[] GetPseudoRandomGradientVector(int x, int y)
            {
                // v - это псевдо-случайное число от 0 до 3 которое всегда неизменно при данных x и y
                int v = (int)(((x * 1836311903) ^ (y * 2971215073) + 4807526976) & 1023);
                v = permutationTable[v] & 3; // & 3 здесь обрезает любое int32 число до 3

                switch (v)
                {
                    case 0: return new double[] { 1, 0 };
                    case 1: return new double[] { -1, 0 };
                    case 2: return new double[] { 0, 1 };
                    default: return new double[] { 0, -1 };
                }
            }

            /// <summary>
            /// Скалярное произведение векторов
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            static double Dot(double[] a, double[] b) => a[0] * b[0] + a[1] * b[1];

            /// <summary>
            /// Главный метод
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public double Noise(double fx, double fy)
            {
                // сразу находим координаты левой верхней вершины квадрата
                int left = (int)System.Math.Floor(fx);
                int top = (int)System.Math.Floor(fy);

                // а теперь локальные координаты точки внутри квадрата
                double pointInQuadX = fx - left;
                double pointInQuadY = fy - top;

                // извлекаем градиентные векторы для всех вершин квадрата:
                double[] topLeftGradient = GetPseudoRandomGradientVector(left, top);
                double[] topRightGradient = GetPseudoRandomGradientVector(left + 1, top);
                double[] bottomLeftGradient = GetPseudoRandomGradientVector(left, top + 1);
                double[] bottomRightGradient = GetPseudoRandomGradientVector(left + 1, top + 1);

                // вектора от вершин квадрата до точки внутри квадрата:
                double[] distanceToTopLeft = new double[] { pointInQuadX, pointInQuadY };
                double[] distanceToTopRight = new double[] { pointInQuadX - 1, pointInQuadY };
                double[] distanceToBottomLeft = new double[] { pointInQuadX, pointInQuadY - 1 };
                double[] distanceToBottomRight = new double[] { pointInQuadX - 1, pointInQuadY - 1 };

                // считаем скалярные произведения между которыми будем интерполировать
                /*
                 tx1--tx2
                  |    |
                 bx1--bx2
                */
                double tx1 = Dot(distanceToTopLeft, topLeftGradient);
                double tx2 = Dot(distanceToTopRight, topRightGradient);
                double bx1 = Dot(distanceToBottomLeft, bottomLeftGradient);
                double bx2 = Dot(distanceToBottomRight, bottomRightGradient);

                // готовим параметры интерполяции, чтобы она не была линейной:
                pointInQuadX = QunticCurve(pointInQuadX);
                pointInQuadY = QunticCurve(pointInQuadY);

                // собственно, интерполяция:
                double tx = Numerics.Lerp(tx1, tx2, pointInQuadX);
                double bx = Numerics.Lerp(bx1, bx2, pointInQuadX);
                double tb = Numerics.Lerp(tx, bx, pointInQuadY);

                // возвращаем результат:
                return tb;  // Возвращает число от -1.0 до 1.0
            }
            public double Noise(double fx, double fy, int octaves, double persistence = 0.5)
            {
                double amplitude = 1;
                double max = 0;
                double result = 0;

                while (octaves-- > 0)
                {
                    max += amplitude;
                    result += Noise(fx, fy) * amplitude;
                    amplitude *= persistence;
                    fx *= 2;
                    fy *= 2;
                }

                return result / max;
            }
        }

        class Perlin3D
        {
            // http://webstaff.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf

            private static int[][] grad3 = {
                                            new int[] {1,1,0},
                                            new int[] {-1,1,0},
                                            new int[] {1,-1,0},
                                            new int[] {-1,-1,0},
                                            new int[] {1,0,1},
                                            new int[] {-1,0,1},
                                            new int[] {1,0,-1},
                                            new int[] {-1,0,-1},
                                            new int[] {0,1,1},
                                            new int[] {0,-1,1},
                                            new int[] {0,1,-1},
                                            new int[] {0,-1,-1}
                                        };

            private static int[] p = {151,160,137,91,90,15,
                                 131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
                                 190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
                                 88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
                                 77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
                                 102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
                                 135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
                                 5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
                                 223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
                                 129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
                                 251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
                                 49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
                                 138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180};

            // To remove the need for index wrapping, double the permutation table length
            private static int[] perm = new int[512];


            public Perlin3D()
            {
                for (int i = 0; i < 512; i++) perm[i] = p[i & 255];
            }

            // This method is a *lot* faster than using (int)Math.floor(x)
            private static int fastfloor(double x)
            {
                return x > 0 ? (int)x : (int)x - 1;
            }
            private static double dot(int[] g, double x, double y, double z)
            {
                return g[0] * x + g[1] * y + g[2] * z;
            }
            private static double mix(double a, double b, double t)
            {
                return (1 - t) * a + t * b;
            }
            private static double fade(double t)
            {
                return t * t * t * (t * (t * 6 - 15) + 10);
            }

            // Classic Perlin noise, 3D version
            public double noise(double x, double y, double z)
            {
                // Find unit grid cell containing point
                int X = fastfloor(x);
                int Y = fastfloor(y);
                int Z = fastfloor(z);

                // Get relative xyz coordinates of point within that cell
                x = x - X;
                y = y - Y;
                z = z - Z;

                // Wrap the integer cells at 255 (smaller integer period can be introduced here)
                X = X & 255;
                Y = Y & 255;
                Z = Z & 255;

                // Calculate a set of eight hashed gradient indices
                int gi000 = perm[X + perm[Y + perm[Z]]] % 12;
                int gi001 = perm[X + perm[Y + perm[Z + 1]]] % 12;
                int gi010 = perm[X + perm[Y + 1 + perm[Z]]] % 12;
                int gi011 = perm[X + perm[Y + 1 + perm[Z + 1]]] % 12;
                int gi100 = perm[X + 1 + perm[Y + perm[Z]]] % 12;
                int gi101 = perm[X + 1 + perm[Y + perm[Z + 1]]] % 12;
                int gi110 = perm[X + 1 + perm[Y + 1 + perm[Z]]] % 12;
                int gi111 = perm[X + 1 + perm[Y + 1 + perm[Z + 1]]] % 12;

                // The gradients of each corner are now:
                // g000 = grad3[gi000];
                // g001 = grad3[gi001];
                // g010 = grad3[gi010];
                // g011 = grad3[gi011];
                // g100 = grad3[gi100];
                // g101 = grad3[gi101];
                // g110 = grad3[gi110];
                // g111 = grad3[gi111];
                // Calculate noise contributions from each of the eight corners
                double n000 = dot(grad3[gi000], x, y, z);
                double n100 = dot(grad3[gi100], x - 1, y, z);
                double n010 = dot(grad3[gi010], x, y - 1, z);
                double n110 = dot(grad3[gi110], x - 1, y - 1, z);
                double n001 = dot(grad3[gi001], x, y, z - 1);
                double n101 = dot(grad3[gi101], x - 1, y, z - 1);
                double n011 = dot(grad3[gi011], x, y - 1, z - 1);
                double n111 = dot(grad3[gi111], x - 1, y - 1, z - 1);
                // Compute the fade curve value for each of x, y, z
                double u = fade(x);
                double v = fade(y);
                double w = fade(z);
                // Interpolate along x the contributions from each of the corners
                double nx00 = mix(n000, n100, u);
                double nx01 = mix(n001, n101, u);
                double nx10 = mix(n010, n110, u);
                double nx11 = mix(n011, n111, u);
                // Interpolate the four results along y
                double nxy0 = mix(nx00, nx10, v);
                double nxy1 = mix(nx01, nx11, v);
                // Interpolate the two last results along z
                double nxyz = mix(nxy0, nxy1, w);

                return nxyz;
            }
        }

        /// <summary>
        /// Методы расширения для Random
        /// </summary>
        public static class RandomExtensionMethods
        {
            /// <summary>
            /// Случайный выбор числа в диапазоне (мин, макс)
            /// </summary>
            /// <param name="random"></param>
            /// <param name="minNumber">Минимальная граница</param>
            /// <param name="maxNumber">Максимальная граница</param>
            /// <returns></returns>
            public static double NextDoubleRange(this System.Random random, double minNumber, double maxNumber)
            {
                return random.NextDouble() * (maxNumber - minNumber) + minNumber;
            }

            /// <summary>
            /// Случайный выбор числа в диапазоне (мин, макс)
            /// </summary>
            /// <param name="random"></param>
            /// <param name="minNumber">Минимальная граница</param>
            /// <param name="maxNumber">Максимальная граница</param>
            /// <returns></returns>
            public static float NextFloatRange(this System.Random random, float minNumber, float maxNumber)
            {
                return (float)random.NextDouble() * (maxNumber - minNumber) + minNumber;
            }

            /// <summary>
            /// Выбор случайного элемента из массива
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="random"></param>
            /// <param name="array"></param>
            /// <returns></returns>
            public static T NextArray<T>(this System.Random random, T[] array)
            {
                return array[random.Next(array.Length)];
            }

            /// <summary>
            /// Returns a random number fitting a Gaussian, or normal, distribution. Преобразование Бокса — Мюллера. Вариант 1
            /// </summary>
            /// <param name="random"></param>
            /// <param name="mean"></param>
            /// <param name="sd"></param>
            /// <returns></returns>
            public static double NextGaussian1(this System.Random random, double mean = 0, double sd = 1)
            {
                if (sd <= 0)
                    throw new ArgumentOutOfRangeException("sd", "Must be greater than zero.");

                // Преобразование Бокса — Мюллера
                // Вариант 1.
                // https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D0%B5%D0%BE%D0%B1%D1%80%D0%B0%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5_%D0%91%D0%BE%D0%BA%D1%81%D0%B0_%E2%80%94_%D0%9C%D1%8E%D0%BB%D0%BB%D0%B5%D1%80%D0%B0

                double u1 = 1 - random.NextDouble(); //uniform(0,1] random doubles
                double u2 = 1 - random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                             Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                double randNormal =
                             mean + sd * randStdNormal; //random normal(mean,stdDev^2)

                return randNormal;
            }

            static bool previous;
            static double y2;

            /// <summary>
            /// Returns a random number fitting a Gaussian, or normal, distribution. Преобразование Бокса — Мюллера. Вариант 2
            /// </summary>
            /// <param name="mean">the mean</param>
            /// <param name="sd">the standard deviation</param>
            /// <returns>the random number</returns>
            public static double NextGaussian2(this System.Random random, double mean = 0, double sd = 1)
            {
                if (sd <= 0)
                    throw new ArgumentOutOfRangeException("sd", "Must be greater than zero.");

                // Взято из библиотеки p5j
                /*
                 * If no args, returns a mean of 0 and standard deviation of 1.
                    If one arg, that arg is the mean (standard deviation is 1).
                    If two args, first is mean, second is standard deviation.
                */

                double y1, x1, x2, w;
                if (previous)
                {
                    y1 = y2;
                    previous = false;
                }
                else
                {
                    do
                    {
                        x1 = random.NextDouble() * 2 - 1;
                        x2 = random.NextDouble() * 2 - 1;
                        w = x1 * x1 + x2 * x2;
                    } while (w >= 1);
                    w = Math.Sqrt(-2 * Math.Log(w) / w);
                    y1 = x1 * w;
                    y2 = x2 * w;
                    previous = true;
                }
                return y1 * sd + mean;
            }

        }

        /// <summary>
        /// Метод расширения Т[] для преобразования в List<T>
        /// </summary>
        public static class ArrayExtensionMethods
        {
            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="array"></param>
            /// <returns></returns>
            public static List<T> ToList<T>(this T[] array)
            {
                List<T> list = new List<T>();
                foreach (var a in array)
                {
                    list.Add(a);
                }
                return list;
            }
        }
    }
}
