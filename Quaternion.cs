using System;
using MiscUtils.Vectors;


namespace MiscUtils
{
    namespace Quaternion
    {
        public class Quaternion
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public double W { get; set; }

            /// <summary>
            /// Конструктор
            /// </summary>
            public Quaternion(double w = 0, double x = 0, double y = 0, double z = 0)
            {
                W = w;
                X = x;
                Y = y;
                Z = z;
            }

            public static double ToRadians(double angle)
            {
                return angle * Math.PI / 180;
            }

            Quaternion Copy()
            {
                Quaternion q = new Quaternion();
                q.X = this.X;
                q.Y = this.Y;
                q.Z = this.Z;
                q.W = this.W;

                return q;
            }
            public override string ToString() => $"Кватернион [w, x, y, z]: [{W}, {X}, {Y}, {Z}]";

            /// <summary>
            /// Магнитуда (длина) кватерниона
            /// </summary>
            /// <returns>Возвращает скаляр длины</returns>
            double Mag() => Math.Sqrt(X * X + Y * Y + Z * Z + W * W);

            /// <summary>
            /// Нормализация текущего кватерниона
            /// </summary>
            /// <returns>Возвращает новый кватернион и изменяет текущий</returns>
            Quaternion Normalize()
            {
                double magnitude = this.Mag();

                this.X /= magnitude;
                this.Y /= magnitude;
                this.Z /= magnitude;
                this.W /= magnitude;

                return this.Copy();
            }
            /// <summary>
            /// Нормализация кватерниона
            /// </summary>
            /// <param name="quat"></param>
            /// <returns>Возвращает новый кватернион</returns>
            public static Quaternion Normalize(Quaternion quat) =>  quat.Normalize().Copy();

            /// <summary>
            /// Проверка на нормализацию кватерниона
            /// </summary>
            /// <returns>True - нормализован, False - не нормализован</returns>
            bool isNormalized()
            {
                if (this.Mag() == 1)
                    return true;
                return false;
            }

            /// <summary>
            /// Инвертирование текущего кватерниона
            /// </summary>
            /// <returns>Возвращает новый кватернион и изменяет текущий</returns>
            Quaternion Invert()
            {
                this.Normalize();

                this.X *= -1;
                this.Y *= -1;
                this.Z *= -1;

                return this.Copy();
            }



            /// <summary>
            /// Конвертация кватерниона в матрицу
            /// </summary>
            /// <returns>Двумерный массив вида [,]</returns>
            public double[,] ToMatrix()
            {
                if (!this.isNormalized())
                    return this.Normalize().ToMatrix();

                double xx = X * X;
                double xy = X * Y;
                double xz = X * Z;
                double xw = X * W;

                double yy = Y * Y;
                double yz = Y * Z;
                double yw = Y * W;

                double zz = Z * Z;
                double zw = Z * W;

                return new double[,]
                {
                { 1 - 2 * (yy + zz), 2 * (xy - zw), 2 * (xz + yw), 0 },
                { 2 * (xy + zw), 1 - 2 * (xx + zz), 2 * (yz - xw), 0 },
                { 2 * (xz - yw), 2 * (yz + xw), 1 - 2 * (xx + yy), 0 },
                { 0, 0, 0, 1 }
                };
            }

            /// <summary>
            /// Конвертация матрицы в кватернион
            /// </summary>
            /// <param name="m">Двумерный массив</param>
            /// <returns>Новый кватернион</returns>
            public Quaternion MatrixToQuaternion(double[,] m)
            {
                // Матрица не должна содержать в себе масштабирование
                double tr, s;
                double[] q = new double[4];
                int i, j, k;

                int[] nxt = new int[3] { 1, 2, 0 };

                tr = m[0, 0] + m[1, 1] + m[2, 2];

                if (tr > 0.0)
                {
                    s = Math.Sqrt(tr + 1.0);
                    this.W = s / 2.0;
                    s = 0.5 / s;
                    this.X = (m[1, 2] - m[2, 1]) * s;
                    this.Y = (m[2, 0] - m[0, 2]) * s;
                    this.Z = (m[0, 1] - m[1, 0]) * s;
                }
                else
                {
                    i = 0;
                    if (m[1, 1] > m[0, 0]) i = 1;
                    if (m[2, 2] > m[i, i]) i = 2;
                    j = nxt[i];
                    k = nxt[j];

                    s = Math.Sqrt((m[i, i] - (m[j, j] + m[k, k])) + 1.0);

                    q[i] = s * 0.5;

                    if (s != 0.0) s = 0.5 / s;

                    q[3] = (m[j, k] - m[k, j]) * s;
                    q[j] = (m[i, j] + m[j, i]) * s;
                    q[k] = (m[i, k] + m[k, i]) * s;

                    this.X = q[0];
                    this.Y = q[1];
                    this.Z = q[2];
                    this.W = q[3];
                }

                return this.Copy();
            }


