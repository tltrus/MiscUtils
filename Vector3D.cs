using System;

namespace MiscUtils
{
    namespace Vectors
    {
        public class Vector3D
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }


            public Vector3D(double x = 0, double y = 0, double z = 0)
            {
                X = x;
                Y = y;
                Z = z;
            }

            /*
             *  КОПИРОВАНИЕ, ОПРЕДЕЛЕНИЕ, ТЕКСТОВЫЙ ВЫВОД
             */
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Vector3D CopyToVector() => new Vector3D(X, Y, Z);

            public double[] CopyToArray() => new double[3] { X, Y, Z };

            /// <summary>
            /// Присвоение скалярных значений вектору
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="z"></param>
            public void Set(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            /// <summary>
            /// Текстовое представление вектора 
            /// </summary>
            /// <returns>Строка вида "[ x, y, z ]"</returns>
            public override string ToString() => $"Vector3D Object: [ {X}, {Y}, {Z} ]";

            /*
             *  СЛОЖЕНИЕ
             */
            /// <summary>
            /// Сложение вектора со скалярами
            /// </summary>
            /// <param name="vector"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Add(double x, double y, double z) => new Vector3D(X + x, Y + y, Z + z);

            /// <summary>
            /// Сложение двух векторов
            /// </summary>
            /// <param name="v"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Add(Vector3D v) => new Vector3D(X + v.X, Y + v.Y, Z + v.Z);

            /// <summary>
            /// Сложение двух векторов
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <returns>Возвращает новый вектор</returns>
            public static Vector3D Add(Vector3D v1, Vector3D v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

            /*
             *  ВЫЧИТАНИЕ
             */
            /// <summary>
            /// Вычитание из вектора скаляров
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Sub(double x, double y, double z) => new Vector3D(X - x, Y - y, Z - z);

            /// <summary>
            /// Вычитание из вектора другого вектора
            /// </summary>
            /// <param name="v"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Sub(Vector3D v) => new Vector3D(X - v.X, Y - v.Y, Z - v.Z);

            /// <summary>
            /// Вычитание двух векторов
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <returns>Возвращает новый вектор</returns>
            public static Vector3D Sub(Vector3D v1, Vector3D v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

            /*
             *  ДЕЛЕНИЕ
             */
            /// <summary>
            /// Деление вектора на скаляр
            /// </summary>
            /// <param name="n"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Div(double n) => new Vector3D(X / n, Y / n);

            /// <summary>
            /// Деление вектора на другой вектор
            /// </summary>
            /// <param name="v"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Div(Vector3D v) => new Vector3D(X / v.X, Y / v.Y, Z / v.Z);

            /// <summary>
            /// Divide (деление) двух векторов
            /// </summary>
            /// <param name="val"></param>
            /// <returns>Возвращает новый вектор</returns>
            public static Vector3D Div(Vector3D v1, Vector3D v2) => new Vector3D(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);

            /// <summary>
            /// Деление вектора на скаляр
            /// </summary>
            /// <param name="v">Вектор</param>
            /// <param name="n">Скаляр</param>
            /// <returns>Возвращает новый вектор</returns>
            public static Vector3D Div(Vector3D v, double n) => new Vector3D(v.X / n, v.Y / n, v.Z / n);

            /*
             *  УМНОЖЕНИЕ НА СКАЛЯР
             */
            /// <summary>
            /// Multiply (умножение) вектора на число
            /// </summary>
            /// <param name="n"></param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Mult(double n) => new Vector3D(X * n, Y * n, Z * n);

            /// <summary>
            /// Multiply (умножение) вектора на число
            /// </summary>
            /// <param name="v"></param>
            /// <param name="n"></param>
            /// <returns>Возвращает новый вектор</returns>
            public static Vector3D Mult(Vector3D v, double n) => new Vector3D(v.X * n, v.Y * n, v.Z * n);

            /*
             *  СКАЛЯРНОЕ (Dot) УМНОЖЕНИЕ ВЕКТОРОВ
             */
            /// <summary>
            /// Скалярное (Dot) умножение векторов
            /// </summary>
            /// <param name="v"></param>
            /// <returns>Скаляр</returns>
            public double Dot(Vector3D v) => X * v.X + Y * v.Y + Z * v.Z;
            /// <summary>
            /// Скалярное произведение векторов
            /// </summary>
            /// <param name="v1">Вектор 1</param>
            /// <param name="v2">Вектор 2</param>
            /// <returns>Скаляр</returns>
            public static double Dot(Vector3D v1, Vector3D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            
            /*
             *  ВЕКТОРНОЕ (Cross) УМНОЖЕНИЕ ВЕКТОРОВ
             */
            /// <summary>
            /// Векторное произведение двух векторов
            /// </summary>
            /// <param name="v1">Вектор 1</param>
            /// <param name="v2">Вектор 2</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D Cross(Vector3D v1, Vector3D v2)
            {
                return new Vector3D(
                    v1.Y * v2.Z - v1.Z * v2.Y,
                    v1.Z * v2.X - v1.X * v2.Z,
                    v1.X * v2.Y - v1.Y * v2.X
                );
            }

            /*
             * ИНТЕРПОЛЯЦИЯ
             */
            /// <summary>
            /// Интерполяция вектора к другому вектору
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="amt">Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new vector. 0.5 is halfway in between</param>
            /// <returns>Возвращает новый вектор и изменяет текущий</returns>
            public Vector3D Lerp(double x, double y, double z, double amt)
            {
                return new Vector3D(
                    X + (x - X) * amt,
                    Y + (y - Y) * amt,
                    Z + (z - Z) * amt
                );
            }
            /// <summary>
            /// Интерполяция вектора к другому вектору
            /// </summary>
            /// <param name="v"></param>
            /// <param name="amt">Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new vector. 0.5 is halfway in between</param>
            /// <returns></returns>
            public Vector3D Lerp(Vector3D v, double amt)
            {
                return new Vector3D(
                    X + (v.X - X) * amt,
                    Y + (v.Y - Y) * amt,
                    Z + (v.Z - Z) * amt
                );
            }
            /// <summary>
            /// Интерполяция вектора к другому вектору
            /// </summary>
            /// <param name="v1"></param>
            /// <param name="v2"></param>
            /// <param name="amt">Value between 0.0 (old vector) and 1.0 (new vector). 0.9 is very near the new vector. 0.5 is halfway in between</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D Lerp(Vector3D v1, Vector3D v2, double amt)
            {
                Vector3D tmp = new Vector3D();
                tmp = v1.CopyToVector();
                return tmp.Lerp(v2, amt);
            }

            /*
             * ПОЛУЧЕНИЕ УГЛА МЕЖДУ ДВУХ ВЕКТОРОВ
             */
            /// <summary>
            /// Вычисление угла между двумя векторами
            /// </summary>
            /// <param name="v"></param>
            /// <returns>Возвращает угол в радианах</returns>
            public double angleBetween(Vector3D v)
            {
                // В основе вычислений формула: a * b = |a| * |b| * cos θ, где
                // a * b - скалярное (dot) произведение
                // |a| и |b| - длины (mag) векторов

                var dotmagmag = Dot(v) / (Mag() * v.Mag());

                // Из содержания библиотеки p5j:
                // Mathematically speaking: the dotmagmag variable will be between -1 and 1
                // inclusive. Practically though it could be slightly outside this range due
                // to floating-point rounding issues. This can make Math.acos return NaN.
                //
                // Solution: we'll clamp the value to the -1,1 range
                var angle = Math.Acos(Math.Min(1, Math.Max(-1, dotmagmag)));
                return angle;
            }

            /*
             *
             */
            /// <summary>
            /// Получение квадрата длины вектора
            /// </summary>
            /// <returns>Скаляр</returns>
            public double MagSq() => X * X + Y * Y + Z * Z;

            /// <summary>
            /// Ограничение (Limit) длины вектора до max значения
            /// </summary>
            /// <param name="max">Требуемая максимальная длина вектора</param>
            /// <returns>Вектор с лимитированной длиной</returns>
            public Vector3D Limit(double max)
            {
                var mSq = MagSq(); // получение квадрата длины текущего вектора
                if (mSq > max * max)
                {
                    Div(Math.Sqrt(mSq)) //normalize it
                      .Mult(max); // домнажаем на максимальную длину
                }
                return CopyToVector();
            }
            /// <summary>
            /// Вычисление длины вектора
            /// </summary>
            /// <returns>Скаляр</returns>
            public double Mag() => Math.Sqrt(MagSq());
            /// <summary>
            /// Нормализация вектора
            /// </summary>
            /// <returns>Вектор единичной длины</returns>
            public Vector3D Normalize()
            {
                var len = Mag();
                if (len != 0) Mult(1 / len);
                return CopyToVector();
            }
            /// <summary>
            /// Задание длины вектора
            /// </summary>
            /// <param name="n">целочисленная длина</param>
            /// <returns></returns>
            public void SetMag(int n) => Normalize().Mult(n);

            /// <summary>
            /// Задание длины вектора
            /// </summary>
            /// <param name="n">вещественная длина</param>
            /// <returns></returns>
            public void SetMag(double n) => Normalize().Mult(n);

            /*
             * СОЗДАНИЕ ВЕКТОРА
             */
            /// <summary>
            /// Создание 3D вектора по паре углов
            /// </summary>
            /// <param name="theta">the polar angle, in radians</param>
            /// <param name="phi">the azimuthal angle, in radians</param>
            /// <param name="len">Длина вектора. По умолчанию длина = 1</param>
            /// <returns>Возвращает новый вектор</returns>
            public static Vector3D FromAngles(double theta, double phi, double len = 1)
            {
                var cosPhi = Math.Cos(phi);
                var sinPhi = Math.Sin(phi);
                var cosTheta = Math.Cos(theta);
                var sinTheta = Math.Sin(theta);

                return new Vector3D(
                  len * sinTheta * sinPhi,
                  -len * cosTheta,
                  len * sinTheta * cosPhi
                );
            }
            /// <summary>
            /// Создание единичного 3D вектора по случайному углу 2PI
            /// </summary>
            /// <param name="rnd"></param>
            /// <returns></returns>
            public static Vector3D Random3D(Random rnd)
            {
                var angle = rnd.NextDouble() * Math.PI * 2;
                var vz = rnd.NextDouble() * 2 - 1;
                var vzBase = Math.Sqrt(1 - vz * vz);
                var vx = vzBase * Math.Cos(angle);
                var vy = vzBase * Math.Sin(angle);
                return new Vector3D(vx, vy, vz);
            }

            /// <summary>
            /// Вычисление расстояния между векторами
            /// </summary>
            /// <param name="v"></param>
            /// <returns>Cкаляр</returns>
            public double Dist(Vector3D v) => Sub(this, v).Mag();
            /// <summary>
            /// Вычисление расстояния между двумя векторами
            /// </summary>
            /// <param name="v1">Вектор 1</param>
            /// <param name="v2">Вектор 2</param>
            /// <returns>Cкаляр</returns>
            public static double Dist(Vector3D v1, Vector3D v2) => v1.Dist(v2);


            /*
             * ПЕРЕГРУЗКА ОПЕРАТОРОВ
             */

            /// <summary>
            /// Adds two vectors together
            /// </summary>
            /// <param name="left">Вектор 1</param>
            /// <param name="right">Вектор 2</param>
            /// <returns>Новый суммированный вектор</returns>
            public static Vector3D operator +(Vector3D left, Vector3D right) => Add(left, right);

            /// <summary>
            /// Изменение вектора на негативный
            /// </summary>
            /// <param name="v">Вектор</param>
            /// <returns>Новый отрицательный вектор</returns>
            public static Vector3D operator -(Vector3D v) => v.Mult(-1);

            /// <summary>
            /// Разность векторов
            /// </summary>
            /// <param name="left">Вектор 1</param>
            /// <param name="right">Вектор 2</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D operator -(Vector3D left, Vector3D right) => Sub(left, right);

            /// <summary>
            /// Умножение скаляра на вектор
            /// </summary>
            /// <param name="left">Скаляр</param>
            /// <param name="right">Вектор</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D operator *(float left, Vector3D right) => Mult(right, left);

            /// <summary>
            /// Умножение пар элементов обоих векторов
            /// </summary>
            /// <param name="left">Вектор 1</param>
            /// <param name="right">Вектор 2</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D operator *(Vector3D left, Vector3D right) => new Vector3D(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

            /// <summary>
            /// Умножение вектора на скаляр
            /// </summary>
            /// <param name="left">Вектор</param>
            /// <param name="right">Скаляр</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D operator *(Vector3D left, float right) => Mult(left, right);

            /// <summary>
            /// Деление первого вектора на второй вектор
            /// </summary>
            /// <param name="left">Вектор 1</param>
            /// <param name="right">Вектор 2</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D operator /(Vector3D left, Vector3D right) => Div(left, right);

            /// <summary>
            /// Деление вектора на скаляр
            /// </summary>
            /// <param name="value1">Вектор</param>
            /// <param name="value2">Скаляр</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D operator /(Vector3D value1, float value2) => Div(value1, value2);

            /// <summary>
            /// Returns a value that indicates whether each pair of elements in two specified vectors is equal.
            /// </summary>
            /// <param name="left">Вектор 1</param>
            /// <param name="right">Вектор 2</param>
            /// <returns>true if left and right are equal; otherwise, false.</returns>
            public static bool operator ==(Vector3D left, Vector3D right)
            {
                if (left.X == right.X && left.Y == right.Y && left.Z == right.Z)
                    return true;
                else
                    return false;
            }

            /// <summary>
            /// Returns a value that indicates whether two specified vectors are not equal.
            /// </summary>
            /// <param name="left">Вектор 1</param>
            /// <param name="right">Вектор 2</param>
            /// <returns>true if left and right are not equal; otherwise, false.</returns>
            public static bool operator !=(Vector3D left, Vector3D right)
            {
                if (left.X != right.X || left.Y != right.Y || left.Z == right.Z)
                    return true;
                else
                    return false;
            }
        }
    }
}