            /// <summary>
            /// Умножение двух кватернионов
            /// </summary>
            /// <param name="q1">Кватернион 1</param>
            /// <param name="q2">Кватернион 2</param>
            /// <returns>Новый кватернион</returns>
            public static Quaternion Multiply(Quaternion q1, Quaternion q2)
            {
                // qq' = [ vxv' + wv' + w'v, ww' – v•v' ]

                Vector3D v1 = new Vector3D() { X = q1.X, Y = q1.Y, Z = q1.Z };
                Vector3D v2 = new Vector3D() { X = q2.X, Y = q2.Y, Z = q2.Z };
                Quaternion result = new Quaternion();
                double angle;

                // Вычисляем ww' – v•v'
                angle = q1.W * q2.W - Vector3D.Dot(v1, v2);

                Vector3D cross = Vector3D.Cross(v1, v2); // vxv'
                v2 = Vector3D.Mult(v2, q1.W); // wv'
                v1 = Vector3D.Mult(v1, q2.W);  // w'v

                // Вычисляем vxv' + wv' + w'v
                result.X = v1.X + v2.X + cross.X;
                result.Y = v1.Y + v2.Y + cross.Y;
                result.Z = v1.Z + v2.Z + cross.Z;

                result.W = angle;

                return result;
            }
            /// <summary>
            /// Умножение кватерниона на вектор. Позволяет повернуть вектор на вращение заданное, кватернионом
            /// </summary>
            /// <param name="q">Кватернион</param>
            /// <param name="v">Вектор</param>
            /// <returns>Новый вектор</returns>
            public static Vector3D Multiply(Quaternion q, Vector3D v) => q.Multiply(v);
            public Vector3D Multiply(Vector3D v)
            {
                // V' = q V q^–1,
                // где V = [o,v], V'=[0,v']
                Quaternion V = new Quaternion(0, v.X, v.Y, v.Z);
                Quaternion inverseQ = new Quaternion(W, X, Y, Z);
                Quaternion resultQ;

                inverseQ.Invert(); // q^–1
                resultQ = Multiply(V, inverseQ); // V q^–1
                resultQ = Multiply(this, resultQ); // q V q^–1

                return new Vector3D() { X = resultQ.X, Y = resultQ.Y, Z = resultQ.Z };
            }
            public Quaternion Multiply(double n)
            {
                W *= n;
                X *= n;
                Y *= n;
                Z *= n;

                return this.Copy();
            }
            public static Quaternion Multiply(Quaternion q, double n) => q.Multiply(n).Copy();


            // Быстрое умножение кватернионов
            public static Quaternion MultiplyQuick(Quaternion q1, Quaternion q2)
            {
                double A, B, C, D, E, F, G, H;
                Quaternion q = new Quaternion();

                A = (q1.W + q1.X) * (q2.W + q2.X);
                B = (q1.Z - q1.Y) * (q2.Y - q2.Z);
                C = (q1.X - q1.W) * (q2.Y + q2.Z);
                D = (q1.Y + q1.Z) * (q2.X - q2.W);
                E = (q1.X + q1.Z) * (q2.X + q2.Y);
                F = (q1.X - q1.Z) * (q2.X - q2.Y);
                G = (q1.W + q1.Y) * (q2.W - q2.Z);
                H = (q1.W - q1.Y) * (q2.W + q2.Z);

                q.W = B + (-E - F + G + H) * 0.5;
                q.X = A - (E + F + G + H) * 0.5;
                q.Y = -C + (E - F + G - H) * 0.5;
                q.Z = -D + (E - F - G + H) * 0.5;

                return q;
            }


            /// <summary>
            /// Получение кватерниона из углов Эйлера для одной оси
            /// </summary>
            /// <param name="v">Вектор, вокруг которого происходит вращение</param>
            /// <param name="angle">Угол</param>
            /// <returns></returns>
            public static Quaternion FromEuler(Vector3D v, double angle)
            {
                Quaternion q = new Quaternion();
                double sinAngle;

                angle *= 0.5;
                v.Normalize();

                sinAngle = Math.Sin(angle);
                q.X = v.X * sinAngle;
                q.Y = v.Y * sinAngle;
                q.Z = v.Z * sinAngle;
                q.W = Math.Cos(angle);
                return q;
            }

            /// <summary>
            /// Получение кватерниона из углов Эйлера для трех осей
            /// </summary>
            /// <param name="roll">X угол в рад.</param>
            /// <param name="pitch">Y угол в рад.</param>
            /// <param name="yaw">Z угол в рад.</param>
            /// <returns>Новый кватернион</returns>
            public static Quaternion FromEulerFull(double roll, double pitch, double yaw)
            {
                Quaternion q, qx, qy, qz;
                Vector3D vx = new Vector3D(1, 0, 0);
                Vector3D vy = new Vector3D(0, 1, 0);
                Vector3D vz = new Vector3D(0, 0, 1);
                qx = FromEuler(vx, roll);
                qy = FromEuler(vy, pitch);
                qz = FromEuler(vz, yaw);
                q = Multiply(qx, qy);
                q = Multiply(q, qz);
                return q;
            }

            /*
             *  ПЕРЕГРУЗКА ОПЕРАТОРОВ
             */
            /// <summary>
            /// Умножение кватернионов
            /// </summary>
            /// <param name="q1"></param>
            /// <param name="q2"></param>
            /// <returns>Новый кватернион</returns>
            public static Quaternion operator *(Quaternion q1, Quaternion q2)
            {
                double nw = q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z;
                double nx = q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y;
                double ny = q1.W * q2.Y + q1.Y * q2.W + q1.Z * q2.X - q1.X * q2.Z;
                double nz = q1.W * q2.Z + q1.Z * q2.W + q1.X * q2.Y - q1.Y * q2.X;
                return new Quaternion(nw, nx, ny, nz);
            }


            /*
             *  ТЕСТИРОВАНИЕ
             */


            public Vector3D ToEuler()
            {
                Vector3D axis = new Vector3D();

                Normalize(); //кватернион должен быть нормализован
                double qx2 = X * X;
                double qy2 = Y * Y;
                double qz2 = Z * Z;
                axis.X = Math.Atan2(2 * (X * W + Y * Z), 1 - 2 * (qx2 + qy2));
                axis.Y = Math.Asin(2 * (Y * W - Z * X));
                axis.Z = Math.Atan2(2 * (Z * W + X * Y), 1 - 2 * (qy2 + qz2));

                return axis;
            }
        }
    }
}
